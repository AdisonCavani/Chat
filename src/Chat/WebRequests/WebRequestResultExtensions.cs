using System.Threading.Tasks;
using Chat.Core.ApiModels;
using Chat.DI.UI;
using Chat.ViewModels.Application;
using Chat.ViewModels.Dialogs;
using Dna;

namespace Chat.WebRequests;

public static class WebRequestResultExtensions
{
    public static async Task<bool> HandleErrorIfFailedAsync(
        this WebRequestResult? response,
        string title,
        SettingsViewModel settingsViewModel,
        IUIManager uiManager)
    {
        // If there was no response, bad data, or a response with a error message...
        if (response is null || response.ServerResponse == null || (response.ServerResponse as ApiResponse)?.Successful == false)
        {
            // Default error message
            // TODO: Localize strings
            var message = "Unknown error from server call";

            // If we got a response from the server...
            if (response?.ServerResponse is ApiResponse apiResponse)
                // Set message to servers response
                message = apiResponse.ErrorMessage;
            // If we have a result but deserialize failed...
            else if (!string.IsNullOrWhiteSpace(response?.RawServerResponse))
                // Set error message
                message = $"Unexpected response from server. {response.RawServerResponse}";
            // If we have a result but no server response details at all...
            else if (response != null)
                // Set message to standard HTTP server response details
                message = response.ErrorMessage ?? $"Server responded with {response.StatusDescription} ({response.StatusCode})";

            // If this is an unauthorized response...
            if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Log it
                // TODO: inject ILogger here!!!
                // Logger.LogInformationSource("Logging user out due to unauthorized response from server");

                // Automatically log the user out
                await settingsViewModel.Logout();
            }
            else
            {
                // Display error
                await uiManager.ShowMessage(new MessageBoxDialogViewModel
                {
                    // TODO: Localize strings
                    Title = title,
                    Message = message ?? string.Empty
                });
            }

            // Return that we had an error
            return true;
        }

        // All was OK, so return false for no error
        return false;
    }
}
