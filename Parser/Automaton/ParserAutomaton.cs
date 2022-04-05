using System.Collections.Generic;
using Parser.Grammars.Paths;
using Parser.Grammars.tokens;
using Parser.Nodes;
using Parser.Utils;

namespace Parser.Automaton
{
    public class ParserAutomaton
    {
        private readonly AutomatonTable _table;
        private readonly AutomatonInfo _info;
        private readonly Stack<(NodeWrapper, AutomatonState)> _automatonStack;
        public ParserAutomaton(AutomatonTable table, AutomatonInfo info, Stack<(NodeWrapper, AutomatonState)> automatonStack)
        {
            _table = table;
            _info = info;
            _automatonStack = automatonStack;
        }

        public NodeWrapper Parse(Stack<NodeWrapper> tokenStream)
        {
            NodeWrapper nodeWrapper = new NodeWrapper(null, null);
            while(tokenStream.Count > 0)
            {
             var s = _automatonStack.Peek();
             nodeWrapper = s.Item1;
             var automatonState = s.Item2;
                var currentToken = tokenStream.Pop();
                var currentTokenNode = currentToken.Node;
                if (currentTokenNode.Type == _info.Grammar.Axiom)
                    return currentToken;
                var lookaheadToken = tokenStream.Count > 0 ? tokenStream.Peek() : currentToken;
                var lookaheadTokenNode = lookaheadToken.Node;
                var action = _table[automatonState, currentTokenNode.Type, lookaheadTokenNode.Type];
                if (!action.ConsumesToken())
                    tokenStream.Push(currentToken);
                action.Apply(_automatonStack, currentToken, lookaheadToken, tokenStream.Push);
                
                if(currentTokenNode.Type == TokenType.Eof)
                    break;
            }
            try
            {
                var eof = new TerminalNode(TokenType.Eof, "");
                var eofNode = new NodeWrapper(eof, nodeWrapper.TokenSpan);
                while(true){
                    var (_, automatonState) = _automatonStack.Peek();
                    var action = _table[automatonState, TokenType.Eof, TokenType.Eof];
                    action.Apply(_automatonStack, eofNode, eofNode, tokenStream.Push);
                }
            }
            catch (Exception e)
            {
            }
            return _automatonStack.Peek().Item1;
        }
    }
}