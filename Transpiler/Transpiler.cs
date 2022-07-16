using System.Text;
using Parser.Grammars.LangGrammar;
using Parser.Nodes;
using Parser.Nodes.Statements;
using Parser.Utils;
using Transpiler.Errors;

namespace Transpiler;

public class Transpiler
{
    public List<IError> Errors = new();
    
    public TranspilationResult Transpile(INode node)
    {
        return TranspileMain(node);
    }

    public TranspilationResult TranspileMain(INode node)
    {
        if (node.Type == NLangRules.Statements.Block)
        {
            return TranspileBlockStatement((StatementListNode) node.Unwrap());
        }
        
        return new TranspilationResult(new List<IError>{new UnsupportedBaseNodeTypeError(node.Type)});
    }

    private TranspilationResult TranspileBlockStatement(StatementListNode statements)
    {
        var resultBuilder = new StringBuilder();

        // var constants = GetConstants(statements);
        
        //todo get constants -> create constant values dicts
        //when constant declaration occurs - just skip it
        //when constant usage occurs - take it's value from dict and place ir instead of constant usage
        return null;
    }
}