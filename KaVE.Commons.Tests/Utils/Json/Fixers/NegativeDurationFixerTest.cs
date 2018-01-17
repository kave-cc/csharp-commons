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
using KaVE.Commons.Model.Events.TestRunEvents;
using KaVE.Commons.Utils.Json;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.Fixers
{
    internal class NegativeDurationFixerTest
    {
        [Test]
        public void Regression_NoTimeZoneButNegativeDuration()
        {
            var json =
                "{\"$type\":\"KaVE.Commons.Model.Events.TestRunEvents.TestRunEvent, KaVE.Commons\"," +
                "\"TriggeredAt\":\"2016-09-19T01:26:58.9461781+00:00\"," +
                "\"Duration\":\"-02:59:56.5470501\"}";

            var obj = json.ParseJsonTo<TestRunEvent>();
            Assert.AreEqual(TimeSpan.Zero, obj.Duration);
        }
    }
}