using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/client/shippingadd")]
    public class ShippingAddressController: ControllerBase
    { 
        private readonly DBContext dbcontext;

        public ShippingAddressController(DBContext context)
        {
            dbcontext = context;
        }

        //get add list by user
        [HttpGet("user/{userId}")]
        public IActionResult Index(int userId)
        {
            List<ShippingAddressDTO> shippingAddresses = dbcontext.ShippingAddresses
                .Where(add => add.userId == userId)
                .Select(add => new ShippingAddressDTO()
                {
                    id = add.id,
                    name = add.name,
                    telephone = add.telephone,
                    address = add.address,
                    wardName = add.Ward.ward_name,
                    districtName = add.Ward.District.district_name,
                    provinceName = add.Ward.District.Province.province_name
                })
                .ToList();
            return Ok(shippingAddresses);
        }
        
        
        //get detail by address id
        [HttpGet("detail/{addId}")]
        public IActionResult AddDetails(int addId)
        {
            try
            {
                ShippingAddress detailShippingAddress = dbcontext.ShippingAddresses.Find(addId);
                if (detailShippingAddress == null)
                {
                    return NotFound("Shipping Address not found.");
                }

                Ward ward = dbcontext.Wards.Find(detailShippingAddress.wardId);
                if (ward == null)
                {
                    return NotFound("Ward not found.");
                }

                District district = dbcontext.Districts.Find(ward.district_id);
                if (district == null)
                {
                    return NotFound("District not found.");
                }

                Province province = dbcontext.Provinces.Find(district.province_id);
                if (province == null)
                {
                    return NotFound("Province not found.");
                }

                ShippingAddressDTO addDetails = new ShippingAddressDTO
                {
                    id = detailShippingAddress.id,
                    name = detailShippingAddress.name,
                    telephone = detailShippingAddress.telephone,
                    address = detailShippingAddress.address,
                    wardId = detailShippingAddress.wardId,
                    wardName = ward.ward_name,
                    districtName = district.district_name,
                    provinceName = province.province_name
                };
                
                return Ok(addDetails);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest("Get Address Details Error.");
        }
        
        
        //create add
        [HttpPost("create")]
        public IActionResult Create(ShippingAddressModel newAddModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ShippingAddress newShippingAddress = new ShippingAddress()
                    {
                        name = newAddModel.name,
                        telephone = newAddModel.telephone,
                        userId = newAddModel.userId,
                        address = newAddModel.address,
                        wardId = newAddModel.wardId
                    };
                    dbcontext.Add(newShippingAddress);
                    dbcontext.SaveChanges();
                    return Created("Created", new ShippingAddressDTO()
                    {
                        name = newShippingAddress.name
                    });
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Create Sipping Address Error.");
        }
        
        
        //update add
        [HttpPut("update/{uId}")]
        public IActionResult Update(ShippingAddressModel updateAddModel, int uId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ShippingAddress updateShippingAddress = dbcontext.ShippingAddresses.Find(uId);
                    if (updateShippingAddress == null)
                    {
                        return NotFound("Shipping Address not found.");
                    }

                    updateShippingAddress.name = updateAddModel.name;
                    updateShippingAddress.telephone = updateAddModel.telephone;
                    updateShippingAddress.userId = updateAddModel.userId;
                    updateShippingAddress.address = updateAddModel.address;
                    updateShippingAddress.wardId = updateAddModel.wardId;

                    dbcontext.SaveChanges();
                    return Ok("Update Shipping Address Success.");
                }
                catch (Exception e)
                {
                    BadRequest(e.Message);
                }
            }

            return BadRequest("Update Shipping Address Error");
        }
        
        
        //delete add
        [HttpDelete("delete/{delId}")]
        public IActionResult Delete(int delId)
        {
            try
            {
                ShippingAddress deleteShippingAddress = dbcontext.ShippingAddresses.Find(delId);
                if (deleteShippingAddress == null)
                {
                    return NotFound("Shipping Address not found.");
                }
                dbcontext.Remove(deleteShippingAddress);
                dbcontext.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

