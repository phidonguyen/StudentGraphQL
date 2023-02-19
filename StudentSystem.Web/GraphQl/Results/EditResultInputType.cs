using FluentValidation;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Common.Messages;
using StudentSystem.Web.Common.Validators;

namespace StudentSystem.Web.GraphQl.Results
{
    public class EditResultInput
    {
        public string SubjectId { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public double Mark { get; set; }
    }

    public class EditResultInputTypeValidator : AbstractValidator<EditResultInput>
    {
        public EditResultInputTypeValidator(PredicateValidators predicateValidators)
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

    public class EditResultInputType : InputObjectType<EditResultInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<EditResultInput> descriptor)
        {
            descriptor.Field(b => b.StudentId).Type<StringType>();
            descriptor.Field(b => b.SubjectId).Type<StringType>();
            descriptor.Field(b => b.Mark).Type<FloatType>();

            base.Configure(descriptor);
        }
    }
}