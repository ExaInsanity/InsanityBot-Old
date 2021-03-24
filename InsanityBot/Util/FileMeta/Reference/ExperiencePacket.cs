 using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InsanityBot.Util.FileMeta.Reference
{
    [XmlRoot("UserExperience")]
    public class ExperiencePacket
    {
        [XmlAttribute]
        public UInt64 UserID { get; set; }

        [XmlAttribute]
        public UInt64 Level { get; set; }

        [XmlAttribute]
        public UInt64 Experience { get; set; }

        public static ExperiencePacket operator +(ExperiencePacket basePacket, ExperiencePacket addition)
        {
            ExperiencePacket returnValue = basePacket;
            returnValue.Level += addition.Level;
            returnValue.Experience += addition.Experience;
            return UpdateExperienceLevel(returnValue);
        }

        public static ExperiencePacket UpdateExperienceLevel(ExperiencePacket packet)
        {
            ExperiencePacket returnValue = packet;
            if(packet.Level < 16)
            {
                if (packet.Experience >= (2 * packet.Level) + 7)
                {
                    returnValue.Level++;
                    returnValue.Experience = 0;
                    LeveledUpEvent(packet.UserID, returnValue.Level);
                }
            }
            else if(packet.Level < 32)
            {
                if (packet.Experience >= (5 * packet.Level) - 39)
                {
                    returnValue.Level++;
                    returnValue.Experience = 0;
                    LeveledUpEvent(packet.UserID, returnValue.Level);
                }
            }
            else
            {
                if(packet.Experience >= (7 * packet.Level) - 159)
                {
                    returnValue.Level++;
                    returnValue.Experience = 0;
                    LeveledUpEvent(packet.UserID, returnValue.Level);
                }
            }

            return returnValue;
        }
        public static event Func<UInt64, UInt64, Task> LeveledUpEvent;
    }
}
