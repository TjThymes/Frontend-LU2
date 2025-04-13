using System;
using System.Text;
using Newtonsoft.Json.Linq; // ✅ make sure you have Newtonsoft.Json installed!

public static class JwtDecoder
{
    public static string GetUserIdFromToken(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3)
            throw new Exception("Invalid JWT token format");

        string payload = parts[1];

        // Add padding if needed
        payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

        byte[] jsonBytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
        string json = Encoding.UTF8.GetString(jsonBytes);

        var payloadData = JObject.Parse(json);

        // "sub" is the standard JWT field for UserID
        return payloadData["sub"]?.ToString();
    }
}
