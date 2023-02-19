using StudentSystem.Web.GraphQl.Students;
using StudentSystem.Web.GraphQl.Subjects;

namespace StudentSystem.Web.GraphQl._Type
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.BindFieldsExplicitly();

            #region Student

            descriptor.Field(_ => _.GetStudents(default!))
                .Type<ListType<StudentType>>()
                .Authorize();
            descriptor.Field(_ => _.GetStudent(default!, default!, default!))
                .Type<StudentType>()
                .Authorize();

            #endregion
            
            #region Subject

            descriptor.Field(_ => _.GetSubjects(default!))
                .Type<ListType<SubjectType>>()
                .Authorize();
            descriptor.Field(_ => _.GetSubject(default!, default!, default!))
                .Type<SubjectType>()
                .Authorize();

            #endregion
        }
    }   
}