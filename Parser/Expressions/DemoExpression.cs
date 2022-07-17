namespace Parser.Expressions;

public abstract class DemoExpression
{
    public abstract void AcceptVisitor(IVisitor visitor);
}