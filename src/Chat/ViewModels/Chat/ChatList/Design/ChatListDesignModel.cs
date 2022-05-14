﻿using System.Collections.Generic;

namespace Chat.ViewModels.Chat.ChatList.Design;
public class ChatListDesignModel : ChatListViewModel
{
    public static ChatListDesignModel Instance => new();

    public ChatListDesignModel()
    {
        Items = new List<ChatListItemViewModel>
        {
            new(null)
            {
                Name = "Luke",
                Initials = "LM",
                Message = "This chat app is awesome! I bet it will be fast too",
                ProfilePictureRGB = "3099c5",
                NewContentAvailable = true
            },
            new (null)
            {
                Name = "Jesse",
                Initials = "JA",
                Message = "Hey dude, here are the new icons",
                ProfilePictureRGB = "fe4503"
            },
            new (null)
            {
                Name = "Parnell",
                Initials = "PL",
                Message = "The new server is up, got 192.168.1.1",
                ProfilePictureRGB = "00d405",
                IsSelected = true
            },
            new (null)
            {
                Name = "Luke",
                Initials = "LM",
                Message = "This chat app is awesome! I bet it will be fast too",
                ProfilePictureRGB = "3099c5"
            },
            new (null)
            {
                Name = "Jesse",
                Initials = "JA",
                Message = "Hey dude, here are the new icons",
                ProfilePictureRGB = "fe4503"
            },
            new (null)
            {
                Name = "Parnell",
                Initials = "PL",
                Message = "The new server is up, got 192.168.1.1",
                ProfilePictureRGB = "00d405"
            },
            new (null)
            {
                Name = "Luke",
                Initials = "LM",
                Message = "This chat app is awesome! I bet it will be fast too",
                ProfilePictureRGB = "3099c5"
            },
            new (null)
            {
                Name = "Jesse",
                Initials = "JA",
                Message = "Hey dude, here are the new icons",
                ProfilePictureRGB = "fe4503"
            },
            new (null)
            {
                Name = "Parnell",
                Initials = "PL",
                Message = "The new server is up, got 192.168.1.1",
                ProfilePictureRGB = "00d405"
            },
        };
    }
}
