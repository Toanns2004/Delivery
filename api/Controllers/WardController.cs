using api.Context;
using api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/wards")]
    public class WardController: Controller
    {
        private readonly DBContext dbContext;

        public WardController(DBContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<WardDTO> wards = dbContext.Wards
                .Select(ward => new WardDTO()
                {
                    id = ward.id,
                    districtId = ward.district_id,
                    name = ward.ward_name
                })
                .ToList();
            return Ok(wards);
        }
    }
}

