using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl._Base;

namespace StudentSystem.Web.GraphQl.Results
{
    public class ResultResolver
    {
        public async Task<Subject> GetSubject(
            [Parent] Result result, 
            GraphQlDataLoader<Subject> dataLoader,
            CancellationToken cancellationToken)
        {
            return await dataLoader.LoadAsync(result.SubjectId, cancellationToken);
        }
        
        public async Task<Student> GetStudent(
            [Parent] Result result, 
            GraphQlDataLoader<Student> dataLoader,
            CancellationToken cancellationToken)
        {
            return await dataLoader.LoadAsync(result.StudentId, cancellationToken);
        }
    }   
}