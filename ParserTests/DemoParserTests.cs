using Parser;
using Superpower;

namespace ParserTests;

public class DemoParserTests
{
    private readonly Tokenizer<DemoToken> _tokenizer = DemoTokenizerBuilder.Build();

    [Theory]
    [InlineData(@"Amount = 100")]
    [InlineData(@"Card = ""Visa""")]
    [InlineData(@"(Card = ""Visa"" AND Amount = 100) AND Card = ""Visa2""")]
    [InlineData(@"Card = ""Visa1"" OR Card = ""Visa2"" OR Card = ""Visa3""")]
    [InlineData(@"((Card = ""Visa"") OR (Card = ""MasterCard"")) AND (Amount > 100)")]
    public void ParseShouldParseTest(string expression)
    {
        var tokens = _tokenizer.Tokenize(expression);
        Assert.True(DemoParser.TryParse(tokens, out var expr, out var x, out var y));
    }
}