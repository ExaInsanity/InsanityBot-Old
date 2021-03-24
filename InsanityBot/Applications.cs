using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using DSharpPlus.Exceptions;

namespace InsanityBot
{
    [Group("apply", CanInvokeWithoutSubcommand = true)]
    [Aliases("app")]
    [Cooldown(1, 60, CooldownBucketType.User)]
    [Description("Group for application commands")]
    public partial class Applications
    {
        public async Task ExecuteGroupAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("I am sorry, this isn't an application command! " +
                "Please check out <#706798519553491046> and `i.help apply` for further information.");
        }

        [Command("accept")]
        [Description("Accepts an application")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task ApplyAccept(CommandContext ctx,
            [Description("The accepted user")]
            DiscordMember member)
        {
            if (ctx.Channel.Name.StartsWith("app-partner-"))
            {
                await ctx.RespondAsync($"Congratulations! You are now partnered with Insanity Network.");
                await member.GrantRoleAsync(ctx.Guild.GetRole(708692157908779028));

                if (ctx.Channel.Name.StartsWith("app-partner-yt"))
                    await member.GrantRoleAsync(ctx.Guild.GetRole(693765947202338946));
                else if (ctx.Channel.Name.StartsWith("app-partner-twitch"))
                    await member.GrantRoleAsync(ctx.Guild.GetRole(693766057571254332));
            }
            else if (ctx.Channel.Name.StartsWith("app-helper-"))
            {
                await ctx.RespondAsync($"Congratulations! Your application has been accepted. " +
                    $"You are hereby promoted to Trial Helper. In seven days, staff will vote on your trial.");

                await member.GrantRoleAsync(ctx.Guild.GetRole(693525038359511141));
                await member.GrantRoleAsync(ctx.Guild.GetRole(693765647821176882));
                await ctx.Guild.GetChannel(709379949638713395).SendMessageAsync($"{member.DisplayName}'s Helper application in {ctx.Channel.Name} has been accepted.");
            }
            else if (ctx.Channel.Name.StartsWith("app-smp-"))
            {
                await ctx.RespondAsync($"Congratulations! Your application has been accepted. " +
                    $"You are now accepted to the SMP. Check out <#698111102898405427>!");

                await member.GrantRoleAsync(ctx.Guild.GetRole(693524214157803641));
                await ctx.Guild.GetChannel(709379949638713395).SendMessageAsync($"{member.DisplayName}'s SMP application in {ctx.Channel.Name} has been accepted.");
            }
            else if (ctx.Channel.Name.StartsWith("app-builder-"))
            {
                await ctx.RespondAsync($"Congratulations! Your application has been accepted. " +
                    $"You are hereby promoted to Builder.");

                await member.GrantRoleAsync(ctx.Guild.GetRole(705856580561666058));
                await member.GrantRoleAsync(ctx.Guild.GetRole(693525038359511141));
                await ctx.Guild.GetChannel(709379949638713395).SendMessageAsync($"{member.DisplayName}'s Builder application in {ctx.Channel.Name} has been accepted.");

            }
            else if (ctx.Channel.Name.StartsWith("app-dev-"))
            {
                await ctx.RespondAsync($"Congratulations! Your application has been accepted. " +
                    $"You are hereby promoted to Developer.");

                await member.GrantRoleAsync(ctx.Guild.GetRole(693525038359511141));
                await member.GrantRoleAsync(ctx.Guild.GetRole(705856479168561262));
                await ctx.Guild.GetChannel(709379949638713395).SendMessageAsync($"{member.DisplayName}'s Developer application in {ctx.Channel.Name} has been accepted.");
            }
            else
                await ctx.RespondAsync($"This is not even a valid application channel.");

                await ctx.RespondAsync("You can now close this ticket using `i.close`");
        }

        [Command("deny")]
        [Description("Denies an application")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task ApplyDeny(CommandContext ctx)
        {
            await ctx.RespondAsync($"Sadly we could not accept your application. Best wishes, Insanity Network Staff Team.\n" +
                $"Use `i.ticket close` to close this application.");
            await ctx.Guild.GetChannel(709379949638713395).SendMessageAsync($"The application in {ctx.Channel.Name} has been denied.");
        }

        [Command("youtube")]
        [Aliases("yt")]
        [Description("Applies for YouTube rank")]
        public async Task ApplyYoutube (CommandContext ctx)
        {
            String name = "app-partner-yt-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            await (await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821))).SendMessageAsync(
                $"{ctx.Member.Mention}, thank you for your application. Please link your YouTube channel here.");
        }

        [Command("twitch")]
        [Description("Applies for Twitch rank")]
        public async Task ApplyTwitch(CommandContext ctx)
        {
            String name = "app-partner-twitch-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            await (await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821))).SendMessageAsync(
                $"{ctx.Member.Mention}, thank you for your application. Please link your Twitch channel here.");
        }

