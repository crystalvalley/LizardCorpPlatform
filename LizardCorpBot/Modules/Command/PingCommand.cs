namespace LizardCorpBot.Modules.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Commands;

    /// <summary>
    /// 테스트용 Ping커맨드.
    /// </summary>
    public class PingCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// ping을 받으면 pong을 돌려줌.
        /// </summary>
        /// <returns>A <see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
