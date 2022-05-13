namespace Chat.ViewModel.Input.Design;

public class TextEntryDesignModel : TextEntryViewModel
{
    public static TextEntryDesignModel Instance => new();

    public TextEntryDesignModel()
    {
        Label = "Name";
        OriginalText = "Luke Malpass";
        EditedText = "Editing :)";
    }
}
