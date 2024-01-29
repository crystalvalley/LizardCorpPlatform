namespace LizardCorpBot.Services.Minecraft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using LizardCorpBot.Data.DataAccess;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 마인크래프트 사원증(노예증서) 서비스.
    /// 특정 채널에 각 마인크래프트 유저의 플레이타임을 embed로 기록함.
    /// </summary>
    /// <remarks>
    /// <see cref="MinecraftStaffIdCard"/> 클래스의 생성자.
    /// </remarks>
    /// <param name="client">The <see cref="DiscordSocketClient"/> that should be inject.</param>
    /// <param name="logger">The <see cref="ILogger"/> that should be inject.</param>
    /// <param name="config">The <see cref="IConfiguration"/> that should be inject.</param>
    /// <param name="accessLayer">The <see cref="DataAccessLayer"/> that should be inject.</param>
    public class MinecraftStaffIdCard(DataAccessLayer accessLayer, DiscordSocketClient client, IConfiguration config, ILogger<CommandHandler> logger) : DiscordClientService(client, logger)
    {
        private readonly DataAccessLayer _accessLayer = accessLayer;
        private readonly IConfiguration _configuration = config;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(60 * 1000);

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("마인크래프트 사원증 서비스 시작");
            Client.MessageReceived += HandleMessage;
            _timer.Elapsed += OnTimedEvent;
            // 나중에 개발하고 해제
            //_timer.Start();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 사원증 표시용 채널에 들어온 메시지는 사원증 제외하고 전부 삭제함.
        /// </summary>
        /// <param name="incomingMessage">입력받은 메시지.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        private async Task HandleMessage(SocketMessage incomingMessage)
        {
            var botId = ulong.Parse(_configuration["BotId"]!);

            if (incomingMessage is not SocketUserMessage message)
            {
                return;
            }

            if (message.Author.Id != botId)
            {
                await incomingMessage.DeleteAsync();
            }
        }

        /// <summary>
        /// 타이버 이벤트.
        /// </summary>
        /// <param name="sender">이벤트 소스.</param>
        /// <param name="e">이벤트 args.</param>
        private async void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            var channelId = ulong.Parse(_configuration["MinecraftStaffIdChannel"]!);
            var guildId = ulong.Parse(_configuration["DevelopingGuildId"]!);

            var ch = Client.GetGuild(guildId).GetChannel(channelId) as ISocketMessageChannel;
            await ch.SendMessageAsync("test");
        }
    }
}
