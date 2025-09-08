namespace Commit.Services.Helpers;

public static class Utils
{
    private static readonly Random Random = new();
    public static Guid AdminId => new("d8156a38-a2e5-4b8d-a639-592739741fcf");
    
    public static string GenerateRandomOtp(int otpLength, string[] allowedCharacters)
    {
        var sOtp = string.Empty;

        for (var i = 0; i < otpLength; i++)

        {
            Random.Next(0, allowedCharacters.Length);

            var sTempChars = allowedCharacters[Random.Next(0, allowedCharacters.Length)];

            sOtp += sTempChars;
        }

        return sOtp;
    }

    public static string GenerateRandomOtp(int length)
        => GenerateRandomOtp(length, ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]);
}