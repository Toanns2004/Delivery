using api.Context;
using api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("emp/roles")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly DBContext dbContext;

        public RoleController(DBContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<RoleDTO> roles = dbContext.Roles
                .Select(role => new RoleDTO()
                {
                    id = role.id,
                    name = role.name
                })
                .ToList();
            return Ok(roles);
        }
    }
}

