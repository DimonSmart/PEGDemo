using Parser;
using Snapshooter.Xunit;
using Superpower;

namespace ParserTests;

public class DemoTokenizerTests
{
    private readonly Tokenizer<DemoToken> _tokenizer = DemoTokenizerBuilder.Build();

    [Theory]
    [InlineData(@"Amount = 100", "Short expression")]
    [InlineData(@"((Card = ""Visa"") OR (Card = ""MasterCard"")) AND (Amount > 100)", "Long Expression")]
    public void PositiveTokenizationTest(string expression, string testName)
    {
        var tokens = _tokenizer.Tokenize(expression);
        Snapshot.Match(tokens, testName);
    }
}