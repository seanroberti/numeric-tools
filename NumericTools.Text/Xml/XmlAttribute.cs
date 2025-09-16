using System.Text.RegularExpressions;

namespace NumericTools.TextReplacement.Xml;

public class XmlAttribute : MatchedSubstring
{
    public CapturedSubstring Name { get; init; }

    public CapturedSubstring? Value { get; init; }

    #region Parse
    internal static IEnumerable<XmlAttribute> Parse(Substring substring) => substring.Matches<XmlAttribute>(Regex);

    private static readonly Regex Regex = new Regex(@"\s+(?<Name>[\w\.\-\:]+)(=""(?<Value>[^""]*)"")?");
    #endregion Parse
}
