using SharpGLHelper.Shaders.ParameterInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.Parameters
{

    public class SimpleShaderParameters : IMaterialShaderParameters, IMVPNParameters, ITransformableParameters
    {
        public string LightPositionId
        {
            get { return null; }
        }

        public string DiffuseId
        {
            get { return null; }
        }

        public string AmbientId
        {
            get { return "PickingColor"; }
        }

        public string SpecularId
        {
            get { return null; }
        }

        public string ShininessId
        {
            get { return null; }
        }


        public string EmissionId
        {
            get { return null; }
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

        string IMVPNParameters.ProjectionMatrixId
        {
            get { return "Projection"; }
        }

        string IMVPNParameters.ModelviewMatrixId
        {
            get { return "Modelview"; }
        }

        string IMVPNParameters.NormalMatrixId
        {
            get { return "NormalMatrix"; }
        }
        public void ApplyMaterialParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, ModelComponents.Material m)
        {
            ParameterAppliers.JOG.ApplyMaterialParameters(gl, esp, m);
        }

        public void ApplyMVPNParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, Scene.Projection pr, Scene.ModelView mv, Scene.Normal nrml)
        {
            ParameterAppliers.JOG.ApplyMVPNParameter(gl, esp, pr, mv, nrml);
        }

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
