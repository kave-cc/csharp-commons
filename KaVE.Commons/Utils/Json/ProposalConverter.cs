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
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Utils.Exceptions;
using KaVE.Commons.Utils.Naming;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KaVE.Commons.Utils.Json
{
    public class ProposalConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var p = new Proposal();

            var propObj = JObject.Load(reader);
            var valName = propObj.GetValue("Name") as JValue;
            if (valName != null)
            {
                var name = valName.Value as string;
                if (name != null)
                {
                    IName n = null;
                    try
                    {
                        n = name.Deserialize<IName>();
                    }
                    catch (ValidationException ex)
                    {
                        Console.WriteLine(
                            "ValidationException during Proposal deserialization: {0}. Should only happen in legacy data, falling back to unknown name.",
                            ex.Message);
                    }
                    p.Name = n ?? Names.UnknownGeneral;
                }
            }
            else
            {
                p.Name = Names.UnknownGeneral;
            }

            var valRelevance = propObj.GetValue("Relevance") as JValue;
            if (valRelevance != null)
            {
                var relevance = (long) valRelevance.Value;
                p.Relevance = unchecked((int) relevance);
            }

            return p;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IProposal).IsAssignableFrom(objectType);
        }
    }
}