using System.Text.RegularExpressions;

namespace NuvTools.Validation;

/// <summary>
/// Provides strongly-typed and pre-compiled regex patterns commonly used in the application.
/// This class exposes both the raw pattern constants and pre-generated compiled Regex instances.
/// </summary>
/// <summary>
/// Provides strongly-typed and pre-compiled regular expression patterns used across the application.
/// This class contains only regex definitions and their corresponding generated instances.
/// </summary>
public static partial class RegexPattern
{
    #region Email

    /// <summary>
    /// Regex pattern for strict e-mail address validation, supporting domain names and IPv4 formats.
    /// </summary>
    private const string EMAIL_ADDRESS =
        @"^([a-z0-9_\-])([a-z0-9_\-\.]*)@(" +
            @"(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}" +
              @"(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])|" + // IPv4 literal
            @"((([a-z0-9\-]+)\.)+([a-z]{2,}))" +                         // domain
        @")$";

    /// <summary>
    /// Compiled and invariant regex instance for e-mail validation.
    /// </summary>
    [GeneratedRegex(EMAIL_ADDRESS, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    public static partial Regex EmailRegex();

    #endregion

    #region Base64 (Raw Content)

    /// <summary>
    /// Base64 capture segment that extracts the content using the named group 'content'.
    /// Supports padding and all standard Base64 encoding rules.
    /// </summary>
    private const string BASE64_CONTENT_CAPTURE =
        @"(?<content>(?:[A-Za-z0-9+/]{4})*" +
                        @"(?:[A-Za-z0-9+/]{2}==|" +
                        @"[A-Za-z0-9+/]{3}=|" +
                        @"[A-Za-z0-9+/]{4}))";

    /// <summary>
    /// Regex pattern for validating only Base64 payload content.
    /// </summary>
    private const string BASE64_CONTENT = $@"^{BASE64_CONTENT_CAPTURE}$";

    /// <summary>
    /// Compiled regex to validate and capture Base64 raw content.
    /// </summary>
    [GeneratedRegex(BASE64_CONTENT, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    public static partial Regex Base64ContentRegex();

    #endregion

    #region Base64 (Data URI)

    /// <summary>
    /// Regex pattern to validate and extract information from Data URIs containing Base64 payloads.
    /// Captured groups:
    ///   - type: full MIME type (e.g. image/png)
    ///   - extension: extension extracted from type
    ///   - content: Base64 payload (same as BASE64_CONTENT)
    /// </summary>
    private const string BASE64_DATAURI =
        $@"^data:(?<type>.+?\/(?<extension>.+?));(?<base>.+),{BASE64_CONTENT_CAPTURE}$";

    /// <summary>
    /// Compiled regex to validate Base64 Data URIs and extract MIME type, extension and content.
    /// </summary>
    [GeneratedRegex(BASE64_DATAURI, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    public static partial Regex Base64DataUriRegex();

    #endregion
}