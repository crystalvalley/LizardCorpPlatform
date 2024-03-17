namespace LizardCorpBot.Modules.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Interactions;
    using Discord.WebSocket;

    /// <summary>
    /// 헬프용 슬래시 커맨드.
    /// LizardCorpBot의 명령어 등등을 소개해주는 역할.
    /// </summary>
    public class HelpInteraction : InteractionModuleBase<SocketInteractionContext>
    {
        /// <summary>
        /// 도움말 표시.
        /// 추후 각 도움말들에 대해서는 DB에 이행할 예정.
        /// 현 시점에서는 하드 코딩.
        /// </summary>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [SlashCommand("help", "도움말을 표시합니다.")]
        public async Task GetHelp(string? type = null)
        {
            if (type is null) await RespondAsync(GetBaseHelp());
            else if (type == "클라우드") await RespondAsync(GetCloudHelp());
        }

        /// <summary>
        /// 나중에 DB로 이행.
        /// 지금은 하드 코딩.
        /// </summary>
        /// <returns>기본 도움말.</returns>
        private static string GetBaseHelp()
        {
            return "Trash Can 디스코드에 오신 것을 환영합니다." + Environment.NewLine
                + "각 카테고리에 대해 추가 설명을 원하신다면 /help {카테고리} 명령어를 이용해 주세요." + Environment.NewLine
                + Environment.NewLine
                + "클라우드 : Trash Can 가입자들에게 무료로 제공하는 클라우드 서비스입니다." + Environment.NewLine
                + "서버 : 각종 게임들의 멀티플레이 서버에 대해 설명합니다." + Environment.NewLine;
        }

        /// <summary>
        /// 나중에 DB로 이행.
        /// 지금은 하드 코딩.
        /// </summary>
        /// <returns>클라우드 도움말.</returns>
        private string GetCloudHelp()
        {
            return "Trash Can 디스코드에서 지원하는 클라우드 서비스에 관련된 설명입니다." + Environment.NewLine
                + "접속 URL은 https://cloud.lizardcorp.net 입니다." + Environment.NewLine
                + Environment.NewLine
                + "명령어" + Environment.NewLine
                + "/entry_cloud {email} : email주소를 통해서 리자드 클라우드에 가입할 수 있습니다." + Environment.NewLine
                + $"초기에 사용 가능한 용량은 10GB이며 자세한 사항은 {Context.Guild.GetUser(265751221229322240).Mention} 에게 문의 주시길 바랍니다." + Environment.NewLine;
        }
    }
}
