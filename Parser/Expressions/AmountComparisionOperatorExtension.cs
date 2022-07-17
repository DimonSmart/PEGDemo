namespace Parser.Expressions;

public static class AmountComparisionOperatorExtension
{
    public static string Visualize(this AmountComparisionOperator value)
    {
        return value switch
        {
            AmountComparisionOperator.Equal => "=",
            AmountComparisionOperator.Greater => ">",
            AmountComparisionOperator.Less => "<",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}