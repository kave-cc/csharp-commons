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

using System;
using KaVE.Commons.Model.Events.CompletionEvents;
using Newtonsoft.Json;

namespace KaVE.Commons.Utils.Json
{
    internal class ProposalCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read(); // start obj
                reader.Read(); // property name ("$type")
                reader.Read(); // string ("fqn.of.Proposal")
                reader.Read(); // property name ("Proposals")
                reader.Read(); // start array
                var res = ReadAllProposals(reader, serializer);
                reader.Read(); // end obj
                return res;
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read(); // startArr
                var res = ReadAllProposals(reader, serializer);
                return res;
            }
            throw new JsonSerializationException("expected either array or object to deserialize proposal collection");
        }

        private static ProposalCollection ReadAllProposals(JsonReader reader, JsonSerializer serializer)
        {
            var res = new ProposalCollection();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var p = serializer.Deserialize<Proposal>(reader);
                res.Add(p);
                reader.Read(); // end object
            }
            return res;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IProposalCollection).IsAssignableFrom(objectType);
        }
    }
}