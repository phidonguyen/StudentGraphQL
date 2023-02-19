using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl._Base;
using SystemTech.Core.Utils;

namespace StudentSystem.Web.GraphQl.Students
{
    public class StudentService : BaseService
    {
        private readonly IMapper _mapper;

        public StudentService(IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            StudentSystemDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
        }

        public IQueryable<Student> GetStudents() => DbContext.Students;

        public async Task<Student> AddStudent(AddStudentInput input, CancellationToken cancellationToken)
        {
            Student student = _mapper.Map<Student>(input);
            student.UpdatedBy = CurrentUserId;

            var studentEntry = DbContext.Add(student);

            await DbContext.SaveChangesAsync(cancellationToken);
            return studentEntry.Entity;
        }

        public async Task<Student> EditStudent(EditStudentInput input, CancellationToken cancellationToken)
        {

            Student student = _mapper.Map<Student>(input);
            student.UpdatedBy = CurrentUserId;

            var studentDb = await DbContext.Students.FirstOrDefaultAsync(_ => _.Id == input.Id, cancellationToken);
            if (studentDb == null) 
                throw new ValidationException($"Student [{input.Id}] not found.");

            ReflectionHelpers.MergeFieldsChanged(input,student, studentDb);
            var studentEntry = DbContext.Update(studentDb);

            await DbContext.SaveChangesAsync(cancellationToken);
            return studentEntry.Entity;
        }

        public async Task<Student> RemoveStudent(string id, CancellationToken cancellationToken)
        {
            var studentDb = await DbContext.Students.FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);
            if (studentDb == null)
                throw new ValidationException($"Student [{id}] not found.");

            studentDb.UpdatedBy = CurrentUserId;
            
            var studentEntry = DbContext.Remove(studentDb);
            
            await DbContext.SaveChangesAsync(cancellationToken);
            return studentEntry.Entity;
        }
    }
}
