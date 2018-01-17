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
using KaVE.Commons.Model.Naming.Impl.v0.Types;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Assertion;
using NUnit.Framework;

namespace KaVE.Commons.TestGeneration.Java
{
    public class JavaTypeNamingTestGenerator
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public static string[] BasicTypes()
        {
            return new[]
            {
                // unknown
                "?",
                // regular
                "T",
                "T -> T,P",
                "T,P",
                "T[],P",
                "d:[?] [n.C+D, P].()",
                "T`1[[P -> T2,P]],P",
                // arrays
                "T[]",
                "T[] -> T,P",
                "T[],P",
                "d:[?] [?].()[]",
                // nested
                "n.C+D`1[[T]], P",
                "n.C`1[[T]]+D, P",
                // predefined
                "p:int"
            };
        }

        public static IEnumerable<string> NonArrayTypeSource()
        {
            var types = new HashSet<string>
            {
                "?",
                "p:int",
                "T",
                "T -> ?",
                "n.T`1[[G]],P",
                "n.T`1[[G -> p:byte]],P",
                "T`1[[T -> d:[TR] [T2, P2].([T] arg)]], P",
                "n.C1+C2,P",
                "C1`1[[T1]],P",
                "C1+C2`1[[T2]],P",
                "C1`1[[T2]]+C2,P",
                "C1`1[[T1]]+C2`1[[T2]],P",
                "T -> T[],P",
                "T1`1[][[T2 -> T3[],P]]+T4`1[][[T5 -> T6[],P]]+T7`1[[T8 -> T9[],P]], P",
                "d:[?] [?].()",
                "d:[T,P] [T,P].()",
                "d:[R, P] [O+D, P].()",
                "d:[T`1[[T -> n.C+D, P]], P] [n.C+D, P].()",
                "d:[?] [n.C+D, P].([T`1[[T -> n.C+D, P]], P] p)",
                "d:[RT[], A] [DT, A].([PT[], A] p)",
                "i:n.T1`1[[T2 -> p:int]], P",
                "n.T,P",
                "n.T, A, 1.2.3.4",
                "s:n.T,P",
                "e:n.T,P",
                "i:n.T,P",
                "n.T1+T2, P",
                "n.T1`1[[T2]]+T3`1[[T4]], P",
                "n.C+N`1[[T]],P",
                "n.C`1[[T]]+N,P",
                "n.C`1[[T]]+N`1[[T]],P",
                "s:System.Nullable`1[[T -> p:sbyte]], mscorlib, 4.0.0.0",
                "System.Nullable`1[[System.Int32, mscorlib, 4.0.0.0]], mscorlib, 4.0.0.0",
                "Task`1[[TResult -> i:IList`1[[T -> T]], mscorlib, 4.0.0.0]], mscorlib, 4.0.0.0"
            };
            foreach (var typeId in BasicTypes())
            {
                types.Add("d:[{0}] [{0}].()".FormatEx(typeId));
                types.Add("d:[{0}] [{0}].([{0}] p1)".FormatEx(typeId));
                types.Add("d:[{0}] [{0}].([{0}] p1, [{0}] p2)".FormatEx(typeId));
            }
            foreach (var tp in BasicTypes())
            {
                if (!TypeParameterName.IsTypeParameterNameIdentifier(tp))
                {
                    types.Add("T -> {0}".FormatEx(tp));
                }
            }
            return types;
        }

        public static IEnumerable<string> TypesSource()
        {
            ISet<string> typeIds = new HashSet<string>();
            foreach (var baseTypeId in NonArrayTypeSource())
            {
                var baseType = TypeUtils.CreateTypeName(baseTypeId);
                Asserts.Not(baseType.IsArray);

                typeIds.Add(baseTypeId);
                typeIds.Add(ArrayTypeName.From(baseType, 1).Identifier);
                typeIds.Add(ArrayTypeName.From(baseType, 2).Identifier);
            }
            return typeIds;
        }

        [Test, Ignore("this \"test\" is only executed manually")]
        public void Run()
        {
            Console.WriteLine(
                "put result into '<cc.kave.commons>:/cc.kave.commons.generated/GeneratedTypeNameTest.java'");

            _sb.OpenClass("cc.kave.commons.generated", "GeneratedTypeNameTest");

            GenerateDefaultValueTests();

            var arrCounter = 0;
            foreach (var typeId in NonArrayTypeSource())
            {
                GenerateDeriveArrayTest(arrCounter++, typeId);
            }

            var counter = 0;
            foreach (var typeId in TypesSource())
            {
                GenerateTypeTest(counter++, typeId);
            }

            _sb.CloseClass();
            Console.WriteLine(_sb);
        }

