using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class District
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int id { get; set; }
    
    public string district_name { get; set; }
    
    public int province_id
    {
        get;
        set;
    }
    [ForeignKey("province_id")]
    public Province Province { get; set; }
    
    public string value { get; set; }
}