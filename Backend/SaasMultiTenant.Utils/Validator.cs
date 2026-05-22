using System.Text.RegularExpressions;

namespace SaasMultiTenant.Utils
{
    public static partial class Validator
    {
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
        public static partial Regex EmailRegex();



        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex().IsMatch(email);
        }
    }
}
