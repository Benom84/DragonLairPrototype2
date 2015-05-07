using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class Level {

    public float LifeToMakeItHarder = 1.01f;
    
    [XmlArray("Attacks")]
    [XmlArrayItem("Attack")]
    public List<Attack> attackList = new List<Attack>();

}
