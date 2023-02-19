using FluentValidation;
using StudentSystem.Web.Common.Messages;

namespace StudentSystem.Web.GraphQl.Auth
{
    public class LoginInput
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(input => input.Email)
                .EmailAddress()
                .NotEmpty()
                .WithMessage(CommonMessages.PropertyRequired(PropertyName.User.Email));

            RuleFor(input => input.Password)
                .NotEmpty()
                .WithMessage(CommonMessages.PropertyRequired(PropertyName.User.Password));
        }
    }

    public class LoginInputType : InputObjectType<LoginInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<LoginInput> descriptor)
        {
            descriptor.Field(b => b.Email).Type<StringType>();
            descriptor.Field(b => b.Password).Type<StringType>();
        }
    }
}