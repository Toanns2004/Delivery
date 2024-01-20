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
        
        
        //inbound report
        [HttpGet]
        [Route("inbound")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult InboundReport([FromQuery] string startDate, [FromQuery] string endDate)
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

            List<ReportDTO> reports = dbContext.Bills
                .Join(dbContext.Status,
                    bill => bill.id,
                    status => status.billId,
                    (bill, status) => new{Bill = bill, Status = status}
                    )
                .Where(rs => rs.Status.time >= startDateTime.Date &&
                                         rs.Status.time <= endDateTime.Date.AddDays(1) &&
                                         rs.Status.Employee.PostOffice.id == poId &&
                                         (rs.Status.StatusType.id == 3  || rs.Status.StatusType.id == 5))
                .Select(bill => new ReportDTO()
                {
                    billNumber = bill.Bill.billNumber,
                    rcvFrom = (bill.Status.StatusType.id == 3 & dbContext.Status.Any(stt => stt.billId == bill.Bill.id && stt.StatusType.id == 2)) ? "Shipper" : bill.Status.StatusType.id == 3 ? "Postman" : "Transport",
                    totalCharge = Math.Round(bill.Bill.totalCharge),
                    cod = bill.Bill.cod,
                    payBy = bill.Bill.payer,
                    collectFromShipper = Math.Round(bill.Bill.payer == "shipper" ? (bill.Bill.cod == 0 ? bill.Bill.totalCharge : Math.Max(0, bill.Bill.totalCharge - bill.Bill.cod)) : 0, 2),
                })
                .ToList();
            return Ok(reports);
        }
        
        //outbound report
        [HttpGet]
        [Route("outbound")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult OutboundReport([FromQuery] string startDate, [FromQuery] string endDate)
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

            List<ReportDTO> reports = dbContext.Bills
                .Join(
                    dbContext.Status,
                    bill => bill.id,
                    status => status.billId,
                    (bill, status) => new { Bill = bill, Status = status }
                )
                .Where(rs => rs.Status.time >= startDateTime &&
                             rs.Status.time <= endDateTime.Date.AddDays(1) &&
                             rs.Status.Employee.PostOffice.id == poId &&
                             (rs.Status.StatusType.id == 4 || rs.Status.StatusType.id == 7)
                )
                .Select(bill => new ReportDTO()
                {
                    billNumber = bill.Bill.billNumber,
                    dlvTo = bill.Status.StatusType.id == 4 ? "For Transport" : "To Consignee",
                    totalCharge = Math.Round(bill.Bill.totalCharge, 2),
                    cod = bill.Bill.cod,
                    payBy = bill.Bill.payer,
                    collectFromCnee = Math.Round(bill.Bill.payer != "shipper" ? (bill.Bill.totalCharge + bill.Bill.cod) : bill.Bill.cod, 2)
                })
                .ToList();
            return Ok(reports);
        }
        
        //revenue report
        [HttpGet]
        [Route("revenue")]
        [Authorize(Policy = "RequireStaffRole")]
        public IActionResult RevenueReport([FromQuery] string startDate, [FromQuery] string endDate)
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

            List<ReportDTO> reports = dbContext.Bills
                .Join(
                    dbContext.Status,
                    bill => bill.id,
                    status => status.billId,
                    (bill, status) => new { Bill = bill, Status = status }
                )
                .Where(
                    rs => rs.Status.time >= startDateTime &&
                          rs.Status.time <= endDateTime.Date.AddDays(1) &&
                          rs.Status.Employee.PostOffice.id == poId &&
                          (rs.Status.StatusType.id == 3 || rs.Status.StatusType.id == 7)
                )
                .Select(bill => new ReportDTO
                {
                    billNumber = bill.Bill.billNumber,
                    insFee = Math.Round(bill.Bill.insuranceFee, 2),
                    cod = bill.Status.StatusType.id == 7 ? bill.Bill.cod : 0,
                    collectFromShipper = Math.Round(bill.Bill.payer == "shipper" ? (bill.Bill.cod == 0 ? bill.Bill.totalCharge : Math.Max(0, bill.Bill.totalCharge - bill.Bill.cod)) : 0, 2),
                    chargeByCnee = Math.Round(bill.Bill.payer != "shipper" ? bill.Bill.totalCharge : 0, 2),
                    payForShipper = Math.Round(bill.Status.StatusType.id == 7 ? (bill.Bill.payer == "consignee" ? bill.Bill.cod : Math.Max(0, bill.Bill.cod - bill.Bill.totalCharge)) : 0, 2),
                    collectFromCnee = Math.Round(bill.Bill.payer != "shipper" ? (bill.Bill.totalCharge + bill.Bill.cod) : bill.Bill.cod, 2)
                })
                .ToList();
            return Ok(reports);
        }
    }
}

