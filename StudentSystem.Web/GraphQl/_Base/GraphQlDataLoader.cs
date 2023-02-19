using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using SystemTech.Core.Entities;

namespace StudentSystem.Web.GraphQl._Base
{
    public class GraphQlDataLoader<T> : BatchDataLoader<string, T> where T : class, IBaseEntities
    {
        protected readonly StudentSystemDbContextFactory DbContextFactory;

        public GraphQlDataLoader(IBatchScheduler batchScheduler, StudentSystemDbContextFactory dbContextFactory) : base(batchScheduler)
        {
            DbContextFactory = dbContextFactory;
        }

        protected override async Task<IReadOnlyDictionary<string, T>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync();
            return await dbContext.Set<T>()
                .Where(_ => keys.Contains(_.Id))
                .ToDictionaryAsync(_ => _.Id, cancellationToken);
        }
    }
}