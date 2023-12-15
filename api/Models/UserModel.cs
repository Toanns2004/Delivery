using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class UserModel
{
    
    [Required(ErrorMessage = "Full name is required.")]
    public string fullname { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Telephone number is required.")]
    public string telephone { get; set; }
    
    //[Required(ErrorMessage = "Address is required.")]
    [StringLength(255, MinimumLength = 0, ErrorMessage = "Address should not exceed 255 characters.")]
    public string address { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string password { get; set; }
}