/*
 * Copyright 2017 University of Zurich
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
using KaVE.Commons.Model.Events.VersionControlEvents;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Utils.Json;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.JsonSerializationSuite
{
    internal class VersionControlEventSerializationTest
    {
        [Test]
        public void Roundtrip_Compact()
        {
            var obj = GetObj_Current();
            var json = obj.ToCompactJson();
            var obj2 = json.ParseJsonTo<VersionControlEvent>();
            Assert.AreEqual(obj, obj2);
        }

        [Test]
        public void Roundtrip_Formatted()
        {
            var obj = GetObj_Current();
            var json = obj.ToFormattedJson();
            var obj2 = json.ParseJsonTo<VersionControlEvent>();
            Assert.AreEqual(obj, obj2);
        }

        [Test]
        public void Deserialization()
        {
            var obj = GetObj_Current();
            var json = GetJson_Current();
            var obj2 = json.ParseJsonTo<VersionControlEvent>();
            Assert.AreEqual(obj, obj2);
        }

        [Test]
        public void Serialization()
        {
            var json = GetJson_Current();
            var obj = GetObj_Current();
            var json2 = obj.ToCompactJson();
            Assert.AreEqual(json, json2);
        }

        public VersionControlEvent GetObj_Current()
        {
            return new VersionControlEvent
            {
                Solution = Names.Solution("Some Solution"),
                Actions =
                {
                    new VersionControlAction
                    {
                        ExecutedAt = GetDate(1),
                        ActionType = VersionControlActionType.Clone
                    }
                }
            };
        }

        public string GetJson_Current()
        {
            return "{\"$type\":\"KaVE.Commons.Model.Events.VersionControlEvents.VersionControlEvent, KaVE.Commons\"," +
                   "\"Actions\":[" +
                   /**/ "{\"$type\":\"KaVE.Commons.Model.Events.VersionControlEvents.VersionControlAction, KaVE.Commons\"," +
                   /**/ "\"ExecutedAt\":\"0001-01-01T00:00:01\"," +
                   /**/ "\"ActionType\":3}" +
                   "]," +
                   "\"Solution\":\"0Sln:Some Solution\"," +
                   "\"TriggeredBy\":0" +
                   "}";
        }

        private static DateTime GetDate(int secs)
        {
            return DateTime.MinValue.AddSeconds(secs);
        }
    }
}