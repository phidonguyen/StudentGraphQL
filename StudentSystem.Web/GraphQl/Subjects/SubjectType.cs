using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl.Results;
using StudentSystem.Web.GraphQl.Students;
using StudentSystem.Web.GraphQl.Users;

namespace StudentSystem.Web.GraphQl.Subjects
{
    public class SubjectType : ObjectType<Subject>
    {
        protected override void Configure(IObjectTypeDescriptor<Subject> descriptor)
        {
            descriptor.Field(b => b.Id).Type<IdType>();
            descriptor.Field(b => b.Name).Type<StringType>();
            descriptor.Field(b => b.CreatedBy).Type<StringType>();
            descriptor.Field(b => b.CreatedDate).Type<DateTimeType>();
            descriptor.Field(b => b.UpdatedBy).Type<StringType>();
            descriptor.Field(b => b.UpdatedDate).Type<DateTimeType>();

            descriptor.Field("creator")
                .ResolveWith<UserResolver>(t => t.GetCreator<Subject>(default!,default!,default!));
            
            descriptor.Field("updater")
                .ResolveWith<UserResolver>(t => t.GetUpdater<Subject>(default!,default!,default!));

            descriptor.Field("results")
                .Type<ListType<ResultType>>()
                .ResolveWith<SubjectResolver>(t => t.GetResults(default!,default!,default!));
            
            base.Configure(descriptor);
        }
    }
}
