namespace Chat.Core;

public static class ApiRoutes
{
    private const string root = "api";

    public static class Account
    {
        private const string endpoint = $"{root}/account";

        public const string Login = $"{endpoint}/login";
        public const string ConfirmEmail = $"{endpoint}/confirmEmail";
        public const string Register = $"{endpoint}/register";
        public const string ResendVerificationEmail = $"{endpoint}/resendVerificationEmail";
        public const string Auth = $"{endpoint}/auth";

        public static class Password
        {
            private const string endpoint = $"{Account.endpoint}/password";

            public const string SendRecoveryEmail = $"{endpoint}/recovery";
            public const string VerifyToken = $"{endpoint}/verifyToken";
            public const string Reset = $"{endpoint}/reset";

            public const string Change = $"{endpoint}/change";
        }
    }
}
