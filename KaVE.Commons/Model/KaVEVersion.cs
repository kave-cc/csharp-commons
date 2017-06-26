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
using System.Text.RegularExpressions;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Assertion;

namespace KaVE.Commons.Model
{
    // ReSharper disable once InconsistentNaming
    public interface IKaVEVersion
    {
        int ReleaseNumber { get; }
        Variant Variant { get; }
        Version Version { get; }
    }

    public enum Variant
    {
        Unknown,
        Development, // in-IDE
        Experimental, // build-server on feature branch
        Release, // officially released version
        // legacy (to be removed):
        Default,
        Datev
    }

    public class KaVEVersion : IKaVEVersion
    {
        private static readonly Regex VersionExpr = new Regex("^0\\.(\\d+)-(\\w+)$");

        public KaVEVersion(int releaseNumber, Variant variant)
        {
            ReleaseNumber = releaseNumber;
            Variant = variant;
        }

        public KaVEVersion(string versionStr)
        {
            Asserts.Not(string.IsNullOrEmpty(versionStr));

            var res = VersionExpr.Match(versionStr);
            Asserts.That(res.Success);

            ReleaseNumber = int.Parse(res.Groups[1].ToString());
            Variant = (Variant) Enum.Parse(typeof(Variant), res.Groups[2].ToString());
        }

        public int ReleaseNumber { get; private set; }
        public Variant Variant { get; private set; }
        public Version Version
        {
            get { return new Version(0, ReleaseNumber); }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        protected bool Equals(KaVEVersion other)
        {
            return Variant == other.Variant && ReleaseNumber == other.ReleaseNumber;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = hashCode ^ ((int) Variant * 397);
                return hashCode ^ ReleaseNumber;
            }
        }

        public override string ToString()
        {
            return "0.{0}-{1}".FormatEx(ReleaseNumber, Variant);
        }
    }
}