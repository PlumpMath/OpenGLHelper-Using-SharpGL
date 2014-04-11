using SharpGLHelper.Shaders.ParameterInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.Parameters
{
    public class PerPixelTransformableParameters : PerPixelParameters, ITransformableParameters
    {

        public string TransformationMatrixId
        {
            get { return "TransformationMatrix"; }
        }

        public void ApplyTransformableParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.mat4 m)
        {
            ParameterAppliers.JOG.ApplyTransformableParameters(gl, esp, m);
        }


    }
}
