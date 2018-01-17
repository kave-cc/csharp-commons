/*
 * Copyright 2018 University of Zurich
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
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Events.Enums;
using KaVE.Commons.Model.Events.Tasks;
using KaVE.Commons.Model.Events.TestRunEvents;
using KaVE.Commons.Model.Events.UserProfiles;
using KaVE.Commons.Model.Events.VersionControlEvents;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.TestUtils.Model.Events
{
    public class EventStreamFactory
    {
        public static IKaVESet<IIDEEvent> CreateEventStream()
        {
            var events = Sets.NewHashSet<IIDEEvent>();
            // specific
            events.Add(_(CreateCompletionEvent()));
            events.Add(_(CreateTestRunEvent()));
            events.Add(_(CreateUserProfileEvent()));
            events.Add(_(CreateVersionControlEvent()));
            events.Add(_(CreateTaskEvent()));
            // visual studio
            events.Add(_(CreateBuildEvent()));
            events.Add(_(CreateDebuggerEvent()));
            events.Add(_(CreateDocumentEvent()));
            events.Add(_(CreateEditEvent()));
            events.Add(_(CreateFindEvent()));
            events.Add(_(CreateIDEStateEvent()));
            events.Add(_(CreateInstallEvent()));
            events.Add(_(CreateSolutionEvent()));
            events.Add(_(CreateUpdateEvent()));
            events.Add(_(CreateWindowEvent()));
            // generic
            events.Add(_(CreateActivityEvent()));
            events.Add(_(CreateCommandEvent()));
            events.Add(_(CreateErrorEvent()));
            events.Add(_(CreateInfoEvent()));
            events.Add(_(CreateNavigationEvent()));
            events.Add(_(CreateSystemEvent()));
            return events;
        }

        private static IDEEvent CreateTaskEvent()
        {
            return new TaskEvent
            {
                Version = "1.2.3-Experimental",
                TaskId = "1234-5678-...",
                Action = TaskAction.Delete,
                NewParentId = "2345-6789-...",
                Annoyance = Likert5Point.Negative2,
                Importance = Likert5Point.Negative1,
                Urgency = Likert5Point.Positive1
            };
        }

        private static IDEEvent CreateSystemEvent()
        {
            return new SystemEvent
            {
                Type = SystemEventType.RemoteDisconnect
            };
        }

        private static IDEEvent CreateNavigationEvent()
        {
            return new NavigationEvent
            {
                Target = Names.General("t"),
                Location = Names.General("l"),
                TypeOfNavigation = NavigationType.CtrlClick
            };
        }

        private static IDEEvent CreateInfoEvent()
        {
            return new InfoEvent
            {
                Info = "info"
            };
        }

        private static IDEEvent CreateErrorEvent()
        {
            return new ErrorEvent
            {
                Content = "c",
                StackTrace = new[] {"s1", "s2"}
            };
        }

        private static IDEEvent CreateActivityEvent()
        {
            return new ActivityEvent();
        }

        private static IDEEvent CreateWindowEvent()
        {
            return new WindowEvent
            {
                Window = Names.Window("w w"),
                Action = WindowAction.Close
            };
        }

        private static IDEEvent CreateUpdateEvent()
        {
            return new UpdateEvent
            {
                OldPluginVersion = "o",
                NewPluginVersion = "n"
            };
        }

        private static IDEEvent CreateSolutionEvent()
        {
            return new SolutionEvent
            {
                Action = SolutionAction.AddSolutionItem,
                Target = Names.Document("d d")
            };
        }

        private static IDEEvent CreateInstallEvent()
        {
            return new InstallEvent
            {
                PluginVersion = "pv"
            };
        }

        private static IDEEvent CreateIDEStateEvent()
        {
            return new IDEStateEvent
            {
                IDELifecyclePhase = IDELifecyclePhase.Shutdown,
                OpenDocuments =
                {
                    Names.Document("d d")
                },
                OpenWindows = {Names.Window("w w")}
            };
        }

        private static IDEEvent CreateFindEvent()
        {
            return new FindEvent
            {
                Cancelled = true
            };
        }

        private static IDEEvent CreateEditEvent()
        {
            return new EditEvent
            {
                Context2 = new Context
                {
                    SST = new SST
                    {
                        EnclosingType = Names.Type("Edit, P")
                    }
                },
                NumberOfChanges = 1,
                SizeOfChanges = 2
            };
        }

        private static IDEEvent CreateDocumentEvent()
        {
            return new DocumentEvent
            {
                Action = DocumentAction.Opened,
                Document = Names.Document("type path")
            };
        }

        private static IDEEvent CreateDebuggerEvent()
        {
            return new DebuggerEvent
            {
                Action = "a",
                Mode = DebuggerMode.Design,
                Reason = "r"
            };
        }

        private static IDEEvent CreateBuildEvent()
        {
            return new BuildEvent
            {
                Action = "a",
                Scope = "s",
                Targets =
                {
                    new BuildTarget
                    {
                        Project = "p",
                        StartedAt = DateTime.Now,
                        Duration = TimeSpan.FromSeconds(12),
                        Platform = "plt",
                        ProjectConfiguration = "pcfg",
                        SolutionConfiguration = "scfg",
                        Successful = true
                    }
                }
            };
        }

        private static IDEEvent CreateVersionControlEvent()
        {
            return new VersionControlEvent
            {
                Actions =
                {
                    new VersionControlAction
                    {
                        ActionType = VersionControlActionType.Commit,
                        ExecutedAt = DateTimeOffset.Now
                    }
                },
                Solution = Names.Solution("s")
            };
        }

        private static IDEEvent CreateCompletionEvent()
        {
            var now = DateTime.Now;

            return new Commons.Model.Events.CompletionEvents.CompletionEvent
            {
                ProposalCollection =
                {
                    new Proposal
                    {
                        Name = Names.General("y"),
                        Relevance = 2
                    }
                },
                ProposalCount = 3,
                Selections =
                {
                    new ProposalSelection
                    {
                        Proposal =
                        {
                            Name = Names.General("z"),
                            Relevance = 4
                        },
                        SelectedAfter = TimeSpan.FromSeconds(1)
                    },
                    new ProposalSelection
                    {
                        SelectedAfter =
                            now.AddYears(1)
                               .AddMonths(2)
                               .AddDays(3)
                               .AddHours(4)
                               .AddMinutes(5)
                               .AddSeconds(6)
                               .AddMilliseconds(7)
                               .AddTicks(8) - now
                    }
                },
                TerminatedBy = EventTrigger.Shortcut,
                TerminatedState = TerminationState.Cancelled,
                Context2 = new Context
                {
                    SST = new SST
                    {
                        EnclosingType = Names.Type("T,P")
                    }
                }
            };
        }

        private static IDEEvent CreateTestRunEvent()
        {
            return new TestRunEvent
            {
                Tests =
                {
                    new TestCaseResult
                    {
                        Parameters = "without start...",
                        Duration = TimeSpan.FromSeconds(1),
                        Result = TestResult.Success,
                        TestMethod = Names.Method("[?] [?].M()")
                    },
                    new TestCaseResult
                    {
                        Parameters = "with start...",
                        StartTime = DateTime.Now,
                        Duration = TimeSpan.FromSeconds(2),
                        Result = TestResult.Error,
                        TestMethod = Names.Method("[?] [?].M()")
                    }
                },
                WasAborted = true
            };
        }

        private static IDEEvent CreateUserProfileEvent()
        {
            return new UserProfileEvent
            {
                Comment = "c",
                Position = Positions.ResearcherAcademic,
                CodeReviews = YesNoUnknown.Yes,
                Education = Educations.Autodidact,
                ProfileId = "p",
                ProgrammingCSharp = Likert7Point.Negative1,
                ProgrammingGeneral = Likert7Point.Positive1,
                ProjectsCourses = true,
                ProjectsPersonal = true,
                ProjectsSharedLarge = true,
                ProjectsSharedMedium = true,
                ProjectsSharedSmall = true,
                TeamsLarge = true,
                TeamsMedium = true,
                TeamsSmall = true,
                TeamsSolo = true
            };
        }

        private static IDEEvent CreateCommandEvent()
        {
            return new CommandEvent
            {
                CommandId = "cid"
            };
        }

        private static T _<T>(T e) where T : IDEEvent
        {
            e.ActiveDocument = Names.Document("d d");
            e.ActiveWindow = Names.Window("w w");
            e.Duration = TimeSpan.FromSeconds(13);
            e.IDESessionUUID = "sid";
            e.Id = "id";
            e.KaVEVersion = "vX";
            e.TerminatedAt = DateTime.Now.AddSeconds(2);
            e.TriggeredAt = DateTime.Now;
            e.TriggeredBy = EventTrigger.Typing;
            return e;
        }
    }
}