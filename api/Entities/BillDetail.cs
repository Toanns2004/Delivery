using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class BillDetail
{
    public int id { get; set; }

    public int billId { get; set; }
    [ForeignKey("billId")]
    public Bill Bill { get; set; }
    
    public string name { get; set; }
    
    public string nature { get; set; }
    
    public double weight { get; set; }
    
    public int length { get; set; }
    
    public int width { get; set; }
    
    public int height { get; set; }
    
    public double value { get; set; }
}