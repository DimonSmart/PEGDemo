using Parser.Expressions;

namespace Parser;

public interface IVisitor
{
    void Visit(BinaryDemoExpression binaryDemoExpression);
    void Visit(CheckAmountExpression checkAmountExpression);
    void Visit(CheckCardNameExpression checkCardNameExpression);
}