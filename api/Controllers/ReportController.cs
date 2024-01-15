using api.Context;
using api.DTOs;
using api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("/report")]
    public class ReportController: Controller
    {
        private readonly DBContext dbContext;

        public ReportController(DBContext context)
        {
            dbContext = context;
        }
        
        //admin report
        
        
        //staff report
        [HttpGet]
        [Route("staff")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult StaffReport([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var emp = HttpContext.User;
            
            var poId = int.Parse(emp.Claims.FirstOrDefault(p => p.Type == "PostOffice")?.Value);
            PostOffice postOffice = dbContext.PostOffices.Include(p => p.Ward).FirstOrDefault(p => p.id == poId);
            if (postOffice == null)
            {
                return NotFound("PostOffice not found.");
            }
            
            if (!DateTime.TryParse(startDate, out DateTime startDateTime) || !DateTime.TryParse(endDate, out DateTime endDateTime))
            {
                return BadRequest("Invalid date format");
            }

            List<BillDTO> bills = dbContext.Bills
                .Join(dbContext.Status,
                    bill => bill.id,
                    status => status.billId,
                    (bill, status) => new{Bill = bill, Status = status}
                    )
                .Where(rs => rs.Status.time >= startDateTime.Date &&
                                         rs.Status.time <= endDateTime.Date.AddDays(1) &&
                                         rs.Status.Employee.PostOffice.id == poId &&
                                         (rs.Status.StatusType.id == 3 || rs.Status.StatusType.id == 5))
                .Select(bill => new BillDTO()
                {
                    billNumber = bill.Bill.billNumber
                })
                .ToList();
            return Ok(bills);
        }
    }
}

