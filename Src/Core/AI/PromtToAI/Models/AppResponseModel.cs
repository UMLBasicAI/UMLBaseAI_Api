using FCommon.FeatureService;
using PromptToAI.Common;

namespace PromptToAI.Models;

public sealed class AppResponseModel : IServiceResponse
{
    public Constant.AppCode AppCode { get; set; }

    public BodyModel Body { get; set; }

    public sealed class BodyModel
    {
        public string PlantUML { get; set; }
        public string ResponseText { get; set; }
        public string HistoryId { get; set; }
    }
}
