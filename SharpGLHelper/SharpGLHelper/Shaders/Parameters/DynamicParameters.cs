using SharpGLHelper.ModelComponents;
using SharpGLHelper.Scene;
using SharpGLHelper.Shaders.ParameterInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.Parameters
{
    public class DynamicParameters : IMaterialShaderParameters, IMVPNParameters, IShaderParameterIds, ISingleLightParameters, ITransformableParameters
    {

        #region fields

        #endregion fields

        #region properties
        #endregion properties

        #region events
        #endregion events

        #region constructors
        #endregion constructors

        #region inherited properties
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

        public string TransformationMatrixId
        {
            get { return "TransformationMatrix"; }
        }
        #endregion inherited properties

        #region inherited methods
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

        public void ApplyTransformableParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.mat4 m)
        {
            ParameterAppliers.JOG.ApplyTransformableParameters(gl, esp, m);
        }
        public void ApplyTransformableParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, GlmNet.mat4[] m)
        {
            throw new NotImplementedException();
        }
        #endregion inherited methods

        public void ApplyAll(SharpGL.OpenGL gl, ExtShaderProgram esp, Material mat = null, Projection pr = null, ModelView mv = null, Normal nrml = null)
        {

        }
    }
}
