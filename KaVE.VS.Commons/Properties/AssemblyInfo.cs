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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Application.BuildScript.Application.Zones;

[assembly: AssemblyTitle("KaVE.VS.Commons")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("KaVE.VS.Commons")]
[assembly: AssemblyCopyright("Copyright © KaVE Project 2011-2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: InternalsVisibleTo("KaVE.VS.Commons.Tests")]

[assembly: Guid("754db010-723d-42e1-b590-8701a106ae35")]

// our syntax: 0.0.<build-num>[-<variant>], see KaVE.Utils.VersionUtil
[assembly: AssemblyVersion("0.0.0")]
[assembly: AssemblyInformationalVersion("0.0.0-Development")]

// ReSharper disable once CheckNamespace
namespace KaVE.VS.Commons
{
    [ZoneMarker]
    public class ZoneMarker { }
}