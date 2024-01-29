namespace LizardCorpBot.Data.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LizardCorpBot.Data.Model;

    /// <summary>
    /// partial클래스로 분할, Todo 관련 데이터 처리.
    /// </summary>
    public partial class DataAccessLayer
    {

        /// <summary>
        /// Todo전용 채널 세팅.
        /// </summary>
        /// <param name="guildId">길드 Id.</param>
        /// <param name="channelId">채널 Id.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task SetTodoChannel(ulong guildId, ulong channelId)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var todoChannel = await context.TodoChannels.FindAsync(guildId);
            if (todoChannel == null)
            {
                todoChannel = new TodoChannel()
                {
                    GuildId = guildId,
                    ChannelId = channelId,
                };
                context.TodoChannels.Add(todoChannel);
            }

            todoChannel.ChannelId = channelId;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Todo전용 채널 정보 참조.
        /// </summary>
        /// <param name="guildId">길드 Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> 비동기 처리결과로 Todo 채널Id를 반환.</returns>
        public async Task<TodoChannel?> GetTodoChannelAsync(ulong guildId)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            return await context.TodoChannels.FindAsync(guildId);
        }
    }
}
