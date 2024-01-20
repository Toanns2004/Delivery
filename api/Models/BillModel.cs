using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class BillModel
{
    [Required(ErrorMessage = "User ID is required.")]
    public int userId { get; set; }
    
    [Required(ErrorMessage = "Shipping Address ID is required.")]
    public int shippingAddId { get; set; }
    
    [Required(ErrorMessage = "Delivery address ID is required.")]
    public int deliveryAddId { get; set; }
    
    [Required(ErrorMessage = "Pickup type is required.")]
    public string pickupType { get; set; }
    
    [Required(ErrorMessage = "Delivery type is required.")]
    public string deliveryType { get; set; }
    
    [Required(ErrorMessage = "Payer is required.")]
    public string payer { get; set; }
    
    [MinLength(10, ErrorMessage = "Note must be at least 10 characters")]
    public string note { get; set; }
    
    public double cod { get; set; }
    
    [Required(ErrorMessage = "Commodity name is required.")]
    [MinLength(5, ErrorMessage = "Commodity name must be at least 5 characters.")]
    public string name { get; set; }
    
    [Required(ErrorMessage = "Nature of goods is required.")]
    public string nature { get; set; }
    
    [Required(ErrorMessage = "Weight is required.")]
    public double weight { get; set; }
    
    [Required(ErrorMessage = "Length is required.")]
    public int length { get; set; }
    
    [Required(ErrorMessage = "Width is required.")]
    public int width { get; set; }
    
    [Required(ErrorMessage = "Height is required.")]
    public int height { get; set; }
    
    public double value { get; set; }
}