using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using InsanityBot.Util;
using InsanityBot.Util.FileMeta;
using InsanityBot.Util.FileMeta.Reference;

namespace InsanityBot
{
    public class Warnings
    {
        [Command("warn")]
        [Description("Warns an user.")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Warn(CommandContext ctx,
            
            [Description("The user to warn.")]
            DiscordMember User,
            
            [Description("Reason")]
            [RemainingText]
            String Reason = "[no reason given]")
        {
            User user = Serializer.Deserialize(User.Id);
            user.AddModlogEntry(ModlogType.warn, Reason);
            Serializer.Serialize(user, User.Id);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Wheat,
                Description = $"<:insanitybotSuccess:737297353316499566> {User.DisplayName} has been warned successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "WARN";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", User.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Reason", Reason, true);

            await ctx.Guild.GetChannel(693567857107402762).SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("verbalwarn")]
        [Aliases("vw")]
        [Description("Verbally warns an user and adds the warning to <#693567857107402762>, but does not add a new entry to the modlog file")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Verbalwarn(CommandContext ctx, 
            [Description("The user to clear modlogs from.")]
            DiscordMember User,
            
            [Description("Reason")]
            [RemainingText]
            String Reason)
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Wheat,
                Description = $"<:insanitybotSuccess:737297353316499566> {User.DisplayName} has been verbally warned successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "VERBAL WARN";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", User.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Reason", Reason, true);

            await ctx.Guild.GetChannel(693567857107402762).SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("clearmodlog")]
        [Aliases("clearwarnings", "clearwarns")]
        [Description("Clears the modlogs of a specific user")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task ClearModlog(CommandContext ctx,
            
            [Description("The user to clear modlogs from.")]
            DiscordMember User)
        {
            if(ctx.Member == User)
            {
                DiscordEmbedBuilder failure = new DiscordEmbedBuilder
                {
                    Description = $"<:insanitybotFailure:737297932595888189> You can't clear your own modlog!",
                    Color = DiscordColor.DarkRed,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "InsanityBot - Exa 2020"
                    }
                };

                await ctx.RespondAsync(embed: failure.Build());
                return;
            }

            User user = Serializer.Deserialize(User.Id);
            user.Modlog.Clear();
            user.ModlogCount = 0;
            Serializer.Serialize(user, User.Id);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"<:insanitybotSuccess:737297353316499566> {User.DisplayName}'s Modlogs have been cleared successfully.",
                Color = DiscordColor.SpringGreen,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "MODLOGS CLEARED";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", User.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);

            await ctx.Guild.GetChannel(693567857107402762).SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("modlog")]
        [Aliases("warns", "warnings", "infractions", "modlogs")]
        [Description("Displays all warnings of an user.")]
        public async Task Modlog(CommandContext ctx,
            [Description("The user to print modlogs of.")]
            DiscordUser member = null)
        {
            List<ModlogEntry> modlog;
            DiscordUser user = member;
            String Title = $"Modlog of {user.Username}";

            if (user == null)
            {
                user = ctx.Member;
                Title = $"Modlog of {Serializer.Deserialize(user.Id).Username}";
            }
            modlog = Serializer.Deserialize(user.Id).Modlog;

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = Title,
                Color = DiscordColor.DarkRed,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };

            if(modlog.Count == 0)
            {
                embedBuilder.Color = DiscordColor.SpringGreen;
                embedBuilder.Description = "<:success:737297371645345832> This user has no modlog entries.";
                await ctx.RespondAsync(embed: embedBuilder.Build());
                return;
            }

            foreach (var item in modlog)
                embedBuilder.Description += $"{item.Type.ToString().ToUpper()}: {item.Time} - {item.Reason}\n";

            await ctx.RespondAsync(embed: embedBuilder.Build());
        }

        [Command("unwarn")]
        [Description("Removes a warn by index")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Unwarn(CommandContext ctx,
            
            [Description("Member to unwarn")]
            DiscordMember member,
            
            [Description("1-based warn index")]
            UInt16 WarnID)
        {
            if (ctx.Member == member)
            {
                DiscordEmbedBuilder failure = new DiscordEmbedBuilder
                {
                    Description = $"<:insanitybotFailure:737297932595888189> You can't remove your own modlog entries!",
                    Color = DiscordColor.DarkRed,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "InsanityBot - Exa 2020"
                    }
                };

                await ctx.RespondAsync(embed: failure.Build());
                return;
            }


            User user = Serializer.Deserialize(member.Id);
            ModlogEntry modlogEntry = user.Modlog[WarnID - 1];
            user.Modlog.RemoveAt(WarnID - 1);
            Serializer.Serialize(user, member.Id);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SpringGreen,
                Description = $"<:insanitybotSuccess:737297353316499566> {member.DisplayName}'s warning #{WarnID} was removed successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "UNWARN";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", member.DisplayName, true)
                .AddField("Moderator", ctx.Member.DisplayName, true)
                .AddField("Warning", $"{modlogEntry.Type.ToString().ToUpper()}: {modlogEntry.Time} - {modlogEntry.Reason}");

            await ctx.Guild.GetChannel(693567857107402762).SendMessageAsync(embed: embedBuilder.Build());
        }

#if DEBUG
        [Command("init")]
        [Hidden]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task Init(CommandContext ctx)
        {
            List<ExperiencePacket> packets = new List<ExperiencePacket>();

            foreach(var m in await ctx.Guild.GetAllMembersAsync())
            {
                packets.Add(new ExperiencePacket
                {
                    Experience = 0,
                    Level = 0,
                    UserID = m.Id
                });
            }

            FileStream file = new FileStream("./data/experience.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<ExperiencePacket>));

            serializer.Serialize(file, packets);
            file.Close();
            Program.discord.DebugLogger.LogMessage(LogLevel.Info, "InsanityBot", "initialized xp", DateTime.Now);
        }
#endif
    }
}
