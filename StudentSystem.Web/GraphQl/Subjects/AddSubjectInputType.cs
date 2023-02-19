using FluentValidation;
using StudentSystem.Web.Common.Messages;

namespace StudentSystem.Web.GraphQl.Subjects
{
    public class AddSubjectInput
    {
        public string Name { get; set; } = null!;
    }

    public class AddSubjectInputTypeValidator : AbstractValidator<AddSubjectInput>
    {
        public AddSubjectInputTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(CommonMessages.PropertyRequired(PropertyName.Subject.Name));
        }
    }

    public class AddSubjectInputType : InputObjectType<AddSubjectInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddSubjectInput> descriptor)
        {
            descriptor.Field(b => b.Name).Type<StringType>();

            base.Configure(descriptor);
        }
    }
}