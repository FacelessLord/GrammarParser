using System;
using System.Collections.Generic;
using Parser.Grammars;
using Parser.Grammars.tokens;

namespace Parser.Automaton
{
    public interface IAutomatonAction
    {
        void Apply(Stack<(Token, AutomatonState)> stack, Token token, Token lookAheadToken, Action<Token> pushTokenToInput);

        bool ConsumesToken();
    }

    public class ShiftAction : IAutomatonAction
    {
        public ShiftAction(AutomatonState targetState)
        {
            _targetState = targetState;
        }
        
        private readonly AutomatonState _targetState;

        public void Apply(Stack<(Token, AutomatonState)> stack, Token token, Token lookAheadToken, Action<Token> pushTokenToInput)
        {
            stack.Push((token, _targetState));
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
        public void Apply(Stack<(Token, AutomatonState)> stack, Token token, Token lookAheadToken, Action<Token> pushTokenToInput)
        {
            var newTokenInternals = new List<Token>();
            for (var i = 0; i < _rule.Production.Count; i++)
            {
                var (stackToken, state) = stack.Pop();
                newTokenInternals.Add(stackToken);
            }
            var newTokenStartPath = newTokenInternals[0].TokenStartPath;
            var newTokenEndPath = newTokenInternals[^1].TokenEndPath;

            var newToken = new Token(_rule.Source, newTokenStartPath, newTokenEndPath, newTokenInternals);
            pushTokenToInput(newToken);
        }

        public bool ConsumesToken() => false;
    }
}