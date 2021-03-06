﻿/*
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

using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Model.SSTs.References;
using KaVE.JetBrains.Annotations;

namespace KaVE.Commons.Model.SSTs.Expressions.Assignable
{
    public interface ICastExpression : IAssignableExpression
    {
        [NotNull]
        ITypeName TargetType { get; }

        CastOperator Operator { get; }

        [NotNull]
        IVariableReference Reference { get; }
    }

    public enum CastOperator
    {
        Unknown,
        Cast, // (int)x
        SafeCast // x as int
    }
}