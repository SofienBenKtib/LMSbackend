namespace eduflowbackend.Infrastructure.Security;

public class TemporaryPasswordGenerator
{
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    private const string AllCharacters = Uppercase + Lowercase + Digits + SpecialChars;

    public static string Generate(int length = 8)
    {
        if (length is < 8 or > 16)
        {
            throw new ArgumentException("Password length must be between 8 and 16 characters.");
        }

        var random = new Random();
        var passwordChars = new char[length];

        // Ensure password meets complexity requirements
        passwordChars[0] = Uppercase[random.Next(Uppercase.Length)];
        passwordChars[1] = Lowercase[random.Next(Lowercase.Length)];
        passwordChars[2] = Digits[random.Next(Digits.Length)];
        passwordChars[3] = SpecialChars[random.Next(SpecialChars.Length)];

        // Fill the password randomly
        for (var i = 4; i < length; i++)
        {
            passwordChars[i] = AllCharacters[random.Next(AllCharacters.Length)];
        }

        // Shuffle password to avoid predictable character positions
        return new string(passwordChars.OrderBy(_ => random.Next()).ToArray());
    }
}