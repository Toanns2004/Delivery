using api.Context;
using api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/districts")]
    public class DistrictController: Controller
    {
        private readonly DBContext dbContext;
        private readonly IConfiguration configuration;

        public DistrictController(DBContext context, IConfiguration config)
        {
            dbContext = context;
            configuration = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<DistrictDTO> districts = dbContext.Districts
                .Select(district => new DistrictDTO()
                {
                    id = district.id,
                    provinceId = district.province_id,
                    name = district.district_name
                })
                .ToList();
            return Ok(districts);
        }
    }
}

