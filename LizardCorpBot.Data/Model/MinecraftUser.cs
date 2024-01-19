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
    /// 우리 서버 Minecraft 유저데이터랑, Discord 유저데이터 매핑용.
    /// 마인크래프 접속 시 유저정보 선 반영, 디스코드 유저 id는 커맨드로 등록가능하게.
    /// </summary>
    [Table("minecraft_user")]
    public class MinecraftUser
    {
        [Key]
        [Column("name")]
        [Description("minecraft character name")]
        public required string Name { get; set; }

        [Column("user_id")]
        [Description("discord user id")]
        public ulong? UserId { get; set; }

        [Column("last_joined")]
        [Description("lastjoined time of minecraft server")]
        public DateTime? LastJoined { get; set; }

        [Column("playtime")]
        [Description("playtime in minecraft server")]
        public long PlayTime { get; set; }

        [Column("is_playing")]
        [Description("is user palying minecraft")]
        public bool IsPlaying { get; set; }
    }
}
