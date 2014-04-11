using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ParameterInterfaces
{
    public interface ITransformableParameters
    {
        string TransformationMatrixId { get; }
        void ApplyTransformableParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, mat4 m);
    }
}
