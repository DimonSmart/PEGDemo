using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Parser;

public static class DemoTokenizerBuilder
{
    public static Tokenizer<DemoToken> Build()
    {
        return new TokenizerBuilder<DemoToken>()
            .Ignore(Span.WhiteSpace)
            .Match(Span.EqualToIgnoreCase("And"), DemoToken.And)
            .Match(Span.EqualToIgnoreCase("Or"), DemoToken.Or)
            .Match(Span.EqualToIgnoreCase("Card"), DemoToken.Card)
            .Match(Span.EqualToIgnoreCase("Amount"), DemoToken.Amount)
            .Match(Span.EqualToIgnoreCase("="), DemoToken.Equal)
            .Match(Span.EqualToIgnoreCase(">"), DemoToken.Greater)
            .Match(Character.EqualTo('('), DemoToken.LParen)
            .Match(Character.EqualTo(')'), DemoToken.RParen)
            .Match(Numerics.Decimal, DemoToken.Number)
            .Match(QuotedString.CStyle, DemoToken.String)
            .Build();
    }
}