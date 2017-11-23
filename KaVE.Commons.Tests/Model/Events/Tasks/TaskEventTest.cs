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

using KaVE.Commons.Model.Events.Enums;
using KaVE.Commons.Model.Events.Tasks;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Events.Tasks
{
    internal class TaskEventTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new TaskEvent();
            Assert.AreEqual("", sut.Version);
            Assert.AreEqual("", sut.TaskId);
            Assert.AreEqual(TaskAction.Create, sut.Action);
            Assert.Null(sut.NewParentId);
            Assert.Null(sut.Annoyance);
            Assert.Null(sut.Importance);
            Assert.Null(sut.Urgency);
        }

        [Test]
        public void SettingValues()
        {
            var sut = new TaskEvent
            {
                Version = "1",
                TaskId = "2",
                Action = TaskAction.Activate,
                NewParentId = "x",
                Annoyance = Likert5Point.Negative1,
                Importance = Likert5Point.Neutral,
                Urgency = Likert5Point.Positive1
            };
            Assert.AreEqual("1", sut.Version);
            Assert.AreEqual("2", sut.TaskId);
            Assert.AreEqual(TaskAction.Activate, sut.Action);
            Assert.AreEqual("x", sut.NewParentId);
            Assert.AreEqual(Likert5Point.Negative1, sut.Annoyance);
            Assert.AreEqual(Likert5Point.Neutral, sut.Importance);
            Assert.AreEqual(Likert5Point.Positive1, sut.Urgency);
        }

        [Test]
        public void Equality_Default()
        {
            var a = new TaskEvent();
            var b = new TaskEvent();
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_SettingValues()
        {
            var a = new TaskEvent
            {
                Version = "1",
                TaskId = "2",
                Action = TaskAction.Activate,
                NewParentId = "x",
                Annoyance = Likert5Point.Negative1,
                Importance = Likert5Point.Neutral,
                Urgency = Likert5Point.Positive1
            };
            var b = new TaskEvent
            {
                Version = "1",
                TaskId = "2",
                Action = TaskAction.Activate,
                NewParentId = "x",
                Annoyance = Likert5Point.Negative1,
                Importance = Likert5Point.Neutral,
                Urgency = Likert5Point.Positive1
            };
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentVersion()
        {
            var a = new TaskEvent
            {
                Version = "1"
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentTask()
        {
            var a = new TaskEvent
            {
                TaskId = "2"
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentAction()
        {
            var a = new TaskEvent
            {
                Action = TaskAction.Activate
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentParent()
        {
            var a = new TaskEvent
            {
                NewParentId = "x"
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentDifferentAnnoyance()
        {
            var a = new TaskEvent
            {
                Annoyance = Likert5Point.Negative1
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentImportance()
        {
            var a = new TaskEvent
            {
                Importance = Likert5Point.Neutral
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentUrgency()
        {
            var a = new TaskEvent
            {
                Urgency = Likert5Point.Positive1
            };
            var b = new TaskEvent();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}