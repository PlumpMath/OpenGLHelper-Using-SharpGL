using SharpGLHelper.Shaders.ParameterInterfaces;
using SharpGLHelper.Shaders.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.Parameters
{
    public class ToonParameters : IMaterialShaderParameters, IMVPNParameters, ISingleLightParameters
    {
        public string LightPositionId
        {
            get { return "LightPosition"; }
        }

        public string DiffuseId
        {
            get { return "DiffuseMaterial"; }
        }

        public string AmbientId
        {
            get { return "AmbientMaterial"; }
        }

        public string SpecularId
        {
            get { return "SpecularMaterial"; }
        }

        public string ShininessId
        {
            get { return "Shininess"; }
        }


        public string EmissionId
        {
            get { return "Emission"; }
        }

        public string ProjectionMatrixId
        {
            get { return "Projection"; }
        }

        public string ModelviewMatrixId
        {
            get { return "Modelview"; }
        }

        public string NormalMatrixId
        {
            get { return "NormalMatrix"; }
        }

        string ISingleLightParameters.LightPositionId
        {
            get { return "LightPosition"; }
        }
        public void ApplyMaterialParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, ModelComponents.Material m)
        {
            ParameterAppliers.JOG.ApplyMaterialParameters(gl, esp, m);
        }

        public void ApplyMVPNParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, Scene.Projection pr, Scene.ModelView mv, Scene.Normal nrml)
        {
            ParameterAppliers.JOG.ApplyMVPNParameter(gl, esp, pr, mv, nrml);
        }
        public void ApplySingleLightParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.vec3 lp)
        {
            ParameterAppliers.JOG.ApplySingleLightParameters(gl, esp, lp);
        }
    }
}
