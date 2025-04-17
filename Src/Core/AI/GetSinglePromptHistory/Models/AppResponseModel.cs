using Base.DataBaseAndIdentity.Entities;
using FCommon.FeatureService;
using GetSinglePromptHistory.Common;

namespace GetSinglePromptHistory.Models;

public sealed class AppResponseModel : IServiceResponse
{
    public Constant.AppCode AppCode { get; set; }

    public BodyModel Body { get; set; }

    public sealed class BodyModel
    {
        public string HistoryId { get; set; }
        public List<MessageEntity> Messages { get; set; }
        public string LastPlantUmlCode { get; set; }
        public Boolean IsHasNextPage { get; set; }
        public Boolean IsHasPreviousPage { get; set; }
    }
}
