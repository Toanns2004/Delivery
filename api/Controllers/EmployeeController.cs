using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        //get all acc
        [HttpGet]
        public IActionResult Index()
        {
            List<EmployeeDTO> employees = dbContext.Employees
                .Select(emp => new EmployeeDTO()
                {
                    id = emp.id,
                    username = emp.username,
                    fullname = emp.fullname,
                    email = emp.email,
                    role = emp.Role.name,
                    postOffice = emp.PostOffice.postName
                })
                .ToList();
            return Ok(employees);
        }
        
        //get acc detail
        [HttpGet]
        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            try
            {
                Employee employee = dbContext.Employees.Find(id);
                if (employee == null)
                {
                    return NotFound("Account not found.");
                }

                Role role = dbContext.Roles.Find(employee.roleId);
                if (role == null)
                {
                    return NotFound("Role not found.");
                }

                PostOffice postOffice = dbContext.PostOffices.Find(employee.postOfficeId);
                if (postOffice == null)
                {
                    return NotFound("Post office not found.");
                }

                Ward ward = dbContext.Wards.Find(postOffice.wardId);
                if (ward == null)
                {
                    return NotFound("Ward not found.");
                }

                District district = dbContext.Districts.Find(ward.district_id);
                if (district == null)
                {
                    return NotFound("District not found.");
                }
                

                EmployeeDTO empDetails = new EmployeeDTO()
                {
                    id = employee.id,
                    postOfficeId = employee.postOfficeId,
                    districtId = district.id,
                    provinceId = district.province_id,
                    roleId = employee.roleId,
                    username = employee.username,
                    fullname = employee.fullname,
                    email = employee.email,
                    role = role.name,
                    postOffice = postOffice.postName
                };

                return Ok(empDetails);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
        }
        
        //create acc
        [HttpPost]
        [Route("create")]
        public IActionResult Create(EmployeeModel newEmployeeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (dbContext.Employees.Any(emp => emp.username == newEmployeeModel.username))
                    {
                        return BadRequest("Username is already exist.");
                    }
                    
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
        
        //update acc
        
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult AccUpdate(int id, EmployeeModel updateEmpModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee updateEmployee = dbContext.Employees.Find(id);
                    if (updateEmployee == null)
                    {
                        return NotFound("Employee Not found.");
                    }

                    updateEmployee.fullname = updateEmpModel.fullname;
                    updateEmployee.email = updateEmpModel.email;
                    updateEmployee.roleId = updateEmpModel.roleId;
                    updateEmployee.postOfficeId = updateEmpModel.postOfficeId;
                    if (!string.IsNullOrEmpty(updateEmpModel.username))
                    {
                        updateEmployee.username = updateEmpModel.username;
                    }

                    if (!string.IsNullOrEmpty(updateEmpModel.password))
                    {
                        updateEmployee.password = updateEmpModel.password;
                    }
                    
                    
                    dbContext.SaveChanges();

                    return Ok(new EmployeeDTO()
                    {
                        fullname = updateEmployee.fullname
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Update employee error.");
        }
        
        //delete Acc
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteAcc(int id)
        {
            try
            {
                Employee deleteEmployee = dbContext.Employees.Find(id);
                if (deleteEmployee == null)
                {
                    return NotFound("Employee not found");
                }

                dbContext.Employees.Remove(deleteEmployee);
                dbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        //login
        [HttpPost]
        [Route("login")]
        public IActionResult Login(EmpLoginModel loginModel)
        {
            var emp = dbContext.Employees.Include(e => e.Role).Include(e => e.PostOffice).SingleOrDefault(e => e.username == loginModel.username);
            if (emp != null)
            {
                bool pwdMatch = BCrypt.Net.BCrypt.Verify(loginModel.password, emp.password);
                if (pwdMatch)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, emp.username),
                        new Claim(ClaimTypes.Email, emp.email),
                        new Claim("Role", emp.Role.name),
                        new Claim("PostOffice", emp.PostOffice.id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
                        new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"])
                    };
                    
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var token = new JwtSecurityToken(
                        issuer: configuration["Issuer"],
                        audience: configuration["Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["Jwt: LifeTime"])),
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
                            permissions,
                            id = emp.id,
                            role = emp.Role.name
                        }
                    );
                }
            }

            return Unauthorized();
        }
        
        //change password
        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public IActionResult ChangePassword(ChangePwdModel changePwdModel)
        {
            var emp = HttpContext.User;
            var username = emp.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            Employee updateEmp = dbContext.Employees.SingleOrDefault(e => e.username == username);

            if (updateEmp != null)
            {
                bool passwordMatch = BCrypt.Net.BCrypt.Verify(changePwdModel.currentPassword, updateEmp.password);

                if (passwordMatch)
                {
                    updateEmp.password = BCrypt.Net.BCrypt.HashPassword(changePwdModel.newPassword);
                    dbContext.SaveChanges();
                    return Ok( new
                        {
                            Message = "Password change",
                            username = username
                        }
                    );
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}