        [Command("partner")]
        [Description("Applies for partnership")]
        public async Task ApplyPartner(CommandContext ctx)
        {
            String name = "app-partner-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            await (await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821))).SendMessageAsync(
                $"{ctx.Member.Mention}, thank you for your application. Please specify your application further.");
        }

        [Command("helper")]
        [Description("Applies for helper.")]
        public async Task ApplyHelper(CommandContext ctx)
        {
            String name = "app-helper-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            var Application = await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821));
            await Application.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
            await Application.SendMessageAsync($"{ctx.Member.Mention}, thank you for applying for the Helper postion on Insanity. " +
                $"Please answer the following questions honestly.");
            await Application.SendMessageAsync($"1. How old are you?\n" +
                $"2. Were you in a staff position before?\n" +
                $"3. (Optional) If yes, where?\n" +
                $"4. Why do you want to be helper?");

            await Application.SendMessageAsync(ctx.Guild.GetRole(693559538720440340).Mention);
        }

        [Command("smp")]
        [Description("Applies to be accepted on the SMP.")]
        public async Task ApplySmp(CommandContext ctx)
        {
            String name = "app-smp-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            var Application = await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821));
            await Application.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
            await Application.SendMessageAsync($"{ctx.Member.Mention}, thank you for applying for the Insanity SMP. " +
                $"Please answer the following questions honestly.");
            await Application.SendMessageAsync($"1. Did you ever play on a SMP before?\n" +
                $"2. Did you read the rules?\n" +
                $"3. What would you do if someone was to violate a rule?\n" +
                $"4. How old are you?\n" +
                $"5. Do you use either Bedrock Edition or a pirated copy (TLauncher, MCLeaks...)?\n" +
                $"6. Are you able to record evidence of rule breaking?\n" +
                $"7. What is your IGN?");
            await Application.SendMessageAsync($"{ctx.Guild.GetRole(693558842889470032).Mention}, {ctx.Member.Mention} is applying for the SMP. " +
                $"Please process their application.");
        }

        [Command("builder")]
        [Description("Applies for Builder.")]
        public async Task ApplyBuilder(CommandContext ctx)
        {
            String name = "app-builder-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;
            var Application = await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821));
            await Application.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
            await Application.AddOverwriteAsync(Program.insanitynetworkguild.GetRole(705861252601217164),
                Permissions.AccessChannels, Permissions.None);
            await Application.SendMessageAsync($"{ctx.Member.Mention}, thank you for applying for the Builder position on the Insanity Network. " +
                $"Please send one or multiple screenshots of your best builds in here.");
            await Application.SendMessageAsync($"{ctx.Guild.GetRole(705861252601217164).Mention}, please process this incoming application.");
        }

        [Command("developer")]
        [Aliases("dev")]
        [Description("Applies for Developer.")]
        public async Task ApplyDev(CommandContext ctx)
        {
            String name = "app-dev-" + Tickets.GetTicketName(ctx.Member) + "-" + ctx.Member.Discriminator;

            var Application = await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.Guild.GetChannel(693523249237196821));
            await Application.AddOverwriteAsync(ctx.Member, Permissions.AccessChannels, Permissions.None);
            await Application.SendMessageAsync($"{ctx.Guild.GetRole(705861252601217164).Mention}, {ctx.Member.Mention} is applying to be a developer. " +
                $"Please process their application.");
        }
    }
}
