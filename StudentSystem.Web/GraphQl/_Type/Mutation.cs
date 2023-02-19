using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl.Auth;
using StudentSystem.Web.GraphQl.Results;
using StudentSystem.Web.GraphQl.Students;
using StudentSystem.Web.GraphQl.Subjects;

namespace StudentSystem.Web.GraphQl._Type
{
    public class Mutation
    {
        #region Auth

        public async Task<Token> Login(AuthService authService, LoginInput input, CancellationToken cancellationToken)
        {
            var tokenEntry = await authService.Login(input, cancellationToken);
            return tokenEntry;
        }

        public async Task<Token> RefreshToken(AuthService authService, RefreshTokenInput input, CancellationToken cancellationToken)
        {
            var tokenEntry = await authService.RefreshToken(input, cancellationToken);
            return tokenEntry;
        }

        #endregion
        
        #region Student
     
        public async Task<Student> AddStudent(StudentService studentService, AddStudentInput input, 
            CancellationToken cancellationToken)
        {
            var studentEntry = await studentService.AddStudent(input, cancellationToken);
            return studentEntry;
        }
        
        public async Task<Student> EditStudent(StudentService studentService, EditStudentInput input,
            CancellationToken cancellationToken)
        {
            var studentEntry = await studentService.EditStudent(input, cancellationToken);
            return studentEntry;
        }
        
        public async Task<Student> RemoveStudent(StudentService studentService, string id, 
            CancellationToken cancellationToken)
        {
            var studentEntry = await studentService.RemoveStudent(id, cancellationToken);
            return studentEntry;
        }

        #endregion
        
        #region Subject
     
        public async Task<Subject> AddSubject(SubjectService subjectService, AddSubjectInput input, 
            CancellationToken cancellationToken)
        {
            var subjectEntry = await subjectService.AddSubject(input, cancellationToken);
            return subjectEntry;
        }
        
        public async Task<Subject> EditSubject(SubjectService subjectService, EditSubjectInput input,
            CancellationToken cancellationToken)
        {
            var subjectEntry = await subjectService.EditSubject(input, cancellationToken);
            return subjectEntry;
        }
        
        public async Task<Subject> RemoveSubject(SubjectService subjectService, string id, 
            CancellationToken cancellationToken)
        {
            var subjectEntry = await subjectService.RemoveSubject(id, cancellationToken);
            return subjectEntry;
        }

        #endregion
        
        #region Result
     
        public async Task<Result> AddResult(ResultService resultService, AddResultInput input, 
            CancellationToken cancellationToken)
        {
            var resultEntry = await resultService.AddResult(input, cancellationToken);
            return resultEntry;
        }
        
        public async Task<Result> EditResult(ResultService resultService, EditResultInput input,
            CancellationToken cancellationToken)
        {
            var resultEntry = await resultService.EditResult(input, cancellationToken);
            return resultEntry;
        }

        #endregion
    }   
}