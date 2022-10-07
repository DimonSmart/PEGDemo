using Microsoft.EntityFrameworkCore;
using Parser;
using Parser.Visitors;
using ParserTests.Models;
using System.Diagnostics;

namespace ParserTests;

public class FiltrationTests
{
    [Theory]
    [InlineData(@"Card = ""Visa""")]
   
    public void DbFiltrationTest(string expression)
    {
        Assert.True(DemoParser.TryParse(DemoTokenizerBuilder.Build().Tokenize(expression), out var expr, out _, out _));
        var visitor = new ConditionFunctionBuilderVisitor();
        expr!.AcceptVisitor(visitor);
        Func<string, int, bool> whereFunction = visitor.GetResult();
        var dbContext = new UserAccountsContext(new DbContextOptionsBuilder<UserAccountsContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .EnableSensitiveDataLogging(true)
            .Options);
        dbContext.GenerateUserAccounts(10);

        var subset = dbContext.UserAccounts.Where(i => whereFunction(i.CardType, i.Amount));

    }
}
