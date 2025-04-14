namespace RefreshAccessToken.Models;

public sealed class RefreshTokenModel
{
    public string LoginProvider { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public DateTime ExpiredAt { get; set; }
    public Guid UserId { get; set; }
}
