using System.Text.Json.Serialization;
using Base.DataBaseAndIdentity.Entities;

namespace GetSinglePromptHistory.Presentation;

public sealed class Response
{
    [JsonIgnore]
    public int HttpCode { get; set; }
    public string AppCode { get; set; } = string.Empty;

    public BodyDto Body { get; set; }

    public sealed class BodyDto
    {
        public string HistoryId { get; set; }
        public List<MessageEntity> Messages { get; set; }
        public string LastPlantUmlCode { get; set; }
        public Boolean IsHasNextPage { get; set; }
        public Boolean IsHasPreviousPage { get; set; }
    }
}
