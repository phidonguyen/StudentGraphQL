using FluentValidation;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Common.Messages;
using StudentSystem.Web.Common.Validators;
using StringType = HotChocolate.Types.StringType;

namespace StudentSystem.Web.GraphQl.Results
{
    public class AddResultInput
    {
        public string SubjectId { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public double Mark { get; set; }
    }

    public class AddResultInputTypeValidator : AbstractValidator<AddResultInput>
    {
        public AddResultInputTypeValidator(PredicateValidators predicateValidators)
        {
            Predicate<string> idStudentIsExist = predicateValidators.IsExist<Student>;
            Predicate<string> idSubjectIsExist = predicateValidators.IsExist<Subject>;

            RuleFor(input => input.StudentId)
                .Must(id => idStudentIsExist(id))
                .WithMessage(CommonMessages.PropertyNotExist(PropertyName.Student.Id));
            
            RuleFor(input => input.SubjectId)
                .Must(id => idSubjectIsExist(id))
                .WithMessage(CommonMessages.PropertyNotExist(PropertyName.Subject.Id));
        }
    }

    public class AddResultInputType : InputObjectType<AddResultInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddResultInput> descriptor)
        {
            descriptor.Field(b => b.StudentId).Type<StringType>();
            descriptor.Field(b => b.SubjectId).Type<StringType>();
            descriptor.Field(b => b.Mark).Type<FloatType>();

            base.Configure(descriptor);
        }
    }
}