using System.ComponentModel;
using FCommon.FeatureService;

namespace RefreshAccessToken.Models;

public sealed class AppRequestModel : IServiceRequest<AppResponseModel>
{
    public string RefreshToken { get; set; }
    public string AccessTokenId { get; set; }
    public string UserId { get; set; }
}
