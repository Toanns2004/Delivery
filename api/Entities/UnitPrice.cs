namespace api.Entities;

public class UnitPrice
{
    public int id { get; set; }
    
    public string range { get; set; }
    
    public string weightLimit { get; set; }
    
    public double chargeRate { get; set; }
}