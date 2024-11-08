using api.Context;
using api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/provinces")]
    public class ProvinceController: Controller
    {
        private readonly DBContext dbContext;
        private readonly IConfiguration configuration;
         
        public ProvinceController(DBContext context, IConfiguration config)
        {
            dbContext = context;
            configuration = config;
        }
        
        //list all province

        [HttpGet]
        public IActionResult Index()
        {
            List<ProvinceDTO> provinces = dbContext.Provinces
                .Select(province => new ProvinceDTO()
                {
                    id = province.id,
                    name = province.province_name
                })
                .ToList();
            return Ok(provinces);
        }
    }
}

