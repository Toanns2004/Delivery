using api.Entities;

namespace api.DTOs;

public class EmployeeDTO
{
    public int id { get; set; }
    
    public int fullname { get; set; }
    
    public string username { get; set; }
    
    public string email { get; set; }
    
    public int roleId { get; set; }
    
    public int postOfficeId { get; set; }
    
    public Permission Permission { get; set; }
}