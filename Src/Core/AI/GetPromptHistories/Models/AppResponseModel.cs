using Base.DataBaseAndIdentity.Entities;
using FCommon.FeatureService;
using GetPromptHistories.Common;

namespace GetPromptHistories.Models;

public sealed class AppResponseModel : IServiceResponse
{
    public Constant.AppCode AppCode { get; set; }

    public BodyModel Body { get; set; }

    public sealed class BodyModel
    {
        public List<HistoryModel> Histories { get; set; }

        public Boolean IsHasNextPage { get; set; }
        public Boolean IsHasPreviousPage { get; set; }
    }
}
