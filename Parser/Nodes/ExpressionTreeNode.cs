using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using static Operation;

namespace Parser.Nodes;

public class ExpressionTreeNode : INode
{
    public TokenType Type { get; } = NLangRules.Expression;
    public Operation Operation { get; }
    public List<ExpressionTreeNode> Args { get; }

    public ExpressionTreeNode(Operation operation, params INode[] args)
    {
        Operation = operation;
        Args = args.Cast<ExpressionTreeNode>().ToList();
    }

    public static ExpressionTreeNode BuildNode(List<INode> expressions)
    {
        for (int i = 0; i < expressions.Count; i++)
        {
            if (expressions[i] is ExpressionTreeLeafNode leaf)
            {
                expressions.RemoveAt(i);
                expressions.Insert(i, leaf.Leaf);
            }
        }
        ProcessUnaryOperations(expressions);
        return ProcessBinaryOperations(expressions);
    }

    private static ExpressionTreeNode ProcessBinaryOperations(List<INode> expressions)
    {
        if (expressions.Count == 1)
        {
            if (expressions[0] is ExpressionTreeNode tree)
                return tree;
            return new ExpressionTreeLeafNode(expressions[0]);
        }
        for (var i = 0; i < Priorities.Count; i++)
        {
            for (var j = expressions.Count - 2; j >= 0; j -= 2)
            {
                var operation = ((OperationTerminalType) expressions[j].Type).Operation;
                if (Priorities[i].Contains(operation))
                {
                    var left = ProcessBinaryOperations(expressions.GetRange(0, j));
                    var right = ProcessBinaryOperations(expressions.GetRange(j + 1, expressions.Count - j - 1));
                    return new ExpressionTreeNode(operation, left, right);
                }
            }
        }
        return null!;
    }

    private static void ProcessUnaryOperations(List<INode> expressions)
    {
        var unaryOperationPositions = UnaryOperation(expressions);
        unaryOperationPositions.Reverse();
        foreach (var position in unaryOperationPositions)
        {
            var operand = expressions[position + 1];
            var tree = new ExpressionTreeNode(((OperationTerminalType) (expressions[position]).Type).Operation,
                operand);
            expressions.RemoveRange(position, 2);
            expressions.Insert(position, tree);
        }
    }

    private static List<int> UnaryOperation(List<INode> operations)
    {
        var unaryOperationPositions = new List<int>();
        var previousNodeIsOperation = false;
        for (var i = 0; i < operations.Count; i++)
        {
            var nextNodeIsOperation = operations[i] is TerminalNode { Type: OperationTerminalType } term;
            if (previousNodeIsOperation && nextNodeIsOperation)
            {
                unaryOperationPositions.Add(i);
            }

            previousNodeIsOperation = nextNodeIsOperation;
        }
        return unaryOperationPositions;
    }

    public static List<HashSet<Operation>> Priorities = new List<HashSet<Operation>>()
    {
        new() { Assign },
        new() { And, Or, Xor },
        new() { StrictEqual, Equal, NotStrictEqual, NotEqual, Less, LessOrEqual, Greater, GreaterOrEqual },
        new() { Plus, Minus },
        new() { Multiply, Divide, DivideInt },
        new() { BitwiseAnd, BitwiseOr, BitwiseXor },
    };

}