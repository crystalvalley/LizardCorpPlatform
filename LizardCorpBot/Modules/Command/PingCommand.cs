namespace LizardCorpBot.Modules.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using LizardCorpBot.Modules.Command.CustomContext;
    using LizardCorpBot.Services.Cloud;

    /// <summary>
    /// 테스트용 Ping커맨드.
    /// </summary>
    /// <param name="lizardCloudService">The <see cref="LizardCloudService"/> that should be inject.</param>
    public class PingCommand(LizardCloudService lizardCloudService) : ModuleBase<BaseSocketCommandContext>
    {
        private readonly LizardCloudService _lizardCloudService = lizardCloudService;

        /// <summary>
        /// ping을 받으면 pong을 돌려줌.
        /// </summary>
        /// <returns>A <see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [Command("base_ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
