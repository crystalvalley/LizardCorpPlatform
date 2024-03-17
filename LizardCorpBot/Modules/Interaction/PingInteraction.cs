namespace LizardCorpBot.Modules.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Interactions;
    using LizardCorpBot.Services.Cloud;

    /// <summary>
    /// 테스트용 Ping슬래시 커맨드.
    /// </summary>
    public class PingInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        /// <summary>
        /// ping을 받으면 pong을 돌려줌.
        /// </summary>
        /// <returns>A <see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [SlashCommand("ping", "pong")]
        public async Task Ping()
        {
            await RespondAsync("pong");
        }
    }
}
