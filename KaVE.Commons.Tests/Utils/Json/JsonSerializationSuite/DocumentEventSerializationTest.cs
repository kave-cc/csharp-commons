﻿/*
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

using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Model.Naming;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.JsonSerializationSuite
{
    internal class DocumentEventSerializationTest
    {
        [Test]
        public void ShouldSerializeToString()
        {
            var e = new DocumentEvent
            {
                Action = DocumentAction.Saved,
                Document = Names.Document("x SomeProcessedDocument")
            };
            const string expected =
                "{\"$type\":\"KaVE.Commons.Model.Events.VisualStudio.DocumentEvent, KaVE.Commons\",\"Document\":\"0Doc:x SomeProcessedDocument\",\"Action\":1,\"TriggeredBy\":0}";

            JsonAssert.SerializesTo(e, expected);
            JsonAssert.DeserializesTo(expected, e);
        }
    }
}