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
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Discord 길드 유저.
    /// 우리 길드만 데이터 수집.
    /// </summary>
    [Table("guild_user")]
    [PrimaryKey(nameof(GuildId), nameof(UserId))]
    public class GuildUser
    {
        [Column("guild_id")]
        [Description("dicord guild id")]
        public required ulong GuildId { get; set; }

        [Column("user_id")]
        [Description("discord user name")]
        public required ulong UserId { get; set; }

        [Column("user_name")]
        [Description("discord user name")]
        public required string Username { get; set; }

        [Column("avatar_url")]
        [Description("url of avatar image")]
        public string? AvatarUrl { get; set; }

    }
}
