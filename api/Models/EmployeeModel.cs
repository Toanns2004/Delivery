using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class EmployeeModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [MinLength(6, ErrorMessage = "Name must be at least 6 characters.")]
    public string fullname { get; set; }
    
    [Required(ErrorMessage = "Username is required.")]
    public string username { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Incorrect email.")]
    public string email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string password { get; set; }
    
    [Required(ErrorMessage = "Role ID is required.")]
    public int roleId { get; set; }
    
    [Required(ErrorMessage = "Post office ID is required.")]
    public int postOfficeId { get; set; }
    
}