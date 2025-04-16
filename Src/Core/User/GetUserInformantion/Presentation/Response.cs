using GetUserInformation.Models;
using System.Text.Json.Serialization;

namespace GetUserInformation.Presentation;

public sealed class Response
{
    [JsonIgnore]
    public int HttpCode { get; set; }
    public string AppCode { get; set; } = string.Empty;

    public UserInformationModal? Body { get; set; }
}
