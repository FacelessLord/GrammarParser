using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parser.Grammars.tokens;
using Parser.Nodes;
using Parser.Rules;
using static Parser.Grammars.LangGrammar.NLangTerminals;

namespace Parser.Grammars.LangGrammar
{
    public static class NLangRules
    {
        public static readonly RuleToken IdList = Rule.Named(nameof(IdList))
            .With(Chain.StartWith(Id).CollectBy(nodes => new IdListNode(nodes[0])))
            .With(_ => Chain.StartWith(_).Then(Comma).Then(Id).CollectBy(nodes => new IdListNode(nodes[0], nodes[1])));

        public static readonly RuleToken VariableModifiersList = Rule.Named(nameof(VariableModifiersList))
            .With(Chain.StartWith(Compile).CollectBy(nodes => new VariableModifiersListNode(nodes[0])))
            .With(Chain.StartWith(Const).CollectBy(nodes => new VariableModifiersListNode(nodes[0])))
            .With(Chain.StartWith(Let).CollectBy(nodes => new VariableModifiersListNode(nodes[0])))
            .With(_ => Chain.StartWith(_).Then(Compile)
                .CollectBy(nodes => new VariableModifiersListNode(nodes[0], nodes[1])))
            .With(_ => Chain.StartWith(_).Then(Const)
                .CollectBy(nodes => new VariableModifiersListNode(nodes[0], nodes[1])))
            .With(_ => Chain.StartWith(_).Then(Let)
                .CollectBy(nodes => new VariableModifiersListNode(nodes[0], nodes[1])));

        public static readonly RuleToken CreateVar = Rule.Named(nameof(CreateVar))
            .With(Chain.StartWith(VariableModifiersList).Then(IdList).CollectBy(nodes => new CreateVarNode(nodes)));

        public static readonly RuleToken MemberAccess = Rule.Named(nameof(MemberAccess))
            .With(_ => Chain.StartWith(_).Then(Dot).Then(Id)
                .CollectBy(nodes => new MemberAccessNode(nodes[0], nodes[2])))
            .With(_ => Chain.StartWith(Expression).Then(Dot).Then(Id)
                .CollectBy(nodes => new MemberAccessNode(nodes[0], nodes[2])));

        public static readonly RuleToken IndexAccess = Rule.Named(nameof(IndexAccess))
            .With(_ => Chain.StartWith(Expression).Then(LBracket).Then(Expression).Then(RBracket)
                .CollectBy(nodes => new IndexAccessNode(nodes[0], nodes[2])));
        public static readonly RuleToken FuncCall = Rule.Named(nameof(FuncCall))
            .With(_ => Chain.StartWith(Expression).Then(LParen).Then(ExpressionList).Then(RParen)
                .CollectBy(nodes => new FuncCallNode(nodes[0], nodes[1])))
            .With(_ => Chain.StartWith(Expression).Then(LParen).Then(Expression).Then(RParen)
                .CollectBy(nodes => new FuncCallNode(nodes[0], nodes[1])));

        public static readonly RuleToken ExpressionList = Rule.Named(nameof(ExpressionList))
            .With(_ => Chain.StartWith(_).Then(Comma).Then(Expression)
                .CollectBy(nodes => new ExpressionListNode(nodes[0], nodes[2])))
            .With(_ => Chain.StartWith(Expression).Then(Comma).Then(Expression)
                .CollectBy(nodes => new ExpressionListNode(nodes[0], nodes[2])));

        public static readonly RuleToken Expression = Rule.Named(nameof(Expression))
            .With(_ => Chain.StartWith(LParen).Then(_).Then(RParen).CollectBy(nodes => new ExpressionNode(nodes[1])))
            .With(_ => Chain.StartWith(Tilda).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(Minus).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(Plus).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(ExclamationMark).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(Chain.StartWith(MemberAccess).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(Chain.StartWith(IndexAccess).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(Chain.StartWith(FuncCall).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(Chain.StartWith(CreateVar).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(_ => Chain.StartWith(_).Then(StrictEqual).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Equal).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Assign).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(NotStrictEqual).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(NotEqual).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(LessOrEqual).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(GreaterOrEqual).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Less).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Greater).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Plus).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Minus).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Asterisk).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(DoubleSlash).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Slash).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Ampersand).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Pipe).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(Hat).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(DoubleAmpersand).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(DoublePipe).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(_ => Chain.StartWith(_).Then(DoubleHat).Then(_).CollectBy(nodes => new ExpressionNode(nodes)))
            .With(Chain.StartWith(Id).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(Chain.StartWith(Int).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(Chain.StartWith(Float).CollectBy(nodes => new ExpressionNode(nodes[0])))
            .With(Chain.StartWith(NLangTerminals.String).CollectBy(nodes => new ExpressionNode(nodes[0])));

        public static RuleToken Statement = Rule.Named(nameof(Statement))
            .With(Chain.StartWith(Expression).Then(TokenType.Eof).CollectBy(nodes => new StatementNode(nodes[0])))
            .With(Chain.StartWith(Expression).Then(Semicolon).CollectBy(nodes => new StatementNode(nodes[0])));

        static NLangRules()
        {
            Rules = typeof(NLangRules).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(f => f.Name != nameof(Rules))
                .Select(f => f.GetValue(null))
                .Cast<RuleToken>()
                .Peek(r => r.FinishConfiguration())
                .ToList();
        }

        public static List<RuleToken> Rules;
    }
}