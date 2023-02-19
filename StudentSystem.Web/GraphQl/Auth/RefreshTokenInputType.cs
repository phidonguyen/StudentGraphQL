using FluentValidation;
using StudentSystem.Web.Common.Messages;

namespace StudentSystem.Web.GraphQl.Auth
{
    public class RefreshTokenInput
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenInputValidator : AbstractValidator<RefreshTokenInput>
    {
        public RefreshTokenInputValidator()
        {
            RuleFor(input => input.AccessToken)
                .NotEmpty()
                .WithMessage(CommonMessages.PropertyRequired(PropertyName.Token.AccessToken));

            RuleFor(input => input.RefreshToken)
                .NotEmpty()
                .WithMessage(CommonMessages.PropertyRequired(PropertyName.Token.RefreshToken));
        }
    }

    public class RefreshTokenInputType : InputObjectType<RefreshTokenInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RefreshTokenInput> descriptor)
        {
            descriptor.Field(b => b.AccessToken).Type<StringType>();
            descriptor.Field(b => b.RefreshToken).Type<StringType>();
        }
    }
}