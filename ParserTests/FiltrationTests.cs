using ExpressionSplicer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Parser;
using Parser.Visitors;
using ParserTests.Models;

namespace ParserTests;

public class FiltrationTests
{
    [Theory]
    [InlineData(@"Card = ""Visa""", 4)]
   
    public void DbFiltrationTest(string expression, int filterredRecordsCount)
    {
        var dbContext = GetDbContext();
        dbContext.AddRange(JsonConvert.DeserializeObject<UserAccountDAOModel[]>(File.ReadAllText("DBStubData.json")));
        dbContext.SaveChanges();

        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));
        var visitor = new ConditionFunctionBuilderVisitor();
        expr!.AcceptVisitor(visitor);
        var whereCondition = visitor.GetExpression();

        var subset = dbContext
            .UserAccounts
            .Where(i => whereCondition.Splice(i.CardType, i.Amount))
            .PerformSplicing()
            .ToList();
        Assert.Equal(filterredRecordsCount, subset.Count);
    }

    private static UserAccountsContext GetDbContext()
    {
        return new UserAccountsContext(new DbContextOptionsBuilder<UserAccountsContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .EnableSensitiveDataLogging(true)
            .Options);
    }
}
