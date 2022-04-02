using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.LangGrammar;
using Parser.Grammars.Paths;
using Parser.Grammars.tokens;

namespace Parser.Nodes
{
    public class CreateVarNode : INode
    {
        public static Dictionary<TokenType, VariableModifiers> ModifiersMap = new()
        {
            [NLangTerminals.Compile] = VariableModifiers.Compile, 
            [NLangTerminals.Const] = VariableModifiers.Const, 
            [NLangTerminals.Let] = VariableModifiers.Let
        };

        public VariableModifiers Modifiers;
        public List<string> Names;
        public TokenType Type { get; } = NLangRules.CreateVar;

        public CreateVarNode(INode[] nodes)
        {
            Modifiers = ((VariableModifiersListNode)nodes[0]).Modifiers;
            Names = ((IdListNode)nodes[1]).Ids;
        }
    }
}