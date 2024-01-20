using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class UserModel
{
    
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Full name must be at least 6 characters.")]
    public string fullname { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Incorrect email.")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Telephone number is required.")]
    public string telephone { get; set; }
    
    //[Required(ErrorMessage = "Address is required.")]
    [StringLength(255, MinimumLength = 0, ErrorMessage = "Address should not exceed 255 characters.")]
    public string address { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string password { get; set; }
}