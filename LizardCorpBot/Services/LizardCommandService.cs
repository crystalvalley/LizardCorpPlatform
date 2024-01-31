using Discord.Commands;
using LizardCorpBot.Modules.Command.CustomContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardCorpBot.Services
{
    public class LizardCommandService(CommandServiceConfig config) : CommandService(config)
    {
        public Task<IResult> ExecuteCommandAsync(ICommandContext context, IServiceProvider services, int argPos = 0, MultiMatchHandling multiMatchHandling = MultiMatchHandling.Exception)
        {
            var command = context.Message.Content[argPos..];
            if (context is BaseSocketCommandContext) command = "base_" + command;
            if (context is TodoSocketCommandContext) command = "todo_" + command;
            return ExecuteAsync(context, command, services, multiMatchHandling);
        }
    }
}
