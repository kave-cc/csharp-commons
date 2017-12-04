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

using JetBrains.Application.Threading;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Utils;
using KaVE.JetBrains.Annotations;

namespace KaVE.VS.Commons.Generators
{
    public interface IGenericCommandGenerator
    {
        void Fire(string cmdId);
    }

    public abstract class GenericCommandGenerator : EventGeneratorBase, IGenericCommandGenerator
    {
        protected GenericCommandGenerator([NotNull] IRSEnv env,
            [NotNull] IMessageBus messageBus,
            [NotNull] IDateUtils dateUtils,
            IThreading threading) : base(env, messageBus, dateUtils, threading) { }

        public void Fire(string cmdId)
        {
            var e = Create<CommandEvent>();
            e.CommandId = cmdId;
            Fire(e);
        }
    }
}