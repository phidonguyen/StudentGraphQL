using AutoMapper;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl.Results;
using StudentSystem.Web.GraphQl.Students;
using StudentSystem.Web.GraphQl.Subjects;

namespace StudentSystem.Web.Configurations
{
    public class GraphQlMapping : Profile
    {
        public GraphQlMapping()
        {
            StudentMapping();
            SubjectMapping();
            ResultMapping();
        }

        private void StudentMapping()
        {
            CreateMap<AddStudentInput, Student>();
            CreateMap<EditStudentInput, Student>();
        }
        
        private void SubjectMapping()
        {
            CreateMap<AddSubjectInput, Subject>();
            CreateMap<EditSubjectInput, Subject>();
        }
        
        private void ResultMapping()
        {
            CreateMap<AddResultInput, Result>();
            CreateMap<EditResultInput, Result>();
        }
    }
}