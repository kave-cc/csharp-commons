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

using System;
using System.Collections.Generic;
using System.Text;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Impl.v0.CodeElements;
using KaVE.Commons.Utils;
using NUnit.Framework;

namespace KaVE.Commons.TestGeneration.Java
{
    public class JavaMemberNamingTestGenerator
    {
        private readonly StringBuilder _sb = new StringBuilder();

        [Test, Ignore("this \"test\" is only executed manually")]
        public void Run()
        {
            Console.WriteLine(
                "put result into '<cc.kave.commons>:/cc.kave.commons.generated/GeneratedCodeElementTest.java'");

            _sb.OpenClass("cc.kave.commons.generated", "GeneratedCodeElementTest");
            _sb.AppendCustomAssertDefinitions();

            _sb.Comment("defaults");
            // defaults
            GenerateFieldTest(0, 0, new FieldName());
            GenerateEventTest(0, 0, new EventName());
            GenerateMethodTest(0, 0, new MethodName());
            GeneratePropertyTest(0, 0, new PropertyName());

            _sb.Comment("generated names");
            var counter = 1;
            foreach (var typeId in JavaTypeNamingTestGenerator.TypesSource())
            {
                GenerateFieldTests(counter, typeId);
                GenerateEventTests(counter, typeId);
                GenerateMethodTests(counter, typeId);
                GeneratePropertyTests(counter, typeId);
                counter++;
            }


            var mids = new[]
            {
                "[p:void] [T,P]..ctor()",
                "[p:void] [T,P]..cctor()",
                "[p:void] [T,P]..init()",
                "[p:void] [T,P]..cinit()",
                "static [p:void] [T,P].Ext(this [p:int] i)"
            };
            counter++;
            var counter2 = 0;
            foreach (var mid in mids)
            {
                GenerateMethodTest(counter, counter2++, new MethodName(mid));
            }

            var pids = new[]
            {
                "get [p:void] [T,P].P()",
                "get [p:void] [T,P].P([p:int] i)",
                "get [p:void] [T,P].P([p:int] i, [p:int] j)"
            };
            counter++;
            counter2 = 0;
            foreach (var pid in pids)
            {
                GeneratePropertyTest(counter, counter2++, new PropertyName(pid));
            }


            _sb.CloseClass();
            Console.WriteLine(_sb);
        }

        private void GenerateFieldTests(int counter, string typeId)
        {
            var counter2 = 0;
            foreach (var memberBase in GenerateMemberBases(typeId, "_f"))
            {
                GenerateFieldTest(counter, counter2++, new FieldName(memberBase));
            }
        }

        private void GenerateFieldTest(int counter, int counter2, IFieldName sut)
        {
            OpenTestAndDeclareSut(counter, counter2, sut);
            _sb.CloseTest();
        }

        private void GenerateEventTests(int counter, string typeId)
        {
            var counter2 = 0;
            foreach (var memberBase in GenerateMemberBases(typeId, "e"))
            {
                GenerateEventTest(counter, counter2++, new EventName(memberBase));
            }
        }

        private void GenerateEventTest(int counter, int counter2, IEventName sut)
        {
            OpenTestAndDeclareSut(counter, counter2, sut);
            _sb.AppendAreEqual(sut.HandlerType, "sut.getHandlerType()");
            _sb.CloseTest();
        }

        private void GenerateMethodTests(int counter, string typeId)
        {
            var counter2 = 0;
            foreach (var memberBase in GenerateMemberBases(typeId, "M"))
            {
                foreach (var genericPart in new[] {"", "`1[[T]]", "`1[[T -> {0}]]".FormatEx(typeId), "`2[[T],[U]]"})
                {
                    GenerateMethodTest(
                        counter,
                        counter2++,
                        new MethodName("{0}{1}()".FormatEx(memberBase, genericPart)));
                }
                foreach (
                    var paramPart in
                    new[]
                    {
                        "",
                        "out [?] p",
                        "[{0}] p".FormatEx(typeId),
                        "[{0}] p1, [{0}] p2".FormatEx(typeId)
                    })
                {
                    GenerateMethodTest(counter, counter2++, new MethodName("{0}({1})".FormatEx(memberBase, paramPart)));
                }
            }
        }

        private void GenerateMethodTest(int counter, int counter2, IMethodName sut)
        {
            OpenTestAndDeclareSut(counter, counter2, sut);
            _sb.AppendAreEqual(sut.ReturnType, "sut.getReturnType()");
            _sb.AppendAreEqual(sut.IsConstructor, "sut.isConstructor()");
            _sb.AppendAreEqual(sut.IsInit, "sut.isInit()");
            _sb.AppendAreEqual(sut.IsExtensionMethod, "sut.isExtensionMethod()");

            _sb.AppendParameterizedNameAssert(sut);
            _sb.CloseTest();
        }

        private void GeneratePropertyTests(int counter, string typeId)
        {
            var counter2 = 0;
            foreach (var memberBase in GenerateMemberBases(typeId, "P"))
            {
                GeneratePropertyTest(counter, counter2++, new PropertyName("get " + memberBase + "()"));
            }
        }

        private void GeneratePropertyTest(int counter, int counter2, IPropertyName sut)
        {
            OpenTestAndDeclareSut(counter, counter2, sut);
            _sb.AppendAreEqual(sut.HasGetter, "sut.hasGetter()");
            _sb.AppendAreEqual(sut.HasSetter, "sut.hasSetter()");
            _sb.AppendAreEqual(sut.IsIndexer, "sut.isIndexer()");

            _sb.AppendParameterizedNameAssert(sut);
            _sb.CloseTest();
        }

        private static IEnumerable<string> GenerateMemberBases(string typeId, string memberName)
        {
            foreach (var staticPart in new[] {"", "static "})
            {
                yield return "{0}[T,P] [{1}].{2}".FormatEx(staticPart, typeId, memberName);
                yield return "{0}[{1}] [T,P].{2}".FormatEx(staticPart, typeId, memberName);
            }
        }

        private void OpenTestAndDeclareSut(int counter, int counter2, IMemberName sut)
        {
            var simpleName = sut.GetType().Name;
            _sb.OpenTest("{0}Test_{1}_{2}".FormatEx(simpleName, counter, counter2));
            _sb.AppendLine("String id = \"{0}\";".FormatEx(sut.Identifier));
            _sb.AppendLine("I{0} sut = new {0}({1});".FormatEx(simpleName, sut.IsUnknown ? "" : "id"));

            _sb.Append("assertBasicMember(sut,id,");

            _sb.Append(sut.IsUnknown ? "true" : "false").Append(',');
            _sb.Append(sut.IsHashed ? "true" : "false").Append(',');
            _sb.Append(sut.IsStatic ? "true" : "false").Append(',');

            _sb.Append("\"" + sut.DeclaringType.Identifier + "\"").Append(',');
            _sb.Append("\"" + sut.ValueType.Identifier + "\"").Append(',');

            _sb.Append("\"" + sut.FullName + "\"").Append(',');
            _sb.Append("\"" + sut.Name + "\"");
            _sb.AppendLine(");");
        }
    }
}