using System.Security.Authentication;
using FluentValidation;

namespace StudentSystem.Web.GraphQl._Base
{
    public class GraphQlErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return HandlerException(error);
        }

        private IError HandlerException(IError error)
        {
            if (error.Exception == null) return error;

            if (error.Exception is ValidationException or System.ComponentModel.DataAnnotations.ValidationException)
            {
                return error.WithCode(GraphQlErrorCode.GraphqlValidationFailed)
                    .WithMessage(error.Exception.Message);
            }
            else if (error.Exception is UnauthorizedAccessException)
            {
                return error.WithCode(GraphQlErrorCode.Forbidden)
                    .WithMessage(error.Exception.Message);
            }
            else if (error.Exception is ArgumentException)
            {
                return error.WithCode(GraphQlErrorCode.BadUserInput)
                    .WithMessage(error.Exception.Message);
            }
            else if (error.Exception is AuthenticationException)
            {
                return error.WithCode(GraphQlErrorCode.AuthNotAuthenticated)
                    .WithMessage(error.Exception.Message);
            }

            return error.WithCode(GraphQlErrorCode.InternalServerError)
                .WithMessage("Will be back soon!");
        }
    }
}