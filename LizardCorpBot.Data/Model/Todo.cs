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
    using Discord;
    using Discord.WebSocket;

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

    public static class TodoStatusExtension
    {
        public static string GetValue(this TodoStatus st)
        {
            switch (st)
            {
                case TodoStatus.Posted:
                    return "게시됨.";
                case TodoStatus.Canceled:
                    return "취소됨.";
                case TodoStatus.Complete:
                    return "완료됨.";
                default:
                    return string.Empty;
            }
        }
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
        [Description("discord id of message where the todo was posted")]
        public ulong MessageId { get; set; }

        [Column("status")]
        public TodoStatus Status { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("complete_time")]
        public DateTime? CompleteTime { get; set; }

        [Column("time_limit")]
        public DateTime? TimeLimit { get; set; }

        [Column("confirmer")]
        [Description("discord id of user who check to confirming cancle or complete todo")]
        public ulong ConfirmerId { get; set; }

        /// <summary>
        /// 승인자 확인, todo담당자 또는 작성자.
        /// </summary>
        /// <param name="userId">확인할 유저의 discord id.</param>
        /// <returns>승인자가 맞으면 true.</returns>
        public bool IsComfirmer(ulong userId)
        {
            return Author == userId || TaskHolder.Contains(userId);
        }

        /// <summary>
        /// 담당자 추가, 제거 메소드.
        /// 이미 todo가 완료거나 취소되면 변경이 불가능함.
        /// </summary>
        /// <param name="userId">추가 또는 제거될 담당자.</param>
        /// <returns>변경되면 true, 변경되지 않았다면 false.</returns>
        public bool ToggleTaskHold(ulong userId)
        {
            if (Status != TodoStatus.Posted) return false;
            if (!TaskHolder.Remove(userId)) TaskHolder.Add(userId);
            return true;
        }

        /// <summary>
        /// todo의 취소 또는 취소의 롤백.
        /// </summary>
        /// <param name="userId">취소한 유저.</param>
        /// <returns>취소면 true, 롤백이면 false.</returns>
        public bool ToggleCancel(ulong userId)
        {
            ConfirmerId = userId;
            if (Status == TodoStatus.Canceled)
            {
                Status = TodoStatus.Posted;
                CompleteTime = null;
                return false;
            }

            Status = TodoStatus.Canceled;
            CompleteTime = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// todo의 작업 완료 또는 롤백.
        /// </summary>
        /// <param name="userId">완료한 유저.</param>
        /// <returns>완료면 true, 롤백이면 false.</returns>
        public bool ToggleComplete(ulong userId)
        {
            ConfirmerId = userId;
            if (Status == TodoStatus.Complete)
            {
                Status = TodoStatus.Posted;
                CompleteTime = null;
                return false;
            }

            Status = TodoStatus.Complete;
            CompleteTime = DateTime.UtcNow;
            return true;
        }

        public EmbedBuilder GetTodoEmbed(SocketGuild guild)
        {
            var embed = new EmbedBuilder()
            {
                Title = Title,
            };
            embed.AddField("작성자", guild.GetUser(Author).Mention, true);
            string holders = string.Empty;
            foreach (var holder in TaskHolder)
            {
                holders += guild.GetUser(holder).Mention + "\r\n";
            }

            embed.AddField("담당자", holders == string.Empty ? "미정" : holders, true);

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
            if (TimeLimit == null) embed.AddField("마감", "미정", true);
            else
            {
                var dt = TimeZoneInfo.ConvertTime(TimeLimit.Value, timeZone);
                embed.AddField("마감", dt.ToString("yyyy/MM/dd"), true);
            }

            embed.WithFooter(Status.GetValue());

            return embed;
        }

    }
}
