using System.Collections;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NumericTools.TextReplacement;

public class MatchedSubstring : Substring, IEnumerable<KeyValuePair<string, CapturedSubstring>>
{
    public required ImmutableDictionary<string, CapturedSubstring> Captures { get; init; }

    public bool HasChild(string key) => Captures.ContainsKey(key);

    public CapturedSubstring this[string key] => Captures[key];

    public IEnumerable<string> Keys => Captures.Keys;

    public IEnumerable<CapturedSubstring> Values => Captures.Values;

    public IEnumerator<KeyValuePair<string, CapturedSubstring>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, CapturedSubstring>>)Captures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static IEnumerable<MatchedSubstring> Matches(Substring substring, Regex regex)
    {
        int offset = substring.Start;
        return regex.Matches(substring.Text).Select(match => new MatchedSubstring
        {
            Start = offset + match.Index,
            Text = match.Value,
            Captures = match.Groups.Keys.Select(key => (key: key, value: match.Groups[key])).Select(kv => new CapturedSubstring
            {
                Start = offset + kv.value.Index,
                Text = kv.value.Value,
                Name = kv.key
            }).ToImmutableDictionary(captured => captured.Name)
        });
    }

    public static IEnumerable<T> Matches<T>(Substring substring, Regex regex) where T : MatchedSubstring
    {
        return Matches(substring, regex).Select(match =>
        {
            T value = (T) typeof(T).GetConstructor([]).Invoke([]);
            foreach (PropertyInfo property in typeof(MatchedSubstring).GetProperties())
                if (property.GetGetMethod() is { } getMethod && !getMethod.IsStatic && property.GetSetMethod() is { } setMethod)
                    setMethod.Invoke(value, [getMethod.Invoke(match, [])]);
            foreach (string key in match.Keys)
            {
                if (typeof(T).GetProperty(key) is { } property && property.DeclaringType != typeof(MatchedSubstring) && property.GetSetMethod() is { } setMethod && !setMethod.IsStatic)
                {
                    object? captured = property.PropertyType.IsAssignableFrom(typeof(Substring)) || property.PropertyType.IsAssignableFrom(typeof(CapturedSubstring)) ? match[key] :
                                       property.PropertyType.IsAssignableFrom(typeof(string)) ? match[key].Text :
                                       null;
                    if (captured is not null)
                        property.SetValue(value, captured);
                }
            }
            return value;
        });
    }
}
