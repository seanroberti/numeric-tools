using System.Text.RegularExpressions;
using NumericTools.TextReplacement.Xml;

namespace NumericTools.TextReplacement.Tests;

public class SubstringTests
{
    [Fact]
    public void SampleTest1()
    {
        Substring substring = "a=1, b=2";
        Regex regex = new Regex(@"(?<Key>[A-Za-z]+)=(?<Value>\d+)");
        MatchedSubstring[] matches = substring.Matches(regex).ToArray();
        Assert.Equal("a=1", matches[0].Content);
        Assert.Equal("a", matches[0]["Key"].Content);
        Assert.Equal("1", matches[0]["Value"].Content);
        Assert.Equal("b=2", matches[1].Content);
        Assert.Equal("b", matches[1]["Key"].Content);
        Assert.Equal("2", matches[1]["Value"].Content);
    }

    [Fact]
    public void SampleTestXml()
    {
        string original = "<tag a=\"1\", b=\"2\" /><second c=\"3\", d=\"4\" />";
        XmlTag[] tags = XmlTag.Parse(original).ToArray();
        Assert.Equal("tag", tags[0].Name.Content);
        Assert.Equal("second", tags[1].Name.Content);
        Assert.Equal(" c=\"3\"", tags[1].Attributes["c"].Content);
        Assert.Equal("c", tags[1].Attributes["c"].Name.Content);
        Assert.Equal("3", tags[1].Attributes["c"].Value.Content);
        string changed = Replacement.ApplyAll(original, tags[1].Attributes["c"].Value.Replace("321"));
        Assert.Equal("<tag a=\"1\", b=\"2\" /><second c=\"321\", d=\"4\" />", changed);
    }
}
