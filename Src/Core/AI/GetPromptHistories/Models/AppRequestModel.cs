using System.ComponentModel;
using FCommon.FeatureService;

namespace GetSinglePromptHistory.Models;

public sealed class AppRequestModel : IServiceRequest<AppResponseModel>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string HistoryId { get; set; }
}
