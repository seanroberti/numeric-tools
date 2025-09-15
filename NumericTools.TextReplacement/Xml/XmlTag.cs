using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace NumericTools.TextReplacement.Xml;

public class XmlTag : MatchedSubstring
{
    private ImmutableDictionary<string, XmlAttribute>? _attributes;

    public CapturedSubstring Name { get; init; }

    public ImmutableDictionary<string, XmlAttribute> Attributes => _attributes ??= XmlAttribute.Parse(this[nameof(Attributes)]).ToImmutableDictionary(attr => attr.Name.Content);

    #region Parse
    public static IEnumerable<XmlTag> Parse(Substring substring) => substring.Matches<XmlTag>(Regex);

    private static readonly Regex Regex = new Regex(@"<(?<Closing>/)?(?<Name>[A-Za-z\d\-_\.\:]+)(?<Attributes>([^\>\/]|""[^""]*"")*)\s*(?<SelfClosing>/)?>");
    #endregion Parse
}
