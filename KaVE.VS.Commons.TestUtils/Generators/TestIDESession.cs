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

using EnvDTE;
using KaVE.VS.Commons.Generators;
using Moq;

namespace KaVE.VS.Commons.TestUtils.Generators
{
    public class TestIDESession : IIDESession
    {
        public TestIDESession()
        {
            DTE = Mock.Of<DTE>();
            Mock.Get(DTE).Setup(dte => dte.ActiveWindow).Returns((Window) null);
            Mock.Get(DTE).Setup(dte => dte.ActiveDocument).Returns((Document) null);
        }

        public DTE DTE { get; }
        public Mock<DTE> MockDTE => Mock.Get(DTE);

        public string UUID => "TestIDESessionUUID";
    }
}