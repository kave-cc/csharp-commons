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

namespace KaVE.Commons.Utils.Json.Fixers
{
    public class NegativeDurationFixer : IDeserializationFixer
    {
        public object Fix(string json, object obj)
        {
            var e = obj as IDEEvent;
            if (e != null && e.Duration < TimeSpan.Zero)
            {
                e.Duration = TimeSpan.Zero;
            }
            return obj;
        }
    }
}