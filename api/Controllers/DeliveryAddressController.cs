using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/client/deliveryadd")]
    public class DeliveryAddressController: Controller
    {
        private readonly DBContext dbcontext;

        public DeliveryAddressController(DBContext context)
        {
            dbcontext = context;
        }

        //get add list by user
        [HttpGet("user/{userId}")]
        public IActionResult Index(int userId)
        {
            List<DeliveryAddressDTO> deliveryAddresses = dbcontext.DeliveryAddresses
                .Where(add => add.userId == userId)
                .Select(add => new DeliveryAddressDTO()
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
            return Ok(deliveryAddresses);
        }
        
        
        //get detail by address id
        [HttpGet("detail/{addId}")]
        public IActionResult AddDetails(int addId)
        {
            try
            {
                DeliveryAddress detailDeliveryAddress = dbcontext.DeliveryAddresses.Find(addId);
                if (detailDeliveryAddress == null)
                {
                    return NotFound("Delivery Address not found.");
                }

                Ward ward = dbcontext.Wards.Find(detailDeliveryAddress.wardId);
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

                DeliveryAddressDTO addDetails = new DeliveryAddressDTO()
                {
                    id = detailDeliveryAddress.id,
                    name = detailDeliveryAddress.name,
                    telephone = detailDeliveryAddress.telephone,
                    address = detailDeliveryAddress.address,
                    wardId = detailDeliveryAddress.wardId,
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
        public IActionResult Create(DeliveryAdressModel newAddModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DeliveryAddress newDeliveryAddress = new DeliveryAddress()
                    {
                        name = newAddModel.name,
                        telephone = newAddModel.telephone,
                        userId = newAddModel.userId,
                        address = newAddModel.address,
                        wardId = newAddModel.wardId
                    };
                    dbcontext.Add(newDeliveryAddress);
                    dbcontext.SaveChanges();
                    return Created("Created", new DeliveryAddressDTO()
                    {
                        name = newDeliveryAddress.name
                    });
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Create Delivery Address Error.");
        }
        
        
        //update add
        [HttpPut("update/{uId}")]
        public IActionResult Update(DeliveryAdressModel updateAddModel, int uId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DeliveryAddress updateDeliveryAddress = dbcontext.DeliveryAddresses.Find(uId);
                    if (updateDeliveryAddress == null)
                    {
                        return NotFound("Delivery Address not found.");
                    }

                    updateDeliveryAddress.name = updateAddModel.name;
                    updateDeliveryAddress.telephone = updateAddModel.telephone;
                    updateDeliveryAddress.userId = updateAddModel.userId;
                    updateDeliveryAddress.address = updateAddModel.address;
                    updateDeliveryAddress.wardId = updateAddModel.wardId;

                    dbcontext.SaveChanges();
                    return Ok("Update Delivery Address Success.");
                }
                catch (Exception e)
                {
                    BadRequest(e.Message);
                }
            }

            return BadRequest("Update Delivery Address Error");
        }
        
        
        //delete add
        [HttpDelete("delete/{delId}")]
        public IActionResult Delete(int delId)
        {
            try
            {
                DeliveryAddress deleteDeliveryAddress = dbcontext.DeliveryAddresses.Find(delId);
                if (deleteDeliveryAddress == null)
                {
                    return NotFound("Delivery Address not found.");
                }
                dbcontext.Remove(deleteDeliveryAddress);
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

