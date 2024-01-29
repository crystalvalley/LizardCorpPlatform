﻿namespace LizardCorpBot.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using LizardCorpBot.Data.DataAccess;
    using LizardCorpBot.Data.Model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Npgsql.Replication.PgOutput.Messages;

    /// <summary>
    /// Todo관련 메시지 처리 클래스.
    /// </summary>
    /// <remarks>
    /// <see cref="TodoService"/> 클래스의 생성자.
    /// </remarks>
    /// <param name="client">The <see cref="DiscordSocketClient"/> that should be inject.</param>
    /// <param name="logger">The <see cref="ILogger"/> that should be inject.</param>
    /// <param name="commandService">The <see cref="CommandService"/> that should be inject.</param>
    /// <param name="config">The <see cref="IConfiguration"/> that should be inject.</param>
    /// <param name="accessLayer">The <see cref="DataAccessLayer"/> that should be inject.</param>
    public class TodoService(DataAccessLayer accessLayer, DiscordSocketClient client, ILogger<CommandHandler> logger, IConfiguration config) : DiscordClientService(client, logger)
    {
        private readonly IConfiguration _configuration = config;
        private readonly DataAccessLayer _accessLayer = accessLayer;

        private readonly string[] _emojis = ["Mecha_OK_Mengdok_01", "Mecha_No_Mengdok_01", "Mecha_Z_Mengdok_01"];

        private readonly ulong channelId = 1197554245063417927;

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.MessageReceived += HandleMessage;

            // Client.MessageDeleted += HandleMessageDeleted;
            // Client.MessageUpdated += HandleMessageUpdate;

            Client.ReactionAdded += HandleReactionAdd;
            Client.ReactionRemoved += HandleReactionRemoved;
            return Task.CompletedTask;
        }

        private async Task HandleReactionAdd(Cacheable<IUserMessage, ulong> cacheable1, Cacheable<IMessageChannel, ulong> cacheable2, SocketReaction reaction)
        {
            if (reaction.Channel.GetChannelType() != ChannelType.Text) return;
            if (reaction.UserId == ulong.Parse(_configuration["BotId"]!)) return;
            Todo? todo = await _accessLayer.GetTodoFromMessageID(reaction.MessageId);
            if (todo == null) return;

            // 자신의 일로 추가
            if (reaction.Emote.Name == "Mecha_Z_Mengdok_01")
            {
                todo.TaskHolder.Add(reaction.UserId);
                await _accessLayer.UpdateTodoAsync(todo);

                var embed = new EmbedBuilder
                {
                    Title = todo.Title,
                };

                var author = await reaction.Channel.GetUserAsync(todo.Author);

                embed.AddField("작성자", author.Mention, true);
                string holders = string.Empty;
                foreach (var holder in todo.TaskHolder)
                {
                    holders += (await reaction.Channel.GetUserAsync(holder)).Mention + "\r\n";
                }

                embed.AddField("담당자", holders == string.Empty ? "미정" : holders, true);
                embed.AddField("마감", todo.TimeLimit == null ? "미정" : todo.TimeLimit, true);

                await reaction.Channel.ModifyMessageAsync(reaction.MessageId, x =>
                {
                    x.Embed = embed.Build();
                });
            }

            // todo를 취소함
            if (reaction.Emote.Name == "Mecha_No_Mengdok_01") { }

            // todo를 완료함
            if (reaction.Emote.Name == "Mecha_OK_Mengdok_01") { }
        }

        private async Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> cacheable1, Cacheable<IMessageChannel, ulong> cacheable2, SocketReaction reaction)
        {
            if (reaction.Channel.GetChannelType() != ChannelType.Text) return;
            if (reaction.UserId == ulong.Parse(_configuration["BotId"]!)) return;
            Todo? todo = await _accessLayer.GetTodoFromMessageID(reaction.MessageId);
            if (todo == null) return;

            // 자신의 일에서 제거
            if (reaction.Emote.Name == "Mecha_Z_Mengdok_01")
            {
                todo.TaskHolder.Remove(reaction.UserId);
                await _accessLayer.UpdateTodoAsync(todo);

                var embed = new EmbedBuilder
                {
                    Title = todo.Title,
                };

                var author = await reaction.Channel.GetUserAsync(todo.Author);

                embed.AddField("작성자", author.Mention, true);
                string holders = string.Empty;
                foreach (var holder in todo.TaskHolder)
                {
                    holders += (await reaction.Channel.GetUserAsync(holder)).Mention;
                }

                embed.AddField("담당자", holders == string.Empty ? "미정" : holders, true);
                embed.AddField("마감", todo.TimeLimit == null ? "미정" : todo.TimeLimit, true);

                await reaction.Channel.ModifyMessageAsync(reaction.MessageId, x =>
                {
                    x.Embed = embed.Build();
                });
            }
        }

        #region 나중에
        private async Task HandleMessageUpdate(Cacheable<IMessage, ulong> cacheable, SocketMessage message, ISocketMessageChannel channel)
        {
            Console.WriteLine(cacheable.Id);
            Console.WriteLine(message.Id);
            if (cacheable.Value.Embeds.Count() == 1 && message.Embeds.Count() == 0)
            {
                var first = cacheable.Value.Embeds.First();
                var embed = new EmbedBuilder
                {
                    Title = first.Title,
                };

                foreach (var f in first.Fields)
                {
                    embed.AddField(f.Name, f.Value, true);
                }

                await message.DeleteAsync();
                await channel.SendMessageAsync(embed: embed.Build());
            }
        }

        private async Task HandleMessageDeleted(Cacheable<IMessage, ulong> deletedMessage, Cacheable<IMessageChannel, ulong> deletedChannel)
        {
            if (deletedMessage.Value.Content == "삭제됨") return;
            if (deletedMessage.Value.Embeds.Count() == 0) return;
            var first = deletedMessage.Value.Embeds.First();
            var embed = new EmbedBuilder
            {
                Title = first.Title,
            };

            foreach (var f in first.Fields)
            {
                embed.AddField(f.Name, f.Value);
            }

            await deletedChannel.Value.SendMessageAsync(embed: embed.Build());

        }
        #endregion

        /// <summary>
        /// Todo Channel에 올라온 메시지는 봇이 관리하기에, 이 봇이 올린 메시지가 아니면 삭제함.
        /// </summary>
        /// <param name="incomingMessage">입력받은 메시지.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        private async Task HandleMessage(SocketMessage incomingMessage)
        {
            if (incomingMessage is not SocketUserMessage msg) return;
            if (msg.Channel.Id == channelId && msg.Author.Id != ulong.Parse(_configuration["BotId"]!)) await msg.DeleteAsync();
            if (msg.Channel.Id == channelId && msg.Author.Id == ulong.Parse(_configuration["BotId"]!) &&
                msg.Content == "등록되었습니다.") await msg.DeleteAsync();

            // 초기 등록 todo의 경우 리액션 추가
            var guild = Client.GetGuild(ulong.Parse(_configuration["DevelopingGuildId"]!));
            List<IEmote> eList = new();
            foreach (var e in _emojis)
            {
                IEmote emote = guild.Emotes.First(emote => emote.Name == e);
                Console.WriteLine(emote.Name);
                eList.Add(emote);
            }

            await msg.AddReactionsAsync(eList);
        }

    }
}
