using Parser.Expressions;

namespace Parser.Visitors;

public class ExpressionInterpreterVisitor : IVisitor
{
    private readonly int _amount;
    private readonly string _cardName;
    private readonly Stack<bool> ResultsStack = new();

    public ExpressionInterpreterVisitor(string cardName, int amount)
    {
        _amount = amount;
        _cardName = cardName;
    }

    public void Visit(BinaryDemoExpression binaryDemoExpression)
    {
        binaryDemoExpression.Left.AcceptVisitor(this);
        binaryDemoExpression.Right.AcceptVisitor(this);

        if (ResultsStack.Count < 2)
            throw new ArgumentException("Invalid expression tree state");

        var leftArg = ResultsStack.Pop();
        var rightArg = ResultsStack.Pop();

        switch (binaryDemoExpression.Operator)
        {
            case LogicalOperator.And:
                ResultsStack.Push(leftArg && rightArg);
                break;
            case LogicalOperator.Or:
                ResultsStack.Push(leftArg || rightArg);
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
                ResultsStack.Push(_amount == checkAmountExpression.Value);
                break;
            case AmountComparisionOperator.Greater:
                ResultsStack.Push(_amount > checkAmountExpression.Value);
                break;
            case AmountComparisionOperator.Less:
                ResultsStack.Push(_amount < checkAmountExpression.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Visit(CheckCardNameExpression checkCardNameExpression)
    {
        ResultsStack.Push(_cardName == checkCardNameExpression.Value);
    }

    public bool GetResult()
    {
        if (ResultsStack.Count != 1)
            throw new ArgumentException("Invalid expression tree");
        return ResultsStack.Pop();
    }
}