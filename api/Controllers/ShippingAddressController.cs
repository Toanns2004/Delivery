using api.Context;
using api.DTOs;
using api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/client/{id}/shippingadd")]
    public class ShippingAddressController: ControllerBase
    {
        private readonly DBContext dbcontext;

        public ShippingAddressController(DBContext context)
        {
            dbcontext = context;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            List<ShippingAddressDTO> shippingAddresses = dbcontext.ShippingAddresses
                .Where(add => add.userId == id)
                .Select(add => new ShippingAddressDTO()
                {
                    id = add.id,
                    name = add.name,
                    telephone = add.telephone,
                    userName = add.User.fullname,
                    wardName = add.Ward.ward_name
                })
                .ToList();
            return Ok(shippingAddresses);
        }
    }
}

