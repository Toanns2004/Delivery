namespace api.Entities;

public class UnitPrice
{
    public int id { get; set; }
    
    public string name { get; set; }
    
    public double weightLimit { get; set; }
    
    public double chargeRate { get; set; }
}