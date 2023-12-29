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
    [Route("/bill")]
    [Authorize]
    public class BillController : Controller
    {
        private readonly DBContext dbContext;

        public BillController(DBContext context)
        {
            dbContext = context;
        } 
        
        //get bills by user
        [HttpGet]
        [Route("user/{userId}")]
        public IActionResult Index(int userId)
        {
            List<BillDTO> bills = dbContext.Bills
                .Where(bill => bill.userId == userId)
                .Select(bill => new BillDTO()
                {
                    billNumber = bill.billNumber,
                    dateCreated = bill.dateCreated,
                    DeliveryAddressDto = dbContext.DeliveryAddresses
                        .Where(add => add.id == bill.deilveryAddId)
                        .Select(add => new DeliveryAddressDTO()
                        {
                            name = add.name,
                            telephone = add.telephone,
                            address = add.address,
                            wardName = add.Ward.ward_name,
                            districtName = add.Ward.District.district_name,
                            provinceName = add.Ward.District.Province.province_name
                        })
                        .FirstOrDefault(),
                    name = dbContext.BillDetails
                        .Where(d => d.billId == bill.id)
                        .Select(d => d.name)
                        .FirstOrDefault()
                })
                .ToList();
            return Ok(bills);
        }
        
        // create bill

        [HttpPost]
        [Route("create")]
        public IActionResult Create(BillModel newBillModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // create bill number
                    string lastBillNum = dbContext.Bills
                        .OrderByDescending(bill => bill.id)
                        .Select(bill => bill.billNumber)
                        .FirstOrDefault();

                    string nextBillNum;
                    if (string.IsNullOrEmpty(lastBillNum))
                    {
                        nextBillNum = "B000000001";
                    }
                    else
                    {
                        string numberPart = lastBillNum.Substring(1);
                        
                        int lastNumber = int.Parse(numberPart);
                        
                        nextBillNum = "B" + (++lastNumber).ToString("D9");
                    }
                    
                    //get price unit

                    string range = string.Empty;

                    ShippingAddress shippingAddress = dbContext.ShippingAddresses.Find(newBillModel.shippingAddId);
                    if (shippingAddress == null)
                    {
                        return NotFound("Shipping Address not found.");
                    }
                    
                    Ward sWard = dbContext.Wards.Find(shippingAddress.wardId);
                    if (sWard == null)
                    {
                        return NotFound("Ward not found.");
                    }

                    District sDistrict = dbContext.Districts.Find(sWard.district_id);
                    if (sDistrict == null)
                    {
                        return NotFound("District not found.");
                    }

                    DeliveryAddress deliveryAddress = dbContext.DeliveryAddresses.Find(newBillModel.deliveryAddId);
                    if (deliveryAddress == null)
                    {
                        return NotFound("Delivery Address not found");
                    }
                    
                    Ward dWard = dbContext.Wards.Find(deliveryAddress.wardId);
                    if (dWard == null)
                    {
                        return NotFound("Ward not found.");
                    }

                    District dDistrict = dbContext.Districts.Find(dWard.district_id);
                    if (dDistrict == null)
                    {
                        return NotFound("District not found.");
                    }

                    if (sDistrict.province_id == dDistrict.province_id)
                    { 
                        range = "provincial";
                    }
                    else
                    {
                        range = "out-province";
                    }

                    string weightLimit = string.Empty;
                    if (newBillModel.weight <= 3000)
                    {
                        weightLimit = "up to 3000";
                    }
                    else if (newBillModel.weight <= 10000)
                    {
                        weightLimit = "up to 10000";
                    }
                    else
                    {
                        weightLimit = "exceed 10000";
                    }

                    UnitPrice newUnitPrice = dbContext.UnitPrices
                        .Where(price => price.range == range && price.weightLimit == weightLimit)
                        .FirstOrDefault();
                    if (newUnitPrice == null)
                    {
                        return NotFound("Unit Price Not found.");
                    }
                    
                    //handle charge

                    double rCharge = newUnitPrice.chargeRate * newBillModel.weight / 1000;

                    double charge = Math.Round(rCharge, 2);

                    double totalCharge = 0;

                    double pickupDiscount = 0;
                    if (newBillModel.pickupType == "post-office")
                    {
                        pickupDiscount = Math.Round(rCharge * 0.05, 2);
                    }

                    double dlvDiscount = 0;
                    if (newBillModel.deliveryType == "post-office")
                    {
                        dlvDiscount = Math.Round(rCharge * 0.05, 2);
                    }

                    double insuranceFee = 0;
                    if (newBillModel.value > 0)
                    {
                        insuranceFee = Math.Round(newBillModel.value * 0.01, 2);
                    }

                    totalCharge = charge - pickupDiscount - dlvDiscount + insuranceFee;
                    

                    Bill newBill = new Bill()
                    {
                        billNumber = nextBillNum,
                        userId = newBillModel.userId,
                        shippingAddId = newBillModel.shippingAddId,
                        deilveryAddId = newBillModel.deliveryAddId,
                        unitPriceId = newUnitPrice.id,
                        charge = charge,
                        pickupType = newBillModel.pickupType,
                        deliveryType = newBillModel.deliveryType,
                        insuranceFee = insuranceFee,
                        totalCharge = totalCharge,
                        payer = newBillModel.payer,
                        note = newBillModel.note,
                        cod = newBillModel.cod,
                        dateCreated = DateTime.Now
                    };

                    dbContext.Add(newBill);
                    dbContext.SaveChanges();

                    BillDetail newBillDetail = new BillDetail()
                    {
                        billId = newBill.id,
                        name = newBillModel.name,
                        nature = newBillModel.nature,
                        weight = newBillModel.weight,
                        length = newBillModel.length,
                        width = newBillModel.width,
                        height = newBillModel.height,
                        value = newBillModel.value
                    };

                    dbContext.Add(newBillDetail);
                    dbContext.SaveChanges();

                    return Created("Create bill successfully", new BillDTO()
                    {
                        billNumber = newBill.billNumber
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Create Bill error");
        }
    }
    
}

