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

using System;
using KaVE.Commons.Utils;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils
{
    internal class RandomizationUtilsTest
    {
        private RandomizationUtils _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new RandomizationUtils();
        }

        [Test]
        public void GuidIsNotEmpty()
        {
            var a = _sut.GetRandomGuid();
            Assert.AreNotEqual(Guid.Empty, a);
        }

        [Test]
        public void DifferntGuidsAreCreated()
        {
            var a = _sut.GetRandomGuid();
            var b = _sut.GetRandomGuid();
            Assert.AreNotEqual(a, b);
        }
    }
}