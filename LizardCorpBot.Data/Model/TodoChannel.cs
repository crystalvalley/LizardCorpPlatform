#pragma warning disable SA1600 // Elements should be documented
namespace LizardCorpBot.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("todo_channel")]
    public class TodoChannel
    {
        [Key]
        [Column("guild_id")]
        public ulong GuildId { get; set; }

        [Column("channel_id")]
        public ulong ChannelId { get; set; }
    }
}
