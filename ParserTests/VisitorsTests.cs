using Parser;
using Parser.Visitors;

namespace ParserTests;

public class VisitorsTests
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
    [InlineData(@"(Amount > 5) AND (Amount < 10)", "", 6, true)]
    [InlineData(@"(Amount > 5) AND (Amount < 10)", "", 5, false)]
    [InlineData(@"(Amount > 5) AND (Amount < 10)", "", 10, false)]
    public void CalculationTest(string expression, string cardName, int amount, bool expectedResult)
    {
        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));

        var visitor = new ConditionFunctionBuilderVisitor();
        expr!.AcceptVisitor(visitor);
        var whereFunction = visitor.GetExpression().Compile();
        Assert.Equal(expectedResult, whereFunction(cardName, amount));

        var interpreterVisitor = new ExpressionInterpreterVisitor(cardName, amount);
        expr.AcceptVisitor(interpreterVisitor);
        Assert.Equal(expectedResult, interpreterVisitor.GetResult());

    }
}