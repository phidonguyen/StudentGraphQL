using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.GraphQl.Results;
using StudentSystem.Web.GraphQl.Users;

namespace StudentSystem.Web.GraphQl.Students
{
    public class StudentType : ObjectType<Student>
    {
        protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
        {
            descriptor.Field(b => b.Id).Type<IdType>();
            descriptor.Field(b => b.Name).Type<StringType>();
            descriptor.Field(b => b.PhoneNumber).Type<StringType>();
            descriptor.Field(b => b.IdentityNumber).Type<IntType>();
            descriptor.Field(b => b.CreatedBy).Type<StringType>();
            descriptor.Field(b => b.CreatedDate).Type<DateTimeType>();
            descriptor.Field(b => b.UpdatedBy).Type<StringType>();
            descriptor.Field(b => b.UpdatedDate).Type<DateTimeType>();

            descriptor.Field("creator")
                .ResolveWith<UserResolver>(t => t.GetCreator<Student>(default!,default!,default!));
            
            descriptor.Field("updater")
                .ResolveWith<UserResolver>(t => t.GetUpdater<Student>(default!,default!,default!));
            
            descriptor.Field("results")
                .Type<ListType<ResultType>>()
                .ResolveWith<StudentResolver>(t => t.GetResults(default!,default!,default!));

            base.Configure(descriptor);
        }
    }
}
