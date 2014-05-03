using SharpGLHelper.Shaders.ParameterInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.Parameters
{
    public class PerPixelTransArrayParameters : PerPixelParameters, ITransformableParameters
    {

        public string TransformationMatrixId
        {
            get { return "TransformationMatrix"; }
        }

        public void ApplyTransArrayParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.mat4[] m)
        {
            ParameterAppliers.JOG.ApplyTransArrayParameters(gl, esp, m);
        }




        public void ApplyTransformableParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.mat4 m)
        {
            ParameterAppliers.JOG.ApplyTransformableParameters(gl, esp, m);
        }

        public void ApplyTransformableParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.mat4[] m)
        {
            throw new NotImplementedException();
        }
    }
}
