using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Upgrade")]
public class Upgrade
{
    [XmlArray("Upgrade_Levels")]
    [XmlArrayItem("Upgrade_Level")]
    public List<UpgradeLevel> upgradeLevels = new List<UpgradeLevel>();

}
