using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using InsanityBot.Util; 
using InsanityBot.Util.FileMeta.Reference;

namespace InsanityBot
{
    public sealed class Program
    {
        public static DiscordClient discord;
        static CommandsNextModule commands;
        public static DiscordGuild insanitynetworkguild;
        static TcpListener hetrix = null;
#pragma warning disable IDE0060 // Remove unused parameter
        static async Task Main(String[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "i.",
                CaseSensitive = false
            });

            discord.MessageCreated += PingMessages;
            discord.MessageCreated += Automod;
            discord.MessageDeleted += MessageDeleteLogger;
            discord.GuildMemberAdded += Discord_GuildMemberAdded;
            discord.MessageReactionAdded += Discord_MessageReactionAdded;
            insanitynetworkguild = await discord.GetGuildAsync(693508439929782332);

            RegisterAll();
            Experience.Initialize();

            _ = discord.ConnectAsync();
            _ = HandleTCP();
            await Task.Delay(-1);
        }

        private static async Task MessageDeleteLogger(MessageDeleteEventArgs e)
        {
            if (e.Channel.ParentId == 693523162729545768 || e.Channel.ParentId == 693523249237196821)
                return;
            if (e.Message.Content.StartsWith("i.t") || e.Message.Content.StartsWith("i.say") || e.Message.Content.StartsWith(","))
                return;

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "InsanityBot - Exa 2020"
                }
            }.AddField(e.Message.Author.Username, e.Message.Content);

            await e.Guild.GetChannel(693567898244874240).SendMessageAsync(embed: embedBuilder.Build());
        }

        private static async Task Automod(MessageCreateEventArgs e)
        {
            String[] slurs =
            {
                "bluegum",
                "redskin",
                "nigger"
            };

            foreach(var s in slurs)
            {
                if(e.Message.Content.ToLower().Contains(s))
                {
                    await e.Message.DeleteAsync();
                    return;
                }
            }

            if ((e.Author as DiscordMember).Roles.Contains(e.Guild.GetRole(693525038359511141)))
                return;
            if ((e.Author as DiscordMember).Roles.Contains(e.Guild.GetRole(708692157908779028)))
                return;

            if (e.Message.Content.Contains("discord.gg/"))
                await e.Message.DeleteAsync();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task XpHandling(MessageCreateEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
         {
             if (e.Author.IsBot)
                 return;

            var x = from xm in Experience.update
                    where xm.UserID == e.Author.Id
                    select xm;

            if (x.Count() != 0)
                return;

             Experience.update.Add(new ExperiencePacket
             {
                 Level = 0,
                 UserID = e.Author.Id,
                 Experience = (UInt64)new Random().Next(0, 3)
             });
         }

        private static async Task Discord_MessageReactionAdded(MessageReactionAddEventArgs e)
        {
            if (e.Channel == e.Channel.Guild.GetChannel(693561503496011856))
                await Suggestions.HandleSuggestionReaction(e);
        }

        private static async Task Discord_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            var channel = await e.Member.CreateDmChannelAsync();
            await channel.SendMessageAsync("Welcome to the Insanity Network Discord! Please be sure to read <#693561317352669254> and <#693561343084593222>!");

            if (!File.Exists($"./data/{e.Member.Id}/modlog.xml"))
                Serializer.CreateNew(e.Member.Id, e.Member.Username);

            FileStream writer = new FileStream("./data/experience.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<ExperiencePacket>));

            List<ExperiencePacket> packets = (List<ExperiencePacket>)serializer.Deserialize(writer);
            packets.Add(new ExperiencePacket
            {
                Level = 0,
                Experience = 0,
                UserID = e.Member.Id
            });
            packets = packets.OrderByDescending(xm => xm.Level).ThenByDescending(xm => xm.Experience).ToList();

            writer.SetLength(0);
            writer.Flush();

            serializer.Serialize(writer, packets);
            writer.Close();
        }

        private static async Task PingMessages(MessageCreateEventArgs e)
        {
            _ = XpHandling(e);

            if ((e.Author as DiscordMember).Roles.Contains(e.Guild.GetRole(693560585241100384))
                || (e.Author as DiscordMember).Roles.Contains(e.Guild.GetRole(693525038359511141)))
                return;


            foreach(DiscordMember user in e.MentionedUsers)
            {
                if (user.Roles.Contains(e.Guild.GetRole(693558691122905239)) && user.Roles.Contains(e.Guild.GetRole(693558837059518467)))
                    return;

                if(user.Roles.Contains(e.Guild.GetRole(738103276687327404)))
                {
                    await e.Message.RespondAsync($"Please do not ping System Admins! If you wish to report a bug or suggest a feature, " +
                        $"leave a post at https://insanitynetwork.atlassian.net/servicedesk/ !");
                    return;
                }
                if(user.Roles.Contains(e.Guild.GetRole(705861252601217164)))
                {
                    await e.Message.RespondAsync($"Please do not ping Network Admins! If you wish to report a bug or suggest a feature, " +
                        $"leave a post at https://insanitynetwork.atlassian.net/servicedesk/ !");
                    return;
                }
                if(user.Roles.Contains(e.Guild.GetRole(693558842889470032)))
                {
                    await e.Message.RespondAsync($"Please do not ping Administrators!");
                    return;
                }
                if(user.Roles.Contains(e.Guild.GetRole(705856479168561262)))
                {
                    await e.Message.RespondAsync($"Please do not ping Developers!");
                    return;
                }
                if(user.Roles.Contains(e.Guild.GetRole(705856580561666058)))
                {
                    await e.Message.RespondAsync($"Please do not ping Builders!");
                    return;
                }
            }
        }

        private static void RegisterAll()
        {
            commands.RegisterCommands<ModCommands>();
            commands.RegisterCommands<Misc>();
            commands.RegisterCommands<Tag>();
            commands.RegisterCommands<Tickets>();
            commands.RegisterCommands<Applications>();
            commands.RegisterCommands<Suggestions>();
            commands.RegisterCommands<Warnings>();
            commands.RegisterCommands<Experience>();
        }

        private static async Task HandleTCP()
        {
            try
            {
                Int32 Port = 6968;

                hetrix = new TcpListener(IPAddress.Parse("192.168.0.129"), Port);

                hetrix.Start();

                Byte[] bytes = new Byte[256];
                TcpClient client = null;
                NetworkStream stream = null;
                Int32 i = 0;

                while (true)
                {
                    client = null;
                    stream = null;

                    client = await hetrix.AcceptTcpClientAsync();
                    stream = client.GetStream();

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        bytes = Encoding.ASCII.GetBytes("200");

                        stream.Write(bytes, 0, bytes.Length);
                    }

                    client.Close();
                }
            }
            catch(SocketException e)
            {
                discord.DebugLogger.LogMessage(LogLevel.Critical, "InsanityBot", $"SocketException - {e.Message}", DateTime.Now);
            }
            finally
            {
                hetrix.Stop(); 
            }
        }
    }
}
