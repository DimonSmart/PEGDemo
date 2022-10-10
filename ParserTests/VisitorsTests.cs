using Parser;
using Parser.Visitors;

namespace ParserTests;

public class VisitorsTests
{
    [Theory]
    [ClassData(typeof(CalculationTestData))]
    public void ExpressionInterpreterVisitorTest(string expression, string cardName, int amount, bool expectedResult)
    {
        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));
        var interpreterVisitor = new ExpressionInterpreterVisitor(cardName, amount);
        expr.AcceptVisitor(interpreterVisitor);
        Assert.Equal(expectedResult, interpreterVisitor.GetResult());
    }

    [Theory]
    [ClassData(typeof(CalculationTestData))]
    public void ConditionFunctionBuilderVisitorTest(string expression, string cardName, int amount, bool expectedResult)
    {
        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));

        var visitor = new ConditionFunctionBuilderVisitor();
        expr!.AcceptVisitor(visitor);
        var whereFunction = visitor.GetExpression().Compile();
        Assert.Equal(expectedResult, whereFunction(cardName, amount));
    }

    public class CalculationTestData : TheoryData<string, string, int, bool>
    {
        public CalculationTestData()
        {
            Add(@"Card = ""Visa""", "Visa", 0, true);
            Add(@"Card = ""Visa""", "MasterCard", 0, false);
            Add(@"Amount = 5", "MasterCard", 5, true);
            Add(@"Amount = 5", "MasterCard", 6, false);
            Add(@"Amount > 5", "MasterCard", 6, true);
            Add(@"Amount > 5", "MasterCard", 5, false);
            Add(@"Amount < 5", "MasterCard", 6, false);
            Add(@"Amount < 5", "MasterCard", 4, true);
            Add(@"(Amount > 5) AND (Amount < 10)", "", 6, true);
            Add(@"(Amount > 5) AND (Amount < 10)", "", 5, false);
            Add(@"(Amount > 5) AND (Amount < 10)", "", 10, false);
        }
    }
}