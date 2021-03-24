using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Xml.Serialization;

using DSharpPlus;
using DSharpPlus.Entities;

using InsanityBot.Util.FileMeta.Reference;
using System.IO;
using System.Xml.Linq;

namespace InsanityBot
{
    public partial class Experience
    {
        public static List<ExperiencePacket> update { get; set; }

        public static Timer UpdateTimer { get; set; }

        public static void Initialize()
        {
            UpdateTimer = new Timer
            {
                AutoReset = true,
                Interval = 16000
            };
            UpdateTimer.Elapsed += UpdateExperience;
            UpdateTimer.Start();

            ExperiencePacket.LeveledUpEvent += ManageLevelUp;
            update = new List<ExperiencePacket>();

            if (!File.Exists("./data/experience.xml"))
                File.Create("./data/experience.xml");
        }

        private static async Task ManageLevelUp(UInt64 arg1, UInt64 arg2)
        {
            DiscordMember member = await Program.insanitynetworkguild.GetMemberAsync(arg1);
            switch (arg2)
            {
                case 10:
                    await member.GrantRoleAsync(Program.insanitynetworkguild.GetRole(693768461159104643));
                    break;
                case 20:
                    await member.GrantRoleAsync(Program.insanitynetworkguild.GetRole(693769000349466684));
                    break;
                case 30:
                    await member.GrantRoleAsync(Program.insanitynetworkguild.GetRole(693769068213174314));
                    break;
                case 40:
                    await member.GrantRoleAsync(Program.insanitynetworkguild.GetRole(693769193736110110));
                    break;
                case 60:
                    await member.GrantRoleAsync(Program.insanitynetworkguild.GetRole(693769342268997643));
                    break;
                case 80:
                    await member.GrantRoleAsync(Program.insanitynetworkguild.GetRole(693769458765791293));
                    break;
            }

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = member.DisplayName,
                Description = $"**Congratulations!**\nYou are now Level {arg2}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Color = DiscordColor.Aquamarine
            };
            await Program.insanitynetworkguild.GetChannel(693568872124776569).SendMessageAsync(embed: embedBuilder.Build());
        }

        private static void UpdateExperience(Object sender, ElapsedEventArgs e)
        {
            String Path = "./data/experience.xml";
            FileStream reader = new FileStream(Path, FileMode.Open);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ExperiencePacket>));

                List<ExperiencePacket> experiencePackets = (List<ExperiencePacket>)serializer.Deserialize(reader);

                reader.SetLength(0);
                reader.Flush();


                foreach (var v in update)
                {
                    List<ExperiencePacket> toUpdate = (from p in experiencePackets
                                                       where p.UserID == v.UserID
                                                       select p).ToList();

                    experiencePackets.Remove(toUpdate[0]);
                    toUpdate[0] += v;
                    experiencePackets.Add(toUpdate[0]);

                }

                experiencePackets = experiencePackets.OrderByDescending(xm => xm.Level)
                    .ThenByDescending(xm => xm.Experience)
                    .ToList();

                serializer.Serialize(reader, experiencePackets);
                update.Clear();
            }
            catch (Exception ex)
            {
                Program.discord.DebugLogger.LogMessage(LogLevel.Critical, "InsanityBot", $"{ex.GetType()}: {ex.Message}", DateTime.Now);
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
