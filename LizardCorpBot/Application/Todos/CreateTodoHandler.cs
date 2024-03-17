namespace LizardCorpBot.Application.Todos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LizardCorpBot.Data;
    using LizardCorpBot.Domain.Todos;

    public class CreateTodoHandler(LizardBotDbContext dbContext)
    {
        private readonly LizardBotDbContext _dbContext = dbContext;

        public async Task<TodoEntity> Handle(CreateTodo command, CancellationToken cancellation)
        {
            var todo = TodoEntity.Create(command.Title);
            throw new NotImplementedException();
        }
    }
}
