using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parser.Grammars.tokens;
using Parser.Nodes;
using Parser.Nodes.Primes;
using Parser.Nodes.Statements;
using Parser.Rules;
using static Parser.Grammars.LangGrammar.NLangTerminals;

namespace Parser.Grammars.LangGrammar
{
    public static class NLangRules
    {

        public static Func<TokenType, Func<INode[], INode>> Wrap = (type) => (node) =>
            new WrapperNode(type, node[0]);
        public static Func<INode[], INode> Unary = nodes => new UnaryExpressionNode(nodes[0], nodes[1]);
        public static Func<TokenType, INode, INode, INode, INode> Tree = (type, operation, left, right) =>
            new TypedBinaryExpressionTreeNode(type, operation, left, right);

        public static Func<TokenType, Func<INode[], INode>> TreeGen = (type) => (nodes) =>
            new TypedBinaryExpressionTreeNode(type, nodes[1], nodes[0], nodes[2]);

        public static class Expressions
        {
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

            public static readonly RuleToken Assignment = Rule.Named(nameof(Assignment))
                .With(_ => Chain.StartWith(UnaryExpression).Then(Assign).Then(Expression).CollectBy(nodes =>
                    Tree(Expression, nodes[1], nodes[0], nodes[2])));

            public static readonly RuleToken Expression = Rule.Named(nameof(Expression))
                .With(_ => Chain.StartWith(UnaryExpression).Then(Assign).Then(_).CollectBy(nodes =>
                    Tree(Expression, nodes[1], nodes[0], nodes[2])))
                .With(_ => Chain.StartWith(ConditionalExpression).CollectBy(Wrap(_)));

            public static readonly RuleToken LiteralExpression = Rule.Named(nameof(LiteralExpression))
                .With(Chain.StartWith(NLangTerminals.String).CollectBy(nodes => new StringExpressionNode(nodes[0])))
                .With(Chain.StartWith(Int).CollectBy(nodes => new NumberExpressionNode(nodes[0])))
                .With(Chain.StartWith(Float).CollectBy(nodes => new NumberExpressionNode(nodes[0])));

            public static readonly RuleToken CastExpression = Rule.Named(nameof(CastExpression))
                .With(_ => Chain.StartWith(LParen).Then(Variables.Type).Then(RParen).Then(UnaryExpression)
                    .CollectBy(nodes => new CastExpressionNode(nodes[1], nodes[3])));

            public static readonly RuleToken MemberAccessExpression = Rule.Named(nameof(MemberAccessExpression))
                .With(_ => Chain.StartWith(PrimaryExpression).Then(Dot).Then(Id)
                    .CollectBy(nodes => new MemberAccessExpressionNode(nodes[0], nodes[2])))
                .With(_ => Chain.StartWith(PrimaryExpression).Then(Dot).Then(Id).Then(Less).Then(Variables.TypeList)
                    .Then(Greater)
                    .CollectBy(nodes => new MemberAccessExpressionNode(nodes[0], nodes[2], nodes[4])));

            public static readonly RuleToken InvocationExpression = Rule.Named(nameof(InvocationExpression))
                .With(_ => Chain.StartWith(PrimaryExpression).Then(LParen).Then(RParen)
                    .CollectBy(nodes => new InvocationExpressionNode(nodes[0])))
                .With(_ => Chain.StartWith(PrimaryExpression).Then(LParen).Then(Variables.ArgumentList).Then(RParen)
                    .CollectBy(nodes => new InvocationExpressionNode(nodes[0], nodes[2])));

            public static readonly RuleToken IndexAccessExpression = Rule.Named(nameof(IndexAccessExpression))
                .With(_ => Chain.StartWith(PrimaryExpression).Then(LBracket).Then(RBracket)
                    .CollectBy(nodes => new IndexAccessExpressionNode(nodes[0])))
                .With(_ => Chain.StartWith(PrimaryExpression).Then(LBracket).Then(Variables.ArgumentList).Then(RBracket)
                    .CollectBy(nodes => new IndexAccessExpressionNode(nodes[0], nodes[2])));

