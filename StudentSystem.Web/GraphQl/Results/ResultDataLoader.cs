using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;

namespace StudentSystem.Web.GraphQl.Results
{
    public class ResultsByStudentIdDataLoader : GroupedDataLoader<string, Result>
    {
        private readonly StudentSystemDbContextFactory _dbContextFactory;
        
        public ResultsByStudentIdDataLoader(StudentSystemDbContextFactory dbContextFactory, IBatchScheduler batchScheduler, DataLoaderOptions options = null) : base(batchScheduler, options)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override async Task<ILookup<string, Result>> LoadGroupedBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            var podRadioGuests = await dbContext.Results
                .Where(_ => keys.Contains(_.StudentId))
                .ToListAsync(cancellationToken);
            
            return podRadioGuests.ToLookup(_ => _.StudentId);
        }
    }
    
    public class ResultsBySubjectIdDataLoader : GroupedDataLoader<string, Result>
    {
        private readonly StudentSystemDbContextFactory _dbContextFactory;
        
        public ResultsBySubjectIdDataLoader(StudentSystemDbContextFactory dbContextFactory, IBatchScheduler batchScheduler, DataLoaderOptions options = null) : base(batchScheduler, options)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override async Task<ILookup<string, Result>> LoadGroupedBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            var podRadioGuests = await dbContext.Results
                .Where(_ => keys.Contains(_.SubjectId))
                .ToListAsync(cancellationToken);
            
            return podRadioGuests.ToLookup(_ => _.SubjectId);
        }
    }
}