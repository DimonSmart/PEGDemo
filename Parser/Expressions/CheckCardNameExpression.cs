namespace Parser.Expressions;

public class CheckCardNameExpression : DemoExpression
{
    public CheckCardNameExpression(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override void AcceptVisitor(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}