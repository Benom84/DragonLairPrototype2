using System.Collections.Generic;
using System.Xml.Serialization;


public class Wave {

    public class Attacker
    {
        public string Type { get; set; }
        public int Position { get; set; }
    }

    public int Id { get; set; } // Unique Id if we want to use a specific wave
    public int Difficulty { get; set; } // The difficulty of this wave
    public int FromLevel { get; set; } // From which level is this wave possible
    [XmlArray("Attackers")]
    [XmlArrayItem("Attacker")]
    public List<Attacker> Attackers = new List<Attacker>();
}
