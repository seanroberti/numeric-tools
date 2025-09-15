using System.Text.RegularExpressions;

namespace NumericTools.TextReplacement;

public class Substring
{
    public required int Start { get; init; }

    public required int End { get; init; }

    public int Length => End - Start;

    public required string Content { get; init; }


    public static implicit operator Substring(string content) => new Substring { Start = 0, End = content.Length, Content = content };

    public static implicit operator Substring(Capture capture) => new Substring { Start = capture.Index, End = capture.Index + capture.Length, Content = capture.Value };

    public Replacement Replace(string newText) => new Replacement(this, newText);

    public Replacement Delete() => Replacement.Delete(this);

    public Replacement InsertBefore(string newText) => Replacement.Insert(Start, newText);
    public Replacement InsertAfter(string newText) => Replacement.Insert(End, newText);

    public IEnumerable<MatchedSubstring> Matches(Regex regex) => MatchedSubstring.Matches(this, regex);

    public IEnumerable<T> Matches<T>(Regex regex) where T : MatchedSubstring => MatchedSubstring.Matches<T>(this, regex);
}
