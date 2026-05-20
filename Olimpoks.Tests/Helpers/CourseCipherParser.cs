using System.Text.RegularExpressions;

namespace Olimpoks.Tests.Helpers;

public static class CourseCipherParser
{
    private static readonly Regex CipherRegex = new(
        @"^\s*(?<prefix>[\p{L}_]+)\s*(?<number>\d+)\.(?<updates>\d+)\s*$",
        RegexOptions.CultureInvariant);

    public static (string Code, int UpdatesCount) Parse(string rawCipher)
    {
        var normalized = rawCipher.Trim();
        var match = CipherRegex.Match(normalized);
        if (!match.Success)
        {
            throw new FormatException($"Не удалось распознать шифр курса: '{rawCipher}'");
        }

        var code = $"{match.Groups["prefix"].Value}{match.Groups["number"].Value}";
        var updates = int.Parse(match.Groups["updates"].Value);
        return (code, updates);
    }
}
