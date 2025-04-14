using System.Text.Json.Serialization;

namespace RefreshAccessToken.Presentation;

public sealed class Response
{
    [JsonIgnore]
    public int HttpCode { get; set; }
    public string AppCode { get; set; } = string.Empty;

    public BodyDto Body { get; set; }

    public sealed class BodyDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
