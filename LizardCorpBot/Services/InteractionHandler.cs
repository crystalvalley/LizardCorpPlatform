namespace LizardCorpBot.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Addons.Hosting.Util;
    using Discord.Interactions;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 슬래시 커맨드, 유저 커맨드 등등을 처리하는 클래스.
    /// </summary>
    /// <remarks>
    /// <see cref="InteractionHandlerService"/> 클래스의 생성자.
    /// </remarks>
    /// <param name="client">The <see cref="DiscordSocketClient"/> that should be inject.</param>
    /// <param name="logger">The <see cref="ILogger"/> that should be inject.</param>
    /// <param name="provider">The <see cref="IServiceProvider"/> that should be inject.</param>
    /// <param name="InteractionService">The <see cref="InteractionService"/> that should be inject.</param>
    /// <param name="config">The <see cref="IConfiguration"/> that should be inject.</param>
    /// <param name="environment">The <see cref="IHostEnvironment"/> that should be inject.</param>
    public class InteractionHandler(DiscordSocketClient client, ILogger<InteractionHandler> logger, IServiceProvider provider, InteractionService interactionService, IConfiguration config, IHostEnvironment environment) : DiscordClientService(client, logger)
    {
        private readonly IServiceProvider _provider = provider;
        private readonly InteractionService _service = interactionService;
        private readonly IConfiguration _configuration = config;
        private readonly IHostEnvironment _environment = environment;

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            Client.InteractionCreated += HandleInteraction;
            _service.SlashCommandExecuted += SlashCommandExecuted;
            _service.ContextCommandExecuted += ContextCommandExecuted;
            _service.ComponentCommandExecuted += ComponentCommandExecuted;

            await Client.WaitForReadyAsync(stoppingToken);

            if (_environment.IsDevelopment()) await _service.AddCommandsToGuildAsync(ulong.Parse(_configuration["DevelopingGuildId"] !));
            else await _service.AddCommandsGloballyAsync();
        }

        /// <summary>
        /// 인터랙션 실행.
        /// </summary>
        /// <param name="interaction">입력받은 커맨드.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var ctx = new SocketInteractionContext(Client, interaction);
                await _service.ExecuteCommandAsync(ctx, _provider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "인터랙션 처리 중에 익셉션이 발생함.");

                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    var msg = await interaction.GetOriginalResponseAsync();
                    await msg.DeleteAsync();
                }
            }
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo commandInfo, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo context, IInteractionContext arg2, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
