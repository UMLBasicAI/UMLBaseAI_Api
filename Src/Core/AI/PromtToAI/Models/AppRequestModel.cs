using System.ComponentModel;
using FCommon.FeatureService;

namespace PromptToAI.Models;

public sealed class AppRequestModel : IServiceRequest<AppResponseModel>
{
    public string Prompt { get; set; }
    public string? HistoryId { get; set; }
}
