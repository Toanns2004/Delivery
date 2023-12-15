using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class Status
{
    public int id { get; set; }
    
    public string name { get; set; }
    
    public int employeeId { get; set; }
    [ForeignKey("employeeId")]
    public Employee Employee { get; set; }
    
    public int billId { get; set; }
    [ForeignKey("billId")]
    public Bill Bill { get; set; }
    
    public DateTime time { get; set; }
}