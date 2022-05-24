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
        public const string RefreshToken = $"{endpoint}/refreshToken";
        public const string PasswordRecovery = $"{endpoint}/passwordRecovery";
        public const string ResetPassword = $"{endpoint}/resetPassword";
        public const string ResendVerificationEmail = $"{endpoint}/resendVerificationEmail";
        public const string ChangePassword = $"{endpoint}/changePassword";
        public const string Auth = $"{endpoint}/auth";
    }
}
