using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : Controller
    {
        private readonly DBContext dbContext;
        private IConfiguration configuration;

        public AuthController(DBContext context, IConfiguration config)
        {
            dbContext = context;
            configuration = config;
        }
    
        //register
        [HttpPost("register")]
        public IActionResult Register(UserModel regModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(regModel.password, salt);

                    User newUser = new User
                    {
                        fullname = regModel.fullname,
                        email = regModel.email,
                        telephone = regModel.telephone,
                        address = regModel.address,
                        password = hashedPassword
                    };

                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                    return Created("", new UserDTO
                    {
                        id = newUser.id,
                        fullname = newUser.fullname,
                        email = newUser.email,
                        token = null
                    });
                }
                catch (Exception e)
                {
                    return Unauthorized("Registration error.");
                }
            }
            return Unauthorized("Registration error.");
        }
    
        //login and receive a token

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                //compare email
                var user = dbContext.Users.SingleOrDefault(u => u.email == loginModel.email);
                if (user != null)
                {
                    //compare password
                    bool passwordMatch = BCrypt.Net.BCrypt.Verify(loginModel.password, user.password);
                    if (passwordMatch)
                    {
                        var payload = new[]
                        {
                            new Claim(ClaimTypes.Email, user.email),
                            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                            new Claim(ClaimTypes.Name, user.fullname),
                            new Claim(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
                            new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"]),
                        };
                    
                        //generate jwt token
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                        var token = new JwtSecurityToken(
                            issuer: configuration["Issuer"],
                            audience: configuration["Audience"],
                            claims: payload,
                            expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["Jwt: LifeTime"])),
                            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );
                    
                        return Ok(new UserDTO()
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                id = user.id,
                                fullname = user.fullname,
                                email = user.email
                            }
                        );
                    }
                    return Unauthorized("Email or password incorrect");
                }

                return Unauthorized("Email or password incorrect");
            }
            
            return Unauthorized("Email or password incorrect");
        }
    
    }
}