        private void GenerateDeriveArrayTest(int counter, string baseTypeId)
        {
            Asserts.Not(ArrayTypeName.IsArrayTypeNameIdentifier(baseTypeId));
            var type = TypeUtils.CreateTypeName(baseTypeId);
            var arr1 = ArrayTypeName.From(type, 1);
            var arr2 = ArrayTypeName.From(type, 2);

            _sb.OpenTest("DeriveArrayTest_{0}".FormatEx(counter));

            _sb.AppendLine("String baseId = \"{0}\";".FormatEx(baseTypeId));
            _sb.AppendLine("String arr1Id = \"{0}\";".FormatEx(arr1.Identifier));
            _sb.AppendLine("String arr2Id = \"{0}\";".FormatEx(arr2.Identifier));
            _sb.AppendLine("ITypeName base = TypeUtils.createTypeName(baseId);");
            _sb.AppendLine("ITypeName arr1 = ArrayTypeName.from(base, 1);");
            _sb.AppendLine("assertTrue(arr1 instanceof {0});".FormatEx(arr1.GetType().Name));
            _sb.AppendLine("assertEquals(arr1Id, arr1.getIdentifier());");
            _sb.AppendLine("ITypeName arr2 = ArrayTypeName.from(base, 2);");
            _sb.AppendLine("assertTrue(arr2 instanceof {0});".FormatEx(arr2.GetType().Name));
            _sb.AppendLine("assertEquals(arr2Id, arr2.getIdentifier());");
            _sb.CloseTest();
        }

        private void GenerateTypeTest(int counter, string typeId)
        {
            _sb.OpenTest("TypeTest_{0}".FormatEx(counter));
            AppendAssertsForTypeName(TypeUtils.CreateTypeName(typeId));
            _sb.CloseTest();
        }

        private void GenerateDefaultValueTests()
        {
            _sb.Separator("default value tests");

            _sb.OpenTest("DefaultValues_TypeName");
            AppendAssertsForTypeName(new TypeName());
            _sb.CloseTest();

            _sb.OpenTest("DefaultValues_DelegateTypeName");
            AppendAssertsForTypeName(new DelegateTypeName());
            _sb.CloseTest();

            // the other type names do not allow a default
        }

