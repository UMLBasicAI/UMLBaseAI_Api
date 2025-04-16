using FCommon.FeatureService;
using UpdatePromptTitleNoteById.Common;

namespace UpdatePromptTitleNoteById.Models;

public sealed class AppResponseModel : IServiceResponse
{
    public Constant.AppCode AppCode { get; set; }

    public BodyModel Body { get; set; }

    public sealed class BodyModel { }
}
