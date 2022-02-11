﻿// Copyright (c) Canaan Inc. All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFabric.Hyperlinq;

namespace Nncase.IR.Tensors;

/// <summary>
/// Gather expression.
/// </summary>
public sealed record Gather() : Op
{
    /// <summary>
    /// Gets input.
    /// </summary>
    public static readonly ParameterInfo Input = new(typeof(Gather), 0, "input");

    /// <summary>
    /// Gets axis.
    /// </summary>
    public static readonly ParameterInfo Axis = new(typeof(Gather), 1, "axis");

    /// <summary>
    /// Gets index.
    /// </summary>
    public static readonly ParameterInfo Index = new(typeof(Gather), 2, "index");
}
