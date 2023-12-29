using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace api.Controllers
{
    [ApiController]
    [Route("/emp")]
    public class EmployeeController : Controller
    {
        private readonly DBContext dbContext;
        private IConfiguration configuration;

        public EmployeeController(DBContext context, IConfiguration config)
        {
            dbContext = context;
            configuration = config;
        }

        
        //create acc
        [Route("create")]
        public IActionResult Create(EmployeeModel newEmployeeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string hashedPwd = BCrypt.Net.BCrypt.HashPassword(newEmployeeModel.password);

                    Employee newEmployee = new Employee()
                    {
                        fullname = newEmployeeModel.fullname,
                        username = newEmployeeModel.username,
                        email = newEmployeeModel.email,
                        password = hashedPwd,
                        roleId = newEmployeeModel.roleId,
                        postOfficeId = newEmployeeModel.postOfficeId
                    };

                    dbContext.Employees.Add(newEmployee);
                    dbContext.SaveChanges();
                    return Created("", new EmployeeDTO()
                    {
                        username = newEmployee.username,
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Create account error");
        }
        
        //login
        [Route("login")]
        public IActionResult Login(EmpLoginModel loginModel)
        {
            var emp = dbContext.Employees.SingleOrDefault(e => e.username == loginModel.username);
            if (emp != null)
            {
                bool pwdMatch = BCrypt.Net.BCrypt.Verify(loginModel.password, emp.password);
                if (pwdMatch)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, emp.username),
                        new Claim(ClaimTypes.Email, emp.email),
                        new Claim(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
                        new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"])
                    };
                    
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var token = new JwtSecurityToken(
                        issuer: configuration["Issuer"],
                        audience: configuration["Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );

                    List<PermissionDTO> permissions = dbContext.Permissions
                        .Where(p => p.roleId == emp.roleId)
                        .Select(p => new PermissionDTO()
                        {
                            prefix = p.prefix,
                            name = p.name
                        })
                        .ToList();
                    
                    return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            username = emp.username,
                            fullname = emp.fullname,
                            permissions
                        }
                    );
                }
            }

            return Unauthorized();
        }
    }
}

