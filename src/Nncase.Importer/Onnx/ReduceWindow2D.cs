// Copyright (c) Canaan Inc. All rights reserved.
// Licensed under the Apache license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Security.Cryptography;
using Nncase.IR;
using Nncase.IR.Tensors;
using Onnx;
using F = Nncase.IR.F;

namespace Nncase.Importer
{
    public partial class OnnxImporter
    {
        // isGlobal used for GlobalXXXPool
        private Expr VisitReduceWindow2D(in NodeProto op, ReduceOp reduceOp, float initValue, bool isGlobal = false)
        {
            // auto_pad had been DEPRECATED
            var input = GetInputExpr(op, 0);
            var ceilMode = GetBoolAttribute(op, "ceil_mode", false);
            var countIncludePad = GetBoolAttribute(op, "count_include_pad", false);
            var kernelShape = isGlobal
                ? Util.GetHW(input).Map((h, w) => (Expr)F.Tensors.Concat(new Tuple(h, w), 0))
                : Const.FromSpan<long>(GetIntsAttribute(op, "kernel_shape"));
            var pads = GetPadsAttribute(op);
            var strides = GetStrideAttribute(op);
            return F.Tensors.ReduceWindow2D(reduceOp, input, initValue,
                kernelShape,
                strides,
                pads,
                ceilMode,
                countIncludePad);
        }
    }
}