namespace NuvTools.Validation;

/// <summary>
/// Regex patterns class.
/// </summary>
public static class RegexPattern
{
    /// <summary>
    /// Regex pattern for e-mail address validation.
    /// </summary>
    public const string EMAIL_ADDRESS = @"^([a-z0-9_\-])([a-z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-z0-9\-]+)\.)+))([a-z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$";

    private const string BASE64_CONTENT_OPTIONAL = "(?<content>(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4}))";

    /// <summary>
    /// Regex to validate Base64 and extract using content label.
    /// </summary>
    public const string BASE64_CONTENT = $@"^{BASE64_CONTENT_OPTIONAL}$";

    /// <summary>
    /// Regex to validate and extract informations (type, extension and content) from Data URI Base64.
    /// </summary>
    public const string BASE64_DATAURI = $@"^data:(?<type>.+?/(?<extension>.+?));(?<base>.+),{BASE64_CONTENT_OPTIONAL}$";
}