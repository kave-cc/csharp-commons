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
using System.Reflection;
using KaVE.Commons.Model;

namespace KaVE.Commons.Utils
{
    // ReSharper disable once InconsistentNaming
    public interface IKaVEVersionUtil
    {
        Version GetAssemblyVersionByContainedType<T>();
        string GetAssemblyInformalVersionByContainedType<T>();
        Variant GetAssemblyVariantByContainedType<T>();
    }

    public class KaVEVersionUtil : IKaVEVersionUtil
    {
        public virtual Version GetAssemblyVersionByContainedType<T>()
        {
            return typeof(T).Assembly.GetName().Version;
        }

        public virtual string GetAssemblyInformalVersionByContainedType<T>()
        {
            try
            {
                var attributeType = typeof(AssemblyInformationalVersionAttribute);
                var versions = typeof(T).Assembly.GetCustomAttributes(attributeType, true);
                // ReSharper disable once PossibleNullReferenceException
                return (versions[0] as AssemblyInformationalVersionAttribute).InformationalVersion;
            }
            catch
            {
                return "0.0-" + Variant.Unknown;
            }
        }

        public virtual Variant GetAssemblyVariantByContainedType<T>()
        {
            try
            {
                var informalVersion = GetAssemblyInformalVersionByContainedType<T>();
                var variantStr = informalVersion.Substring(informalVersion.LastIndexOf('-') + 1);
                return (Variant)Enum.Parse(typeof(Variant), variantStr);
            }
            catch
            {
                return Variant.Unknown;
            }
        }
    }
}