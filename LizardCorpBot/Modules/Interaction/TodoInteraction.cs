namespace LizardCorpBot.Modules.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Interactions;
    using Discord.WebSocket;
    using LizardCorpBot.Data.DataAccess;
    using LizardCorpBot.Data.Model;

    /// <summary>
    /// Todo용 커맨드.
    /// </summary>
    /// <param name="accessLayer">The <see cref="DataAccessLayer"/> that should be inject.</param>
    public class TodoInteraction(DataAccessLayer accessLayer) : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly DataAccessLayer _accessLayer = accessLayer;

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

        [SlashCommand("set_todo", "todo등록")]
        public async Task SetTodo(
            [Summary("할일")] string title,
            [Summary("언제까지")] string? dateTime = null)
        {
            var todoCh = await _accessLayer.GetTodoChannelAsync(Context.Guild.Id);
            if (todoCh == null) return;
            var ch = Context.Guild.GetTextChannel(todoCh.ChannelId);
            if (ch == null) return;

            var embed = new EmbedBuilder
            {
                Title = title,
            };
            embed.AddField("작성자", Context.User.Mention, true);
            embed.AddField("담당자", "미정", true);
            embed.AddField("마감", dateTime ?? "미정", true);

            var response = await ch.SendMessageAsync(embed: embed.Build());

            Todo todo = new()
            {
                Author = Context.User.Id,
                Guild = Context.Guild.Id,
                MessageId = response.Id,
                Title = title,
                Status = TodoStatus.Posted,
                CreateTime = DateTime.UtcNow,
            };

            await _accessLayer.AddTodoAsync(todo);

            await RespondAsync("등록되었습니다.");
        }
    }
}
