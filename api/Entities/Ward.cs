using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class Ward
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int id { get; set; }
    
    public string ward_name { get; set; }

    public int district_id
    {
        get;
        set;
    }
    [ForeignKey("district_id")]
    public District District { get; set; }
    
    public string location_code { get; set; }
}