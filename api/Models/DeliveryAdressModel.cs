using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class DeliveryAdressModel
{
    public int id { get; set; }
    
    [Required(ErrorMessage = "Shipper name is required.")]
    public string name { get; set; }
    
    [Required(ErrorMessage = "Telephone number is required.")]
    public string telephone { get; set; }
    
    [Required(ErrorMessage = "User ID is required.")]
    public int userId { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    public string address { get; set; }
    
    [Required(ErrorMessage = "Ward ID is required.")]
    public int wardId { get; set; }
}