        private void AppendAssertsForTypeName(ITypeName t)
        {
            _sb.AppendLine("String id = \"{0}\";".FormatEx(t.Identifier));

            _sb.Append("assertEquals(")
               .AppendBool(TypeUtils.IsUnknownTypeIdentifier(t.Identifier))
               .AppendLine(", TypeUtils.isUnknownTypeIdentifier(id));");
            _sb.Append("assertEquals(")
               .AppendBool(TypeName.IsTypeNameIdentifier(t.Identifier))
               .AppendLine(", TypeName.isTypeNameIdentifier(id));");
            _sb.Append("assertEquals(")
               .AppendBool(ArrayTypeName.IsArrayTypeNameIdentifier(t.Identifier))
               .AppendLine(", ArrayTypeName.isArrayTypeNameIdentifier(id));");
            _sb.Append("assertEquals(")
               .AppendBool(TypeParameterName.IsTypeParameterNameIdentifier(t.Identifier))
               .AppendLine(", TypeParameterName.isTypeParameterNameIdentifier(id));");
            _sb.Append("assertEquals(")
               .AppendBool(DelegateTypeName.IsDelegateTypeNameIdentifier(t.Identifier))
               .AppendLine(", DelegateTypeName.isDelegateTypeNameIdentifier(id));");
            _sb.Append("assertEquals(")
               .AppendBool(PredefinedTypeName.IsPredefinedTypeNameIdentifier(t.Identifier))
               .AppendLine(", PredefinedTypeName.isPredefinedTypeNameIdentifier(id));");


            _sb.AppendLine("ITypeName sut = TypeUtils.createTypeName(id);");
            _sb.AppendLine("assertTrue(sut instanceof {0});".FormatEx(t.GetType().Name));

            _sb.AppendAreEqual(t.IsHashed, "sut.isHashed()");
            _sb.AppendAreEqual(t.IsUnknown, "sut.isUnknown()");

            _sb.AppendAreEqual(t.Namespace, "sut.getNamespace()");
            _sb.AppendAreEqual(t.Assembly, "sut.getAssembly()");
            _sb.AppendAreEqual(t.FullName, "sut.getFullName()");
            _sb.AppendAreEqual(t.Name, "sut.getName()");

            _sb.AppendAreEqual(t.IsClassType, "sut.isClassType()");
            _sb.AppendAreEqual(t.IsEnumType, "sut.isEnumType()");
            _sb.AppendAreEqual(t.IsInterfaceType, "sut.isInterfaceType()");
            _sb.AppendAreEqual(t.IsNullableType, "sut.isNullableType()");
            _sb.AppendAreEqual(t.IsPredefined, "sut.isPredefined()");
            _sb.AppendAreEqual(t.IsReferenceType, "sut.isReferenceType()");
            _sb.AppendAreEqual(t.IsSimpleType, "sut.isSimpleType()");
            _sb.AppendAreEqual(t.IsStructType, "sut.isStructType()");
            _sb.AppendAreEqual(t.IsTypeParameter, "sut.isTypeParameter()");
            _sb.AppendAreEqual(t.IsValueType, "sut.isValueType()");
            _sb.AppendAreEqual(t.IsVoidType, "sut.isVoidType()");

            _sb.AppendAreEqual(t.IsNestedType, "sut.isNestedType()");
            _sb.AppendAreEqual(t.DeclaringType, "sut.getDeclaringType()");

            _sb.AppendAreEqual(t.HasTypeParameters, "sut.hasTypeParameters()");
            _sb.AppendAreEqual(t.TypeParameters, "sut.getTypeParameters()");

            // used for several checks;
            _sb.AppendLine("boolean hasThrown;");

            // array
            _sb.Comment("array");
            _sb.AppendAreEqual(t.IsArray, "sut.isArray()");
            if (t.IsArray)
            {
                var tArr = t.AsArrayTypeName;
                _sb.AppendLine("IArrayTypeName sutArr = sut.asArrayTypeName();");
                _sb.AppendAreEqual(tArr.Rank, "sutArr.getRank()");
                _sb.AppendAreEqual(tArr.ArrayBaseType, "sutArr.getArrayBaseType()");
            }
            else
            {
                _sb.AppendThrowValidation("sut.asArrayTypeName();", "AssertionException");
            }

            // delegates
            _sb.Comment("delegates");
            _sb.AppendAreEqual(t.IsDelegateType, "sut.isDelegateType()");
            if (t.IsDelegateType)
            {
                var tD = t.AsDelegateTypeName;
                _sb.AppendLine("IDelegateTypeName sutD = sut.asDelegateTypeName();");
                _sb.AppendAreEqual(tD.DelegateType, "sutD.getDelegateType()");
                _sb.AppendAreEqual(tD.HasParameters, "sutD.hasParameters()");
                _sb.AppendAreEqual(tD.IsRecursive, "sutD.isRecursive()");
                _sb.AppendAreEqual(tD.Parameters, "sutD.getParameters()");
                _sb.AppendAreEqual(tD.ReturnType, "sutD.getReturnType()");
            }
            else
            {
                _sb.AppendThrowValidation("sut.asDelegateTypeName();", "AssertionException");
            }

            // predefined
            _sb.Comment("predefined");
            _sb.AppendAreEqual(t.IsPredefined, "sut.isPredefined()");
            if (t.IsPredefined)
            {
                var sutP = t.AsPredefinedTypeName;
                _sb.AppendLine("IPredefinedTypeName sutP = sut.asPredefinedTypeName();");
                _sb.AppendAreEqual(sutP.FullType, "sutP.getFullType()");
            }
            else
            {
                _sb.AppendThrowValidation("sut.asPredefinedTypeName();", "AssertionException");
            }

            // type parameters
            _sb.Comment("type parameters");
            _sb.AppendAreEqual(t.IsTypeParameter, "sut.isTypeParameter()");
            if (t.IsTypeParameter)
            {
                var sutT = t.AsTypeParameterName;
                _sb.AppendLine("ITypeParameterName sutT = sut.asTypeParameterName();");
                _sb.AppendAreEqual(sutT.IsBound, "sutT.isBound()");
                _sb.AppendAreEqual(sutT.TypeParameterShortName, "sutT.getTypeParameterShortName()");
                _sb.AppendAreEqual(sutT.TypeParameterType, "sutT.getTypeParameterType()");
            }
            else
            {
                _sb.AppendThrowValidation("sut.asTypeParameterName();", "AssertionException");
            }
        }
    }
}