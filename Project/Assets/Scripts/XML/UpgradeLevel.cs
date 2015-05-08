using System.Xml.Serialization;


public class UpgradeLevel
{
    public int ID { get; set; } // Unique Id 
    public int Cost { get; set; } // The cost of this upgrade
    public float Data { get; set; } // the upgrade data
    public string Name { get; set; } // name of the upgrade
    public string Description { get; set; } // description of upgrade
}
