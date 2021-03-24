using System;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using InsanityBot.Util;
using InsanityBot.Util.FileMeta;
using InsanityBot.Util.FileMeta.Reference;

namespace InsanityBot
{
    public class ModCommands
    {
        #region mute
        [Command("mute")]
        [Description("Mutes a member by for a given reason.")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Mute(CommandContext ctx,
            
            [Description("User to mute")]
            DiscordMember user,
            
            [Description("Reason for the mute")]
            [RemainingText]
            String reason = "[no reason given]")
        {
            await ctx.Guild.GrantRoleAsync(user, ctx.Guild.GetRole(706591279227600936));

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"<:insanitybotSuccess:737297353316499566> {user.DisplayName} has been muted successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Color = DiscordColor.Orange
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            User modlogUser = Serializer.Deserialize(user.Id);
            modlogUser.AddModlogEntry(ModlogType.mute, reason);
            Serializer.Serialize(modlogUser, user.Id);

            var modlog = ctx.Guild.GetChannel(693567857107402762);

            embedBuilder.Description = null;
            embedBuilder.Title = "MUTE";
            embedBuilder.AddField("User", user.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Reason", reason, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("tempmute")]
        [Aliases("temp-mute")]
        [Description("Tempmutes a member for a given reason")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task TempMute(CommandContext ctx,
            
            [Description("User to mute")]
            DiscordMember user,
            
            [Description("Mute duration")]
            Int16 Time = 30,
            
            [Description("Time ID")]
            Char TimeID = 's',
            
            [Description("Reason for the mute")]
            [RemainingText]
            String Reason = "[no reason given]")
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);

            await ctx.Guild.GrantRoleAsync(user, ctx.Guild.GetRole(706591279227600936));

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"<:insanitybotSuccess:737297353316499566> {user.DisplayName} has been temp-muted successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Color = DiscordColor.Orange
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "TEMPMUTE";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Duration", $"{Time}{TimeID}", true);
            embedBuilder.AddField("Reason", Reason, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());

            User modlogUser = Serializer.Deserialize(user.Id);
            modlogUser.AddModlogEntry(ModlogType.mute, Reason);
            Serializer.Serialize(modlogUser, user.Id);

            TimeSpan MuteTime = TimeConverter.GetTimeSpan(Time, TimeID);
            await Task.Delay(MuteTime);

            await ctx.Guild.RevokeRoleAsync(user, ctx.Guild.GetRole(706591279227600936), "[InsanityBot]: Unmuting member");

            embedBuilder.Title = "UNMUTE";
            embedBuilder.ClearFields();
            embedBuilder.Color = DiscordColor.SpringGreen;
            embedBuilder.AddField("User", user.DisplayName, true);
            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("unmute")]
        [Description("Unmutes a member")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Unmute(CommandContext ctx,
            
            [Description("User to unmute")]
            DiscordMember user)
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);
            var muterole = ctx.Guild.GetRole(706591279227600936);

            await user.RevokeRoleAsync(muterole);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"<:insanitybotSuccess:737297353316499566> {user.DisplayName} was unmuted successfully.",
                Color = DiscordColor.SpringGreen,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };

            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "UNMUTE";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }
        #endregion

        #region kick
        [Command("kick")]
        [Description("Kicks an user")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Kick(CommandContext ctx,
            
            [Description("User to kick")] 
            DiscordMember user,
            
            [Description("Reason")]
            [RemainingText]
            String Reason = "[no reason given]")
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);
            var dmchannel = await user.CreateDmChannelAsync();
            var invite = await ctx.Channel.CreateInviteAsync(max_uses: 1);

            User modlogUser = Serializer.Deserialize(user.Id);
            modlogUser.AddModlogEntry(ModlogType.kick, Reason);
            Serializer.Serialize(modlogUser, user.Id);

            _ = ctx.Guild.RemoveMemberAsync(user);
            _ = dmchannel.SendMessageAsync($"Oh no, you have been kicked for {Reason}! Use this fancy invite link to rejoin: {invite}");

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"<:insanitybotSuccess:737297353316499566> {user.Username} has been kicked successfully.",
                Color = DiscordColor.Red,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "KICK";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.Username, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Reason", Reason, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }
        #endregion

        #region ban
        [Command("ban")]
        [Description("Bans an user")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext ctx,

            [Description("User to ban")]
            DiscordMember user,
            
            [Description("Reason")]
            [RemainingText]
            String Reason = "[no reason given]")
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);
            var dmchannel = await user.CreateDmChannelAsync();

            _ = ctx.Guild.BanMemberAsync(user, reason: Reason);
            _ = dmchannel.SendMessageAsync($"Oh no, you have been banned for \"{Reason}\"!");

            User modlogUser = Serializer.Deserialize(user.Id);
            modlogUser.AddModlogEntry(ModlogType.ban, Reason);
            Serializer.Serialize(modlogUser, user.Id);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = $"<:insanitybotSuccess:737297353316499566> {user.Username} has been banned successfully.",
                Color = DiscordColor.Red,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "BAN";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.Username, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Reason", Reason, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("tempban")]
        [Aliases("temp-ban")]
        [Description("Temporarily bans an user")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task Tempban(CommandContext ctx,
            
            [Description("UserID to tempban")]
            DiscordMember user,
            
            [Description("Duration")]
            Int16 Time,
            
            [Description("Timer ID")]
            Char TimeID,
            
            [Description("Reason")]
            [RemainingText]
            String Reason = "No reason given.")
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);
            var dmchannel = await user.CreateDmChannelAsync();
            var invite = await ctx.Channel.CreateInviteAsync(max_uses: 1, max_age: -1);