            public static readonly RuleToken ThisAccessExpression = Rule.Named(nameof(ThisAccessExpression))
                .With(_ => Chain.StartWith(This).CollectBy(Wrap(_)));
            public static readonly RuleToken BaseAccessExpression = Rule.Named(nameof(BaseAccessExpression))
                .With(_ => Chain.StartWith(Base).CollectBy(Wrap(_)));

            public static readonly RuleToken ObjectCreationExpression = Rule.Named(nameof(ObjectCreationExpression))
                .With(Chain.StartWith(New).Then(Variables.Type).Then(LParen).Then(RParen)
                    .CollectBy(nodes => new ObjectCreationExpressionNode(nodes[1])))
                .With(Chain.StartWith(New).Then(Variables.Type).Then(LParen).Then(Variables.ArgumentList).Then(RParen)
                    .CollectBy(nodes => new ObjectCreationExpressionNode(nodes[1], nodes[3])));

            public static readonly RuleToken SimpleNameExpression = Rule.Named(nameof(SimpleNameExpression))
                .With(Chain.StartWith(Id).CollectBy(nodes => new SimpleNameExpressionNode(nodes[0])))
                .With(Chain.StartWith(Id).Then(Less).Then(Variables.TypeList).Then(Greater)
                    .CollectBy(nodes => new SimpleNameExpressionNode(nodes[0], nodes[1])));

            public static readonly RuleToken ArrayCreationExpression = Rule.Named(nameof(ArrayCreationExpression))
                .With(Chain.StartWith(New).Then(Variables.Type).Then(LBracket).Then(Variables.ArgumentList)
                    .Then(RBracket)
                    .CollectBy(nodes => new ArrayCreationExpressionNode(nodes[1], nodes[3])))
                .With(Chain.StartWith(New).Then(Variables.Type).Then(LBracket).Then(Variables.ArgumentList)
                    .Then(RBracket)
                    .Then(Variables.ArrayInitializer)
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
        }

        public static class Variables
        {
            public static readonly RuleToken TypeList = Rule.Named(nameof(TypeList))
                .With(_ => Chain.StartWith(Type).CollectBy(nodes => new TypeListNode(nodes[0])))
                .With(_ => Chain.StartWith(TypeList).Then(Comma).Then(Type)
                    .CollectBy(nodes => new TypeListNode(nodes[0], nodes[2])));

            public static readonly RuleToken Type = Rule.Named(nameof(Type))
                .With(Chain.StartWith(Id).CollectBy(nodes => new TypeNode(nodes[0])))
                .With(Chain.StartWith(Id).Then(Less).Then(TypeList).Then(Greater)
                    .CollectBy(nodes => new TypeNode(nodes[0], nodes[1])));

            public static readonly RuleToken ArgumentList = Rule.Named(nameof(ArgumentList))
                .With(_ => Chain.StartWith(ArgumentList).Then(Comma).Then(Expressions.Expression)
                    .CollectBy(nodes => new ArgumentListNode(nodes[2], nodes[0])))
                .With(_ => Chain.StartWith(Expressions.Expression).CollectBy(nodes => new ArgumentListNode(nodes[0])));

            public static readonly RuleToken LocalConstantDeclarator = Rule.Named(nameof(LocalConstantDeclarator))
                .With(_ => Chain.StartWith(Id).Then(Assign).Then(VariableInitializer)
                    .CollectBy(nodes => new LocalConstantDeclaratorNode(nodes[0], nodes[2])));

            public static readonly RuleToken LocalConstantDeclaratorList = Rule
                .Named(nameof(LocalConstantDeclaratorList))
                .With(_ => Chain.StartWith(LocalConstantDeclaratorList).Then(Comma).Then(LocalConstantDeclaratorList)
                    .CollectBy(nodes => new LocalConstantDeclaratorListNode(nodes[0], nodes[1])))
                .With(_ => Chain.StartWith(LocalConstantDeclarator)
                    .CollectBy(nodes => new LocalConstantDeclaratorListNode(nodes[0])));

