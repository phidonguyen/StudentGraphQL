namespace StudentSystem.Web.Common.Messages
{
    public class PropertyName
    {
        public static class User
        {
            public const string Id = "User";
            public const string Email = "Email";
            public const string Name = "Name";
            public const string Password = "Password";
        }
        
        public static class Token
        {
            public const string AccessToken = "AccessToken";
            public const string RefreshToken = "RefreshToken";
        }
        
        public static class Student
        {
            public const string Id = "Student";
            public const string Name = "Name";
        }
        
        public static class Subject
        {
            public const string Id = "Subject";
            public const string Name = "Name";
        }
    }
}
