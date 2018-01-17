/*
 * Copyright 2018 University of Zurich
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

using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.TypeShapes;
using KaVE.Commons.TestUtils.Model.SSTs;

namespace KaVE.Commons.TestUtils.Model.Events
{
    public static class ContextFactory
    {
        public static Context CreateContextThatContainsAllPossibleNodeTypes()
        {
            return new Context
            {
                TypeShape = CreateTypeShape(),
                SST = SSTFixture.GetSSTContainingAllPossibleNodeTypes()
            };
        }

        private static TypeShape CreateTypeShape()
        {
            return new TypeShape
            {
                MethodHierarchies =
                {
                    new MethodHierarchy
                    {
                        Element = Names.Method("[?] [?].M1()"),
                        Super = Names.Method("[?] [?].M2()"),
                        First = Names.Method("[?] [?].M3()")
                    }
                },
                TypeHierarchy = new TypeHierarchy
                {
                    Element = Names.Type("T1, P"),
                    Extends = new TypeHierarchy
                    {
                        Element = Names.Type("T2, P")
                    },
                    Implements =
                    {
                        new TypeHierarchy
                        {
                            Element = Names.Type("T3, P")
                        }
                    }
                },
                Delegates =
                {
                    Names.Type("d:[p:void] [T,P].()").AsDelegateTypeName
                },
                EventHierarchies =
                {
                    new EventHierarchy
                    {
                        Element = Names.Event("[p:int] [T,P].E"),
                        Super = Names.Event("[p:int] [S,P].E"),
                        First = Names.Event("[p:int] [F,P].E")
                    }
                },
                Fields =
                {
                    Names.Field("[p:int] [T,P]._f")
                },
                NestedTypes =
                {
                    Names.Type("A.B.T+N, P")
                },
                PropertyHierarchies =
                {
                    new PropertyHierarchy
                    {
                        Element = Names.Property("get [p:int] [T,P].P()"),
                        Super = Names.Property("get [p:int] [S,P].P()"),
                        First = Names.Property("get [p:int] [F,P].P()")
                    }
                }
            };
        }
    }
}