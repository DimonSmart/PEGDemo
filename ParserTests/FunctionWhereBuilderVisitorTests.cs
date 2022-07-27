using Parser;
using Parser.Visitors;

namespace ParserTests;

public class FunctionWhereBuilderVisitorTests
{
    [Theory]
    [InlineData(@"Card = ""Visa""", "Visa", 0, true)]
    [InlineData(@"Card = ""Visa""", "MasterCard", 0, false)]
    [InlineData(@"Amount = 5", "MasterCard", 5, true)]
    [InlineData(@"Amount = 5", "MasterCard", 6, false)]
    [InlineData(@"Amount > 5", "MasterCard", 6, true)]
    [InlineData(@"Amount > 5", "MasterCard", 5, false)]
    [InlineData(@"Amount < 5", "MasterCard", 6, false)]
    [InlineData(@"Amount < 5", "MasterCard", 4, true)]
    
    public void PrettyPrintTest(string expression, string cardName, int amount, bool expectedResult)
    {
        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));
        var visitor = new FunctionWhereBuilderVisitor();
        expr!.AcceptVisitor(visitor);
        var whereFunction = visitor.GetResult();

        Assert.Equal(expectedResult, whereFunction(cardName, amount));
    }
}