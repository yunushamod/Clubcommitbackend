namespace Commit.Services.Helpers;

public class JwtSettings
{
    public required string Secret { get; init; }
    public required double Expires { get; init; }
    public required string ValidIssuer { get; init; }
    public required string ValidAudience { get; init; }
}