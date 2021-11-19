using System.Threading.Tasks;
using Dna;

namespace Chat.Core;

/// <summary>
/// Extension methods for the <see cref="WebRequestResultExtensions"/>
/// </summary>
public static class WebRequestResultExtensions
{
    /// <summary>
    /// Checks the web request result for any errors, displaying them if there are any
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="response"></param>
    /// <param name="title">The title of the error dialog if there is an error</param>
    /// <returns>Returns true if there was an error, or false if all was OK</returns>
    public static async Task<bool> DisplayErrorIfFailedAsync<T>(this WebRequestResult<ApiResponse<T>> response, string title)
    {
        // If there was no response, bad data or a response with a error message
        if (response is null || response.ServerResponse is null || !response.ServerResponse.Successful)
        {
            // TODO: Localize strings
            // Default error message
            var message = "Unknown error from server call";

            // If we got a response from the server
            if (response?.ServerResponse is not null)
                // Set message to server response
                message = response.ServerResponse.ErrorMessage;

            // If we have a result, but deserialize failed
            else if (string.IsNullOrWhiteSpace(response?.RawServerResponse))
                // Set error message
                message = $"Unexpected response from server. {response.RawServerResponse}";

            // If we have a result, but no server response details at all
            else if (response is not null)
                // Set message to standard HTTP server response details
                message = $"Failed to communicate with server. Status code: {response.StatusCode}. {response.StatusDescription}";

            // Display error
            await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
            {
                // TODO: Localize strings
                Title = title,
                Message = message
            });

            // We had an error
            return true;
        }

        // All was OK, no errors reported
        return false;
    }
}