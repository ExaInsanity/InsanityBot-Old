using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace InsanityBot.Util.FileMeta.Reference
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct ModlogEntry
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        [XmlAttribute]
        public ModlogType Type { get; set; }

        [XmlAttribute]
        public String Reason { get; set; }

        [XmlAttribute]
        public DateTime Time { get; set; }

        public static ModlogEntry CreateNew(ModlogType type, String reason)
        {
            return new ModlogEntry
            {
                Type = type,
                Reason = reason,
                Time = DateTime.UtcNow
            };
        }
    }
}
