using FluentValidation;
using StudentSystem.Web.Common.Messages;

namespace StudentSystem.Web.GraphQl.Students
{
    public class AddStudentInput
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; }
        public int IdentityNumber { get; set; }
    }

    public class AddStudentInputTypeValidator : AbstractValidator<AddStudentInput>
    {
        public AddStudentInputTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(CommonMessages.PropertyRequired(PropertyName.Student.Name));
        }
    }

    public class AddStudentInputType : InputObjectType<AddStudentInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddStudentInput> descriptor)
        {
            descriptor.Field(b => b.Name).Type<StringType>();
            descriptor.Field(b => b.PhoneNumber).Type<StringType>();
            descriptor.Field(b => b.IdentityNumber).Type<IntType>();

            base.Configure(descriptor);
        }
    }
}