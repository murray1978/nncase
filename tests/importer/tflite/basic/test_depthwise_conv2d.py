# Copyright 2019-2021 Canaan Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
"""System test: test depthwise conv2d"""
# pylint: disable=invalid-name, unused-argument, import-outside-toplevel

import pytest
import tensorflow as tf
import numpy as np
from test_runner import TfliteTestRunner

def _make_module(n, i_channels, i_size, k_size, strides, padding, dilations):
    class DepthwiseConv2DModule(tf.Module):
        def __init__(self):
            super(DepthwiseConv2DModule).__init__()
            self.w = tf.constant(np.random.rand(
                *k_size, i_channels, 1).astype(np.float32) - 1)

        @tf.function(input_signature=[tf.TensorSpec([n, *i_size, i_channels], tf.float32)])
        def __call__(self, x):
            out = tf.nn.depthwise_conv2d(x, self.w, [1, *strides, 1], padding,
                               dilations=dilations)
            return out
    return DepthwiseConv2DModule()

n = [
    1,
    3
]

i_channels = [
    1,
    16
]

i_sizes = [
    [1, 1],
    [33, 65]
]

k_sizes = [
    [1, 1],
    [3, 3],
    [5, 5]
]

strides = [
    [1, 1],
    [1, 3],
    [5, 5]
]

paddings = [
    'SAME',
    'VALID'
]

dilations = [
    [1, 1],
    #[2, 2]  there is a bug in tf.nn.depthwise_conv2d that produces incorrect output shape
]


@pytest.mark.parametrize('n', n)
@pytest.mark.parametrize('i_channels', i_channels)
@pytest.mark.parametrize('i_size', i_sizes)
@pytest.mark.parametrize('k_size', k_sizes)
@pytest.mark.parametrize('strides', strides)
@pytest.mark.parametrize('padding', paddings)
@pytest.mark.parametrize('dilations', dilations)
def test_depthwise_conv2d(n, i_channels, i_size, k_size, strides, padding, dilations, request):
    if padding != 'VALID' or (k_size[0] <= i_size[0] and k_size[1] <= i_size[1]):
        module = _make_module(n, i_channels, i_size, k_size,
                              strides, padding, dilations)

        runner = TfliteTestRunner(request.node.name)
        model_file = runner.from_tensorflow(module)
        runner.run(model_file)

if __name__ == "__main__":
    pytest.main(['-vv', 'test_depthwise_conv2d.py'])
