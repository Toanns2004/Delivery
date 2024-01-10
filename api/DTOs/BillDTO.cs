using api.Entities;

namespace api.DTOs;

public class BillDTO
{
    public int id { get; set; }
    
    public string billNumber { get; set; }
    
    public int userId { get; set; }
    
    public DeliveryAddressDTO DeliveryAddressDto { get; set; }
    
    public ShippingAddressDTO ShippingAddressDto { get; set; }
    
    
    public double charge { get; set; }
    
    public string pickupType { get; set; }
    
    public string deliveryType { get; set; }
    
    public double insuranceFee { get; set; }
    
    public double totalCharge { get; set; }
    
    public string payer { get; set; }
    
    public string note { get; set; }
    
    public double cod { get; set; }
    
    public BillDetailsDTO BillDetailsDto { get; set; }
        
    public DateTime dateCreated { get; set; }
    
    public StatusDTO latestStatus { get; set; }
}