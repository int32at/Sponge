﻿using System.Xml.Serialization;
namespace Sponge.Configuration
{
    [XmlType("Item")]
    public class ConfigurationItem
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("Value")]
        public object Value { get; set; }
    }
}
