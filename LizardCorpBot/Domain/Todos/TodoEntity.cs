namespace LizardCorpBot.Domain.Todos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Todo 엔터티.
    /// </summary>
    public class TodoEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public static TodoEntity Create(string Title)
        {
            return new TodoEntity { Title = Title };
        }
    }
}
