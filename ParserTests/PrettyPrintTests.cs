using Parser;
using Parser.Visitors;
using Snapshooter.Xunit;

namespace ParserTests;

public class PrettyPrintTests
{
    [Theory]
    [InlineData(@"((Card = ""Visa"") OR (Card = ""MasterCard"")) AND (Amount > 100)", "Test1")]
    [InlineData(@"( Card=""Visa"" Or Card=""MasterCard"" ) And Amount > 100", "Test2")]
    [InlineData(@"Card=""Visa1"" Or Card=""Visa2"" AND Card=""Visa3""", "Test3")]
    [InlineData(@"Card=""Visa1"" and Card=""Visa2"" or Card=""Visa3""", "Test4")]
    public void PrettyPrintTest(string expression, string testName)
    {
        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));
        var visitor = new PrettyPrintVisitor(new PrettyPrintOptions { CapitalizeLogicalOperators = true });
        expr!.AcceptVisitor(visitor);
        var prettyExpression = visitor.GetResult();
        Snapshot.Match(prettyExpression, testName);
    }
}