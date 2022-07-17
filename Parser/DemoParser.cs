using Parser.Expressions;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using ExpressionTokenParser = Superpower.TokenListParser<Parser.DemoToken, Parser.Expressions.DemoExpression>;


namespace Parser;

public static class DemoParser
{
    private static readonly TokenListParser<DemoToken, int> Number =
        Token.EqualTo(DemoToken.Number).Apply(Numerics.IntegerInt32);

    private static readonly TokenListParser<DemoToken, LogicalOperator> AndOperator =
        Token.EqualTo(DemoToken.And).Value(LogicalOperator.And);

    private static readonly TokenListParser<DemoToken, LogicalOperator> OrOperator =
        Token.EqualTo(DemoToken.Or).Value(LogicalOperator.Or);

    private static readonly TokenListParser<DemoToken, AmountComparisionOperator> AmountCompare =
        Token.EqualTo(DemoToken.Equal).Value(AmountComparisionOperator.Equal)
            .Or(Token.EqualTo(DemoToken.Greater).Value(AmountComparisionOperator.Greater))
            .Or(Token.EqualTo(DemoToken.Less).Value(AmountComparisionOperator.Less));

    public static ExpressionTokenParser Literal { get; } =
        (from card in Token.EqualTo(DemoToken.Card)
            from _ in Token.EqualTo(DemoToken.Equal)
            from cardName in Token.EqualTo(DemoToken.String).Apply(QuotedString.CStyle)
            select (DemoExpression)new CheckCardNameExpression(cardName))
        .Or
        (from amount in Token.EqualTo(DemoToken.Amount)
            from comparision in Parse.Ref(() => AmountCompare)
            from amountValue in Parse.Ref(() => Number)
            select (DemoExpression)new CheckAmountExpression(amountValue, comparision));

    private static ExpressionTokenParser Factor { get; } =
        (from lparen in Token.EqualTo(DemoToken.LParen)
            from expr in Parse.Ref(() => Expression)
            from rparen in Token.EqualTo(DemoToken.RParen)
            select expr)
        .Or(Literal);

    private static ExpressionTokenParser Term { get; } =
        Parse.Chain(AndOperator, Factor, BinaryDemoExpression.Create);

    private static ExpressionTokenParser Expression { get; } =
        Parse.Chain(OrOperator, Term, BinaryDemoExpression.Create);

    public static bool TryParse(TokenList<DemoToken> tokens, out DemoExpression? expr, out string? error,
        out Position errorPosition)
    {
        var result = Expression.AtEnd()(tokens);
        if (!result.HasValue)
        {
            expr = null;
            error = result.ToString();
            errorPosition = result.ErrorPosition;
            return false;
        }

        expr = result.Value;
        error = null;
        errorPosition = Position.Empty;
        return true;
    }
}