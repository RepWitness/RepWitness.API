using System.Text;
using System.Text.RegularExpressions;

namespace RepWitness.Application.Common.Behaviors;

public static partial class AuthenticationValidationHelper
{
    public static string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?.,><}{|@#$%^&*";
        var res = new StringBuilder();
        var rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }

    public static KeyValuePair<bool, string> EmailValid(string email)
    {
        return !MyRegex().IsMatch(email) ? new KeyValuePair<bool, string>(false, "Invalid email format.") : new KeyValuePair<bool, string>(true, string.Empty);
    }

    public static KeyValuePair<bool, string> PasswordValidation(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Password is required.");
        }
        else
        {
            if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }

            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            if (!password.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }

            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one number.");
            }

            if (password.All(char.IsLetterOrDigit))
            {
                errors.Add("Password must contain at least one special character.");
            }
        }

        return errors.Count != 0 ? new KeyValuePair<bool, string>(false, string.Join(Environment.NewLine, errors)) : new KeyValuePair<bool, string>(true, string.Empty);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex MyRegex();
}
