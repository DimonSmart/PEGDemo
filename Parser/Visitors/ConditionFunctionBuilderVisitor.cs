using System.Linq.Expressions;
using Parser.Expressions;

namespace Parser.Visitors;

public class ConditionFunctionBuilderVisitor : IVisitor
{
    private readonly ParameterExpression AmountParameter = Expression.Parameter(typeof(int), "Amount");
    private readonly ParameterExpression CardNameParameter = Expression.Parameter(typeof(string), "CardName");

    private readonly Stack<Expression> ExpressionsStack = new();

    public void Visit(BinaryDemoExpression binaryDemoExpression)
    {
        binaryDemoExpression.Left.AcceptVisitor(this);
        binaryDemoExpression.Right.AcceptVisitor(this);

        if (ExpressionsStack.Count < 2)
            throw new ArgumentException("Invalid expression tree state");

        var leftArg = ExpressionsStack.Pop();
        var rightArg = ExpressionsStack.Pop();
        switch (binaryDemoExpression.Operator)
        {
            case LogicalOperator.And:
                ExpressionsStack.Push(Expression.And(leftArg, rightArg));
                break;
            case LogicalOperator.Or:
                ExpressionsStack.Push(Expression.Or(leftArg, rightArg));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Visit(CheckAmountExpression checkAmountExpression)
    {
        switch (checkAmountExpression.ComparisionOperator)
        {
            case AmountComparisionOperator.Equal:
                ExpressionsStack.Push(
                    Expression.Equal(AmountParameter, Expression.Constant(checkAmountExpression.Value)));
                break;
            case AmountComparisionOperator.Greater:
                ExpressionsStack.Push(
                    Expression.GreaterThan(AmountParameter, Expression.Constant(checkAmountExpression.Value)));
                break;
            case AmountComparisionOperator.Less:
                ExpressionsStack.Push(
                    Expression.LessThan(AmountParameter, Expression.Constant(checkAmountExpression.Value)));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Visit(CheckCardNameExpression checkCardNameExpression)
    {
        ExpressionsStack.Push(
            Expression.Equal(CardNameParameter, Expression.Constant(checkCardNameExpression.Value)));
    }

    public Func<string, int, bool> GetResult()
    {
        if (ExpressionsStack.Count != 1)
            throw new ArgumentException("Invalid expression tree");

        return Expression.Lambda<Func<string, int, bool>>(ExpressionsStack.Pop(), CardNameParameter, AmountParameter)
            .Compile();
    }
}