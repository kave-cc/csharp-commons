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
using KaVE.Commons.Utils;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils
{
    internal class DateUtilsTest
    {
        [Test]
        public void Now_Offset()
        {
            var now = new DateUtils().Now;
            var offset = now.Offset;
            Assert.AreEqual(DateTimeOffset.Now.Offset, offset);
        }

        [Test]
        public void Today_Offset()
        {
            var now = new DateUtils().Today;
            var offset = now.Offset;
            Assert.AreEqual(DateTimeOffset.Now.Offset, offset);
        }
    }
}