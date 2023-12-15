using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities;

public class Permission
{
    public int id { get; set; }
    
    public string name { get; set; }

    public int roleId
    {
        get;
        set;
    }
    [ForeignKey("roleId")]
    public Role Role { get; set; }
}