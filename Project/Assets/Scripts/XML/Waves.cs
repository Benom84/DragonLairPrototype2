using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("WavesCollection")]
public class Waves {

    [XmlArray("Waves")]
    [XmlArrayItem("Wave")]
    public List<Wave> WavesList = new List<Wave>();
}
