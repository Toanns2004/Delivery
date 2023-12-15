using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class PostOffice
{
    public int id { get; set; }
    
    public int wardId { get; set; }
    [ForeignKey("wardId")]
    public Ward Ward
    {
        get;
        set;
    }
    
    public string postCode { get; set; }
    
    public string postName { get; set; }
    
    public string address { get; set; }

    public string latitude
    {
        get;
        set;
    }
    
    public string longtitude { get; set; }
}