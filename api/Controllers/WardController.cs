using api.Context;
using api.DTOs;
using api.Entities;
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

        [HttpGet("d={id}")]
        public IActionResult WardByDistrict(int id)
        {
            List<WardDTO> wards = dbContext.Wards
                .Where(w => w.district_id == id)
                .Select(w => new WardDTO()
                {
                    id = w.id,
                    districtId = w.district_id,
                    name = w.ward_name
                })
                .ToList();
            return Ok(wards);
        }

        [HttpGet("details/{id}")]
        public IActionResult WardDetails(int id)
        {
            try
            {
                Ward wardDetail = dbContext.Wards.Find(id);
                if (wardDetail == null)
                {
                    return NotFound("Ward not found.");
                }

                District districtDetail = dbContext.Districts.Find(wardDetail.district_id);
                if (districtDetail == null)
                {
                    return NotFound("District not found");
                }

                WardDTO ward = new WardDTO()
                {
                    id = wardDetail.id,
                    name = wardDetail.ward_name,
                    districtId = wardDetail.district_id,
                    provinceId = districtDetail.province_id
                };

                return Ok(ward);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

