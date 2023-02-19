using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl.Results;

namespace StudentSystem.Web.GraphQl.Students
{
    public class StudentResolver
    {
        public async Task<IEnumerable<Result>> GetResults(
            [Parent] Student student,
            ResultsByStudentIdDataLoader resultsByStudentIdDataLoader,
            CancellationToken cancellationToken)
        {
            return await resultsByStudentIdDataLoader.LoadAsync(student.Id, cancellationToken);
        }
    }   
}