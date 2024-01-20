using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class EmpLoginModel
{
    [Required(ErrorMessage = "Username is required.")]
    public string username { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string password { get; set; }
}