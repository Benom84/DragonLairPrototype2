using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class Level {

    [XmlArray("Attacks")]
    [XmlArrayItem("Attack")]
    public List<Attack> attackList = new List<Attack>();

}
