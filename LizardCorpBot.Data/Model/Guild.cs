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
    /// Discord Bot이 참여중인 길드.
    /// 우리 길드만 데이터 수집.
    /// </summary>
    [Table("guild")]
    public class Guild
    {
        [Key]
        [Column("guild_id")]
        [Description("dicord guild id")]
        public required ulong GuildId { get; set; }

        [Column("name")]
        [Description("discord guild name")]
        public required string Name { get; set; }
    }
}
