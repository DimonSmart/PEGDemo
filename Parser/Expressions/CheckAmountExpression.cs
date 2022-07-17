namespace Parser.Expressions;

public class CheckAmountExpression : DemoExpression
{
    public CheckAmountExpression(int value, AmountComparisionOperator comparisionOperator)
    {
        Value = value;
        ComparisionOperator = comparisionOperator;
    }

    public int Value { get; }
    public AmountComparisionOperator ComparisionOperator { get; }

    public override void AcceptVisitor(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}