using api.Entities;

namespace api.DTOs;

public class ShippingAddressDTO
{
    public int id { get; set; }
    public string name { get; set; }
    
    public string telephone { get; set; }
    public string userName { get; set; }
    public string wardName { get; set; }
}