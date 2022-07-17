namespace Parser.Expressions;

public class BinaryDemoExpression : DemoExpression
{
    public BinaryDemoExpression(LogicalOperator @operator, DemoExpression left, DemoExpression right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right = right ?? throw new ArgumentNullException(nameof(right));
        Operator = @operator;
    }

    public DemoExpression Left { get; }
    public DemoExpression Right { get; }
    public LogicalOperator Operator { get; }

    public static BinaryDemoExpression Create(LogicalOperator @operator, DemoExpression left, DemoExpression right)
    {
        return new BinaryDemoExpression(@operator, left, right);
    }

    public override void AcceptVisitor(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}