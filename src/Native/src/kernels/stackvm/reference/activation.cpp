/* Copyright 2019-2021 Canaan Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#include "kernel_template.h"
#include <nncase/kernels/cpu/reference/runtime_types.h>
#include <nncase/kernels/kernel_utils.h>
#include <nncase/kernels/stackvm/tensor_ops.h>
#include <nncase/runtime/allocator.h>
#include <nncase/runtime/host_buffer.h>
#include <nncase/runtime/runtime_op_utility.h>
#include <nncase/runtime/util.h>

using namespace nncase;
using namespace nncase::runtime;
using namespace nncase::runtime::stackvm;
using namespace nncase::kernels;
using namespace nncase::kernels::cpu::reference;
using namespace nncase::kernels::stackvm;

FLOAT_UNARY_TEMPLATE(relu, std::max((float)0, x))
FLOAT_UNARY_TEMPLATE(sigmoid, 1 / (1 + exp(-x)))
FLOAT_UNARY_TEMPLATE(hard_swish, x * std::max(0.f, std::min((float)1.f, (float)(1.f/6 * x + 0.5))))
FLOAT_UNARY_WITH_MUL_TEMPLATE(prelu, slope, x < 0 ? slope * x : x)
FLOAT_UNARY_WITH_MUL_TEMPLATE(celu, alpha,
                              std::max((float)0, x) +
                                  std::min((float)0,
                                           (float)(alpha *(exp(x / alpha) - 1))))
FLOAT_UNARY_WITH_MUL_TEMPLATE(leaky_relu, alpha, x < 0 ? alpha * x : x)
