namespace RefreshAccessToken.Models;

public sealed class UpdateRefreshTokenModel
{
    public string CurrentId { get; set; }

    public string NewId { get; set; }

    public string NewValue { get; set; }
}
