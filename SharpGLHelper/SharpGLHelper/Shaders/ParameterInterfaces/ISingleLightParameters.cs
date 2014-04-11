using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ParameterInterfaces
{
    public interface ISingleLightParameters
    {
        string LightPositionId { get; }
       
        void ApplySingleLightParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, vec3 lp);
    
    }
}
