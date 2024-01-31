namespace LizardCorpBot.Modules.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.Interactions;
    using LizardCorpBot.Data.DataAccess;
    using LizardCorpBot.Data.Model;
    using LizardCorpBot.Modules.Command.CustomContext;

    /// <summary>
    /// Todo 관련 텍스트 커맨드.
    /// </summary>
    /// <param name="accessLayer">The <see cref="DataAccessLayer"/> that should be inject.</param>
    public class TodoCommand(DataAccessLayer accessLayer) : ModuleBase<TodoSocketCommandContext>
    {
        private readonly DataAccessLayer _accessLayer = accessLayer;

        /// <summary>
        /// 마감일 설정.
        /// 마감일이 DateTime으로 변환 불가능하면 처리하지 않음.
        /// </summary>
        /// <param name="datetime">마감일.</param>
        /// <returns><see cref="Task"/> 비동기 처리 결과 반환.</returns>
        [Command("todo_timelimit")]
        [Alias("todo_마감")]
        public async Task SetTimeLimit(string datetime)
        {
            var todo = await _accessLayer.GetTodoFromMessageIDAsync(Context.Message.ReferencedMessage.Id);
            var todoChannel = await _accessLayer.GetTodoChannelAsync(Context.Guild.Id);
            if (todo == null || todoChannel == null) return;

            DateTime dt;
            if (!DateTime.TryParse(datetime, out dt))
            {
                await ReplyAsync("날짜를 취득할 수 없습니다,");
                return;
            }

            todo.TimeLimit = dt.ToUniversalTime();
            await _accessLayer.UpdateTodoAsync(todo);

            var channel = Context.Guild.GetTextChannel(todoChannel.ChannelId);
            var embed = todo.GetTodoEmbed(Context.Guild);
            await channel.ModifyMessageAsync(Context.Message.ReferencedMessage.Id, m =>
            {
                m.Embed = embed.Build();
            });
        }

        [Command("todo_addtaskholder")]
        [Alias("담당자")]
        public async Task SetTaskHolder(params IUser[] users)
        {
            var todo = await _accessLayer.GetTodoFromMessageIDAsync(Context.Message.ReferencedMessage.Id);
            var todoChannel = await _accessLayer.GetTodoChannelAsync(Context.Guild.Id);
            if (todo == null || todoChannel == null) return;

            foreach (var user in users)
            {
                todo.ToggleTaskHold(user.Id);
            }

            await _accessLayer.UpdateTodoAsync(todo);
            var channel = Context.Guild.GetTextChannel(todoChannel.ChannelId);
            var embed = todo.GetTodoEmbed(Context.Guild);
            await channel.ModifyMessageAsync(Context.Message.ReferencedMessage.Id, m =>
            {
                m.Embed = embed.Build();
            });

        }
    }
}
