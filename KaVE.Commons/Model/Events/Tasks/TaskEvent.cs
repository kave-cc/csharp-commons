/*
 * Copyright 2017 Nico Strebel
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

using System.Runtime.Serialization;
using KaVE.Commons.Model.Events.Enums;
using KaVE.Commons.Utils;
using KaVE.JetBrains.Annotations;

namespace KaVE.Commons.Model.Events.Tasks
{
    public interface ITaskEvent
    {
        [NotNull]
        string Version { get; set; }

        [NotNull]
        string TaskId { get; set; }

        TaskAction Action { get; set; }

        /// <summary>
        ///     Set for create and move actions.
        /// </summary>
        [CanBeNull]
        string NewParentId { get; set; }

        /// <summary>
        ///     Set for create and edit actions.
        /// </summary>
        [CanBeNull]
        Likert5Point? Annoyance { get; set; }

        /// <summary>
        ///     Set for create and edit actions.
        /// </summary>
        [CanBeNull]
        Likert5Point? Importance { get; set; }

        /// <summary>
        ///     Set for create and edit actions.
        /// </summary>
        [CanBeNull]
        Likert5Point? Urgency { get; set; }
    }

    [DataContract]
    public class TaskEvent : IDEEvent, ITaskEvent
    {
        public TaskEvent()
        {
            Version = "";
            TaskId = "";
        }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string TaskId { get; set; }

        [DataMember]
        public TaskAction Action { get; set; }

        [DataMember]
        public string NewParentId { get; set; }

        [DataMember]
        public Likert5Point? Annoyance { get; set; }

        [DataMember]
        public Likert5Point? Importance { get; set; }

        [DataMember]
        public Likert5Point? Urgency { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        internal bool Equals(TaskEvent other)
        {
            return base.Equals(other) && string.Equals(Version, other.Version) && string.Equals(TaskId, other.TaskId) &&
                   Action == other.Action && string.Equals(NewParentId, other.NewParentId) &&
                   Annoyance == other.Annoyance && Importance == other.Importance && Urgency == other.Urgency;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Version.GetHashCode();
                hashCode = (hashCode * 397) ^ TaskId.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Action;
                hashCode = (hashCode * 397) ^ (NewParentId != null ? NewParentId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Annoyance.GetHashCode();
                hashCode = (hashCode * 397) ^ Importance.GetHashCode();
                hashCode = (hashCode * 397) ^ Urgency.GetHashCode();
                return hashCode;
            }
        }
    }
}