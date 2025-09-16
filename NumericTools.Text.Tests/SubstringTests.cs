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
        Assert.Equal("a=1", matches[0].Text);
        Assert.Equal("a", matches[0]["Key"].Text);
        Assert.Equal("1", matches[0]["Value"].Text);
        Assert.Equal("b=2", matches[1].Text);
        Assert.Equal("b", matches[1]["Key"].Text);
        Assert.Equal("2", matches[1]["Value"].Text);
    }

    [Fact]
    public void SampleTestXml()
    {
        string original = "<tag a=\"1\", b=\"2\" /><second c=\"3\", d=\"4\" />";
        XmlTag[] tags = XmlTag.Parse(original).ToArray();
        Assert.Equal("tag", tags[0].Name.Text);
        Assert.Equal("second", tags[1].Name.Text);
        Assert.Equal(" c=\"3\"", tags[1].Attributes["c"].Text);
        Assert.Equal("c", tags[1].Attributes["c"].Name.Text);
        Assert.Equal("3", tags[1].Attributes["c"].Value.Text);
        string changed = Replacement.ApplyAll(original, tags[1].Attributes["c"].Value.Replace("321"));
        Assert.Equal("<tag a=\"1\", b=\"2\" /><second c=\"321\", d=\"4\" />", changed);
    }
}
