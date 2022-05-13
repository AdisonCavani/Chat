using System.Net;
using System.Text;
using System.Xml.Serialization;
using Dna;
using Newtonsoft.Json;

namespace Chat.Core.Extensions;
public static class WebRequestsHelpers
{
    public static async Task<HttpWebResponse> GetAsync(string url, Action<HttpWebRequest> configureRequest = null, string bearerToken = null)
    {
        // Create the web request
        var request = WebRequest.CreateHttp(url);

        // Make it a GET request method
        request.Method = HttpMethod.Get.ToString();

        // If we have a bearer token...
        if (bearerToken != null)
            // Add bearer token to header
            request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {bearerToken}");

        // Any custom work
        configureRequest?.Invoke(request);

        // Wrap call...
        try
        {
            // Return the raw server response
            return await request.GetResponseAsync() as HttpWebResponse;
        }
        // Catch Web Exceptions (which throw for things like 401)
        catch (WebException ex)
        {
            // If we got a response...
            if (ex.Response is HttpWebResponse httpResponse)
                // Return the response
                return httpResponse;

            // Otherwise, we don't have any information to be able to return
            // So re-throw
            throw;
        }
    }

    public static async Task<WebRequestResult<TResponse>> GetAsync<TResponse>(string url, Action<HttpWebRequest> configureRequest = null, string bearerToken = null, KnownContentSerializers returnType = KnownContentSerializers.Json)
    {
        // Create server response holder
        var serverResponse = default(HttpWebResponse);

        try
        {
            // Make the standard Post call first
            serverResponse = await GetAsync(url, configureRequest, bearerToken);
        }
        catch (Exception ex)
        {
            // If we got unexpected error, return that
            return new WebRequestResult<TResponse>
            {
                // Include exception message
                ErrorMessage = ex.Message
            };
        }

        // Create a result
        var result = serverResponse.CreateWebRequestResult<TResponse>();

        // If the response status code is not 200...
        if (result.StatusCode != HttpStatusCode.OK)
        {
            // Call failed
            // Return no error message so the client can display its own based on the status code

            // Done
            return result;
        }

        // If we have no content to deserialize...
        if (result.RawServerResponse.IsNullOrEmpty())
            // Done
            return result;

        // Deserialize raw response
        try
        {
            // If the server response type was not the expected type...
            if (!serverResponse.ContentType.ToLower().Contains(returnType.ToMimeString().ToLower()))
            {
                // Fail due to unexpected return type
                result.ErrorMessage = $"Server did not return data in expected type. Expected {returnType.ToMimeString()}, received {serverResponse.ContentType}";

                // Done
                return result;
            }

            // Json?
            if (returnType == KnownContentSerializers.Json)
            {
                // Deserialize Json string
                result.ServerResponse = JsonConvert.DeserializeObject<TResponse>(result.RawServerResponse);
            }
            // Xml?
            else if (returnType == KnownContentSerializers.Xml)
            {
                // Create Xml serializer
                var xmlSerializer = new XmlSerializer(typeof(TResponse));

                // Create a memory stream for the raw string data
                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result.RawServerResponse)))
                    // Deserialize XML string
                    result.ServerResponse = (TResponse)xmlSerializer.Deserialize(memoryStream);
            }
            // Unknown
            else
            {
                // If deserialize failed then set error message
                result.ErrorMessage = "Unknown return type, cannot deserialize server response to the expected type";

                // Done
                return result;
            }
        }
        catch (Exception)
        {
            // If deserialize failed then set error message
            result.ErrorMessage = "Failed to deserialize server response to the expected type";

            // Done
            return result;
        }

        // Return result
        return result;
    }
}
