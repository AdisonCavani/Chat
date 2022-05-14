using System;
using System.Collections.ObjectModel;
using Chat.Core.DataModels;
using Chat.ViewModels.Application;
using Chat.ViewModels.Chat.ChatMessage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chat.ViewModels.Chat.ChatList;

/// <summary>
/// A view model for each chat list item in the overview chat list
/// </summary>
public partial class ChatListItemViewModel : ObservableObject
{
    private readonly ApplicationViewModel _applicationViewModel;

    public ChatListItemViewModel(ApplicationViewModel applicationViewModel)
    {
        _applicationViewModel = applicationViewModel;
    }

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private string? initials;

    [ObservableProperty]
    private string? profilePictureRGB;

    [ObservableProperty]
    private bool newContentAvailable;

    [ObservableProperty]
    private bool isSelected;

    [ICommand]
    public void OpenMessage()
    {
        if (Name == "Jesse")
        {
            _applicationViewModel.GoToPage(ApplicationPage.Login, new LoginViewModel
            {
                Email = "jesse@helloworld.com"
            });
            return;
        }

        _applicationViewModel.GoToPage(ApplicationPage.Chat, new ChatMessageListViewModel
        {
            DisplayTitle = "Parnell, Me",

            Items = new ObservableCollection<ChatMessageListItemViewModel>
            {
                new()
                {
                    Message = Message ?? string.Empty,
                    Initials = Initials ?? string.Empty,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF00FF",
                    SenderName = "Luke",
                    SentByMe = true,
                },
                new()
                {
                    Message = "A received message",
                    Initials = Initials ?? string.Empty,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
                new()
                {
                    Message = "A received message",
                    Initials = Initials ?? string.Empty,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
                new()
                {
                    Message = Message ?? string.Empty,
                    Initials = Initials ?? string.Empty,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF00FF",
                    SenderName = "Luke",
                    SentByMe = true,
                },
                new()
                {
                    Message = "A received message",
                    Initials = Initials ?? string.Empty,
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
                    Initials = Initials ?? string.Empty,
                    MessageSentTime = DateTime.UtcNow,
                    ProfilePictureRGB = "FF0000",
                    SenderName = "Parnell",
                    SentByMe = false,
                },
            }
        });
    }
}
