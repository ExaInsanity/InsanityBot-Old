using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InsanityBot
{
    [Group("tag")]
    [Aliases("t")]
    [Description("Bot Tags every member can use.")]
    class Tag
    {
        [Command("rules")]
        [Aliases("r")]
        [Description("Redirects to the rules")]
        public async Task TagRules(CommandContext ctx)
        {
            await ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync($"Check <#693561317352669254>, you might be interested there.");
        }

        [Command("egg")]
        [Description("Eggs.")]
        public async Task TagEgg(CommandContext ctx)
        {
            await ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("What, you :egg:! \n*He stabs him.*");
        }

        [Command("ception")]
        [Hidden]
        public async Task TagCeption(CommandContext ctx,

            [Description("The string to add")]
            String cepted)
        {
            await ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync($"{cepted}ception :v");
        }

        [Command("witch")]
        [Description("Burns.")]
        public async Task TagWitch(CommandContext ctx)
        {
            await ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("What, you damn witch? \n*He burns you.*");
        }

        [Command("ninja")]
        [Description("Shows off power.")]
        public async Task TagNinja(CommandContext ctx)
        {
            await ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("**You dare attack my master?**\n*turns around*\n**You underestimate my powers!**");
        }

        [Command("hiring")]
        [Description("Redirects people to #hiring.")]
        public async Task TagHiring(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("You want to become Staff on Insanity? Check out <#706798519553491046>!");
        }

        [Command("faq")]
        [Description("Sends people to #faq")]
        public async Task TagFaq(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("Pretty sure your question has been answered already. Head to <#693561343084593222> please.");
        }

        [Command("spam")]
        [Description("Tells spammers to stop.")]
        public async Task TagStop(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync($"{await ctx.Guild.GetEmojiAsync(693569242422837269)} Please stop the spam!");
        }

        [Command("civil")]
        [Description("Tells people to stay civil")]
        public async Task TagCivil(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync($"{await ctx.Guild.GetEmojiAsync(697734338531426347)} Keep it cool, kind and civil please!");
        }

        [Command("smp")]
        [Description("Explains the SMP a bit")]
        public async Task TagSMP(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("The Insanity SMP is a whitelisted vanilla SMP. In order to apply for it, you need to have reached Level 1. After that, open a ticket and staff will guide you from there.");
        }

        [Command("lost")]
        [Description("Instructions when lost.")]
        public async Task TagLost(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("Feeling lost? Open a ticket using `i.ticket new`!");
        }

        [Command("deadchat")]
        [Aliases("dc", "dead")]
        [Description("Posts Dead Chat emote")]
        public async Task TagDeadchat(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync($"{await ctx.Guild.GetEmojiAsync(702433345572831342)}{await ctx.Guild.GetEmojiAsync(702433360185786419)}");
        }

        [Command("ip")]
        [Description("Redirects people to #seed-ip")]
        public async Task TagIP(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("You are searching for the IP? If you are permitted to the SMP already, check out <#698111102898405427>");
        }

        [Command("welcome")]
        [Description("Contains a welcome message for people")]
        public async Task TagWelcome(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.Message.RespondAsync("Welcome to the server! Please be sure to read <#693561317352669254> and <#693561343084593222>!");
        }

        [Command("typing")]
        [Description("Simulates the bot frantically typing")]
        public async Task TagTyping(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.TriggerTypingAsync();
        }

        [Command("xp")]
        [Description("Gives information on how the XP system works.")]
        public async Task TagXp(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"You get 1 - 4 XP for each message, with 8 seconds cooldown.");
        }

        [Command("datapacks")]
        [Description("Lists the datapacks executed on the SMP")]
        public async Task TagDatapacks(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"The installed datapacks on the SMP are:\nInsanity Crafting Tweaks, Insanity Custom Loot, Faster Villager " +
                $"Restock, AFK Display, Customizable Armor Stands, Coordinates HUD, Bonus Dragon Drops, Durability Ping, More Mob Heads, Multiplayer Sleep, Nether " +
                $"Portal Coordinates, Player Head Drop, Rotation Wrench, Thunder Shrine, Track Statistics, Treasure Gems, VT Crafting, Villager Death Messages, " +
                $"Villager Workstation Highlights.");
        }

        [Command("servicedesk")]
        [Description("Links the user to the service desk")]
        public async Task TagServicedesk(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"Want to report a bug or suggest a new feature? Please head to https://insanitynetwork.atlassian.net/servicedesk/ !");
        }

        [Command("status")]
        [Aliases("statuspage")]
        [Description("Links the user to the statuspage")]
        public async Task TagStatus(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"Uptime of all Insanity servers can be viewed here: https://status.insanity.network/");
        }

        [Command("github")]
        [Description("Links the user to the InsanityBot 2.0 GitHub")]
        public async Task TagGithub(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"Check out InsanityBot 2.0 Development, report issues or suggest new features here: https://github.com/InsanityNetwork/InsanityBot/");
        }

        [Command("abooz")]
        [Description("Sends the abuse emote")]
        public async Task TagAbooz(CommandContext ctx)
        {
            _ = ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"<:adminabooz:743132340800323595>");
        }
    }
}
