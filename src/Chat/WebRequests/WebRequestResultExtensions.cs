﻿using System.Threading.Tasks;
using Chat.Core.ApiModels;
using Chat.ViewModel.Dialogs;
using Dna;
using static Chat.DI.DI;
using static Dna.FrameworkDI;

namespace Chat.WebRequests;

/// <summary>
/// Extension methods for the <see cref="WebRequestResultExtensions"/> class
/// </summary>
public static class WebRequestResultExtensions
{
    /// <summary>
    /// Checks the web request result for any errors, displaying them if there are any,
    /// or if we are unauthorized automatically logging us out
    /// </summary>
    /// <param name="response">The response to check</param>
    /// <param name="title">The title of the error dialog if there is an error</param>
    /// <returns>Returns true if there was an error, or false if all was OK</returns>
    public static async Task<bool> HandleErrorIfFailedAsync(this WebRequestResult? response, string title)
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
                Logger.LogInformationSource("Logging user out due to unauthorized response from server");

                // Automatically log the user out
                await ViewModelSettings.LogoutAsync();
            }
            else
            {
                // Display error
                await UI.ShowMessage(new MessageBoxDialogViewModel
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
