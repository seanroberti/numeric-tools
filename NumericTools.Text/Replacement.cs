using System.Text;

namespace NumericTools.TextReplacement;

public record Replacement(int Start, int End, string NewText)
{
    public Replacement(Substring substring, string newText)
        : this(substring.Start, substring.End, newText) { }

    public int Length => End - Start;

    public bool IsInserting => Length == 0;

    public bool IsDeleting => string.IsNullOrEmpty(NewText);

    public static Replacement Insert(int index, string newText) => new Replacement(index, index, newText);

    public static Replacement Delete(Substring substring) => new Replacement(substring, "");

    public static string ApplyAll(string text, params IEnumerable<Replacement> replacements)
    {
        StringBuilder sb = new();
        List<Replacement> list = replacements.ToList();
        list.Sort((a, b) => a.Start.CompareTo(b.Start));
        int index = 0;
        foreach (Replacement r in replacements)
        {
            sb.Append(text[index..r.Start]);
            sb.Append(r.NewText);
            index = r.End;
        }
        sb.Append(text[index..]);
        return sb.ToString();
    }
}
