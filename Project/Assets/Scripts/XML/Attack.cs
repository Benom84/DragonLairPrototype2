using System.Xml.Serialization;


public class Attack
{

    public int Id { get; set; } // Unique Id if we want to use a specific wave
    public int Difficulty { get; set; } // The difficulty of this wave
    public float Time { get; set; } // At what time after the start of the level to perform this attack
    public float ChangeChance { get; set; } // What are the chances this attack will be switched with another
}