            public static readonly RuleToken LocalVariableType = Rule.Named(nameof(LocalVariableType))
                .With(_ => Chain.StartWith(Type).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(Var).CollectBy(Wrap(_)));

            public static readonly RuleToken LocalVariableDeclarator = Rule.Named(nameof(LocalVariableDeclarator))
                .With(Chain.StartWith(Id).CollectBy(nodes => new LocalVariableDeclaratorNode(nodes[0])))
                .With(_ => Chain.StartWith(Id).Then(Assign).Then(VariableInitializer)
                    .CollectBy(nodes => new LocalVariableDeclaratorNode(nodes[0], nodes[2])));

            public static readonly RuleToken LocalVariableDeclaratorList = Rule
                .Named(nameof(LocalVariableDeclaratorList))
                .With(_ => Chain.StartWith(LocalVariableDeclaratorList).Then(Comma).Then(LocalVariableDeclarator)
                    .CollectBy(nodes => new LocalVariableDeclaratorListNode(nodes[0], nodes[2])))
                .With(_ => Chain.StartWith(LocalVariableDeclarator)
                    .CollectBy(nodes => new LocalVariableDeclaratorListNode(nodes[0])));

            public static readonly RuleToken VariableInitializer = Rule.Named(nameof(VariableInitializer))
                .With(_ => Chain.StartWith(Expressions.Expression)
                    .CollectBy(nodes => new VariableInitializerNode(nodes[0])))
                .With(_ => Chain.StartWith(ArrayInitializer)
                    .CollectBy(nodes => new VariableInitializerNode(nodes[0])));

            public static readonly RuleToken VariableInitializerList = Rule
                .Named(nameof(VariableInitializerList))
                .With(Chain.StartWith(VariableInitializer)
                    .CollectBy(nodes => new VariableInitializerListNode(nodes[0])))
                .With(_ => Chain.StartWith(VariableInitializerList).Then(Comma).Then(VariableInitializer)
                    .CollectBy(nodes => new VariableInitializerListNode(nodes[0], nodes[2])));

            public static readonly RuleToken ArrayInitializer = Rule.Named(nameof(ArrayInitializer))
                .With(Chain.StartWith(LBrace).Then(RBrace).CollectBy(nodes => new ArrayInitializerNode()))
                .With(Chain.StartWith(LBrace).Then(VariableInitializerList).Then(RBrace)
                    .CollectBy(nodes => new ArrayInitializerNode(nodes[1])))
                .With(Chain.StartWith(LBrace).Then(VariableInitializerList).Then(Comma).Then(RBrace)
                    .CollectBy(nodes => new ArrayInitializerNode(nodes[1])));

            public static readonly RuleToken LocalConstantDeclaration = Rule.Named(nameof(LocalConstantDeclaration))
                .With(Chain.StartWith(Const).Then(Type).Then(LocalConstantDeclaratorList)
                    .CollectBy(nodes => new LocalConstantDeclarationNode(nodes[1], nodes[2])));

            public static readonly RuleToken LocalVariableDeclaration = Rule.Named(nameof(LocalVariableDeclaration))
                .With(Chain.StartWith(LocalVariableType).Then(LocalVariableDeclaratorList)
                    .CollectBy(nodes => new LocalVariableDeclarationNode(nodes[0], nodes[1])));
        }

        public static class Statements
        {
            public static readonly RuleToken Block = Rule.Named(nameof(Block))
                .With(_ => Chain.StartWith(LBrace).Then(StatementList).Then(RBrace)
                    .CollectBy(nodes => new WrapperNode(Block, nodes[1])));

            public static readonly RuleToken EmptyStatement = Rule.Named(nameof(EmptyStatement))
                .With(Chain.StartWith(Semicolon).CollectBy(nodes => new EmptyStatementNode()));

            public static readonly RuleToken WhileStatement = Rule.Named(nameof(WhileStatement))
                .With(_ => Chain.StartWith(While).Then(LParen).Then(Expressions.Expression).Then(RParen)
                    .Then(EmbeddedStatement).CollectBy(nodes => new WhileStatementNode(nodes[2], nodes[4])));

