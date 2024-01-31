namespace LizardCorpBot.Services
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using LizardCorpBot.Modules.Command.CustomContext;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 각종 텍스트 커맨드를 처리하는 클래스.
    /// </summary>
    /// <remarks>
    /// <see cref="CommandHandler"/> 클래스의 생성자.
    /// </remarks>
    /// <param name="client">The <see cref="DiscordSocketClient"/> that should be inject.</param>
    /// <param name="logger">The <see cref="ILogger"/> that should be inject.</param>
    /// <param name="provider">The <see cref="IServiceProvider"/> that should be inject.</param>
    /// <param name="commandService">The <see cref="CommandService"/> that should be inject.</param>
    /// <param name="config">The <see cref="IConfiguration"/> that should be inject.</param>
    public class CommandHandler(DiscordSocketClient client, ILogger<CommandHandler> logger, IServiceProvider provider, LizardCommandService commandService, IConfiguration config) : DiscordClientService(client, logger)
    {
        private readonly IServiceProvider _provider = provider;
        private readonly LizardCommandService _service = commandService;
        private readonly IConfiguration _configuration = config;

        /// <summary>
        /// 커맨드가 실행되었을 때 이벤트 핸들러.
        /// 누가 커맨드 어떤 커맨드를 사용했는지 로그에 남김.
        /// </summary>
        /// <param name="command">커맨드.</param>
        /// <param name="context">커맨드 컨텍스트.</param>
        /// <param name="result">커맨드 실행 결과.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            Logger.LogInformation("유저 : {user}가 {command}를 실행함", context.User, command.Value.Name);

            if (!command.IsSpecified || result.IsSuccess)
            {
                return;
            }

            await context.Channel.SendMessageAsync($"Error: {result}");
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.MessageReceived += HandleMessage;
            _service.CommandExecuted += CommandExecutedAsync;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        /// <summary>
        /// 커맨드라면 실행, 커맨드가 아니면 무시.
        /// 커맨드의 조건은 1. 입력된 메시지가 SocketUserMessage일 것.
        /// 2. 유저의 입력일 것.
        /// 3. Prefix가 일치할 것.
        /// </summary>
        /// <param name="incomingMessage">입력받은 메시지.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        private async Task HandleMessage(SocketMessage incomingMessage)
        {
            if (incomingMessage is not SocketUserMessage message)
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            int argPos = 0;

            if (!message.HasStringPrefix(_configuration["Prefix"], ref argPos) && !message.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new BaseSocketCommandContext(Client, message);
            await _service.ExecuteCommandAsync(context, _provider, argPos);
        }
    }
}
