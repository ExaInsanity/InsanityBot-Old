using System.Threading.Tasks;
using System;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace InsanityBot
{
    public class Misc
    {
        [Command("say")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        [Hidden]
        public async Task Say(CommandContext ctx,
            params String[] text)
        {
            await ctx.Message.DeleteAsync();
            if (ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.MentionEveryone))
                await ctx.Channel.SendMessageAsync(String.Join(' ', text));
            else
                await ctx.Channel.SendMessageAsync("You can't do that! Lacking Permission Node insanitybot.commands.misc.say");
        }
    }
}
