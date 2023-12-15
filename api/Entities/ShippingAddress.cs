using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class ShippingAddress
{
    public int id { get; set; }
    
    public string name { get; set; }
    
    public string telephone { get; set; }
    
    public int userId { get; set; }
    [ForeignKey("userId")]
    public User User { get; set; }
    
    public string address { get; set; }
    
    public int wardId { get; set; }
    [ForeignKey("wardId")]
    public Ward Ward { get; set; }
}