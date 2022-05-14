using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chat.ViewModels;

public partial class SignalrViewModel : ObservableObject
{
    private readonly HubConnection connection;

    public SignalrViewModel()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7181/chathub")
            .WithAutomaticReconnect()
            .Build();

        ConfigureConnection();
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    string message = null;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanSend))]
    bool connectionOpened;

    public bool CanSend => !string.IsNullOrWhiteSpace(message) && ConnectionOpened;

    public ObservableCollection<string> Messages { get; private set; } = new();

    [ICommand]
    async void OpenConnection()
    {
        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Messages.Add($"{user}: {message}");
        });

        try
        {
            await connection.StartAsync();
            Messages.Add("Connection Started");
            ConnectionOpened = true;
        }
        catch (Exception ex)
        {
            Messages.Add(ex.Message);
        }
    }

    [ICommand]
    async void SendMessage()
    {
        try
        {
            await connection.InvokeAsync("SendMessage",
                "WPF Client", Message);
        }
        catch (Exception ex)
        {
            Messages.Add(ex.Message);
        }

        Message = string.Empty;
    }

    void ConfigureConnection()
    {
        connection.Reconnecting += (sender) =>
        {
            Messages.Add("Attempting to reconnect...");
            ConnectionOpened = false;

            return Task.CompletedTask;
        };

        connection.Reconnected += (sender) =>
        {
            Messages.Add("Reconnected to the server");
            ConnectionOpened = true;

            return Task.CompletedTask;
        };

        connection.Closed += (sender) =>
        {
            Messages.Add("Connection Closed");
            ConnectionOpened = false;

            return Task.CompletedTask;
        };
    }
}
