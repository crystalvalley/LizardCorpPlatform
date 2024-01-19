namespace LizardCorpBot.Services.Minecraft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using LizardCorpBot.Data.DataAccess;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.FileSystemGlobbing;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 마인크래프트 서버의 로그를 감시하고, 다양한 기능을 처리하는 서비스.
    /// </summary>
    /// <remarks>
    /// <see cref="MinecraftWatcher"/> 클래스의 생성자.
    /// </remarks>
    /// <param name="client">The <see cref="DiscordSocketClient"/> that should be inject.</param>
    /// <param name="logger">The <see cref="ILogger"/> that should be inject.</param>
    /// <param name="config">The <see cref="IConfiguration"/> that should be inject.</param>
    /// <param name="accessLayer">The <see cref="DataAccessLayer"/> that should be inject.</param>
    public class MinecraftWatcher(DataAccessLayer accessLayer, DiscordSocketClient client, ILogger<MinecraftWatcher> logger, IConfiguration config) : DiscordClientService(client, logger)
    {
        private readonly IConfiguration _configuration = config;
        private readonly FileSystemWatcher _watcher = new();
        private readonly DataAccessLayer _accessLayer = accessLayer;

        private int readLinesCount = -1;

        /// <summary>
        /// 마인크래프 서버에 대해서 Watcher기동.
        /// </summary>
        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 마인크래프트 서버가 동작하는 곳은 리눅스.
            // 이유는 모르겠는데 윈도우에서는 잘 안 먹힘.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Logger.LogDebug("Watcher On");
                _watcher.Path = _configuration["MCLogPathLnx"]!;

                _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                _watcher.Filter = "latest.log";

                _watcher.Changed += OnChanged;

                _watcher.EnableRaisingEvents = true;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 로그 파일이 변경 되었을 때의 이벤트 핸들러.
        /// </summary>
        /// <param name="source">이벤트 소스.</param>
        /// <param name="e">이벤트 정보.</param>
        private async void OnChanged(object source, FileSystemEventArgs e)
        {
            ulong guildId = ulong.Parse(_configuration["DevelopingGuildId"]!);
            ulong channelId = ulong.Parse(_configuration["MinecraftChannelId"]!);

            string path = $"{_configuration["MCLogPathLnx"]}/latest.log";
            var ch = Client.GetGuild(guildId).GetChannel(channelId) as ISocketMessageChannel;

            int totalLines = File.ReadLines(path).Count();
            int newLinesCount = totalLines - readLinesCount;
            if (readLinesCount == -1)
            {
                readLinesCount = totalLines;
                return;
            }

            var newLines = File.ReadLines(path).Skip(readLinesCount).Take(newLinesCount);
            readLinesCount = totalLines;

            foreach (string line in newLines)
            {
                if (line.Contains("joined the game"))
                {
                    // 로그에 접속했다는 기록이 발견된 경우.
                    // AAA joined the game
                    if (line.Contains("formerly known as"))
                    {
                        // 캐릭터 이름이 변경된 경우.
                        // 닉네임 변경 시 BBB (formerly known as AAA) joined the game이라고 뜸
                        var newName = line.Split("(formerly known as").First().Trim();
                        var oldName = line.Split("(formerly known as").Last().Replace(")", string.Empty).Trim();
                        await _accessLayer.ChanageName(oldName, newName);
                    }

                    var user = line.Split(":").Last().Replace("joined the game", string.Empty).Trim();
                    Logger.LogDebug("{} 가 lizard minecraft server에 접속함", user);
                    await _accessLayer.JoinMinecraftServer(user);
                    await ch!.SendMessageAsync($"{user}님이 접속함");
                }
                else if (line.Contains("left the game"))
                {
                    // 로그에 접속 종료 기록이 발견된 경우.
                    // AAA left the game
                    var user = line.Split(":").Last().Replace("left the game", string.Empty).Trim();
                    Logger.LogDebug("{} 가 lizard minecraft server에서 탈주함", user);
                    await _accessLayer.LeftMinecraft(user);
                    await ch!.SendMessageAsync($"노예 {user}가 탈주함");
                }
            }
        }
    }
}
