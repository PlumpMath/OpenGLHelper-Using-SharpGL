using SharpGL;
using SharpGLHelper.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ParameterInterfaces
{
    /// <summary>
    /// Implement this interface for shaders that accept  Modelview-, Projection- and/or Normal matrices.
    /// </summary>
    interface IMVPNParameters
    {
        string ProjectionMatrixId { get; }
        string ModelviewMatrixId { get; }
        string NormalMatrixId { get; }
        void ApplyMVPNParameters(OpenGL gl, ExtShaderProgram esp, Projection pr, ModelView mv, Normal nrml);
    
    }
}
