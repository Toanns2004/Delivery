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


        [HttpGet("p={id}")]
        public IActionResult DistrictByProvince(int id)
        {
            List<DistrictDTO> districts = dbContext.Districts
                .Where(dis => dis.province_id == id)
                .Select(dis => new DistrictDTO()
                {
                    id = dis.id,
                    provinceId = dis.province_id,
                    name = dis.district_name
                })
                .ToList();
            return Ok(districts);
        }
    }
}

