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
using KaVE.Commons.TestUtils.Model.Events;
using KaVE.Commons.Utils.Json;
using NUnit.Framework;

namespace KaVE.Commons.TestGeneration.Java
{
    internal class EventStreamGenerator
    {
        [Test, Ignore("this \"test\" is only executed manually")]
        public void Run()
        {
            var stream = EventStreamFactory.CreateEventStream();
            var json = stream.ToFormattedJson();
            json = "\"" + json.Replace("\r", "").Replace("\"", "\\\"").Replace("\n", "\\n\" + //\n\"") + "\"";
            Console.WriteLine(json);
        }
    }
}