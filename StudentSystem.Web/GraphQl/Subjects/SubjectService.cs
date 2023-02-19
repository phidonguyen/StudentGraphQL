using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl._Base;
using SystemTech.Core.Utils;

namespace StudentSystem.Web.GraphQl.Subjects
{
    public class SubjectService : BaseService
    {
        private readonly IMapper _mapper;

        public SubjectService(IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            StudentSystemDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
        }

        public IQueryable<Subject> GetSubjects() => DbContext.Subjects;
        
        public async Task<Subject> AddSubject(AddSubjectInput input, CancellationToken cancellationToken)
        {
            Subject subject = _mapper.Map<Subject>(input);
            subject.UpdatedBy = CurrentUserId;

            var subjectEntry = DbContext.Add(subject);

            await DbContext.SaveChangesAsync(cancellationToken);
            return subjectEntry.Entity;
        }

        public async Task<Subject> EditSubject(EditSubjectInput input, CancellationToken cancellationToken)
        {

            Subject subject = _mapper.Map<Subject>(input);
            subject.UpdatedBy = CurrentUserId;

            var subjectDb = await DbContext.Subjects.FirstOrDefaultAsync(_ => _.Id == input.Id, cancellationToken);
            if (subjectDb == null) 
                throw new ValidationException($"Subject [{input.Id}] not found.");

            ReflectionHelpers.MergeFieldsChanged(input,subject, subjectDb);
            var subjectEntry = DbContext.Update(subjectDb);

            await DbContext.SaveChangesAsync(cancellationToken);
            return subjectEntry.Entity;
        }

        public async Task<Subject> RemoveSubject(string id, CancellationToken cancellationToken)
        {
            var subjectDb = await DbContext.Subjects.FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);
            if (subjectDb == null)
                throw new ValidationException($"Subject [{id}] not found.");

            subjectDb.UpdatedBy = CurrentUserId;
            
            var subjectEntry = DbContext.Remove(subjectDb);
            
            await DbContext.SaveChangesAsync(cancellationToken);
            return subjectEntry.Entity;
        }
    }
}
