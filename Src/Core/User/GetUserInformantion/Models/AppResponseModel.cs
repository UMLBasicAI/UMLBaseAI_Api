using FCommon.FeatureService;
using GetUserInformation.Common;

namespace GetUserInformation.Models;

public sealed class AppResponseModel : IServiceResponse
{
    public Constant.AppCode AppCode { get; set; }

    public UserInformationModal Body { get; set; }

}
