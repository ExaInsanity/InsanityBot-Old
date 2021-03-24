using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;

namespace InsanityBot
{
    public class Tickets
    {
        [Command("new")]
        [Aliases("create")]
        [Description("Opens a new, generic ticket")]
        [Cooldown(2, 60, CooldownBucketType.User)]
        public async Task TicketNew(CommandContext ctx)
        {
            String ticketname = "ticket-" + GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            var ticket = await ctx.Guild.CreateChannelAsync(ticketname, ChannelType.Text, parent: ctx.Guild.GetChannel(693523249237196821));
            var ticketlog = ctx.Guild.GetChannel(712664174857289809);

            await ticket.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
            await ticket.AddOverwriteAsync(ctx.Guild.GetRole(718782613212364862), Permissions.AccessChannels, Permissions.None);
            await ticket.SendMessageAsync($"Hello {ctx.Member.Mention}, how can {ctx.Guild.GetRole(718782613212364862).Mention} help you?");

            DiscordEmbedBuilder embedBuilderClose = new DiscordEmbedBuilder
            {
                Color = DiscordColor.MidnightBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Title = "Ticket Created"
            };
            embedBuilderClose.AddField("Ticket", ticket.Name, true)
                .AddField("User", ctx.Member.Username, true);

            await ticketlog.SendMessageAsync(embed: embedBuilderClose.Build());

            var embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Description = $"Thank you for opening a ticket! Your ticket channel is {ticket.Mention}"
            };

            await ctx.RespondAsync(embed: embedBuilder.Build());
        }

        [Command("close")]
        [Description("Closes the current ticket")]
        public async Task TicketClose(CommandContext ctx)
        {
            if ((!ctx.Channel.Name.StartsWith("ticket-")) && (!ctx.Channel.Name.StartsWith("app-")))
            {
                await ctx.RespondAsync("I'm sorry, this is no ticket channel.");
                return;
            }

            var ticketlog = ctx.Guild.GetChannel(712664174857289809);

            await ctx.RespondAsync("Closing ticket...");
            await Task.Delay(15000);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.MidnightBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Title = "Ticket Closed"
            };
            embedBuilder.AddField("Ticket", ctx.Channel.Name, true)
                .AddField("User", ctx.Member.Username, true);

            await ticketlog.SendMessageAsync(embed: embedBuilder.Build());

