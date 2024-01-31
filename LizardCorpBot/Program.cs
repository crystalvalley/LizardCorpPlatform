#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Runtime.InteropServices;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using LizardCorpBot.Data;
using LizardCorpBot.Data.DataAccess;
using LizardCorpBot.Services;
using LizardCorpBot.Services.Minecraft;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddDiscordHost((config, _) =>
{
    config.SocketConfig = new DiscordSocketConfig
    {
        LogLevel = LogSeverity.Verbose,
        AlwaysDownloadUsers = true,
        MessageCacheSize = 1024,
        GatewayIntents = GatewayIntents.All,
    };

    config.Token = builder.Configuration["Token"]!;
});

/*
builder.Services.AddCommandService((config, _) =>
{
    config.DefaultRunMode = Discord.Commands.RunMode.Async;
    config.CaseSensitiveCommands = false;
});
*/

builder.Services.AddSingleton<LizardCommandService>(x =>
{
    CommandServiceConfig config = new()
    {
        DefaultRunMode = RunMode.Async,
        CaseSensitiveCommands = false,
    };
    return new LizardCommandService(config);
});

builder.Services.AddInteractionService((config, _) =>
{
    config.LogLevel = LogSeverity.Debug;
    config.UseCompiledLambda = true;
});

builder.Services.AddHostedService<MinecraftWatcher>();
builder.Services.AddHostedService<InteractionHandler>();
builder.Services.AddHostedService<CommandHandler>();
builder.Services.AddHostedService<TodoService>();

builder.Services.AddDbContextFactory<LizardBotDbContext>(options =>
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) options.UseNpgsql(builder.Configuration.GetConnectionString("psqlLocal"));
    else options.UseNpgsql(builder.Configuration.GetConnectionString("psqlPrd"));
})
.AddSingleton<DataAccessLayer>();

var host = builder.Build();
await host.RunAsync();