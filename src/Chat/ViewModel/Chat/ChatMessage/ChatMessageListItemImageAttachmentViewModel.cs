using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModel.Chat.ChatMessage;

/// <summary>
/// A view model for each chat message thread item's attachment
/// (in this case an image) in a chat thread
/// </summary>
[INotifyPropertyChanged]
public partial class ChatMessageListItemImageAttachmentViewModel
{
    private string? thumbnailUrl;

    public string? ThumbnailUrl
    {
        get => thumbnailUrl;
        set
        {
            if (SetProperty(ref thumbnailUrl, value))
                OnPropertyChanged(nameof(ThumbnailUrl));

            // TODO: Download image from website
            //       Save file to local storage/cache
            //       Set LocalFilePath value
            //
            //       For now, just set the file path directly
            Task.Delay(2000).ContinueWith(_ => LocalFilePath = "/Images/Samples/rusty.jpg");
        }
    }

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? fileName;

    [ObservableProperty]
    private long fileSize; // Size in bytes

    [ObservableProperty]
    private string? localFilePath;

    public bool ImageLoaded => localFilePath is not null;
}
