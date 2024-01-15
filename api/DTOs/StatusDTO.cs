namespace api.DTOs;

public class StatusDTO
{
    public int id { get; set; }
    
    public int typeId { get; set; }
    
    public string name { get; set; }
    
    public DateTime time { get; set; }
    
    public string postOffice { get; set; }
}