            await ctx.Channel.DeleteAsync();
        }

        [Command("add")]
        [Description("Adds an user to a ticket")]
        public async Task TicketAdd(CommandContext ctx,

            [Description("The user to add")]
            DiscordMember user)
        {
            if (!ctx.Channel.Name.StartsWith("ticket-"))
            {
                await ctx.RespondAsync("I'm sorry, this is no ticket channel.");
                return;
            }

            await ctx.Channel.AddOverwriteAsync(user, Permissions.AccessChannels, Permissions.None);
            await ctx.RespondAsync($"{user.Mention} has been added to the ticket.");
        }

        [Command("leave")]
        [Description("Leaves the current ticket")]
        public async Task TicketLeave(CommandContext ctx)
        {
            if (!ctx.Channel.Name.StartsWith("ticket-"))
            {
                await ctx.RespondAsync("I'm sorry, this is no ticket channel.");
                return;
            }
            await ctx.Channel.AddOverwriteAsync(ctx.Member, Permissions.None, Permissions.AccessChannels);
        }

        [Group("ticket")]
        [Aliases("ti")]
        [Description("Encapsulates ticket subcommands")]
        public class TicketCommands
        {

            [Command("support")]
            [Description("Opens a new private support request")]
            [Cooldown(1, 60, CooldownBucketType.User)]
            public async Task TicketSupport(CommandContext ctx)
            {
                String TicketName = "ticket-support-" + GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;

                var ticketchannel = await ctx.Guild.CreateChannelAsync(TicketName, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821));
                var ticketlog = ctx.Guild.GetChannel(712664174857289809);
                var reportrole = ctx.Guild.GetRole(755806841262309486);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "InsanityBot - Exa 2020"
                    },
                    Description = $"Thank you for opening a support request! Your support channel is {ticketchannel.Mention}"
                };

                await ctx.RespondAsync(embed: embedBuilder.Build());

                embedBuilder.Description = $"{ctx.Member.Mention} opened a ticket in {ticketchannel.Mention}";

                await ticketlog.SendMessageAsync(embed: embedBuilder.Build());

                _ = ticketchannel.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
                _ = ticketchannel.AddOverwriteAsync(reportrole, Permissions.AccessChannels, Permissions.None);
            }

            [Command("management")]
            [Description("Opens a new, generic ticket as management-only")]
            [Cooldown(1, 60, CooldownBucketType.User)]
            public async Task TicketNew(CommandContext ctx)
            {
                String ticketname = "ticket-management-" + GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
                var ticket = await ctx.Guild.CreateChannelAsync(ticketname, ChannelType.Text, parent: ctx.Guild.GetChannel(693523249237196821));
                var ticketlog = ctx.Guild.GetChannel(712664174857289809);

                await ticket.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
                await ticket.SendMessageAsync($"Hello {ctx.Member.Mention}, how can {ctx.Guild.GetRole(738335986374934578).Mention} help you?");

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.MidnightBlue,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "InsanityBot - Exa 2020"
                    },
                    Title = "Ticket Created"
                };
                embedBuilder.AddField("Ticket", ticket.Name, true)
                    .AddField("User", ctx.Member.Username, true);

                await ticketlog.SendMessageAsync(embed: embedBuilder.Build());

                embedBuilder.Description = $"<:insanitybotSuccess:737297353316499566> Your ticket channel is {ticket.Mention}";
            }

            [Command("up")]
            [Description("Moves the ticket up to Management-Only")]
            public async Task TicketUp(CommandContext ctx)
            {
                if(!ctx.Channel.Name.StartsWith("ticket-"))
                {
                    await ctx.RespondAsync("This is no ticket channel! You can only move ticket channels up!");
                    return;
                }

                await ctx.Channel.AddOverwriteAsync(ctx.Guild.GetRole(718782613212364862), Permissions.None, Permissions.AccessChannels);
                await ctx.RespondAsync("This ticket was successfully moved up to management-only.");
            }

            [Command("down")]
            [Description("Moves the ticket down from Management-Only")]
            public async Task TicketDown(CommandContext ctx)
            {
                if (!ctx.Channel.Name.StartsWith("ticket-"))
                {
                    await ctx.RespondAsync("This is no ticket channel! You can only move ticket channels down!");
                    return;
                }

                await ctx.Channel.AddOverwriteAsync(ctx.Guild.GetRole(718782613212364862), Permissions.AccessChannels, Permissions.None);
                await ctx.RespondAsync("This ticket was successfully moved down from management-only.");
            }

            [Command("to")]
            [Description("Moves the ticket to a specified role (only works downwards)")]
            public async Task TicketTo(CommandContext ctx,
                
                [Description("The role to move the ticket to")]
                DiscordRole role)
            {
                if (!ctx.Channel.Name.StartsWith("ticket-"))
                {
                    await ctx.RespondAsync("This is no ticket channel! You can only move ticket channels to a certain role!");
                    return;
                }

                await ctx.Channel.AddOverwriteAsync(role, Permissions.AccessChannels, Permissions.None);
                await ctx.RespondAsync($"This ticket was successfully moved to {role.Name}.");
            }

            [Command("from")]
            [Description("Moves the ticket away from a specified role (only works upwards)")]
            public async Task TicketFrom(CommandContext ctx,

                [Description("The role to move the ticket away from")]
                DiscordRole role)
            {
                if (!ctx.Channel.Name.StartsWith("ticket-"))
                {
                    await ctx.RespondAsync("This is no ticket channel! You can only move ticket channels away from a certain role!");
                    return;
                }

                await ctx.Channel.AddOverwriteAsync(role, Permissions.None, Permissions.AccessChannels);
                await ctx.RespondAsync($"This ticket was successfully moved away from {role.Name}.");
            }
        }


        public static String GetTicketName(DiscordMember member)
        {
            if (member.Username.Length <= 3)
                return member.Username;
            return member.Username.Substring(0, 4);
        }
    }
}
