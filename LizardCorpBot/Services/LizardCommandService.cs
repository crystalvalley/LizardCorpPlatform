namespace LizardCorpBot.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Commands;
    using LizardCorpBot.Modules.Command.CustomContext;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// !커맨드 처리용 서비스.
    /// </summary>
    /// <param name="config">The <see cref="CommandServiceConfig"/> that should be inject.</param>
    public class LizardCommandService(CommandServiceConfig config) : CommandService(config)
    {
        /// <summary>
        /// 커맨드가 실행되었을 때 이벤트 핸들러.
        /// base용, todo용 나눠서 처리함.
        /// </summary>
        /// <param name="context">커맨드 컨텍스트.</param>
        /// <param name="services">등록된 서비스들.</param>
        /// <param name="argPos">문자열 위치.</param>
        /// <param name="multiMatchHandling">커맨드 중복으로 발견될 경우.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public Task<IResult> ExecuteCommandAsync(ICommandContext context, IServiceProvider services, int argPos = 0, MultiMatchHandling multiMatchHandling = MultiMatchHandling.Exception)
        {
            var command = context.Message.Content[argPos..];
            if (context is BaseSocketCommandContext) command = "base_" + command;
            if (context is TodoSocketCommandContext) command = "todo_" + command;
            return ExecuteAsync(context, command, services, multiMatchHandling);
        }
    }
}
