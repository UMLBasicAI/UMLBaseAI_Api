using System.ComponentModel;
using FCommon.FeatureService;

namespace GetPromptHistories.Models;

public sealed class AppRequestModel : IServiceRequest<AppResponseModel>
{
    public int Page { get; set; }
    public int Size { get; set; }
}
