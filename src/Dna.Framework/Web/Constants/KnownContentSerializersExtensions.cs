namespace Dna;

/// <summary>
/// Extension methods for <see cref="KnownContentSerializersExtensions"/>
/// </summary>
public static class KnownContentSerializersExtensions
{
    /// <summary>
    /// Takes a known serializer type and returns the Mime type associated with it
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public static string ToMimeString(this KnownContentSerializers serializer)
    {
        switch (serializer)
        {
            case KnownContentSerializers.Json:
                return "application/json";

            case KnownContentSerializers.Xml:
                return "application/xml";

            default:
                return "application/octet-stream";
        }
    }
}
