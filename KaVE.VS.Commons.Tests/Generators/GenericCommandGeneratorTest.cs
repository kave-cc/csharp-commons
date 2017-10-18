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

using JetBrains.Threading;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Utils;
using KaVE.VS.Commons.Generators;
using KaVE.VS.Commons.TestUtils.Generators;
using NUnit.Framework;

namespace KaVE.VS.Commons.Tests.Generators
{
    internal class GenericCommandGeneratorTest : EventGeneratorTestBase
    {
        private GenericCommandGenerator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new GenericCommandGeneratorImpl(TestRSEnv, TestMessageBus, TestDateUtils, TestThreading);
        }

        [Test]
        public void DefaultCase()
        {
            _sut.Fire("x");

            var actual = GetSinglePublished<CommandEvent>();
            var expected = new CommandEvent
            {
                CommandId = "x",
                IDESessionUUID = TestIDESession.UUID,
                KaVEVersion = TestRSEnv.KaVEVersion,
                TriggeredAt = TestDateUtils.Now,
                TriggeredBy = EventTrigger.Unknown
            };
            Assert.AreEqual(expected, actual);
        }
    }

    public class GenericCommandGeneratorImpl : GenericCommandGenerator
    {
        public GenericCommandGeneratorImpl(IRSEnv env,
            IMessageBus messageBus,
            IDateUtils dateUtils,
            IThreading threading) : base(env, messageBus, dateUtils, threading) { }
    }
}