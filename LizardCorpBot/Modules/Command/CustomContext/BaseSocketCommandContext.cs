namespace LizardCorpBot.Modules.Command.CustomContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;

    /// <summary>
    /// 다른 소켓 커맨드컨텍스트랑 분리용.
    /// </summary>
    public class BaseSocketCommandContext(DiscordSocketClient client, SocketUserMessage msg) : SocketCommandContext(client, msg)
    {
    }
}
