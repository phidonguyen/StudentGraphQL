using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl._Base;
using SystemTech.Core.Utils;

namespace StudentSystem.Web.GraphQl.Results
{
    public class ResultService : BaseService
    {
        private readonly IMapper _mapper;

        public ResultService(IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            StudentSystemDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
        }

        public async Task<Result> AddResult(AddResultInput input, CancellationToken cancellationToken)
        {
            Result result = _mapper.Map<Result>(input);
            
            var resultDb = await DbContext.Results.FirstOrDefaultAsync(_ => _.StudentId == input.StudentId 
                                                                            && _.SubjectId == input.SubjectId, cancellationToken);
            if (resultDb != null) 
                throw new ValidationException("Result already exists.");
            
            var resultEntry = DbContext.Add(result);

            await DbContext.SaveChangesAsync(cancellationToken);
            return resultEntry.Entity;
        }

        public async Task<Result> EditResult(EditResultInput input, CancellationToken cancellationToken)
        {

            Result result = _mapper.Map<Result>(input);

            var resultDb = await DbContext.Results.FirstOrDefaultAsync(_ => _.StudentId == input.StudentId 
                                                                            && _.SubjectId == input.SubjectId, cancellationToken);
            if (resultDb == null) 
                throw new ValidationException("Result not found.");

            ReflectionHelpers.MergeFieldsChanged(input,result, resultDb);
            var resultEntry = DbContext.Update(resultDb);

            await DbContext.SaveChangesAsync(cancellationToken);
            return resultEntry.Entity;
        }
    }
}