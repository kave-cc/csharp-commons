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
using System.Linq;
using KaVE.Commons.Model.Events.TestRunEvents;
using KaVE.Commons.Utils.Json;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.Fixers
{
    // ReSharper disable InconsistentNaming
    internal class TestEventFixerTest
    {
        private const string Full_BrokenPlusOne =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""WasAborted"": false,
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""TestMethod"": ""0M:[p:void] [T,P].m()"",
                        ""Parameters"": """",
                        ""StartTime"": ""2017-09-26T12:34:05.1876862+01:00"",
                        ""Duration"": ""00:00:00.0004959"",
                        ""Result"": 1
                    }
                ],
                ""IDESessionUUID"": ""abcd"",
                ""KaVEVersion"": ""0.1016-Default"",
                ""TriggeredAt"": ""2017-09-26T11:33:16.2643268Z"",
                ""TriggeredBy"": 0,
                ""Duration"": ""01:00:49.1"",
                ""ActiveWindow"": ""0Win:vsWindowTypeToolWindow some title"",
                ""ActiveDocument"": ""0Doc:CSharp \\\\some\\path.cs""
            }";

        private const string Full_FixedPlusOne =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""WasAborted"": false,
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""TestMethod"": ""0M:[p:void] [T,P].m()"",
                        ""Parameters"": """",
                        ""StartTime"": ""2017-09-26T12:34:05.1876862+01:00"",
                        ""Duration"": ""00:00:00.0004959"",
                        ""Result"": 1
                    }
                ],
                ""IDESessionUUID"": ""abcd"",
                ""KaVEVersion"": ""0.1016-Default"",
                ""TriggeredAt"": ""2017-09-26T12:33:16.2643268+01:00"",
                ""TriggeredBy"": 0,
                ""Duration"": ""00:00:49.1"",
                ""ActiveWindow"": ""0Win:vsWindowTypeToolWindow some title"",
                ""ActiveDocument"": ""0Doc:CSharp \\\\some\\path.cs""
            }";

        private const string BrokenPlusEight =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""StartTime"": ""2017-09-26T12:34:05.1876862+08:00"",
                    }
                ],
                ""TriggeredAt"": ""2017-09-26T04:33:16.2643268Z"",
                ""Duration"": ""08:00:49.0144991""
            }";

        private const string FixedPlusEight =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""StartTime"": ""2017-09-26T12:34:05.1876862+08:00"",
                    }
                ],
                ""TriggeredAt"": ""2017-09-26T12:33:16.2643268+08:00"",
                ""Duration"": ""00:00:49.0144991""
            }";

        private const string BrokenMinusTwo =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""StartTime"": ""2017-09-26T12:34:05.1876862-02:00""
                    }
                ],
                ""TriggeredAt"": ""2017-09-26T14:33:16.2643268Z"",
                ""Duration"": ""-01:59:10.9""
            }";

        private const string FixedMinusTwo =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""StartTime"": ""2017-09-26T12:34:05.1876862-02:00""
                    }
                ],
                ""TriggeredAt"": ""2017-09-26T12:33:16.2643268-02:00"",
                ""Duration"": ""00:00:49.1""
            }";

        private const string BrokenDisagreement =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""StartTime"": ""2017-09-26T16:34:05.1876862+02:00""
                    }
                ],
                ""TriggeredAt"": ""2017-09-26T11:33:16.2643268-03:00"",
                ""Duration"": ""05:00:00.123""
            }";

        private const string FixedDisagreement =
            @"{
                ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons"",
                ""Tests"": [
                    {
                        ""$type"": ""KaVE.Commons.Model.Events.TestRunEvents.TestCaseResult, KaVE.Commons"",
                        ""StartTime"": ""2017-09-26T16:34:05.1876862+02:00""
                    }
                ],
                ""TriggeredAt"": ""2017-09-26T16:33:16.2643268+02:00"",
                ""Duration"": ""00:00:00.123""
            }";

        [Test]
        public void RegressionTestOfFixingTimingError_DoesNotBreakUnbrokenThings()
        {
            var actual = Full_FixedPlusOne.ParseJsonTo<TestRunEvent>();

            var actualTimeOfEvent = actual.TriggeredAt;
            var expectedTimeOfEvent = DateTimeOffset.Parse("2017-09-26T12:33:16.2643268+01:00");
            Assert.AreEqual(expectedTimeOfEvent, actualTimeOfEvent);

            // ReSharper disable once PossibleInvalidOperationException
            var actualTimeOfTest = actual.Tests.First().StartTime.Value;
            var expectedTimeOfTest = DateTimeOffset.Parse("2017-09-26T12:34:05.1876862+01:00");
            Assert.AreEqual(expectedTimeOfTest, actualTimeOfTest);

            var actualDuration = actual.Duration;
            var expectedDuration = TimeSpan.FromSeconds(49).Add(TimeSpan.FromMilliseconds(100));
            Assert.AreEqual(expectedDuration, actualDuration);
        }

        [Test]
        public void RegressionTestOfFixingTimingError_broken_plusone()
        {
            var actual = Full_BrokenPlusOne.ParseJsonTo<TestRunEvent>();
            var expected = Full_FixedPlusOne.ParseJsonTo<TestRunEvent>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegressionTestOfFixingTimingError_broken_plusone_differentParsingFunction()
        {
            var actual = Full_BrokenPlusOne.ParseJsonTo(typeof(TestRunEvent));
            var expected = Full_BrokenPlusOne.ParseJsonTo(typeof(TestRunEvent));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegressionTestOfFixingTimingError_broken_pluseight()
        {
            var actual = BrokenPlusEight.ParseJsonTo<TestRunEvent>();
            var expected = FixedPlusEight.ParseJsonTo<TestRunEvent>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegressionTestOfFixingTimingError_broken_minustwo()
        {
            var actual = BrokenMinusTwo.ParseJsonTo<TestRunEvent>();
            var expected = FixedMinusTwo.ParseJsonTo<TestRunEvent>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegressionTestOfFixingTimingError_broken_disagreement()
        {
            var actual = BrokenDisagreement.ParseJsonTo<TestRunEvent>();
            var expected = FixedDisagreement.ParseJsonTo<TestRunEvent>();
            Assert.AreEqual(expected, actual);
        }
    }
}