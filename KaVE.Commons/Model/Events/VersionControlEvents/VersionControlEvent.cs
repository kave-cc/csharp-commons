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

using System.Linq;
using System.Runtime.Serialization;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.IDEComponents;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.Events.VersionControlEvents
{
    public interface IVersionControlEvent
    {
        ISolutionName Solution { get; }
        IKaVEList<IVersionControlAction> Actions { get; }

        /// <summary>
        ///     throws an exception if more than one action is associated with the event. This should no longer happen with the
        ///     published events, but is possible in theory.
        /// </summary>
        VersionControlActionType ActionTypeOfSingleAction { get; }
    }


    [DataContract]
    public class VersionControlEvent : IDEEvent, IVersionControlEvent
    {
        [DataMember]
        public IKaVEList<IVersionControlAction> Actions { get; set; }

        public VersionControlActionType ActionTypeOfSingleAction
        {
            get
            {
                Asserts.That(
                    1 == Actions.Count,
                    "This convenience method is only valid for the preprocessed event stream published by " +
                    "the KaVE procject, in which only exactly one version control action is stored in each " +
                    "VersionControlEvent. However, this assumption does not hold for the current event, which " +
                    "contains {0} actions that need to be processed individually.", Actions.Count);
                return Actions.First().ActionType;
            }
        }

        [DataMember]
        public ISolutionName Solution { get; set; }

        public VersionControlEvent()
        {
            Actions = Lists.NewList<IVersionControlAction>();
            Solution = Names.UnknownSolution;
        }

        private bool Equals(VersionControlEvent other)
        {
            return base.Equals(other) && Equals(Actions, other.Actions) && Equals(Solution, other.Solution);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Actions.GetHashCode();
                hashCode = (hashCode * 397) ^ Solution.GetHashCode();
                return hashCode;
            }
        }
    }
}