using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class Bill
{
    public int id { get; set; }
    
    public int userId { get; set; }
    [ForeignKey("userId")]
    public User User { get; set; }
    
    public int shippingAddId { get; set; }
    
    public int deilveryAddId { get; set; }
    
    public int unitPriceId { get; set; }
    [ForeignKey("unitPriceId")]
    public UnitPrice UnitPrice { get; set; }
    
    public double charge { get; set; }
    
    public string pickupType { get; set; }
    
    public string deliverType { get; set; }
    
    public double insuranceFee { get; set; }
    
    public double totalCharge { get; set; }
    
    public string payer { get; set; }
    
    public string note { get; set; }
    
    public double cod { get; set; }
}