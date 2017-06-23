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
using KaVE.Commons.Model;
using KaVE.Commons.Utils.Assertion;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model
{
    public class KaVEVersionTest
    {
        [Test]
        public void Init_WithValues()
        {
            var sut = new KaVEVersion(123, Variant.Development);
            Assert.AreEqual(new Version(0, 123), sut.Version);
            Assert.AreEqual(Variant.Development, sut.Variant);
            Assert.AreEqual(123, sut.ReleaseNumber);
        }

        [Test]
        public void Init_WithString()
        {
            var sut = new KaVEVersion("0.123-Development");
            Assert.AreEqual(new Version(0, 123), sut.Version);
            Assert.AreEqual(Variant.Development, sut.Variant);
            Assert.AreEqual(123, sut.ReleaseNumber);
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new KaVEVersion(123, Variant.Development);
            var b = new KaVEVersion(123, Variant.Development);
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentVersionNumber()
        {
            var a = new KaVEVersion(123, Variant.Development);
            var b = new KaVEVersion(234, Variant.Development);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentVariant()
        {
            var a = new KaVEVersion(123, Variant.Development);
            var b = new KaVEVersion(123, Variant.Default);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void ToStringIsImplemented()
        {
            var sut = new KaVEVersion(1234, Variant.Development);
            Assert.AreEqual("0.1234-Development", sut.ToString());
        }

        [Test]
        [ExpectedException(typeof(AssertException))]
        public void Parsing_ParameterValidation_null()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new KaVEVersion(null);
        }

        [Test]
        [ExpectedException(typeof(AssertException))]
        public void Parsing_ParameterValidation_empty()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new KaVEVersion("");
        }

        [Test]
        [ExpectedException(typeof(AssertException))]
        public void Parsing_ParameterValidation_invalid()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new KaVEVersion("abc");
        }

        [Test]
        public void Parsing_Roundtrip()
        {
            var actual = new KaVEVersion("0.123-Development");
            var expected = new KaVEVersion(123, Variant.Development);
            Assert.AreEqual(expected, actual);
        }
    }
}