/*
 * Copyright 2014 Technische Universität Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.SSTs;
using KaVE.Commons.Model.SSTs.Blocks;
using KaVE.Commons.Model.SSTs.Expressions;
using KaVE.Commons.Model.SSTs.Expressions.Assignable;
using KaVE.Commons.Model.SSTs.Expressions.LoopHeader;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.SSTs.Impl.Blocks;
using KaVE.Commons.Model.SSTs.Impl.Declarations;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.Commons.Model.SSTs.Impl.Expressions.LoopHeader;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.References;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.Commons.Model.SSTs.Statements;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.TestUtils.Model.SSTs
{
    public class SSTFixture
    {
        public static ISST GetSSTContainingAllPossibleNodeTypes()
        {
            return new SST
            {
                EnclosingType = Names.Type("T,P"),
                Delegates =
                {
                    new DelegateDeclaration
                    {
                        Name = Names.Type("d:[R,P] [T2,P].()").AsDelegateTypeName
                    }
                },
                Events =
                {
                    new EventDeclaration
                    {
                        Name = Names.Event("[T2,P] [T3,P].E")
                    }
                },
                Fields =
                {
                    new FieldDeclaration
                    {
                        Name = Names.Field("[T4,P] [T5,P].F")
                    }
                },
                Methods =
                {
                    new MethodDeclaration
                    {
                        Name = Names.Method("[T6,P] [T7,P].M1()"),
                        IsEntryPoint = false,
                        Body = CreateCurrentBody()
                    },
                    new MethodDeclaration
                    {
                        Name = Names.Method("[T8,P] [T9,P].M2()"),
                        IsEntryPoint = true
                    }
                },
                Properties =
                {
                    new PropertyDeclaration
                    {
                        Name = Names.Property("get [T10,P] [T11,P].P()"),
                        Get =
                        {
                            new ReturnStatement()
                        },
                        Set =
                        {
                            new Assignment()
                        }
                    }
                }
            };
        }

        public static IKaVEList<IStatement> CreateCurrentBody()
        {
            var anyVarRef = new VariableReference();
            var anyStmt = new BreakStatement();
            var anyExpr = new ConstantValueExpression();
            var anyBody = Lists.NewList<IStatement>(new BreakStatement());

            return Lists.NewList(
                //
                new DoLoop
                {
                    Condition = anyExpr,
                    Body = anyBody
                },
                new ForEachLoop
                {
                    Declaration = new VariableDeclaration
                    {
                        Reference = anyVarRef,
                        Type = Names.Type("T1,P")
                    },
                    LoopedReference = anyVarRef,
                    Body = anyBody
                },
                new ForLoop
                {
                    Init = anyBody,
                    Condition = anyExpr,
                    Step = anyBody,
                    Body = anyBody
                },
                new IfElseBlock
                {
                    Condition = anyExpr,
                    Then = anyBody,
                    Else = anyBody
                },
                new LockBlock
                {
                    Reference = anyVarRef,
                    Body = anyBody
                },
                new SwitchBlock
                {
                    Reference = anyVarRef,
                    Sections =
                    {
                        new CaseBlock
                        {
                            Label = anyExpr,
                            Body = anyBody
                        }
                    },
                    DefaultSection = anyBody
                },
                new TryBlock
                {
                    Body = anyBody,
                    CatchBlocks =
                    {
                        new CatchBlock
                        {
                            Parameter = Names.Parameter("[?] p"),
                            Kind = CatchBlockKind.General,
                            Body = anyBody
                        }
                    },
                    Finally = anyBody
                },
                new UncheckedBlock {Body = anyBody},
                new UnsafeBlock(),
                new UsingBlock {Reference = anyVarRef, Body = anyBody},
                new WhileLoop {Condition = anyExpr, Body = anyBody},
                //
                new Assignment
                {
                    Reference = anyVarRef,
                    Expression = anyExpr
                },
                new BreakStatement(),
                new ContinueStatement(),
                new EventSubscriptionStatement
                {
                    Reference = anyVarRef,
                    Operation = EventSubscriptionOperation.Add,
                    Expression = anyExpr
                },
                new ExpressionStatement {Expression = anyExpr},
                new GotoStatement {Label = "l"},
                new LabelledStatement {Label = "l", Statement = anyStmt},
                new ReturnStatement {Expression = anyExpr, IsVoid = true},
                new ThrowStatement {Reference = anyVarRef},
                new UnknownStatement(),
                new VariableDeclaration {Type = Names.Type("T2, P"), Reference = anyVarRef},
                //
                Nested(
                    new BinaryExpression
                    {
                        LeftOperand = anyExpr,
                        Operator = BinaryOperator.BitwiseAnd,
                        RightOperand = anyExpr
                    }),
                Nested(
                    new CastExpression
                    {
                        Reference = anyVarRef,
                        Operator = CastOperator.SafeCast,
                        TargetType = Names.Type("T3, P")
                    }),
                Nested(
                    new CompletionExpression
                    {
                        Token = "t",
                        TypeReference = Names.Type("T4, P"),
                        VariableReference = anyVarRef
                    }),
                Nested(new ComposedExpression {References = {anyVarRef}}),
                Nested(new IfElseExpression {Condition = anyExpr, ThenExpression = anyExpr, ElseExpression = anyExpr}),
                Nested(new IndexAccessExpression {Reference = anyVarRef, Indices = {anyExpr}}),
                Nested(
                    new InvocationExpression
                    {
                        Reference = anyVarRef,
                        MethodName = Names.Method("[?] [?].M()"),
                        Parameters = {anyExpr}
                    }),
                Nested(new LambdaExpression {Name = Names.Lambda("[?] ()"), Body = anyBody}),
                Nested(new TypeCheckExpression {Type = Names.Type("T4, P"), Reference = anyVarRef}),
                Nested(new UnaryExpression {Operator = UnaryOperator.Minus, Operand = anyExpr}),
                Nested(new LoopHeaderBlockExpression {Body = anyBody}),
                Nested(new ConstantValueExpression {Value = "v"}),
                Nested(new NullExpression()),
                Nested(new ReferenceExpression {Reference = anyVarRef}),
                Nested(new UnknownExpression()),
                //
                Nested(new EventReference {Reference = anyVarRef, EventName = Names.Event("[?] [?].e")}),
                Nested(new FieldReference {Reference = anyVarRef, FieldName = Names.Field("[?] [?]._f")}),
                Nested(
                    new IndexAccessReference
                    {
                        Expression = new IndexAccessExpression {Reference = anyVarRef, Indices = {anyExpr}}
                    }),
                Nested(new MethodReference {Reference = anyVarRef, MethodName = Names.Method("[?] [?].M()")}),
                Nested(new PropertyReference {Reference = anyVarRef, PropertyName = Names.Property("get [?] [?].P()")}),
                Nested(new UnknownReference()),
                Nested(new VariableReference {Identifier = "id"})
                //
            );
        }

        public static IStatement Nested(ILoopHeaderBlockExpression expr)
        {
            return new WhileLoop {Condition = expr};
        }

        public static IStatement Nested(IAssignableExpression expr)
        {
            return new Assignment {Expression = expr};
        }

        public static IStatement Nested(IReference reference)
        {
            return new Assignment {Expression = new ReferenceExpression {Reference = reference}};
        }
    }
}