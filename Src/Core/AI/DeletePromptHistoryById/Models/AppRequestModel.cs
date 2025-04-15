using System.ComponentModel;
using FCommon.FeatureService;

namespace DeletePromptHistoryById.Models;

public sealed class AppRequestModel : IServiceRequest<AppResponseModel>
{
    public string Email { get; set; }

    public string Password { get; set; }
}