            public static readonly RuleToken StatementExpressionList = Rule.Named(nameof(StatementExpressionList))
                .With(_ => Chain.StartWith(StatementExpression)
                    .CollectBy(nodes => new StatementExpressionListNode(nodes[0])))
                .With(_ => Chain.StartWith(StatementExpressionList).Then(Comma).Then(StatementExpression)
                    .CollectBy(nodes => new StatementExpressionListNode(nodes[0], nodes[2])));

            public static readonly RuleToken ForInitializer = Rule.Named(nameof(ForInitializer))
                .With(_ => Chain.StartWith(Variables.LocalVariableDeclaration).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(StatementExpressionList).CollectBy(Wrap(_)));

            public static readonly RuleToken ForCondition = Rule.Named(nameof(ForCondition))
                .With(_ => Chain.StartWith(Expressions.Expression).CollectBy(Wrap(_)));

            public static readonly RuleToken ForIterator = Rule.Named(nameof(ForIterator))
                .With(_ => Chain.StartWith(StatementExpressionList).CollectBy(Wrap(_)));

            // T_T I want lambda rules T_T
            public static readonly RuleToken ForStatement = Rule.Named(nameof(ForStatement))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(ForInitializer).Then(Semicolon).Then(ForCondition)
                    .Then(Semicolon).Then(ForIterator).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[8], nodes[2], nodes[4], nodes[6])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(Semicolon).Then(ForCondition)
                    .Then(Semicolon).Then(ForIterator).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[7], null, nodes[3], nodes[5])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(ForInitializer).Then(Semicolon)
                    .Then(Semicolon).Then(ForIterator).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[7], nodes[2], null, nodes[5])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(ForInitializer).Then(Semicolon).Then(ForCondition)
                    .Then(Semicolon).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[7], nodes[2], nodes[4])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(ForInitializer).Then(Semicolon)
                    .Then(Semicolon).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[6], nodes[2])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(Semicolon).Then(ForCondition)
                    .Then(Semicolon).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[6], null, nodes[3])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(Semicolon)
                    .Then(Semicolon).Then(ForIterator).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[6], null, null, nodes[4])))
                .With(_ => Chain.StartWith(For).Then(LParen).Then(Semicolon).Then(Semicolon).Then(RParen)
                    .Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForStatementNode(nodes[8])));

            public static readonly RuleToken ForeachStatement = Rule.Named(nameof(ForeachStatement))
                .With(_ => Chain.StartWith(Foreach).Then(LParen).Then(Variables.LocalVariableType).Then(Id).Then(In)
                    .Then(Expressions.Expression).Then(RParen).Then(EmbeddedStatement).CollectBy(nodes =>
                        new ForeachStatementNode(nodes[2], nodes[3], nodes[5], nodes[7])));

            public static readonly RuleToken StatementExpression = Rule.Named(nameof(StatementExpression))
                .With(_ => Chain.StartWith(Expressions.InvocationExpression).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(Expressions.Assignment).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(Expressions.ObjectCreationExpression).CollectBy(Wrap(_)));

            public static RuleToken ExpressionStatement = Rule.Named(nameof(ExpressionStatement))
                .With(_ => Chain.StartWith(StatementExpression).Then(Semicolon).CollectBy(Wrap(_)));

            public static readonly RuleToken IterationStatement = Rule.Named(nameof(IterationStatement))
                .With(_ => Chain.StartWith(WhileStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(ForStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(ForeachStatement).CollectBy(Wrap(_)));

            public static readonly RuleToken BreakStatement = Rule.Named(nameof(BreakStatement))
                .With(_ => Chain.StartWith(Break).Then(Semicolon).CollectBy(nodes => new JumpStatementNode(_)));

            public static readonly RuleToken ContinueStatement = Rule.Named(nameof(ContinueStatement))
                .With(_ => Chain.StartWith(Continue).Then(Semicolon).CollectBy(nodes => new JumpStatementNode(_)));

            public static readonly RuleToken GotoStatement = Rule.Named(nameof(GotoStatement))
                .With(_ => Chain.StartWith(Goto).Then(Id).Then(Semicolon)
                    .CollectBy(nodes => new JumpStatementNode(_, targetLabel: nodes[1])));
            public static readonly RuleToken ReturnStatement = Rule.Named(nameof(ReturnStatement))
                .With(_ => Chain.StartWith(Return).Then(Semicolon).CollectBy(nodes => new JumpStatementNode(_)))
                .With(_ => Chain.StartWith(Return).Then(Expressions.Expression).Then(Semicolon)
                    .CollectBy(nodes => new JumpStatementNode(_, expression: nodes[1])));

            public static readonly RuleToken ThrowStatement = Rule.Named(nameof(ThrowStatement))
                .With(_ => Chain.StartWith(Throw).Then(Semicolon).CollectBy(nodes => new JumpStatementNode(_)))
                .With(_ => Chain.StartWith(Throw).Then(Expressions.Expression).Then(Semicolon)
                    .CollectBy(nodes => new JumpStatementNode(_, expression: nodes[1])));

            public static readonly RuleToken JumpStatement = Rule.Named(nameof(JumpStatement))
                .With(_ => Chain.StartWith(BreakStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(ContinueStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(GotoStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(ReturnStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(ThrowStatement).CollectBy(Wrap(_)));

            public static readonly RuleToken IfStatement = Rule.Named(nameof(IfStatement))
                .With(_ => Chain.StartWith(If).Then(LParen).Then(Expressions.Expression).Then(RParen)
                    .Then(EmbeddedStatement)
                    .CollectBy(nodes => new IfStatementNode(nodes[2], nodes[4])))
                .With(_ => Chain.StartWith(If).Then(LParen).Then(Expressions.Expression).Then(RParen)
                    .Then(EmbeddedStatement).Then(Else).Then(EmbeddedStatement)
                    .CollectBy(nodes => new IfStatementNode(nodes[2], nodes[4], nodes[6])));

            public static readonly RuleToken SelectionStatement = Rule.Named(nameof(SelectionStatement))
                .With(_ => Chain.StartWith(IfStatement).CollectBy(Wrap(_)));

            public static readonly RuleToken EmbeddedStatement = Rule.Named(nameof(EmbeddedStatement))
                .With(_ => Chain.StartWith(Block).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(EmptyStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(SelectionStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(IterationStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(JumpStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(ExpressionStatement).CollectBy(Wrap(_)));

            public static readonly RuleToken LabeledStatement = Rule.Named(nameof(LabeledStatement))
                .With(_ => Chain.StartWith(Id).Then(Colon).Then(Statement)
                    .CollectBy(nodes => new LabeledStatementNode(nodes[0], nodes[1])));

            public static readonly RuleToken DeclarationStatement = Rule.Named(nameof(DeclarationStatement))
                .With(_ => Chain.StartWith(Variables.LocalVariableDeclaration).Then(Semicolon).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(Variables.LocalConstantDeclaration).Then(Semicolon).CollectBy(Wrap(_)));

            public static RuleToken Statement = Rule.Named(nameof(Statement))
                .With(_ => Chain.StartWith(LabeledStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(DeclarationStatement).CollectBy(Wrap(_)))
                .With(_ => Chain.StartWith(EmbeddedStatement).CollectBy(Wrap(_)));

            public static readonly RuleToken StatementList = Rule.Named(nameof(StatementList))
                .With(_ => Chain.StartWith(Statement).CollectBy(nodes => new StatementListNode(nodes[0])))
                .With(_ => Chain.StartWith(StatementList).Then(Statement)
                    .CollectBy(nodes => new StatementListNode(nodes[0], nodes[1])));
        }

        static NLangRules()
        {
            Rules = typeof(NLangRules).GetNestedTypes()
                .SelectMany(c => c.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly))
                .Where(f => f.Name != nameof(Rules))
                .Select(f => f.GetValue(null))
                .Cast<RuleToken>()
                .Peek(r => r.FinishConfiguration())
                .ToList();
        }

        public static List<RuleToken> Rules;
    }
}