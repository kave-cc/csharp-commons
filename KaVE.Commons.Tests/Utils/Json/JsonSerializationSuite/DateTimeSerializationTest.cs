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

using System;
using System.Linq;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.TestRunEvents;
using KaVE.Commons.Utils.Json;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.JsonSerializationSuite
{
    internal class DateTimeSerializationTest
    {
        [Test]
        public void DateTimeSerializationIncludesTimeZoneOffset()
        {
            var now = DateTime.Now;
            var json = now.ToCompactJson();
            Assert.That(json.Contains("+"));

            var obj = json.ParseJsonTo<DateTime>();
            Assert.AreEqual(now, obj);
        }

        private static IIDEEvent CreateEvent(string timeStr)
        {
            var json = "{" +
                       "\"$type\": \"KaVE.Commons.Model.Events.ActivityEvent, KaVE.Commons\"," +
                       "\"IDESessionUUID\": \"0b60aaa2-863e-4fb3-b3a2-3f923e9339b4\"," +
                       "\"KaVEVersion\": \"0.1016-Default\"," +
                       "\"TriggeredAt\": \"" + timeStr + "\"," +
                       "\"TriggeredBy\": 1," +
                       "\"Duration\": \"00:00:06.3353644\"," +
                       "\"ActiveWindow\": \"0Win:vsWindowTypeSolutionExplorer Solution Explorer\"," +
                       "\"ActiveDocument\": \"0Doc:CSharp \\\\KaVE.Commons.Tests\\\\ExternalSerializationTests\\\\SerializationTestSuite.cs\"" +
                       "}";
            return json.ParseJsonTo<IIDEEvent>();
        }

        [Test]
        public void Deserialize_Unmarked_Summertime()
        {
            var actual = CreateEvent("2017-10-05T18:38:42.285").TriggeredAt;
            var expected = new DateTimeOffset(new DateTime(2017, 10, 05, 18, 38, 42, 285, DateTimeKind.Unspecified));
            Assert.AreEqual(expected, actual);

            // this will break in non-German timezones
            Assert.AreEqual(TimeSpan.FromHours(2), expected.Offset);
        }

        [Test]
        public void Deserialize_Unmarked_Wintertime()
        {
            var actual = CreateEvent("2012-02-23T18:54:59.549").TriggeredAt;
            var expected = new DateTimeOffset(new DateTime(2012, 2, 23, 18, 54, 59, 549, DateTimeKind.Unspecified));
            Assert.AreEqual(expected, actual);

            // this will break in non-German timezones
            Assert.AreEqual(TimeSpan.FromHours(1), expected.Offset);
        }

        [Test]
        public void Deserialize_Utc()
        {
            var actual = CreateEvent("2017-10-05T18:38:42.285Z").TriggeredAt;
            var expected = new DateTimeOffset(2017, 10, 05, 18, 38, 42, 285, TimeSpan.Zero);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Deserialize_Offset()
        {
            var actual = CreateEvent("2017-10-05T18:38:42.285-07:00").TriggeredAt;
            var expected = new DateTimeOffset(2017, 10, 05, 18, 38, 42, 285, TimeSpan.FromHours(-7));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegressionTest_ZeroDeserialization()
        {
            var json = @"
            {
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""TriggeredAt"": ""0001-01-01T00:00:00"",
                ""Tests"": [ {
                    ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                    ""StartTime"": ""0001-01-01T00:00:00""
                } ]
            }";

            var obj = json.ParseJsonTo<TestRunEvent>();
            Assert.AreEqual(DateTimeOffset.MinValue, obj.TriggeredAt);
            Assert.AreEqual(DateTimeOffset.MinValue, obj.Tests.First().StartTime);
        }
    }
}