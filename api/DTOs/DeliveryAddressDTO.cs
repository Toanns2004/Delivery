namespace api.DTOs;

public class DeliveryAddressDTO
{
    public int id { get; set; }
    public string name { get; set; }
    
    public string telephone { get; set; }
    
    public string address { get; set; }
    
    public int wardId { get; set; }
    public string wardName { get; set; }
    
    public string districtName { get; set; }
    
    public string provinceName { get; set; }
}