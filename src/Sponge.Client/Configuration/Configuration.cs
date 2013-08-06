using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Sponge.Client.Configuration
{
    [XmlRoot("Configuration")]
    public class Configuration
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("SpongeUrl")]
        public string SpongeUrl { get; set; }

        [XmlElement("IsOnline")]
        public bool IsOnline { get; set; }

        [XmlArray("ConfigurationItems")]
        [XmlArrayItem("ConfigurationItem")]
        public List<ConfigurationItem> Items;

        public Configuration(string name, string spongeUrl, bool isOnline, IEnumerable<ConfigurationItem> items)
        {
            Name = name;
            SpongeUrl = spongeUrl;
            IsOnline = IsOnline;
            Items = items.ToList();
        }

        public Configuration()
        {
        }

        public T Get<T>(string key)
        {
            if (Items != null && Items.Count > 0)
            {
                var result = Items.Single(i => i.Key.Equals(key));

                if (result == null)
                    throw new Exception(string.Format("No Item with Key '{0}' found", key));

                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(result.Value);
            }
            else
                throw new Exception("Configuration does not contain any items");
        }
    }
}
