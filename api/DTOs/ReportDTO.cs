namespace api.DTOs;

public class ReportDTO
{
    public string billNumber { get; set; }
    
    public string rcvFrom { get; set; }
    
    public string dlvTo { get; set; }
    
    
    public double totalCharge { get; set; }
    
    public double cod { get; set; }
    
    public string payBy { get; set; }
    
    public double collectFromShipper { get; set; }
    
    public double chargeByCnee { get; set; }
    
    public double insFee { get; set; }
    
    public double payForShipper { get; set; }
    
    public double collectFromCnee { get; set; }
    
}