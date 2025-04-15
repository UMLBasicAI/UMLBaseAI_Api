using System.Text.Json.Serialization;

namespace PromptToAI.Presentation;

public sealed class Response
{
    [JsonIgnore]
    public int HttpCode { get; set; }
    public string AppCode { get; set; } = string.Empty;

    public BodyDto Body { get; set; }

    public sealed class BodyDto
    {
        public string HistoryId { get; set; }
        public string PlantUML { get; set; }

        public string ResponseText { get; set; }
    }
}
