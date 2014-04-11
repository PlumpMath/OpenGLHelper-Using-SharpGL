using SharpGL;
using SharpGL.Shaders;
using SharpGLHelper.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ParameterInterfaces
{
    /// <summary>
    /// Implement this interface for shaders that have similar input parameters as SharpGLHelper.ModelComponents.Material.
    /// </summary>
    public interface IMaterialShaderParameters : IShaderParameterIds
    {
        string LightPositionId { get; }
        string DiffuseId { get; }
        string AmbientId { get; }
        string SpecularId { get; }
        string ShininessId { get; }
        string EmissionId { get; }

        void ApplyMaterialParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, ModelComponents.Material m);
    }
}
