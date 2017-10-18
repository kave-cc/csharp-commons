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

using System.Diagnostics.CodeAnalysis;
using EnvDTE;
using KaVE.JetBrains.Annotations;
using Moq;

namespace KaVE.VS.Commons.TestUtils.Mocking
{
    public static class MockDTE
    {
        public static Solution CreateSolution([NotNull] string fullName)
        {
            var mockSolution = new Mock<Solution>();
            mockSolution.Setup(solution => solution.FullName).Returns(fullName);
            mockSolution.Setup(solution => solution.DTE).Returns(Create(mockSolution.Object));
            return mockSolution.Object;
        }

        public static DTE Create(Solution solution = null)
        {
            var mockDTE = new Mock<DTE>();
            mockDTE.Setup(dte => dte.Solution).Returns(solution);
            return mockDTE.Object;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void DoNotUse_RequiredForProperInitializationOfDTEMocks()
        {
            DTE dte = null;
            var s = dte.Solution;
            dte.Events.SolutionEvents.Opened += () => { };
            dte.Events.SolutionEvents.ProjectRenamed += (project, name) => { };
            dte.Events.SolutionEvents.Renamed += name => { };
            dte.Events.SolutionEvents.BeforeClosing += () => { };
            dte.Events.SolutionItemsEvents.ItemRenamed += (item, name) => { };
            dte.Events.SolutionItemsEvents.ItemRemoved += item => { };
            dte.Events.SelectionEvents.OnChange += () => { };

            Document d = null;
            var l = d.Language;
        }
    }
}