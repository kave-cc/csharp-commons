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

namespace KaVE.Commons.Utils.Json.Fixers
{
    public class TestEventFixer : IDeserializationFixer
    {
        public object Fix(string json, object obj)
        {
            var e = obj as TestRunEvent;
            if (e == null)
            {
                return obj;
            }

            if (e.Tests.Count == 0)
            {
                return obj;
            }

            var eventStart = e.TriggeredAt;
            var testStart = e.Tests.First().StartTime;

            // make sure timing is set
            if (!eventStart.HasValue || !testStart.HasValue)
            {
                return obj;
            }

            // cheap way to detect absence of problem
            var delta = testStart.Value.Offset - eventStart.Value.Offset;
            if (TimeSpan.Zero == delta)
            {
                return obj;
            }

            var newDateTime = eventStart.Value.DateTime + delta;
            var newOffset = testStart.Value.Offset;

            e.TriggeredAt = new DateTimeOffset(newDateTime, newOffset);
            e.Duration -= delta;
            return e;
        }
    }
}