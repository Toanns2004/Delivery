using System.Security.Claims;
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
        
        //get all bills
        [HttpGet]
        public IActionResult Index()
        {
            List<BillDTO> bills = dbContext.Bills
                .Select(bill => new BillDTO()
                {
                    billNumber = bill.billNumber,
                    dateCreated = bill.dateCreated,
                    DeliveryAddressDto = dbContext.DeliveryAddresses
                        .Where(add => add.id == bill.deliveryAddId)
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
                    BillDetailsDto = dbContext.BillDetails
                        .Where(d => d.billId == bill.id)
                        .Select(d => new BillDetailsDTO()
                        {
                            name = d.name,
                            weight = d.weight,
                            length = d.length,
                            width = d.width,
                            height = d.height
                        })
                        .FirstOrDefault(),
                    latestStatus = dbContext.Status
                        .Where(stt => stt.billId == bill.id)
                        .OrderByDescending(stt => stt.time)
                        .Select(stt => new StatusDTO()
                        {
                            id = stt.id,
                            typeId = stt.StatusType.id,
                            name = stt.StatusType.name
                        })
                        .FirstOrDefault()
                })
                .ToList();
            return Ok(bills);
        }
        
        //get bills by user
        [HttpGet]
        [Route("user/{userId}")]
        public IActionResult BillByUser(int userId)
        {
            List<BillDTO> bills = dbContext.Bills
                .Where(bill => bill.userId == userId)
                .Select(bill => new BillDTO()
                {
                    id = bill.id,
                    billNumber = bill.billNumber,
                    dateCreated = bill.dateCreated,
                    DeliveryAddressDto = dbContext.DeliveryAddresses
                        .Where(add => add.id == bill.deliveryAddId)
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
                    BillDetailsDto = dbContext.BillDetails
                        .Where(d => d.billId == bill.id)
                        .Select(d => new BillDetailsDTO()
                        {
                            name = d.name,
                            weight = d.weight,
                            length = d.length,
                            width = d.width,
                            height = d.height
                        })
                        .FirstOrDefault(),
                    latestStatus = dbContext.Status
                        .Where(stt => stt.billId == bill.id)
                        .OrderByDescending(stt => stt.time)
                        .Select(stt => new StatusDTO()
                        {
                            id = stt.id,
                            typeId = stt.StatusType.id,
                            name = stt.StatusType.name,
                            time = stt.time
                        })
                        .FirstOrDefault()
                })
                .ToList();
            return Ok(bills);
        }
        
        //get bill by month
        [HttpGet]
        [Route("month")]
        public IActionResult BillByMount(int id, int month, int year)
        {
            if (month < 1 || month > 12 || year < 1)
            {
                return BadRequest("Invalid month");
            }

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            
            List<BillDTO> billOfMonth = dbContext.Bills
                .Where(bill => bill.userId == id && bill.dateCreated >= startDate && bill.dateCreated <= endDate)
                .Select(bill => new BillDTO()
                    {
                        billNumber = bill.billNumber,
                        dateCreated = bill.dateCreated,
                        DeliveryAddressDto = dbContext.DeliveryAddresses
                            .Where(add => add.id == bill.deliveryAddId)
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
                        BillDetailsDto = dbContext.BillDetails
                            .Where(d => d.billId == bill.id)
                            .Select(d => new BillDetailsDTO()
                            {
                                name = d.name,
                                weight = d.weight,
                                length = d.length,
                                width = d.width,
                                height = d.height
                            })
                            .FirstOrDefault(),
                        latestStatus = dbContext.Status
                            .Where(stt => stt.billId == bill.id)
                            .OrderByDescending(stt => stt.time)
                            .Select(stt => new StatusDTO()
                            {
                                id = stt.id,
                                typeId = stt.StatusType.id,
                                name = stt.StatusType.name,
                                time = stt.time
                            })
                            .FirstOrDefault()
                    })
                    .ToList();
            return Ok(billOfMonth);
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
                    
                    UnitPrice newUnitPrice = dbContext.UnitPrices
                        .Where(price => price.range == range && price.minWeight <= newBillModel.weight && price.maxWeight > newBillModel.weight)
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
                        deliveryAddId = newBillModel.deliveryAddId,
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

                    Status status = new Status()
                    {
                        typeId = 1,
                        billId = newBill.id,
                        time = DateTime.Now
                    };
                    dbContext.Add(status);
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
        
        //get bill details by Bill number
        [HttpGet]
        [Route("detail/{bn}")]
        public IActionResult BillDetails(string bn)
        {
            BillDTO billDetails = dbContext.Bills
                .Where(bill => bill.billNumber == bn)
                .Select(bill => new BillDTO()
                {
                    id = bill.id,
                    billNumber = bill.billNumber,
                    totalCharge = bill.totalCharge,
                    payer = bill.payer,
                    cod = bill.cod,
                    dateCreated = bill.dateCreated,
                    note = bill.note,
                    DeliveryAddressDto = dbContext.DeliveryAddresses
                        .Where(add => add.id == bill.deliveryAddId)
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
                    ShippingAddressDto = dbContext.ShippingAddresses
                        .Where(add => add.id == bill.shippingAddId)
                        .Select(add => new ShippingAddressDTO()
                        {
                            name = add.name,
                            telephone = add.telephone,
                            address = add.address,
                            wardName = add.Ward.ward_name,
                            districtName = add.Ward.District.district_name,
                            provinceName = add.Ward.District.Province.province_name
                        })
                        .FirstOrDefault(),
                    BillDetailsDto = dbContext.BillDetails
                        .Where(d => d.billId == bill.id)
                        .Select(d => new BillDetailsDTO()
                        {
                            name = d.name,
                            weight = d.weight,
                            length = d.length,
                            width = d.width,
                            height = d.height,
                            nature = d.nature
                        })
                        .FirstOrDefault(),
                    latestStatus = dbContext.Status
                        .Where(stt => stt.billId == bill.id)
                        .OrderByDescending(stt => stt.time)
                        .Select(stt => new StatusDTO()
                        {
                            id = stt.id,
                            typeId = stt.StatusType.id,
                            name = stt.StatusType.name
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefault();
            return Ok(billDetails);
        }
        
        //get bills to receive from shipper
        [HttpGet]
        [Route("receive/shipper")]
        public IActionResult RcvFromShipper()
        {
            var emp = HttpContext.User;

            var role = emp.Claims.FirstOrDefault(r => r.Type == "Role")?.Value;
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            PostOffice postOffice = dbContext.PostOffices.Include(p => p.Ward).FirstOrDefault(p => p.id == poId);
            if (postOffice == null)
            {
                return NotFound("PostOffice not found.");
            }
            if (role == "Postman")
            {
                List<BillDTO> rcvBills = dbContext.Bills
                    .Where(bill =>
                        bill.ShippingAddress.Ward.district_id == postOffice.Ward.district_id &&
                        bill.pickupType == "home"
                    )
                    .Select(bill => new
                    {
                        Bill = bill,
                        LatestStatus = dbContext.Status
                            .Where(stt => stt.billId == bill.id)
                            .OrderByDescending(stt => stt.time)
                            .FirstOrDefault()
                    })
                    .Where(result => result.LatestStatus.StatusType.name == "Created")
                    .Select(result => new BillDTO
                    {
                        id = result.Bill.id,
                        billNumber = result.Bill.billNumber,
                        totalCharge = result.Bill.totalCharge,
                    payer = result.Bill.payer,
                    cod = result.Bill.cod,
                    dateCreated = result.Bill.dateCreated,
                    note = result.Bill.note,
                    DeliveryAddressDto = dbContext.DeliveryAddresses
                        .Where(add => add.id == result.Bill.deliveryAddId)
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
                    ShippingAddressDto = dbContext.ShippingAddresses
                        .Where(add => add.id == result.Bill.shippingAddId)
                        .Select(add => new ShippingAddressDTO()
                        {
                            name = add.name,
                            telephone = add.telephone,
                            address = add.address,
                            wardName = add.Ward.ward_name,
                            districtName = add.Ward.District.district_name,
                            provinceName = add.Ward.District.Province.province_name
                        })
                        .FirstOrDefault(),
                    BillDetailsDto = dbContext.BillDetails
                        .Where(d => d.billId == result.Bill.id)
                        .Select(d => new BillDetailsDTO()
                        {
                            name = d.name,
                            weight = d.weight,
                            length = d.length,
                            width = d.width,
                            height = d.height,
                            nature = d.nature
                        })
                        .FirstOrDefault(),
                    latestStatus = dbContext.Status
                        .Where(stt => stt.billId == result.Bill.id)
                        .OrderByDescending(stt => stt.time)
                        .Select(stt => new StatusDTO()
                        {
                            id = stt.id,
                            typeId = stt.StatusType.id,
                            name = stt.StatusType.name
                        })
                        .FirstOrDefault()
                    })
                    .ToList();

                return Ok(rcvBills);

            }else if (role == "Staff")
                {
                    List<BillDTO> rcvBills = dbContext.Bills
                    .Where(bill =>
                        bill.ShippingAddress.Ward.district_id == postOffice.Ward.district_id &&
                        bill.pickupType == "post-office"
                    )
                    .Select(bill => new
                    {
                        Bill = bill,
                        LatestStatus = dbContext.Status
                            .Where(stt => stt.billId == bill.id)
                            .OrderByDescending(stt => stt.time)
                            .FirstOrDefault()
                    })
                    .Where(result => result.LatestStatus.StatusType.name == "Created") // Lọc theo trạng thái 'Created'
                    .Select(result => new BillDTO
                    {
                        id = result.Bill.id,
                        billNumber = result.Bill.billNumber,
                        totalCharge = result.Bill.totalCharge,
                    payer = result.Bill.payer,
                    cod = result.Bill.cod,
                    dateCreated = result.Bill.dateCreated,
                    note = result.Bill.note,
                    DeliveryAddressDto = dbContext.DeliveryAddresses
                        .Where(add => add.id == result.Bill.deliveryAddId)
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
                    ShippingAddressDto = dbContext.ShippingAddresses
                        .Where(add => add.id == result.Bill.shippingAddId)
                        .Select(add => new ShippingAddressDTO()
                        {
                            name = add.name,
                            telephone = add.telephone,
                            address = add.address,
                            wardName = add.Ward.ward_name,
                            districtName = add.Ward.District.district_name,
                            provinceName = add.Ward.District.Province.province_name
                        })
                        .FirstOrDefault(),
                    BillDetailsDto = dbContext.BillDetails
                        .Where(d => d.billId == result.Bill.id)
                        .Select(d => new BillDetailsDTO()
                        {
                            name = d.name,
                            weight = d.weight,
                            length = d.length,
                            width = d.width,
                            height = d.height,
                            nature = d.nature
                        })
                        .FirstOrDefault(),
                    latestStatus = dbContext.Status
                        .Where(stt => stt.billId == result.Bill.id)
                        .OrderByDescending(stt => stt.time)
                        .Select(stt => new StatusDTO()
                        {
                            id = stt.id,
                            typeId = stt.StatusType.id,
                            name = stt.StatusType.name
                        })
                        .FirstOrDefault()
                    })
                    .ToList();

                return Ok(rcvBills);
                }
                else
                {
                    return BadRequest("Invalid role");
                }
        }
        
        //receive from postman
        [HttpGet]
        [Route("receive/postman")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult RcvFromPostman()
        {
            var emp = HttpContext.User;
            
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            PostOffice postOffice = dbContext.PostOffices.Include(p => p.Ward).FirstOrDefault(p => p.id == poId);
            if (postOffice == null)
            {
                return NotFound("PostOffice not found.");
            }
            List<BillDTO> rcvBills = dbContext.Bills
            .Where(bill =>
                bill.ShippingAddress.Ward.district_id == postOffice.Ward.district_id
            )
            .Select(bill => new
            {
                Bill = bill,
                LatestStatus = dbContext.Status
                    .Where(stt => stt.billId == bill.id)
                    .OrderByDescending(stt => stt.time)
                    .FirstOrDefault()
            })
            .Where(result => result.LatestStatus.StatusType.name == "Received from shipper") // Lọc theo trạng thái 'Created'
            .Select(result => new BillDTO
            {
                id = result.Bill.id,
                billNumber = result.Bill.billNumber,
                totalCharge = result.Bill.totalCharge,
            payer = result.Bill.payer,
            cod = result.Bill.cod,
            dateCreated = result.Bill.dateCreated,
            note = result.Bill.note,
            DeliveryAddressDto = dbContext.DeliveryAddresses
                .Where(add => add.id == result.Bill.deliveryAddId)
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
            ShippingAddressDto = dbContext.ShippingAddresses
                .Where(add => add.id == result.Bill.shippingAddId)
                .Select(add => new ShippingAddressDTO()
                {
                    name = add.name,
                    telephone = add.telephone,
                    address = add.address,
                    wardName = add.Ward.ward_name,
                    districtName = add.Ward.District.district_name,
                    provinceName = add.Ward.District.Province.province_name
                })
                .FirstOrDefault(),
            BillDetailsDto = dbContext.BillDetails
                .Where(d => d.billId == result.Bill.id)
                .Select(d => new BillDetailsDTO()
                {
                    name = d.name,
                    weight = d.weight,
                    length = d.length,
                    width = d.width,
                    height = d.height,
                    nature = d.nature
                })
                .FirstOrDefault(),
            latestStatus = dbContext.Status
                .Where(stt => stt.billId == result.Bill.id)
                .OrderByDescending(stt => stt.time)
                .Select(stt => new StatusDTO()
                {
                    id = stt.id,
                    typeId = stt.StatusType.id,
                    name = stt.StatusType.name,
                    time = stt.time
                })
                .FirstOrDefault()
            })
            .ToList();

        return Ok(rcvBills);
        }
        
        //bill for dispatch
        [HttpGet]
        [Route("dispatch")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult BillsAtPO()
        {
            var emp = HttpContext.User;
            
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            
            List<BillDTO> distpatchBill = dbContext.Bills
               .Select(bill => new
                {
                    Bill = bill,
                    LatestStatus = dbContext.Status
                        .Where(stt => stt.billId == bill.id)
                        .OrderByDescending(stt => stt.time)
                        .FirstOrDefault()
                })
            .Where(result => result.LatestStatus.StatusType.name == "Received at Post Office" &&
               result.LatestStatus.Employee.PostOffice.id == poId
               )
            .Select(result => new BillDTO
                {
                    id = result.Bill.id,
                    billNumber = result.Bill.billNumber,
                    totalCharge = result.Bill.totalCharge,
                payer = result.Bill.payer,
                cod = result.Bill.cod,
                dateCreated = result.Bill.dateCreated,
                note = result.Bill.note,
                DeliveryAddressDto = dbContext.DeliveryAddresses
                    .Where(add => add.id == result.Bill.deliveryAddId)
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
                ShippingAddressDto = dbContext.ShippingAddresses
                    .Where(add => add.id == result.Bill.shippingAddId)
                    .Select(add => new ShippingAddressDTO()
                    {
                        name = add.name,
                        telephone = add.telephone,
                        address = add.address,
                        wardName = add.Ward.ward_name,
                        districtName = add.Ward.District.district_name,
                        provinceName = add.Ward.District.Province.province_name
                    })
                    .FirstOrDefault(),
                BillDetailsDto = dbContext.BillDetails
                    .Where(d => d.billId == result.Bill.id)
                    .Select(d => new BillDetailsDTO()
                    {
                        name = d.name,
                        weight = d.weight,
                        length = d.length,
                        width = d.width,
                        height = d.height,
                        nature = d.nature
                    })
                    .FirstOrDefault(),
                latestStatus = dbContext.Status
                    .Where(stt => stt.billId == result.Bill.id)
                    .OrderByDescending(stt => stt.time)
                    .Select(stt => new StatusDTO()
                    {
                        id = stt.id,
                        typeId = stt.StatusType.id,
                        name = stt.StatusType.name,
                        time = stt.time
                    })
                    .FirstOrDefault()
                })
            .ToList();
            return Ok(distpatchBill);
        }
        
        //receive from transport
        [HttpGet]
        [Route("receive/transport")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult BillFromTransport()
        {
            var emp = HttpContext.User;
            
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            PostOffice postOffice = dbContext.PostOffices.Include(p => p.Ward).FirstOrDefault(p => p.id == poId);
            if (postOffice == null)
            {
                return NotFound("PostOffice not found.");
            }
            
            List<BillDTO> billFromTransport = dbContext.Bills
                .Where(bill => bill.DeliveryAddress.Ward.district_id == postOffice.Ward.district_id)
                .Select(bill => new
                {
                    Bill = bill,
                    LatestStatus = dbContext.Status
                        .Where(stt => stt.billId == bill.id)
                        .OrderByDescending(stt => stt.time)
                        .FirstOrDefault()
                })
                .Where(result => result.LatestStatus.StatusType.name == "Transporting"
                )
                .Select(result => new BillDTO
                {
                    id = result.Bill.id,
                    billNumber = result.Bill.billNumber,
                    totalCharge = result.Bill.totalCharge,
                payer = result.Bill.payer,
                cod = result.Bill.cod,
                dateCreated = result.Bill.dateCreated,
                note = result.Bill.note,
                DeliveryAddressDto = dbContext.DeliveryAddresses
                    .Where(add => add.id == result.Bill.deliveryAddId)
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
                ShippingAddressDto = dbContext.ShippingAddresses
                    .Where(add => add.id == result.Bill.shippingAddId)
                    .Select(add => new ShippingAddressDTO()
                    {
                        name = add.name,
                        telephone = add.telephone,
                        address = add.address,
                        wardName = add.Ward.ward_name,
                        districtName = add.Ward.District.district_name,
                        provinceName = add.Ward.District.Province.province_name
                    })
                    .FirstOrDefault(),
                BillDetailsDto = dbContext.BillDetails
                    .Where(d => d.billId == result.Bill.id)
                    .Select(d => new BillDetailsDTO()
                    {
                        name = d.name,
                        weight = d.weight,
                        length = d.length,
                        width = d.width,
                        height = d.height,
                        nature = d.nature
                    })
                    .FirstOrDefault(),
                latestStatus = dbContext.Status
                    .Where(stt => stt.billId == result.Bill.id)
                    .OrderByDescending(stt => stt.time)
                    .Select(stt => new StatusDTO()
                    {
                        id = stt.id,
                        typeId = stt.StatusType.id,
                        name = stt.StatusType.name,
                        time = stt.time
                    })
                    .FirstOrDefault()
                })
            .ToList();
            return Ok(billFromTransport);
        }
        
        //delivery to shipper 
        [HttpGet]
        [Route("receive/postoffice")]
        [Authorize(Policy = "RequirePostmanRole")]
        public IActionResult DlvFromPO()
        {
            var emp = HttpContext.User;
            
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            PostOffice postOffice = dbContext.PostOffices.Include(p => p.Ward).FirstOrDefault(p => p.id == poId);
            if (postOffice == null)
            {
                return NotFound("PostOffice not found.");
            }
            
            List<BillDTO> rcvBills = dbContext.Bills
                .Where(bill => bill.DeliveryAddress.Ward.district_id == postOffice.Ward.district_id &&
                               bill.deliveryType == "home"
                )
                .Select(bill => new
                {
                    Bill = bill,
                    LatestStatus = dbContext.Status
                        .Where(stt => stt.billId == bill.id)
                        .OrderByDescending(stt => stt.time)
                        .FirstOrDefault()
                })
                .Where(result => result.LatestStatus.StatusType.name == "Delivered to Post Office"
                )
                .Select(result => new BillDTO
                {
                    id = result.Bill.id,
                    billNumber = result.Bill.billNumber,
                    totalCharge = result.Bill.totalCharge,
                payer = result.Bill.payer,
                cod = result.Bill.cod,
                dateCreated = result.Bill.dateCreated,
                note = result.Bill.note,
                DeliveryAddressDto = dbContext.DeliveryAddresses
                    .Where(add => add.id == result.Bill.deliveryAddId)
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
                ShippingAddressDto = dbContext.ShippingAddresses
                    .Where(add => add.id == result.Bill.shippingAddId)
                    .Select(add => new ShippingAddressDTO()
                    {
                        name = add.name,
                        telephone = add.telephone,
                        address = add.address,
                        wardName = add.Ward.ward_name,
                        districtName = add.Ward.District.district_name,
                        provinceName = add.Ward.District.Province.province_name
                    })
                    .FirstOrDefault(),
                BillDetailsDto = dbContext.BillDetails
                    .Where(d => d.billId == result.Bill.id)
                    .Select(d => new BillDetailsDTO()
                    {
                        name = d.name,
                        weight = d.weight,
                        length = d.length,
                        width = d.width,
                        height = d.height,
                        nature = d.nature
                    })
                    .FirstOrDefault(),
                latestStatus = dbContext.Status
                    .Where(stt => stt.billId == result.Bill.id)
                    .OrderByDescending(stt => stt.time)
                    .Select(stt => new StatusDTO()
                    {
                        id = stt.id,
                        typeId = stt.StatusType.id,
                        name = stt.StatusType.name,
                        time = stt.time
                    })
                    .FirstOrDefault()
                })
            .ToList();
            return Ok(rcvBills);
        }
        
        //delivery to cnee
        [HttpGet]
        [Route("delivery")]
        [Authorize]
        public IActionResult DlvToCnee()
        {
            var emp = HttpContext.User;
            var role = emp.Claims.FirstOrDefault(r => r.Type == "Role")?.Value;
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            var username = emp.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value;
            PostOffice postOffice = dbContext.PostOffices.Include(p => p.Ward).FirstOrDefault(p => p.id == poId);
            if (postOffice == null)
            {
                return NotFound("PostOffice not found.");
            }

            if (role == "Postman")
            {
                List<BillDTO> dlvToCnee = dbContext.Bills
                    .Select(bill => new
                    {
                        Bill = bill,
                        LatestStatus = dbContext.Status
                            .Where(stt => stt.billId == bill.id)
                            .OrderByDescending(stt => stt.time)
                            .FirstOrDefault()
                    })
                    .Where(result => result.LatestStatus.Employee.username == username &&
                                     result.LatestStatus.StatusType.name == "Delivering to Consignee"
                    )
                    .Select(result => new BillDTO
                {
                    id = result.Bill.id,
                    billNumber = result.Bill.billNumber,
                    totalCharge = result.Bill.totalCharge,
                payer = result.Bill.payer,
                cod = result.Bill.cod,
                dateCreated = result.Bill.dateCreated,
                note = result.Bill.note,
                DeliveryAddressDto = dbContext.DeliveryAddresses
                    .Where(add => add.id == result.Bill.deliveryAddId)
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
                ShippingAddressDto = dbContext.ShippingAddresses
                    .Where(add => add.id == result.Bill.shippingAddId)
                    .Select(add => new ShippingAddressDTO()
                    {
                        name = add.name,
                        telephone = add.telephone,
                        address = add.address,
                        wardName = add.Ward.ward_name,
                        districtName = add.Ward.District.district_name,
                        provinceName = add.Ward.District.Province.province_name
                    })
                    .FirstOrDefault(),
                BillDetailsDto = dbContext.BillDetails
                    .Where(d => d.billId == result.Bill.id)
                    .Select(d => new BillDetailsDTO()
                    {
                        name = d.name,
                        weight = d.weight,
                        length = d.length,
                        width = d.width,
                        height = d.height,
                        nature = d.nature
                    })
                    .FirstOrDefault(),
                latestStatus = dbContext.Status
                    .Where(stt => stt.billId == result.Bill.id)
                    .OrderByDescending(stt => stt.time)
                    .Select(stt => new StatusDTO()
                    {
                        id = stt.id,
                        typeId = stt.StatusType.id,
                        name = stt.StatusType.name,
                        time = stt.time
                    })
                    .FirstOrDefault()
                })
            .ToList();
            return Ok(dlvToCnee);
            }
            else if (role == "Staff")
            {
                List<BillDTO> dlvToCnee = dbContext.Bills
                    .Where(bill => bill.DeliveryAddress.Ward.district_id == postOffice.Ward.district_id &&
                                   bill.deliveryType == "post-office"
                    )
                    .Select(bill => new
                    {
                        Bill = bill,
                        LatestStatus = dbContext.Status
                            .Where(stt => stt.billId == bill.id)
                            .OrderByDescending(stt => stt.time)
                            .FirstOrDefault()
                    })
                    .Where(result => result.LatestStatus.Employee.username == username &&
                                     result.LatestStatus.StatusType.name == "Delivered to Post Office"
                    )
                    .Select(result => new BillDTO
                {
                    id = result.Bill.id,
                    billNumber = result.Bill.billNumber,
                    totalCharge = result.Bill.totalCharge,
                payer = result.Bill.payer,
                cod = result.Bill.cod,
                dateCreated = result.Bill.dateCreated,
                note = result.Bill.note,
                DeliveryAddressDto = dbContext.DeliveryAddresses
                    .Where(add => add.id == result.Bill.deliveryAddId)
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
                ShippingAddressDto = dbContext.ShippingAddresses
                    .Where(add => add.id == result.Bill.shippingAddId)
                    .Select(add => new ShippingAddressDTO()
                    {
                        name = add.name,
                        telephone = add.telephone,
                        address = add.address,
                        wardName = add.Ward.ward_name,
                        districtName = add.Ward.District.district_name,
                        provinceName = add.Ward.District.Province.province_name
                    })
                    .FirstOrDefault(),
                BillDetailsDto = dbContext.BillDetails
                    .Where(d => d.billId == result.Bill.id)
                    .Select(d => new BillDetailsDTO()
                    {
                        name = d.name,
                        weight = d.weight,
                        length = d.length,
                        width = d.width,
                        height = d.height,
                        nature = d.nature
                    })
                    .FirstOrDefault(),
                latestStatus = dbContext.Status
                    .Where(stt => stt.billId == result.Bill.id)
                    .OrderByDescending(stt => stt.time)
                    .Select(stt => new StatusDTO()
                    {
                        id = stt.id,
                        typeId = stt.StatusType.id,
                        name = stt.StatusType.name,
                        time = stt.time
                    })
                    .FirstOrDefault()
                })
            .ToList();
            return Ok(dlvToCnee);
            }
            else
            {
                return BadRequest("Invalid role");
            }
        }
        
    }
    
}

