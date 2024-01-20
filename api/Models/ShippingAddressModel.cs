using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class ShippingAddressModel
{
    public int id { get; set; }
    
    [Required(ErrorMessage = "Shipper name is required.")]
    [MinLength(6, ErrorMessage = "Name must be at least 6 characters.")]
    public string name { get; set; }
    
    [Required(ErrorMessage = "Telephone number is required.")]
    public string telephone { get; set; }
    
    [Required(ErrorMessage = "User ID is required.")]
    public int userId { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    [MinLength(10, ErrorMessage = "Address must be at least 10 characters.")]
    public string address { get; set; }
    
    [Required(ErrorMessage = "Ward ID is required.")]
    public int wardId { get; set; }
    
}