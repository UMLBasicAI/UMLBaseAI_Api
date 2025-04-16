using FCommon.FeatureService;
using DeletePromptHistoryById.Common;

namespace DeletePromptHistoryById.Models;

public sealed class AppResponseModel : IServiceResponse
{
    public Constant.AppCode AppCode { get; set; }

    public BodyModel Body { get; set; }

    public sealed class BodyModel { }
}
