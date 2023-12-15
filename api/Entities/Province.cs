using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class Province
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int id { get; set; }
    
    public string province_code { get; set; }
    
    public string province_name { get; set; }
    
    public string value { get; set; }
}