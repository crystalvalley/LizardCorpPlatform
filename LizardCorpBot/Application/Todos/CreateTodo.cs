namespace LizardCorpBot.Application.Todos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql.PostgresTypes;

    /// <summary>
    /// Todo관련 커맨드.
    /// </summary>
    public class CreateTodo(string title)
    {
        public int? Id { get; set; } = null;

        public string Title { get; set; } = title;
    }
}
