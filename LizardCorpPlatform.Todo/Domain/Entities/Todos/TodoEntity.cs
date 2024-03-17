namespace LizardCorpPlatform.Todo.Domain.Entities.Todos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LizardCorpPlatform.Common;

    public class TodoEntity : BaseEntity<int>
    {
        public required ulong Author { get; set; }
        
        public List<ulong> TaskHolder { get; set; }

        public required ulong Guild { get; set; }

    }
}
