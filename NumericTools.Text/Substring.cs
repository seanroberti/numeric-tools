using System.Text.RegularExpressions;

namespace NumericTools.TextReplacement;

public class Substring
{
    public required int Start { get; init; }

    public int End => Start + Length;

    public int Length => Text.Length;

    public required string Text { get; init; }


    public static implicit operator Substring(string text) => new Substring { Start = 0, Text = text };

    public static implicit operator Substring(Capture capture) => new Substring { Start = capture.Index, Text = capture.Value };

    public Replacement Replace(string newText) => new Replacement(this, newText);

    public Replacement Delete() => Replacement.Delete(this);

    public Replacement InsertBefore(string newText) => Replacement.Insert(Start, newText);
    public Replacement InsertAfter(string newText) => Replacement.Insert(End, newText);

    public IEnumerable<MatchedSubstring> Matches(Regex regex) => MatchedSubstring.Matches(this, regex);

    public IEnumerable<T> Matches<T>(Regex regex) where T : MatchedSubstring => MatchedSubstring.Matches<T>(this, regex);
}
