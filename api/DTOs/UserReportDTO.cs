using api.Entities;

namespace api.DTOs;

public class UserReportDTO
{
    public string status { get; set; }
    
    public int totalBills { get; set; }
    
    public double totalCod { get; set; }
    
    public double totalCharge { get; set; }
}