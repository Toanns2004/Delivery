using System.Security.Claims;
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
        
        //bill by status
        [HttpGet]
        [Route("user")]
        [Authorize]
        public IActionResult UserReport([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var user = HttpContext.User;

            var userId = int.Parse(user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value);

            if (userId == null)
            {
                return NotFound("User not found.");
            }
            
            if (!DateTime.TryParse(startDate, out DateTime startDateTime) || !DateTime.TryParse(endDate, out DateTime endDateTime))
            {
                return BadRequest("Invalid date format");
            }

            List<Status> status = dbContext.Status
                .Where(stt => stt.Bill.userId == userId && stt.Bill.dateCreated >= startDateTime && stt.Bill.dateCreated < endDateTime.Date.AddDays(1))
                .GroupBy(stt => stt.Bill.id)
                .Select(stt => stt.OrderByDescending(t => t.time).FirstOrDefault())
                .ToList();

            var reportByType = status
                .GroupBy(stt => stt.typeId)
                .Select(rs => new
                {
                    StatusType = rs.Key,
                    Total = rs.Count(),
                    Status = status.Where(stt => stt.typeId == rs.Key)
                })
                .ToList();

            var reportByStt = reportByType
                .SelectMany(rs => rs.Status)
                .SelectMany(stt => dbContext.Status.Where(status => status.id == stt.id))
                .ToList();

            var reportBill = reportByStt
                .Select(rp => new
                {
                    typeId = rp.typeId,
                    status = dbContext.StatusTypes.Where(type => type.id == rp.typeId).FirstOrDefault().name,
                    cod = dbContext.Bills.Where(b => b.id == rp.billId).FirstOrDefault().cod,
                    payer = dbContext.Bills.Where(b => b.id == rp.billId).FirstOrDefault().payer,
                    chagre = dbContext.Bills.Where(b => b.id == rp.billId).FirstOrDefault().totalCharge
                })
                .ToList();
            var totalBillsByStatus = reportBill
                .GroupBy(rp => rp.typeId)
                .Select(group => new
                {
                    StatusType = group.Key,
                    StatusName = group.FirstOrDefault()?.status,
                    TotalBills = group.Count(),
                    TotalCod = group.Sum(rp => rp.cod),
                    TotalCharge = Math.Round(group.Sum(rp => rp.payer == "shipper" ? rp.chagre : 0), 2)
                })
                .ToList();
            List < UserReportDTO > reports = totalBillsByStatus
                .Select(rp => new UserReportDTO()
                {
                    status = rp.StatusName,
                    totalBills = rp.TotalBills,
                    totalCod = rp.TotalCod,
                    totalCharge = rp.TotalCharge
                })
                .ToList();

            // List < UserReportDTO > reports = dbContext.Bills
            //     .Where(bill => bill.dateCreated >= startDateTime && bill.dateCreated < endDateTime.Date.AddDays(1) && bill.userId == userId)
            //     .Join(
            //         dbContext.Status,
            //         bill => bill.id,
            //         status => status.billId,
            //         (bill, status) => new { Bill = bill, Status = status }
            //     )
            //     .Select(bill => new
            //     {
            //         Bill = bill,
            //         LatestStatus = dbContext.Status
            //         .Where(stt => stt.billId == bill.Bill.id)
            //         .OrderByDescending(stt => stt.time)
            //         .FirstOrDefault()
            //     })
            //     .GroupBy(rs => rs.Bill.Status.StatusType)
            //     .OrderBy(group => group.Key.id)
            //     .Select(rs => new UserReportDTO()
            //     {
            //         status = rs.Key.name,
            //         totalBills = rs.Count(),
            //         totalCod = rs.Sum(i => i.Bill.Bill.cod),
            //         totalCharge = Math.Round(rs.Sum(i => i.Bill.Bill.payer == "shipper" ? i.Bill.Bill.totalCharge : 0), 2),
            //     })
            //     .ToList();
            
            return Ok(reports);
        }
    }
}

