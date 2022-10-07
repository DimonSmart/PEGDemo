using Bogus;

namespace ParserTests.Models
{
    public static class FakeDataGenerator
    {
        public static void GenerateUserAccounts(this UserAccountsContext context, int q)
        {
            var accountIds = 1;
            var cardTypes = new string[] { "Visa", "MasterCard", "NoName" };
            var randomUserAccounts = new Faker<UserAccountDAOModel>()
            .StrictMode(true)
            .RuleFor(o => o.Id, f => accountIds++)
            .RuleFor(o => o.CardType, f => f.PickRandom(cardTypes))
            .RuleFor(o => o.Amount, f => f.Random.Number(0, 1001));

            foreach (var item in randomUserAccounts.Generate(10))
            {
                context.UserAccounts.Add(item);
            }
            context.SaveChanges();
        }
    }
}
