using System.Data.SqlTypes;
using System.Text;
using Parser.Expressions;

namespace Parser.Visitors;


public class PrettyPrintOptions
{
    public bool CapitalizeLogicalOperators { get; set; } 
}

public class PrettyPrintVisitor : IVisitor
{
    private readonly PrettyPrintOptions _options;
    private readonly StringBuilder sb = new();
    
    public PrettyPrintVisitor(PrettyPrintOptions options)
    {
        _options = options;
    }
    public void Visit(BinaryDemoExpression binaryDemoExpression)
    {
        if (binaryDemoExpression.Operator == LogicalOperator.Or)
            sb.Append("( ");

        binaryDemoExpression.Left.AcceptVisitor(this);
        
        var op = binaryDemoExpression.Operator.ToString();
        if (_options.CapitalizeLogicalOperators)
            op = op.ToUpper();
        sb.Append($" {op} ");
        
        binaryDemoExpression.Right.AcceptVisitor(this);

        if (binaryDemoExpression.Operator == LogicalOperator.Or)
            sb.Append(" )");
    }

    public void Visit(CheckAmountExpression checkAmountExpression)
    {
        sb.Append($@"Amount {checkAmountExpression.ComparisionOperator.Visualize()} {checkAmountExpression.Value}");
    }

    public void Visit(CheckCardNameExpression cardNameExpression)
    {
        sb.Append($@"Card=""{cardNameExpression.Value}""");
    }

    public string GetResult()
    {
        return sb.ToString();
    }
}