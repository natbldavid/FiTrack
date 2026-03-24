using Microsoft.AspNetCore.Identity;

namespace FiTrack.Api.Services.Auth;

public class PasscodeService
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPasscode(string passcode)
    {
        ValidatePasscode(passcode);
        return _passwordHasher.HashPassword(new object(), passcode);
    }

    public bool VerifyPasscode(string passcode, string storedHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(new object(), storedHash, passcode);
        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }

    private static void ValidatePasscode(string passcode)
    {
        if (string.IsNullOrWhiteSpace(passcode))
            throw new ArgumentException("Passcode is required.");

        if (passcode.Length != 6 || !passcode.All(char.IsDigit))
            throw new ArgumentException("Passcode must be exactly 6 digits.");
    }
}