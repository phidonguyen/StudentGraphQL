namespace StudentSystem.Web.GraphQl._Base
{
    public class GraphQlErrorCode
    {
        public const string GraphqlValidationFailed = "GRAPHQL_VALIDATION_FAILED";
        public const string BadUserInput = "BAD_USER_INPUT";
        public const string Unauthenticated = "UNAUTHENTICATED";
        public const string Forbidden = "FORBIDDEN";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string AuthNotAuthenticated = "AUTH_NOT_AUTHENTICATED";
    }
}