using StudentSystem.DataAccess.EntityFramework.Entities;

namespace StudentSystem.Web.GraphQl.Results
{
    public class ResultType : ObjectType<Result>
    {
        protected override void Configure(IObjectTypeDescriptor<Result> descriptor)
        {
            descriptor.Field(b => b.SubjectId).Type<IdType>();
            descriptor.Field(b => b.StudentId).Type<StringType>();
            descriptor.Field(b => b.Mark).Type<FloatType>();
            descriptor.Field(b => b.CreatedDate).Type<DateTimeType>();

            descriptor.Field("subject")
                .ResolveWith<ResultResolver>(t => t.GetSubject(default!,default!,default!));
            
            descriptor.Field("student")
                .ResolveWith<ResultResolver>(t => t.GetStudent(default!,default!,default!));

            base.Configure(descriptor);
        }
    }
}