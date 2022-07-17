using Parser.Expressions;

namespace Parser;

public interface IVisitor
{
    void Visit(BinaryDemoExpression binaryDemoExpression);
    void Visit(CheckAmountExpression binaryDemoExpression);
    void Visit(CheckCardNameExpression binaryDemoExpression);
}