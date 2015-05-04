using System.Xml.Serialization;


public class UpgradeLevel
{

    public int ID { get; set; } // Unique Id 
    public int Cost { get; set; } // The difficulty of this wave
    public float Data { get; set; } // At what time after the start of the level to perform this attack
}
