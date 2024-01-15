namespace api.Entities;

public class UnitPrice
{
    public int id { get; set; }
    
    public string range { get; set; }
    
    public int minWeight { get; set; }
    
    public int maxWeight { get; set; }
    
    public double chargeRate { get; set; }
}