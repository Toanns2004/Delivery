using api.Entities;

namespace api.DTOs;

public class BillDTO
{
    public int id { get; set; }
    
    public string billNumber { get; set; }
    
    public int userId { get; set; }
    
    public int shippingAddId { get; set; }
    
    public int deilveryAddId { get; set; }
    
    public DeliveryAddressDTO DeliveryAddressDto { get; set; }
    
    public int unitPriceId { get; set; }
    
    public double charge { get; set; }
    
    public string pickupType { get; set; }
    
    public string deliveryType { get; set; }
    
    public double insuranceFee { get; set; }
    
    public double totalCharge { get; set; }
    
    public string payer { get; set; }
    
    public string note { get; set; }
    
    public double cod { get; set; }
    
    public string name { get; set; }
    
    public string nature { get; set; }
    
    public double weight { get; set; }
    
    public int length { get; set; }
    
    public int width { get; set; }
    
    public int height { get; set; }
    
    public double value { get; set; }
    
    public DateTime dateCreated { get; set; }
}