using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(regModel.password);

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
                        fullname = newUser.fullname
                    });
                }
                catch (Exception e)
                {

                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Registration error.");
        }
    
        //login and receive a token

        [HttpPost("login")]
        public IActionResult Login(UserModel loginModel)
        {
            
        }
    
    }
}


