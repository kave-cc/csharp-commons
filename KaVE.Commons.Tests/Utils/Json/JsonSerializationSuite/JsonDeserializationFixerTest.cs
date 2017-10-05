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

using KaVE.Commons.Utils.Json;
using KaVE.Commons.Utils.Json.Fixers;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.JsonSerializationSuite
{
    internal class JsonDeserializationFixerTest
    {
        private readonly TestFixer _testFixer = new TestFixer();

        [SetUp]
        public void SetUp()
        {
            JsonSerialization.Fixers.Add(_testFixer);
        }

        [TearDown]
        public void TearDown()
        {
            JsonSerialization.Fixers.Remove(_testFixer);
        }

        [Test]
        public void TestEventFixerIsRegistered()
        {
            foreach (var f in JsonSerialization.Fixers)
            {
                if (f is TestEventFixer)
                {
                    return;
                }
            }
            Assert.Fail("TestEventFixer is not registered");
        }

        [Test]
        public void UnfixedEventsAreNotChanged_string()
        {
            var actual = "\"abc\"".ParseJsonTo<string>();
            const string expected = "abc";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UnfixedEventsAreNotChanged_int()
        {
            var actual = "123".ParseJsonTo<int>();
            const int expected = 123;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FixingIsCalled_ParseTo()
        {
            var actual = "1".ParseJsonTo(typeof(int));
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FixingIsCalled_ParseToT()
        {
            var actual = "1".ParseJsonTo<int>();
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }
    }

    internal class TestFixer : IDeserializationFixer
    {
        public object Fix(string json, object obj)
        {
            if (obj is int && "1".Equals(json))
                return 2;
            return obj;
        }
    }
}