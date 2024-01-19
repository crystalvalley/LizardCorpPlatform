#pragma warning disable SA1600 // Elements should be documented
namespace LizardCorpBot.Data
{
    using LizardCorpBot.Data.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    /// <summary>
    /// LizardBot용 DbContext.
    /// </summary>
    /// <remarks>
    /// <see cref="LizardBotDbContext"/> 생성자.
    /// </remarks>
    /// <param name="options">The <see cref="DbContextOptions"/> that should be inject.</param>
    public class LizardBotDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Guild> Guilds { get; set; }

        public DbSet<GuildUser> GuildUsers { get; set; }

        public DbSet<MinecraftUser> MinecraftUsers { get; set; }

        public DbSet<Todo> Todos { get; set; }
    }
}
