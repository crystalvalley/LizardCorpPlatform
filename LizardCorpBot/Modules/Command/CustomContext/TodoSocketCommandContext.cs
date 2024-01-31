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
    /// todo용 커맨드 분리용.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="msg"></param>
    public class TodoSocketCommandContext(DiscordSocketClient client, SocketUserMessage msg) : SocketCommandContext(client, msg)
    {
    }
}
