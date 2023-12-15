using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class Employee
{
    public int id { get; set; }
    
    public int fullname { get; set; }
    
    public string username { get; set; }
    
    public string email { get; set; }
    
    public int roleId { get; set; }
    [ForeignKey("roleId")]
    public Role Role { get; set; }
    
    public int postOfficeId { get; set; }
    [ForeignKey("postOfficeId")]
    public PostOffice PostOffice { get; set; }
}