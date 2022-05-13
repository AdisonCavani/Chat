using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chat.Core.DataModels;
using Chat.ViewModel.Application;
using Chat.ViewModel.Chat.ChatMessage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Chat.DI.DI;

namespace Chat.ViewModel.Chat.ChatList;

/// <summary>
/// A view model for each chat list item in the overview chat list
/// </summary>
public partial class ChatListItemViewModel : ObservableObject
{
    public string Name { get; set; }

    public string Message { get; set; }

    public string Initials { get; set; }

    public string ProfilePictureRGB { get; set; }

    public bool NewContentAvailable { get; set; }

    public bool IsSelected { get; set; }

    [ICommand]
    public void OpenMessage()
    {
        if (Name == "Jesse")
        {
            ViewModelApplication.GoToPage(ApplicationPage.Login, new LoginViewModel
            {
                Email = "jesse@helloworld.com"
            });
            return;
        }

        ViewModelApplication.GoToPage(ApplicationPage.Chat, new ChatMessageListViewModel
        {
            DisplayTitle = "Parnell, Me",

            Items = new ObservableCollection<ChatMessageListItemViewModel>
            {
                new()
                {
                    Message = Message,
                    Initials = Initials,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF00FF",
                    SenderName = "Luke",
                    SentByMe = true,
                },
                new()
                {
                    Message = "A received message",
                    Initials = Initials,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
                new()
                {
                    Message = "A received message",
                    Initials = Initials,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
                new()
                {
                    Message = Message,
                    Initials = Initials,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF00FF",
                    SenderName = "Luke",
                    SentByMe = true,
                },
                new()
                {
                    Message = "A received message",
                    Initials = Initials,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
                new()
                {
                    Message = "A received message",
                    ImageAttachment = new()
                    {
                        ThumbnailUrl = "http://anywhere"
                    },
                    Initials = Initials,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
            }
        });
    }
}
