using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parser.Grammars.tokens;
using Parser.Nodes;
using Parser.Nodes.Primes;
using Parser.Rules;
using static Parser.Grammars.LangGrammar.NLangTerminals;

namespace Parser.Grammars.LangGrammar
{
    public static class NLangRules
    {
        public static Func<TokenType, INode, INode, INode, INode> Tree = (type, operation, left, right) =>
            new TypedBinaryExpressionTreeNode(type, operation, left, right);

        public static Func<TokenType, Func<INode[], INode>> TreeGen = (type) => (nodes) =>
            new TypedBinaryExpressionTreeNode(type, nodes[1], nodes[0], nodes[2]);

        public static Func<TokenType, Func<INode[], INode>> Wrap = (type) => (node) =>
            new WrapperNode(type, node[0]);

        public static readonly RuleToken MultiplicationExpression = Rule.Named(nameof(MultiplicationExpression))
            .With(_ => Chain.StartWith(UnaryExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(UnaryExpression).Then(Asterisk).Then(_).CollectBy(TreeGen(_)));

        public static readonly RuleToken AdditionExpression = Rule.Named(nameof(AdditionExpression))
            .With(_ => Chain.StartWith(MultiplicationExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(Plus).Then(MultiplicationExpression)
                .CollectBy(TreeGen(AdditionExpression)))
            .With(_ => Chain.StartWith(_).Then(Minus).Then(MultiplicationExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken ShiftExpression = Rule.Named(nameof(ShiftExpression))
            .With(_ => Chain.StartWith(AdditionExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(ShiftLeft).Then(AdditionExpression).CollectBy(TreeGen(_)))
            .With(_ => Chain.StartWith(_).Then(ShiftRight).Then(AdditionExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken RelationExpression = Rule.Named(nameof(RelationExpression))
            .With(_ => Chain.StartWith(ShiftExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(Greater).Then(ShiftExpression).CollectBy(TreeGen(_)))
            .With(_ => Chain.StartWith(_).Then(Less).Then(ShiftExpression).CollectBy(TreeGen(_)))
            .With(_ => Chain.StartWith(_).Then(GreaterOrEqual).Then(ShiftExpression).CollectBy(TreeGen(_)))
            .With(_ => Chain.StartWith(_).Then(LessOrEqual).Then(ShiftExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken EqualityExpression = Rule.Named(nameof(EqualityExpression))
            .With(_ => Chain.StartWith(RelationExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(Equal).Then(RelationExpression).CollectBy(TreeGen(_)))
            .With(_ => Chain.StartWith(_).Then(NotEqual).Then(RelationExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken InclusiveAndExpression = Rule.Named(nameof(InclusiveAndExpression))
            .With(_ => Chain.StartWith(EqualityExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(Ampersand).Then(EqualityExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken ExclusiveOrExpression = Rule.Named(nameof(ExclusiveOrExpression))
            .With(_ => Chain.StartWith(InclusiveAndExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(Hat).Then(InclusiveAndExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken InclusiveOrExpression = Rule.Named(nameof(InclusiveOrExpression))
            .With(_ => Chain.StartWith(ExclusiveOrExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(Pipe).Then(ExclusiveOrExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken AndExpression = Rule.Named(nameof(AndExpression))
            .With(_ => Chain.StartWith(InclusiveOrExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(DoubleAmpersand).Then(InclusiveOrExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken OrExpression = Rule.Named(nameof(OrExpression))
            .With(_ => Chain.StartWith(AndExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(_).Then(DoubleQuestionMark).Then(AndExpression).CollectBy(TreeGen(_)));

        public static readonly RuleToken NullCoalesceExpression = Rule.Named(nameof(NullCoalesceExpression))
            .With(_ => Chain.StartWith(OrExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(OrExpression).Then(DoubleQuestionMark).Then(_).CollectBy(TreeGen(_)));

        public static readonly RuleToken ConditionalExpression = Rule.Named(nameof(ConditionalExpression))
            .With(_ => Chain.StartWith(NullCoalesceExpression).Then(QuestionMark).Then(Expression)
                .CollectBy(nodes => new ConditionalExpressionNode(nodes[0], nodes[1], nodes[2])))
            .With(_ => Chain.StartWith(NullCoalesceExpression).CollectBy(Wrap(_)));

        public static readonly RuleToken Expression = Rule.Named(nameof(Expression))
            .With(_ => Chain.StartWith(UnaryExpression).Then(Assign).Then(_).CollectBy(nodes =>
                Tree(Expression, nodes[1], nodes[0], nodes[2])))
            .With(_ => Chain.StartWith(ConditionalExpression).CollectBy(Wrap(_)));

        public static Func<INode[], INode> Unary = nodes => new UnaryExpressionNode(nodes[0], nodes[1]);

        public static readonly RuleToken LiteralExpression = Rule.Named(nameof(LiteralExpression))
            .With(Chain.StartWith(NLangTerminals.String).CollectBy(nodes => new StringExpressionNode(nodes[0])))
            .With(Chain.StartWith(Int).CollectBy(nodes => new NumberExpressionNode(nodes[0])))
            .With(Chain.StartWith(Float).CollectBy(nodes => new NumberExpressionNode(nodes[0])));

        public static readonly RuleToken TypeExpressionList = Rule.Named(nameof(TypeExpressionList))
            .With(_ => Chain.StartWith(TypeExpression).CollectBy(nodes => new TypeExpressionListNode(nodes[0])))
            .With(_ => Chain.StartWith(TypeExpressionList).Then(Comma).Then(TypeExpression)
                .CollectBy(nodes => new TypeExpressionListNode(nodes[0], nodes[2])));
        public static readonly RuleToken TypeExpression = Rule.Named(nameof(TypeExpression))
            .With(Chain.StartWith(Id).CollectBy(nodes => new TypeExpressionNode(nodes[0])))
            .With(Chain.StartWith(Id).Then(Less).Then(TypeExpressionList).Then(Greater)
                .CollectBy(nodes => new TypeExpressionNode(nodes[0], nodes[1])));
        public static readonly RuleToken CastExpression = Rule.Named(nameof(CastExpression))
            .With(_ => Chain.StartWith(LParen).Then(TypeExpression).Then(RParen).Then(UnaryExpression)
                .CollectBy(nodes => new CastExpressionNode(nodes[1], nodes[3])));

        public static readonly RuleToken SimpleNameExpression = Rule.Named(nameof(SimpleNameExpression))
            .With(Chain.StartWith(Id).CollectBy(nodes => new SimpleNameExpressionNode(nodes[0])))
            .With(Chain.StartWith(Id).Then(Less).Then(TypeExpressionList).Then(Greater)
                .CollectBy(nodes => new SimpleNameExpressionNode(nodes[0], nodes[1])));

        public static readonly RuleToken MemberAccessExpression = Rule.Named(nameof(MemberAccessExpression))
            .With(_ => Chain.StartWith(PrimaryExpression).Then(Dot).Then(Id)
                .CollectBy(nodes => new MemberAccessExpressionNode(nodes[0], nodes[2])))
            .With(_ => Chain.StartWith(PrimaryExpression).Then(Dot).Then(Id).Then(Less).Then(TypeExpressionList)
                .Then(Greater)
                .CollectBy(nodes => new MemberAccessExpressionNode(nodes[0], nodes[2], nodes[4])));

        public static readonly RuleToken ArgumentListExpression = Rule.Named(nameof(InvocationExpression))
            .With(_ => Chain.StartWith(ArgumentListExpression).Then(Comma).Then(Expression)
                .CollectBy(nodes => new ArgumentListNode(nodes[2], nodes[0])))
            .With(_ => Chain.StartWith(Expression).CollectBy(nodes => new ArgumentListNode(nodes[0])));

        public static readonly RuleToken InvocationExpression = Rule.Named(nameof(InvocationExpression))
            .With(_ => Chain.StartWith(PrimaryExpression).Then(LParen).Then(RParen)
                .CollectBy(nodes => new InvocationExpressionNode(nodes[0])))
            .With(_ => Chain.StartWith(PrimaryExpression).Then(LParen).Then(ArgumentListExpression).Then(RParen)
                .CollectBy(nodes => new InvocationExpressionNode(nodes[0], nodes[2])));

        public static readonly RuleToken IndexAccessExpression = Rule.Named(nameof(IndexAccessExpression))
            .With(_ => Chain.StartWith(PrimaryExpression).Then(LBracket).Then(RBracket)
                .CollectBy(nodes => new IndexAccessExpressionNode(nodes[0])))
            .With(_ => Chain.StartWith(PrimaryExpression).Then(LBracket).Then(ArgumentListExpression).Then(RBracket)
                .CollectBy(nodes => new IndexAccessExpressionNode(nodes[0], nodes[2])));

        public static readonly RuleToken ThisAccessExpression = Rule.Named(nameof(ThisAccessExpression))
            .With(_ => Chain.StartWith(This).CollectBy(Wrap(_)));
        public static readonly RuleToken BaseAccessExpression = Rule.Named(nameof(BaseAccessExpression))
            .With(_ => Chain.StartWith(Base).CollectBy(Wrap(_)));

        public static readonly RuleToken ObjectCreationExpression = Rule.Named(nameof(ObjectCreationExpression))
            .With(Chain.StartWith(New).Then(TypeExpression).Then(LParen).Then(RParen)
                .CollectBy(nodes => new ObjectCreationExpressionNode(nodes[1])))
            .With(Chain.StartWith(New).Then(TypeExpression).Then(LParen).Then(ArgumentListExpression).Then(RParen)
                .CollectBy(nodes => new ObjectCreationExpressionNode(nodes[1], nodes[3])));

        public static readonly RuleToken VariableIntializerExpression = Rule.Named(nameof(VariableIntializerExpression))
            .With(_ => Chain.StartWith(Expression).CollectBy(nodes => new VariableIntializerExpressionNode(nodes[0])))
            .With(_ => Chain.StartWith(ArrayInitializerExpression).CollectBy(nodes => new VariableIntializerExpressionNode(nodes[0])));
        
        public static readonly RuleToken VariableIntializerListExpression = Rule
            .Named(nameof(VariableIntializerListExpression))
            .With(Chain.StartWith(VariableIntializerExpression)
                .CollectBy(nodes => new VariableIntializerListExpressionNode(nodes[0])))
            .With(_ => Chain.StartWith(VariableIntializerListExpression).Then(Comma).Then(VariableIntializerExpression)
                .CollectBy(nodes => new VariableIntializerListExpressionNode(nodes[0], nodes[2])));
        
        public static readonly RuleToken ArrayInitializerExpression = Rule.Named(nameof(ArrayInitializerExpression))
            .With(Chain.StartWith(LBrace).Then(RBrace).CollectBy(nodes => new ArrayInitializerExpressionNode()))
            .With(Chain.StartWith(LBrace).Then(VariableIntializerListExpression).Then(RBrace).CollectBy(nodes => new ArrayInitializerExpressionNode(nodes[1])))
            .With(Chain.StartWith(LBrace).Then(VariableIntializerListExpression).Then(Comma).Then(RBrace).CollectBy(nodes => new ArrayInitializerExpressionNode(nodes[1])));

        public static readonly RuleToken ArrayCreationExpression = Rule.Named(nameof(ArrayCreationExpression))
            .With(Chain.StartWith(New).Then(TypeExpression).Then(LBracket).Then(ArgumentListExpression).Then(RBracket)
                .CollectBy(nodes => new ArrayCreationExpressionNode(nodes[1], nodes[3])))
            .With(Chain.StartWith(New).Then(TypeExpression).Then(LBracket).Then(ArgumentListExpression).Then(RBracket)
                .Then(ArrayInitializerExpression)
                .CollectBy(nodes => new ArrayCreationExpressionNode(nodes[1], nodes[3], nodes[5])));

        public static readonly RuleToken PrimaryExpression = Rule.Named(nameof(PrimaryExpression))
            .With(_ => Chain.StartWith(LiteralExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(SimpleNameExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(MemberAccessExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(InvocationExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(IndexAccessExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(ThisAccessExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(BaseAccessExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(ObjectCreationExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(ArrayCreationExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(LParen).Then(Expression).Then(RParen)
                .CollectBy(nodes => new WrapperNode(_, nodes[1])));

        public static readonly RuleToken UnaryExpression = Rule.Named(nameof(UnaryExpression))
            .With(Chain.StartWith(ExclamationMark).Then(Expression).CollectBy(Unary))
            .With(Chain.StartWith(Tilda).Then(Expression).CollectBy(Unary))
            .With(Chain.StartWith(Plus).Then(Expression).CollectBy(Unary))
            .With(Chain.StartWith(Minus).Then(Expression).CollectBy(Unary))
            .With(Chain.StartWith(DoubleMinus).Then(Expression).CollectBy(Unary))
            .With(Chain.StartWith(DoublePlus).Then(Expression).CollectBy(Unary))
            .With(Chain.StartWith(DoublePlus).Then(Expression).CollectBy(Unary))
            .With(_ => Chain.StartWith(CastExpression).CollectBy(Wrap(_)))
            .With(_ => Chain.StartWith(PrimaryExpression).CollectBy(Wrap(_)));

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