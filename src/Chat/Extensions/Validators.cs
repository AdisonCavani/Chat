namespace Chat.Extensions;

public static class Validators
{
    public static bool IsEmailAdress(string value)
    {
        if (value is null)
            return false;

        int index = value.IndexOf('@');

        return
            index > 0 &&
            index != value.Length - 1 &&
            index == value.LastIndexOf('@');
    }
}
