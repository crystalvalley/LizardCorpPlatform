#pragma warning disable SA1600 // Elements should be documented
namespace LizardCorpBot.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Todo스테이터스 enum.
    /// </summary>
    public enum TodoStatus
    {
        /// <summary>
        /// 기표됨.
        /// </summary>
        Posted,

        /// <summary>
        /// 취소됨.
        /// </summary>
        Canceled,

        /// <summary>
        /// 완료됨.
        /// </summary>
        Complete,
    }

    /// <summary>
    /// Todo.
    /// </summary>
    [Table("todo")]
    public class Todo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public required string Title { get; set; }

        [Column("author")]
        [Description("discord user id of author")]
        public required ulong Author { get; set; }

        [Column("taskholders")]
        [Description("discord user id of taskholder")]
        public List<ulong> TaskHolder { get; set; } = [];

        [Column("guild_id")]
        [Description("discord id of guild where the todo was posted")]
        public required ulong Guild { get; set; }

        [Column("message_id")]
        [Description("discord id of guild where the todo was posted")]
        public required ulong MessageId { get; set; }

        [Column("is_completed")]
        public bool IsCompleted { get; set; }

        [Column("status")]
        public TodoStatus Status { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("complete_time")]
        public DateTime? CompleteTime { get; set; }

        [Column("time_limit")]
        public DateTime? TimeLimit { get; set; }
    }

}
