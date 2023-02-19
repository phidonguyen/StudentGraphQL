using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl._Base;
using StudentSystem.Web.Common.Constants;
using StudentSystem.Web.GraphQl.Students;
using StudentSystem.Web.GraphQl.Subjects;

namespace StudentSystem.Web.GraphQl._Type
{
    public class Query
    {
        #region Student
        
        [UseOffsetPaging(MaxPageSize = GeneralConstants.MaxPageSize,
            DefaultPageSize = GeneralConstants.PageSize,
            IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Student> GetStudents(StudentService studentService) => studentService.GetStudents();
        
        public async Task<Student> GetStudent(string id, GraphQlDataLoader<Student> dataLoader,
            CancellationToken cancellationToken)
            => await dataLoader.LoadAsync(id, cancellationToken);
        
        #endregion
        
        #region Subject
        
        [UseOffsetPaging(MaxPageSize = GeneralConstants.MaxPageSize,
            DefaultPageSize = GeneralConstants.PageSize,
            IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Subject> GetSubjects(SubjectService subjectService) => subjectService.GetSubjects();
        
        public async Task<Subject> GetSubject(string id, GraphQlDataLoader<Subject> dataLoader,
            CancellationToken cancellationToken)
            => await dataLoader.LoadAsync(id, cancellationToken);
        
        #endregion
    }
}