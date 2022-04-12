using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars;
using Parser.Grammars.Paths;
using Parser.Grammars.Tokens;

namespace Parser.Automaton
{
    public interface IAutomatonAction
    {
        void Apply(Stack<(NodeWrapper, AutomatonState)> stack, NodeWrapper nonTerminalNode, NodeWrapper lookAheadNonTerminalNode, Action<NodeWrapper> pushTokenToInput);

        bool ConsumesToken();
    }

    public class ShiftAction : IAutomatonAction
    {
        public ShiftAction(AutomatonState targetState)
        {
            _targetState = targetState;
        }
        
        private readonly AutomatonState _targetState;

        public void Apply(Stack<(NodeWrapper, AutomatonState)> stack, NodeWrapper nonTerminalNode, NodeWrapper lookAheadNonTerminalNode, Action<NodeWrapper> pushTokenToInput)
        {
            stack.Push((nonTerminalNode, _targetState));
        }

        public bool ConsumesToken() => true;
    }
    
    public class ReduceAction : IAutomatonAction
    {   
        private GrammarRule _rule;
        public ReduceAction(GrammarRule rule)
        {
            _rule = rule;
        }
        public void Apply(Stack<(NodeWrapper, AutomatonState)> stack, NodeWrapper nonTerminalNode, NodeWrapper lookAheadNonTerminalNode, Action<NodeWrapper> pushTokenToInput)
        {
            var newTokenInternals = new List<NodeWrapper>();
            for (var i = 0; i < _rule.Production.Count; i++)
            {
                var (stackToken, state) = stack.Pop();
                newTokenInternals.Add(stackToken);
            }
            newTokenInternals.Reverse();
            var newTokenStartPath = newTokenInternals[0].TokenSpan.FilePathStart;
            var newTokenEndPath = newTokenInternals[^1].TokenSpan.FilePathEnd;

            var node = _rule.Collector(newTokenInternals.Select(w => w.Node).ToArray());
            var wrapper = new NodeWrapper(node, new ContentSpan(newTokenStartPath, newTokenEndPath));
            pushTokenToInput(wrapper);
        }

        public bool ConsumesToken() => false;
    }
}