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

using KaVE.Commons.Model;
using KaVE.Commons.Utils;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.KaVEVersionUtilTestSuite
{
    internal class ExperimentalTest
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
            Assert.That(actual.StartsWith("0.0."));
            Assert.That(actual.EndsWith("-Experimental"));
        }

        [Test]
        public void GetVariant()
        {
            var actual = _sut.GetAssemblyVariantByContainedType<KaVEVersionUtil>();
            Assert.AreEqual(Variant.Experimental, actual);
        }

        [Test]
        public void GetVersion()
        {
            var actual = _sut.GetAssemblyVersionByContainedType<KaVEVersionUtil>();
            Assert.AreEqual(0, actual.Major);
            Assert.AreEqual(0, actual.Minor);
            Assert.AreNotEqual(0, actual.Revision);
        }
    }
}