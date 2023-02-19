using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl.Results;

namespace StudentSystem.Web.GraphQl.Subjects
{
    public class SubjectResolver
    {
        public async Task<IEnumerable<Result>> GetResults(
            [Parent] Subject subject,
            ResultsBySubjectIdDataLoader resultsBySubjectIdDataLoader,
            CancellationToken cancellationToken)
        {
            return await resultsBySubjectIdDataLoader.LoadAsync(subject.Id, cancellationToken);
        }
    }
}