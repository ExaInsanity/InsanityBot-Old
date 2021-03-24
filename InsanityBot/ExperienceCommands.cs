using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using InsanityBot.Util.FileMeta.Reference;

namespace InsanityBot
{
    public partial class Experience
    {
        [Command("rank")]
        [Aliases("level")]
        [Description("Returns the member rank")]
        public async Task Rank(CommandContext ctx,
            
            [Description("The user whose rank you try to access. Defaults to yourself.")]
            DiscordMember member = null)
        {
            DiscordMember user;

            if (member != null)
                user = member;
            else
                user = ctx.Member;

            FileStream reader = new FileStream("./data/experience.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<ExperiencePacket>));
            List<ExperiencePacket> experience = (List<ExperiencePacket>)serializer.Deserialize(reader);

            var userXP = (from e in experience
                          where e.UserID == user.Id
                          select e).ToList();

            if(userXP.Count == 0)
            {
                userXP.Add(new ExperiencePacket
                {
                    Level = 0,
                    Experience = 0,
                    UserID = 0
                });
            }

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{user.DisplayName}",
                Color = DiscordColor.Chartreuse,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };

            embedBuilder.AddField("Rank", (experience.FindIndex(xm => xm == userXP[0]) + 1).ToString(), true);
            embedBuilder.AddField("Level", userXP[0].Level.ToString(), true);
            embedBuilder.AddField("Experience", userXP[0].Experience.ToString(), true);

            await ctx.RespondAsync(embed: embedBuilder.Build());
            reader.Close();
        }

        [Command("leaderboard")]
        [Aliases("lb")]
        [Description("Returns the first ten members on the leaderboard")]
        public async Task Leaderboard(CommandContext ctx)
        {
            FileStream reader = new FileStream("./data/experience.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<ExperiencePacket>));
            List<ExperiencePacket> experiences = (List<ExperiencePacket>)serializer.Deserialize(reader);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Chartreuse,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Title = "Insanity Network Leaderboard"
            };

            var top10 = experiences.GetRange(0, 10);
            for (Int32 i = 1; i < 11; i++)
            {
                try
                {
                    embedBuilder.Description += $"#{i} - {(await ctx.Guild.GetMemberAsync(top10[i - 1].UserID)).Mention} " +
                        $"Level: {top10[i - 1].Level}, XP: {top10[i - 1].Experience}\n";
                }
                catch(Exception e)
                {
                    i--;
                    Program.discord.DebugLogger.LogMessage(LogLevel.Critical, "InsanityBot", $"{e.GetType()} - {e.Message}", DateTime.UtcNow);
                }
            }

            await ctx.RespondAsync(embed: embedBuilder.Build());
            reader.Close();
        }
    }
}
