using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RefreshAccessToken.Presentation;

[ValidateNever]
public sealed class Request
{
    public string RefreshToken { get; set; }
    public string AccessTokenId { get; set; }
    public string UserId { get; set; }
}
