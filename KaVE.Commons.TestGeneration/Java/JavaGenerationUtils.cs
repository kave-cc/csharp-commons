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
using System.Linq;
using System.Text;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.TestGeneration.Java
{
    public static class JavaGenerationUtils
    {
        public static StringBuilder AppendThrowValidation(this StringBuilder sb, string stmt, string exType)
        {
            sb.AppendLine("hasThrown = false;");
            sb.AppendLine("try { " + stmt + " } catch(" + exType + " e) { hasThrown = true; }");
            return sb.AppendLine("assertTrue(hasThrown);");
        }

        public static StringBuilder Comment(this StringBuilder sb, string comment)
        {
            sb.Append("// ").Append(comment).AppendLine();
            return sb;
        }

        public static StringBuilder AppendBool(this StringBuilder sb, bool condition)
        {
            sb.Append(condition ? "true" : "false");
            return sb;
        }

        public static StringBuilder AppendCustomAssertDefinitions(this StringBuilder sb)
        {
            sb.AppendLine(
                "public static void assertParameterizedName(IParameterizedName n, boolean hasParameters, String... pids){");
            sb.AppendLine("  assertEquals(hasParameters, n.hasParameters());");
            sb.AppendLine("  assertEquals(pids.length, n.getParameters().size());");
            sb.AppendLine("  for(int i = 0; i< pids.length; i++){");
            sb.AppendLine("    assertEquals(new ParameterName(pids[i]), n.getParameters().get(i));");
            sb.AppendLine("  }");
            sb.AppendLine("}");

            sb.AppendLine(
                "public static void assertBasicMember(IMemberName sut, String id, boolean isUnknown, boolean isHashed, " +
                "boolean isStatic, String declTypeId, String valueTypeId, String fullName, String name){");
            sb.AppendLine("  assertEquals(id, sut.getIdentifier());");
            sb.AppendLine("  assertEquals(isUnknown, sut.isUnknown());");
            sb.AppendLine("  assertEquals(isHashed, sut.isHashed());");
            sb.AppendLine("  assertEquals(isStatic, sut.isStatic());");
            sb.AppendLine("  assertEquals(TypeUtils.createTypeName(declTypeId), sut.getDeclaringType());");
            sb.AppendLine("  assertEquals(TypeUtils.createTypeName(valueTypeId), sut.getValueType());");
            sb.AppendLine("  assertEquals(fullName, sut.getFullName());");
            sb.AppendLine("  assertEquals(name, sut.getName());");
            sb.AppendLine("}");
            return sb;
        }

        public static StringBuilder AppendParameterizedNameAssert(this StringBuilder sb, IParameterizedName sut)
        {
            sb.Append("assertParameterizedName(sut, {0}".FormatEx(sut.HasParameters ? "true" : "false"));
            foreach (var p in sut.Parameters)
            {
                sb.Append(", \"").Append(p.Identifier).Append("\"");
            }
            sb.AppendLine(");");
            return sb;
        }

        public static StringBuilder AppendAreEqual(this StringBuilder sb, object expected, string call)
        {
            if (expected == null)
            {
                sb.AppendLine("assertNull({0});".FormatEx(call));
                return sb;
            }

            if (expected is bool)
            {
                sb.AppendLine("assert{0}({1});".FormatEx(expected, call));
                return sb;
            }


            sb.Append("assertEquals(");
            if (expected is string)
            {
                sb.Append('"').Append(expected).Append('"');
            }
            else if (expected is IName)
            {
                var n = (IName) expected;
                sb.Append("new {0}(\"{1}\")".FormatEx(n.GetType().Name, n.Identifier));
            }
            else if (expected is IKaVEList<IParameterName>)
            {
                var ps = (IList<IParameterName>) expected;
                AppendList(sb, ps);
            }
            else if (expected is IKaVEList<ITypeParameterName>)
            {
                var ps = (IList<ITypeParameterName>) expected;
                AppendList(sb, ps);
            }
            else
            {
                sb.Append(expected);
            }
            sb.Append(", ").Append(call).AppendLine(");");
            return sb;
        }

        public static StringBuilder AppendList<TName>(this StringBuilder sb, IList<TName> ps) where TName : IName
        {
            var ids = ps.Select(p => "new {0}(\"{1}\")".FormatEx(p.GetType().Name, p.Identifier));
            sb.Append("Lists.newArrayList({0})".FormatEx(string.Join(",", ids)));
            return sb;
        }

        public static StringBuilder OpenClass(this StringBuilder sb, string package, string className)
        {
            sb.Comment("##############################################################################");
            sb.Comment("Attention: This file was auto-generated, do not modify its contents manually!!");
            sb.Comment("(generated at: {0})".FormatEx(DateTime.Now));
            sb.Comment("##############################################################################");
            sb.AddLicenseHeader();
            sb.AppendLine("package {0};".FormatEx(package));

            var imports = new[]
            {
                "static org.junit.Assert.*",
                "org.junit.Test",
                "org.junit.Ignore",
                "cc.kave.commons.exceptions.*",
                "cc.kave.commons.model.naming.impl.v0.codeelements.*",
                "cc.kave.commons.model.naming.impl.v0.types.*",
                "cc.kave.commons.model.naming.impl.v0.types.organization.*",
                "cc.kave.commons.model.naming.codeelements.*",
                "cc.kave.commons.model.naming.types.*",
                "cc.kave.commons.model.naming.types.organization.*",
                "cc.kave.commons.model.naming.*",
                "com.google.common.collect.*"
            };
            foreach (var import in imports)
            {
                sb.AppendLine("import {0};".FormatEx(import));
            }
            return sb.Append("public class ").Append(className).AppendLine(" {");
        }

        public static StringBuilder Separator(this StringBuilder sb, string comment)
        {
            return sb.AppendLine("/*\n * {0}\n */".FormatEx(comment));
        }

        public static StringBuilder CloseClass(this StringBuilder sb)
        {
            return sb.AppendLine("}");
        }

        public static StringBuilder OpenTest(this StringBuilder sb, string name)
        {
            return sb.AppendLine("@Test").Append("public void ").Append(name).AppendLine("() {");
        }

        public static StringBuilder CloseTest(this StringBuilder sb)
        {
            return sb.AppendLine("}");
        }

        public static StringBuilder AddLicenseHeader(this StringBuilder sb)
        {
            sb.AppendLine("/**");
            sb.AppendLine("* Copyright 2016 Technische Universität Darmstadt");
            sb.AppendLine("*");
            sb.AppendLine("* Licensed under the Apache License, Version 2.0 (the \"License\"); you may not");
            sb.AppendLine("* use this file except in compliance with the License. You may obtain a copy of");
            sb.AppendLine("* the License at");
            sb.AppendLine("* ");
            sb.AppendLine("* http://www.apache.org/licenses/LICENSE-2.0");
            sb.AppendLine("* ");
            sb.AppendLine("* Unless required by applicable law or agreed to in writing, software");
            sb.AppendLine("* distributed under the License is distributed on an \"AS IS\" BASIS, WITHOUT");
            sb.AppendLine("* WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the");
            sb.AppendLine("* License for the specific language governing permissions and limitations under");
            sb.AppendLine("* the License.");
            return sb.AppendLine("*/");
        }
    }
}