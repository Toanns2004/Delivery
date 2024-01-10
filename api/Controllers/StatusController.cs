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
    [Authorize]
    public class StatusController : Controller
    {
        private readonly DBContext dbContext;

        public StatusController(DBContext context)
        {
            dbContext = context;
        }

        [HttpPost]
        [Route("create")]
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

                    Employee employee = dbContext.Employees.Find(newStatusModel.employeeId);
                    if (employee == null)
                    {
                        return NotFound("Employee not found");
                    }

                    Bill bill = dbContext.Bills.Find(newStatusModel.billId);
                    if (bill == null)
                    {
                        return NotFound("Bill not found.");
                    }
                    
                    Status newStatus = new Status()
                    {
                        typeId = statusType.id,
                        employeeId = employee.id,
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
    }
}