            _ = ctx.Guild.BanMemberAsync(user, reason: Reason);
            _ = dmchannel.SendMessageAsync($"Oh no, you have been temporarily banned for \"{Reason}\"! Your ban will expire in {Time}{TimeID}");

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.DarkRed,
                Description = $"<:insanitybotSuccess:737297353316499566> {user.Username} has been banned successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "TEMPBAN";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.Username, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Duration", $"{Time}{TimeID}", true);
            embedBuilder.AddField("Reason", Reason, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());

            User modlogUser = Serializer.Deserialize(user.Id);
            modlogUser.AddModlogEntry(ModlogType.ban, Reason);
            Serializer.Serialize(modlogUser, user.Id);

            await Task.Delay(TimeConverter.GetTimeSpan(Time, TimeID));

            _ = ctx.Guild.UnbanMemberAsync(user, reason: "[InsanityBot]: Tempban expired.");
            _ = dmchannel.SendMessageAsync($"Your tempban on Insanity has expired. Use this rejoin link if you want to be a part of the community again: {invite}");

            embedBuilder.Title = "UNBAN";
            embedBuilder.Color = DiscordColor.SpringGreen;
            embedBuilder.ClearFields();
            embedBuilder.AddField("User", user.Username, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("unban")]
        [Aliases("pardon")]
        [Description("Unbans an user by ID")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task Unban(CommandContext ctx,
            
            [Description("UserID to unban")]
            DiscordUser user)
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);

            _ = ctx.Guild.UnbanMemberAsync(user, reason: "[InsanityBot]: User unbanned.");

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SpringGreen,
                Description = $"<:insanitybotSuccess:737297353316499566> {user.Username} has been unbanned successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };

            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "UNBAN";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.Username, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }
        #endregion

        #region blacklist
        [Command("blacklist")]
        [Description("Mutes users and gets them into a modchat")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Blacklist(CommandContext ctx,
            
            [Description("User to blacklist")]
            DiscordMember user,
            
            [Description("The blacklisting reason")]
            [RemainingText]
            String Reason = "[no reason given]")
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);
            var blacklist = ctx.Guild.GetRole(693524517892390922);

            await user.GrantRoleAsync(blacklist, "[InsanityBot]: Blacklisting user...");

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Black,
                Description = $"<:insanitybotSuccess:737297353316499566> {user.DisplayName} was blacklisted successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "BLACKLIST";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);
            embedBuilder.AddField("Reason", Reason, true);
            await modlog.SendMessageAsync(embed: embedBuilder.Build());

            User modlogUser = Serializer.Deserialize(user.Id);
            modlogUser.AddModlogEntry(ModlogType.blacklist, Reason);
            Serializer.Serialize(modlogUser, user.Id);
        }

        [Command("whitelist")]
        [Aliases("unblacklist", "un-blacklist")]
        [Description("Removes users from the modchat")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Whitelist(CommandContext ctx,
            
            [Description("User to whitelist")]
            DiscordMember user)
        {
            var modlog = ctx.Guild.GetChannel(693567857107402762);
            var blacklist = ctx.Guild.GetRole(693524517892390922);

            await user.RevokeRoleAsync(blacklist, "[InsanityBot]: Whitelisting user...");

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.White,
                Description = $"<:insanitybotSuccess:737297353316499566> {user.DisplayName} was whitelisted successfully.",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "WHITELIST";
            embedBuilder.Description = null;
            embedBuilder.AddField("User", user.DisplayName, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }
        #endregion

        #region lock
        [Command("lock")]
        [Aliases("lock-channel", "lockchannel")]
        [Description("Locks a channel from Members")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task Lock(CommandContext ctx)
        {
            var everyonerole = ctx.Guild.GetRole(693508439929782332); 
            var modlog = ctx.Guild.GetChannel(693567857107402762);

            _ = ctx.Channel.AddOverwriteAsync(everyonerole, Permissions.None, Permissions.SendMessages);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = "<:insanitybotSuccess:737297353316499566> This channel was locked successfully.",
                Color = DiscordColor.MidnightBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "LOCK";
            embedBuilder.Description = null;
            embedBuilder.AddField("Channel", ctx.Channel.Name, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("unlock")]
        [Aliases("unlock-channel", "unlockchannel")]
        [Description("Unlocks a channel for members")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task Unlock(CommandContext ctx)
        {
            var everyonerole = ctx.Guild.GetRole(693508439929782332);
            var modlog = ctx.Guild.GetChannel(693567857107402762);

            _ = ctx.Channel.AddOverwriteAsync(everyonerole, Permissions.SendMessages, Permissions.None);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Description = "<:insanitybotSuccess:737297353316499566> This channel was unlocked successfully.",
                Color = DiscordColor.MidnightBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            };
            await ctx.RespondAsync(embed: embedBuilder.Build());

            embedBuilder.Title = "UNLOCK";
            embedBuilder.Description = null;
            embedBuilder.AddField("Channel", ctx.Channel.Name, true);
            embedBuilder.AddField("Moderator", ctx.Member.DisplayName, true);

            await modlog.SendMessageAsync(embed: embedBuilder.Build());
        }
        #endregion

        #region misc mod
        [Command("purge")]
        [Aliases("clear")]
        [Description("Purges a set amount of messages in chat")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task Purge(CommandContext ctx,
            
            [Description("Amount of messages to purge")]
            Int32 PurgeCount)
        {
            var messages = await ctx.Channel.GetMessagesAsync(PurgeCount);
            _ = ctx.Channel.DeleteMessagesAsync(messages);
        }
        #endregion
    }
}
