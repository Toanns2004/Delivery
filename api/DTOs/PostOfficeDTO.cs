namespace api.DTOs;

public class PostOfficeDTO
{
    public int id { get; set; }
    
    public int wardId { get; set; }
    
    public string postCode { get; set; }
    
    public string postName { get; set; }
    
    public string address { get; set; }

    public string latitude { get; set; }
    
    public string longtitude { get; set; }
}