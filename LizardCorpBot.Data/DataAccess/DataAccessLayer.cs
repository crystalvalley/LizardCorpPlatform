namespace LizardCorpBot.Data.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DataAccessLayer.
    /// 비즈니스 로직과 데이터의 분리.
    /// </summary>
    /// <param name="contextFactory">The <see cref="IDbContextFactory"/> that should be inject.</param>
    public partial class DataAccessLayer(IDbContextFactory<LizardBotDbContext> contextFactory)
    {
        private readonly IDbContextFactory<LizardBotDbContext> _contextFactory = contextFactory;
    }
}
