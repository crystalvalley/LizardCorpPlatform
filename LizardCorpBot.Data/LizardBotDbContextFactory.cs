namespace LizardCorpBot.Data.Context
{
    using System.Runtime.InteropServices;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// LizardBotDbContext의 팩토리 클래스.
    /// </summary>
    public class LizardBotDbContextFactory : IDesignTimeDbContextFactory<LizardBotDbContext>
    {
        /// <inheritdoc/>
        public LizardBotDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("psqlLocal"));
            else
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("psqlPrd"));

            return new LizardBotDbContext(optionsBuilder.Options);
        }
    }
}
