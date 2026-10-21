using System.Net;
using System.Net.Sockets;

namespace NuvTools.Validation;

public static partial class Validator
{
    private const int MaxEmailLength = 256;
    private const int MaxEmailLocalPartLength = 64;
    private const int MaxEmailDomainLength = 255;
    private const int MaxEmailDomainLabelLength = 63;

    /// <summary>
    /// Validates the e-mail address using RFC 5321 (SMTP mailbox) rules: dot-atom or quoted local part,
    /// domain name with a top-level domain of at least two letters, or an address literal (IPv4, IPv6 or general).
    /// </summary>
    /// <param name="value">E-mail address.</param>
    /// <returns><c>true</c> when the value is a valid SMTP mailbox; otherwise, <c>false</c>.</returns>
    public static bool IsEmail(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxEmailLength)
            return false;

        if (!value.All(char.IsAscii) || value.Any(char.IsControl))
            return false;

        var atPosition = FindEmailAtSeparator(value);
        if (atPosition <= 0 || atPosition >= value.Length - 1)
            return false;

        var localPart = value[..atPosition];
        var domainPart = value[(atPosition + 1)..];

        if (localPart.Length > MaxEmailLocalPartLength || domainPart.Length > MaxEmailDomainLength)
            return false;

        return IsValidEmailLocalPart(localPart) && IsValidEmailDomainPart(domainPart);
    }

    private static int FindEmailAtSeparator(string email)
    {
        var atIndex = -1;
        var inQuotes = false;
        var escaped = false;

        for (var index = 0; index < email.Length; index++)
        {
            var current = email[index];

            if (escaped)
            {
                escaped = false;
                continue;
            }

            if (inQuotes)
            {
                if (current == '\\')
                    escaped = true;
                else if (current == '"')
                    inQuotes = false;

                continue;
            }

            if (current == '"' && index == 0)
            {
                inQuotes = true;
                continue;
            }

            if (current == '@')
            {
                if (atIndex != -1)
                    return -1;

                atIndex = index;
            }
        }

        return inQuotes || escaped ? -1 : atIndex;
    }

    private static bool IsValidEmailLocalPart(string localPart)
    {
        if (localPart.Length >= 2 && localPart.StartsWith('"') && localPart.EndsWith('"'))
            return IsValidEmailQuotedLocalPart(localPart);

        return IsValidEmailDotAtomLocalPart(localPart);
    }

    private static bool IsValidEmailDotAtomLocalPart(string localPart)
    {
        var atoms = localPart.Split('.');
        return atoms.All(atom => atom.Length > 0 && atom.All(IsEmailAtext));
    }

    private static bool IsValidEmailQuotedLocalPart(string localPart)
    {
        var escaped = false;
        for (var index = 1; index < localPart.Length - 1; index++)
        {
            var current = localPart[index];

            if (escaped)
            {
                if (!IsEmailQuotedPair(current))
                    return false;

                escaped = false;
                continue;
            }

            if (current == '\\')
            {
                escaped = true;
                continue;
            }

            if (!IsEmailQtext(current))
                return false;
        }

        return !escaped;
    }

    private static bool IsValidEmailDomainPart(string domainPart)
    {
        if (domainPart.StartsWith('[') && domainPart.EndsWith(']'))
            return IsValidEmailAddressLiteral(domainPart[1..^1]);

        var labels = domainPart.Split('.');
        if (labels.Length < 2)
            return false;

        if (!labels.All(label => label.Length <= MaxEmailDomainLabelLength && IsLdhString(label)))
            return false;

        var topLevelDomain = labels[^1];
        return topLevelDomain.Length >= 2 && topLevelDomain.All(char.IsLetter);
    }

    private static bool IsValidEmailAddressLiteral(string literal)
    {
        if (string.IsNullOrWhiteSpace(literal))
            return false;

        if (literal.StartsWith("IPv6:", StringComparison.OrdinalIgnoreCase))
            return IPAddress.TryParse(literal[5..], out var ipv6) && ipv6.AddressFamily == AddressFamily.InterNetworkV6;

        if (IPAddress.TryParse(literal, out var ipv4) && ipv4.AddressFamily == AddressFamily.InterNetwork)
            return true;

        // General address literal: Standardized-tag ":" 1*dcontent.
        var separatorIndex = literal.IndexOf(':');
        if (separatorIndex <= 0 || separatorIndex >= literal.Length - 1)
            return false;

        return IsLdhString(literal[..separatorIndex]) && literal[(separatorIndex + 1)..].All(IsEmailDcontent);
    }

    // Letters, digits and hyphens, starting and ending with a letter or digit.
    private static bool IsLdhString(string value) =>
        !string.IsNullOrWhiteSpace(value)
        && char.IsLetterOrDigit(value[0])
        && char.IsLetterOrDigit(value[^1])
        && value.All(character => char.IsLetterOrDigit(character) || character == '-');

    private static bool IsEmailAtext(char value) =>
        char.IsLetterOrDigit(value)
        || value is '!' or '#' or '$' or '%' or '&' or '\'' or '*' or '+' or '-' or '/' or '=' or '?' or '^' or '_' or '`' or '{' or '|' or '}' or '~';

    private static bool IsEmailQtext(char value) =>
        value is (char)32 or (char)33 or (>= (char)35 and <= (char)91) or (>= (char)93 and <= (char)126);

    private static bool IsEmailQuotedPair(char value) => value is >= (char)32 and <= (char)126;

    private static bool IsEmailDcontent(char value) =>
        value is (>= (char)33 and <= (char)90) or (>= (char)94 and <= (char)126);
}
