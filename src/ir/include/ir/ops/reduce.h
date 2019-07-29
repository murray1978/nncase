#pragma once
#include "../node.h"
#include <xtensor/xtensor.hpp>

namespace nncase
{
namespace ir
{
    class reduce : public node
    {
    public:
        input_connector &input() { return input_at(0); }
        output_connector &output() { return output_at(0); }

        reduce_op_t reduce_op() const noexcept { return reduce_op_; }
        const axis_t &axis() const noexcept { return axis_; }
        float init_value() const noexcept { return init_value_; }
        bool keep_dims() const noexcept { return keep_dims_; }

        reduce(reduce_op_t reduce_op, shape_t input_shape, axis_t axis, float init_value, bool keep_dims);

        node_opcode opcode() const noexcept override { return op_reduce; }

    private:
        reduce_op_t reduce_op_;
        axis_t axis_;
        float init_value_;
        bool keep_dims_;
    };
}
}
