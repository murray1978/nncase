// Copyright (c) Canaan Inc. All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.

using System.Numerics.Tensors;
using static Nncase.IR.Utility;

namespace Nncase.IR.Tensors
{
    /// <summary>
    /// Range expression.
    /// </summary>
    public sealed record Range() : Op
    {
        /// <summary>
        /// Gets begin.
        /// </summary>
        public static readonly ParameterInfo Begin = new(typeof(Range), 0, "begin", IsIntegralScalar());

        /// <summary>
        /// Gets end.
        /// </summary>
        public static readonly ParameterInfo End = new(typeof(Range), 1, "end", IsIntegralScalar());

        /// <summary>
        /// Gets step.
        /// </summary>
        public static readonly ParameterInfo Step = new(typeof(Range), 2, "step", IsIntegralScalar());

        /// <inheritdoc/>
        public IRType InferInvokeResultType(ITypeInferenceContext context, TensorType begin, TensorType end, TensorType step)
        {
            if (context.GetArgument(this, Begin) is Const beginValue
                && context.GetArgument(this, End) is Const endValue
                && context.GetArgument(this, Step) is Const stepValue)
            {
                return new TensorType(
                    DataType.Int32,
                    new Shape((beginValue.ToScalar<int>() + endValue.ToScalar<int>()) / stepValue.ToScalar<int>())
                    );
            }

            return new InvalidType("Range begin, end, step should be constant");
        }
    }
}