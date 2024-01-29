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
        /// Todo 추가.
        /// </summary>
        /// <param name="todo">추가할 Todo.</param>
        /// <returns>A <see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task AddTodoAsync(Todo todo)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            context.Add(todo);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 메시지ID로 Todo반환.
        /// </summary>
        /// <param name="messageId">메시지 ID.</param>
        /// <returns>A <see cref="Task{TResult}"/> 비동기 처리 결과로 Todo를 반환.</returns>
        public async Task<Todo?> GetTodoFromMessageID(ulong messageId)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            return context.Todos.Where(x => x.MessageId == messageId).FirstOrDefault();
        }

        /// <summary>
        /// Todo 갱신.
        /// </summary>
        /// <param name="todo">갱신할 todo.</param>
        /// <returns>A <see cref="Task"/> 비동기 처리 결과 반환.</returns>
        public async Task UpdateTodoAsync(Todo todo)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            context.Update(todo);
            await context.SaveChangesAsync();
        }
    }
}
