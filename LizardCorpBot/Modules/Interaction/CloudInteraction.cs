namespace LizardCorpBot.Modules.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Interactions;
    using LizardCorpBot.Services.Cloud;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// 클라우드용 슬래시 커맨드.
    /// </summary>
    /// <param name="lizardCloudService">The <see cref="LizardCloudService"/> that should be inject.</param>
    public class CloudInteraction(LizardCloudService lizardCloudService) : InteractionModuleBase<SocketInteractionContext>
    {
        private LizardCloudService _lizardCloudService = lizardCloudService;

        /// <summary>
        /// 클라우드 계정 생성용 커맨드.
        /// </summary>
        /// <param name="email">email.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [SlashCommand("entry_cloud", "리자드 클라우드 계정을 생성합니다.")]
        public async Task Entry(string email)
        {
            await _lizardCloudService.AddUser(email);
            await RespondAsync("초기패스워드는 lizard_password입니다.", ephemeral: true);
        }
    }
}
