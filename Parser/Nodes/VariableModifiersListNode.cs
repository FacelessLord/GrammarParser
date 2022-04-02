using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.LangGrammar;
using Parser.Grammars.Paths;
using Parser.Grammars.tokens;

namespace Parser.Nodes
{
    public class VariableModifiersListNode : INode
    {
        public TokenType Type { get; } = NLangRules.VariableModifiersList;
        public readonly VariableModifiers Modifiers;

        public VariableModifiersListNode(INode node)
        {
            Modifiers = CreateVarNode.ModifiersMap[((TerminalNode) node).Type];
        }
        public VariableModifiersListNode(INode prev, INode next)
        {
            Modifiers = ((VariableModifiersListNode) prev).Modifiers |
                        CreateVarNode.ModifiersMap[((TerminalNode) next).Type];
        }
    }
}