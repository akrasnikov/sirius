using System.Text.RegularExpressions;

namespace ProjectName.Core.Abstractions.Extensions;

public static class StringExtensions
{
    public static string ToLowerCamelCase(this string str)
    {
        return Regex.Replace(
            str,
            "((?<!^)[A-Z0-9])",
            m => "_" + m.ToString().ToLower(),
            RegexOptions.None
        ).ToLower();
    }
}