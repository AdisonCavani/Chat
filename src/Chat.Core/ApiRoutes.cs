namespace Chat.Core;

public static class ApiRoutes
{
    private const string root = "/api";

    public static class Account
    {
        private const string endpoint = $"{root}/account";

        public const string Login = $"{endpoint}/login";
        public const string ConfirmEmail = $"{endpoint}/confirmEmail";
        public const string Register = $"{endpoint}/register";
        public const string ResendVerificationEmail = $"{endpoint}/resendVerificationEmail";

        public static class Password
        {
            private const string endpoint = $"{Account.endpoint}/password";

            public const string SendRecoveryEmail = $"{endpoint}/recovery";
            public const string VerifyToken = $"{endpoint}/verifyToken";
            public const string Reset = $"{endpoint}/reset";

            public const string Change = $"{endpoint}/change";
        }

        public static class Profile
        {
            private const string endpoint = $"{Account.endpoint}/password";

            public const string Details = $"{endpoint}/details";
        }
    }

    public static class Chat
    {
        private const string endpoint = $"{root}/chat";

        public static class Message
        {
            private const string endpoint = $"{Chat.endpoint}/message";

            public const string Connect = $"{endpoint}/connect";
            public const string Send = $"{endpoint}/send";
            public const string WebSocket = $"{endpoint}/wss";
        }
    }
}
