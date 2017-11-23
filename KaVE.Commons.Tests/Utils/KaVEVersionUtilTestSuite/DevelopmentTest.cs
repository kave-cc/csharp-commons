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
using KaVE.Commons.Model;
using KaVE.Commons.Utils;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.KaVEVersionUtilTestSuite
{
    internal class DevelopmentTest
    {
        private KaVEVersionUtil _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new KaVEVersionUtil();
        }

        [Test]
        public void GetInformalVersion()
        {
            var actual = _sut.GetAssemblyInformalVersionByContainedType<KaVEVersionUtil>();
            const string expected = "0.0.0-Development";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetVariant()
        {
            var actual = _sut.GetAssemblyVariantByContainedType<KaVEVersionUtil>();
            Assert.AreEqual(Variant.Development, actual);
        }

        [Test]
        public void GetVersion()
        {
            var actual = _sut.GetAssemblyVersionByContainedType<KaVEVersionUtil>();
            var expected = new Version(0, 0, 0, 0);
            Assert.AreEqual(expected, actual);
        }
    }
}