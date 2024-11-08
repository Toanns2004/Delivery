using api.Context;
using api.DTOs;
using api.Entities;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/status")]
    public class StatusController : Controller
    {
        private readonly DBContext dbContext;
         
        public StatusController(DBContext context)
        {
            dbContext = context;
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Create(StatusModel newStatusModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StatusType statusType = dbContext.StatusTypes.Find(newStatusModel.typeId);
                    if (statusType == null)
                    {
                        return NotFound("Status type not found.");
                    }

                    

                    Bill bill = dbContext.Bills.Find(newStatusModel.billId);
                    if (bill == null)
                    {
                        return NotFound("Bill not found.");
                    }
                    
                    Employee employee = dbContext.Employees.Find(newStatusModel.employeeId);
                    if (employee == null)
                    {
                        return NotFound("Employee not found");
                    }
                    
                    Status newStatus = new Status()
                    {
                        typeId = statusType.id,
                        employeeId = newStatusModel.employeeId,
                        billId = bill.id,
                        time = DateTime.Now
                    };
                    dbContext.Add(newStatus);
                    dbContext.SaveChanges();
                    return Created("", new StatusDTO()
                    {
                        name = newStatus.StatusType.name
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Create new status error.");
        }
        //cancel stt
        [HttpPost]
        [Route("cancel")]
        [Authorize]
        public IActionResult Cancel(StatusModel newStatusModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StatusType statusType = dbContext.StatusTypes.Find(newStatusModel.typeId);
                    if (statusType == null)
                    {
                        return NotFound("Status type not found.");
                    }
                    
                    Bill bill = dbContext.Bills.Find(newStatusModel.billId);
                    if (bill == null)
                    {
                        return NotFound("Bill not found.");
                    }
                    
                    
                    Status newStatus = new Status()
                    {
                        typeId = 8,
                        billId = bill.id,
                        time = DateTime.Now
                    };
                    dbContext.Add(newStatus);
                    dbContext.SaveChanges();
                    return Created("", new StatusDTO()
                    {
                        name = newStatus.StatusType.name
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Create new status error.");
        }
        
        //get shipment status
        [HttpGet]
        [Route("shipment/{bn}")]
        public IActionResult StatusByShipment(string bn)
        {
            var id = dbContext.Bills
                .FirstOrDefault(b => b.billNumber == bn)
                .id;
            List<StatusDTO> status = dbContext.Status
                .Where(stt => stt.billId == id)
                .OrderBy(stt => stt.time)
                .Select(stt => new StatusDTO()
                {
                    id = stt.id,
                    time = stt.time,
                    name = stt.StatusType.name
                })
                .ToList();
            return Ok(status);
        }
    }
}

