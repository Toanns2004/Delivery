using api.Entities;

namespace api.DTOs;

public class EmployeeDTO
{
    public int id { get; set; }
    
    public string fullname { get; set; }
    
    public string username { get; set; }
    
    public string email { get; set; }
    
    public int roleId { get; set; }
    public string role { get; set; }
    
    public int postOfficeId { get; set; }
    
    public int districtId { get; set; }
    
    public int provinceId { get; set; }
    public string postOffice { get; set; }
    
    public Permission Permission { get; set; }
}