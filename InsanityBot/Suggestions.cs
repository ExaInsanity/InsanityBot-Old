using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus;

namespace InsanityBot
{
    public class Suggestions 
    {
        [Command("suggest")]
        [Description("Creates a new suggestion")]
        public async Task Suggest(CommandContext ctx,
            
            [Description("Content of the Suggestion")]
            [RemainingText]
            String Suggestion)
        {
            if(Suggestion == null)
            {
                DiscordEmbedBuilder embedBuilderFailure = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.DarkRed,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "InsanityBot - Exa 2020"
                    },
                    Description = "<:failure:737297913168003073> Empty suggestions are not supported!"
                };
                await ctx.RespondAsync(embed: embedBuilderFailure.Build());
                return;
            }
            if ((ctx.Message.MentionedRoles.Count != 0) 
                || ctx.Message.MentionEveryone 
                || ctx.Message.Content.Contains("@everyone") 
                || ctx.Message.Content.Contains("@here"))
            {
                DiscordEmbedBuilder embedbuilderFailure = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.DarkRed,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "InsanityBot - Exa 2020"
                    },
                    Description = "<:failure:737297913168003073> Mass pings are not allowed!"
                };
                await ctx.RespondAsync(embed: embedbuilderFailure.Build());
                return;
            }


            var suggestion = await ctx.Guild.GetChannel(693561503496011856).SendMessageAsync($"Suggestion from {ctx.Member.DisplayName}\n{Suggestion}");

            _ = suggestion.CreateReactionAsync(await ctx.Guild.GetEmojiAsync(713686971465334844));
            _ = suggestion.CreateReactionAsync(await ctx.Guild.GetEmojiAsync(713686950883754000));

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SpringGreen,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                },
                Description = "<:success:737297371645345832> Your suggestion was sent to <#693561503496011856> and will be voted on"
            };
            _ = ctx.RespondAsync(embed: embedBuilder.Build());
        }

        public static async Task HandleSuggestionReaction(MessageReactionAddEventArgs e)
        {
            if (e.Emoji == (await e.Channel.Guild.GetEmojiAsync(713686950883754000)))
                SuggestionDownvoted(e);
            else if (e.Emoji == (await e.Channel.Guild.GetEmojiAsync(693767021170655273)))
                SuggestionAccepted(e);
            else if (e.Emoji == (await e.Channel.Guild.GetEmojiAsync(693767039302500362)))
                SuggestionDenied(e);
        }

        private static async void SuggestionDownvoted(MessageReactionAddEventArgs e)
        {
            if ((await e.Message.GetReactionsAsync(await e.Channel.Guild.GetEmojiAsync(713686950883754000))).Count >= 5)
            {
                SuggestionDenied(e, "Denied by the Community");
            }
        }

        private static async void SuggestionAccepted(MessageReactionAddEventArgs e)
        {
            try
            {
                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
                embedBuilder.AddField("Suggestion accepted", e.Message.Content.Split('\n')[1]);
                embedBuilder.Color = DiscordColor.SpringGreen;

                await e.Message.DeleteAsync();
                await e.Channel.Guild.GetChannel(693561556000047144).SendMessageAsync(embed: embedBuilder.Build());
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Program.discord.DebugLogger.LogMessage(LogLevel.Critical, "InsanityBot.Suggestions", $"{ex.GetType()}: {ex.Message}", DateTime.Now);
            }
        }

        private static async void SuggestionDenied(MessageReactionAddEventArgs e, String reason = "Denied by Staff")
        {
            try
            {
                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
                {
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = reason
                    },
                    Color = DiscordColor.DarkRed
                };
                embedBuilder.AddField("Suggestion denied", e.Message.Content.Split('\n')[1]);

                await e.Message.DeleteAsync();
                await e.Channel.Guild.GetChannel(693561535729238026).SendMessageAsync(embed: embedBuilder.Build());
            }
            catch (Exception ex)
            {
                Program.discord.DebugLogger.LogMessage(LogLevel.Critical, "InsanityBot.Suggestions", $"{ex.GetType()}: {ex.Message}", DateTime.Now);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
