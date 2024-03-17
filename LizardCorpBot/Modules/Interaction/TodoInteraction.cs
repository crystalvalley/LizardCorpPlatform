namespace LizardCorpBot.Modules.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Interactions;
    using Discord.WebSocket;
    using LizardCorpBot.Data.DataAccess;
    using LizardCorpBot.Data.Model;
    using LizardCorpBot.Services.Cloud;

    /// <summary>
    /// Todo용 커맨드.
    /// </summary>
    /// <param name="accessLayer">The <see cref="DataAccessLayer"/> that should be inject.</param>
    public class TodoInteraction(DataAccessLayer accessLayer) : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly DataAccessLayer _accessLayer = accessLayer;
        private readonly Dictionary<ulong, ulong> cache = [];

        /// <summary>
        /// Todo채널 지정용 커맨드.
        /// </summary>
        /// <param name="channel">todo로 지정할 채널.</param>
        /// <returns>A <see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [SlashCommand("set_todo_channel", "Todo채널 설정")]
        public async Task SetTodoChannel(IChannel channel)
        {
            var msgCh = channel as IMessageChannel;
            if (msgCh == null || msgCh is not IMessageChannel) return;

            await _accessLayer.SetTodoChannel(Context.Guild.Id, channel.Id);
            await ReplyAsync("등록되었습니다.");
        }

        /// <summary>
        /// Todo등록하기.
        /// </summary>
        /// <param name="title">할일 제목.</param>
        /// <param name="dateTime">마감.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [SlashCommand("set_todo", "todo등록")]
        public async Task SetTodo([Summary("할일")] string title)
        {
            var todoCh = await _accessLayer.GetTodoChannelAsync(Context.Guild.Id);
            if (todoCh == null) return;
            var ch = Context.Guild.GetTextChannel(todoCh.ChannelId);
            if (ch == null) return;

            Todo todo = new()
            {
                Author = Context.User.Id,
                Guild = Context.Guild.Id,
                Title = title,
                Status = TodoStatus.Posted,
                CreateTime = DateTime.UtcNow,
            };

            var embed = todo.GetTodoEmbed(Context.Guild);

            var response = await ch.SendMessageAsync("신규투고", embed: embed.Build());
            todo.MessageId = response.Id;

            await _accessLayer.AddTodoAsync(todo);

            await RespondAsync("등록되었습니다.");
        }
    }
}
