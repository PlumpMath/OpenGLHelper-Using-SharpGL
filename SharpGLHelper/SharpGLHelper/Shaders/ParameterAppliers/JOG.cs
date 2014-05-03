using GlmNet;
using SharpGL;
using SharpGL.Shaders;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.Scene;
using SharpGLHelper.Shaders.ParameterInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ParameterAppliers
{
    /// <summary>
    /// Class serving as a collection of Applying methods for shaders written by Jochem Geussens for the purpose of preventing double code.
    /// </summary>
    public static class JOG
    {
        public static void ApplyMaterialParameters(OpenGL gl, ExtShaderProgram esp, Material m)
        {
            var p = esp.Program;
            var prms = esp.Parameters as IMaterialShaderParameters;

            var amb = m.Ambient;
            var diff = m.Diffuse;
            var spec = m.Specular;
            var shini = m.Shininess;
            var emit = m.Emission;

            if (prms.AmbientId != null && amb != null)
                p.SetUniform3(gl, prms.AmbientId, amb.R, amb.G, amb.B);
            if (prms.DiffuseId != null && diff != null)
                p.SetUniform3(gl, prms.DiffuseId, diff.R, diff.G, diff.B);
            if (prms.SpecularId != null && spec != null)
                p.SetUniform3(gl, prms.SpecularId, spec.R, spec.G, spec.B);
            if (prms.EmissionId != null && emit != null)
                p.SetUniform3(gl, prms.EmissionId, emit.R, emit.G, emit.B);
            if (prms.ShininessId != null)
                p.SetUniform1(gl, prms.ShininessId, shini);
        }

        public static void ApplyMVPNParameter(OpenGL gl, ExtShaderProgram esp, Projection pr, ModelView mv, Normal nrml)
        {
            var prms = esp.Parameters as IMVPNParameters;
            var p = esp.Program;

            // Set the matrices.
            p.SetUniformMatrix4(gl, prms.ProjectionMatrixId, pr.ToArray());
            p.SetUniformMatrix4(gl, prms.ModelviewMatrixId, mv.ToArray());
            p.SetUniformMatrix3(gl, prms.NormalMatrixId, nrml.ToArray());
        
        }


        public static void ApplySingleLightParameters(SharpGL.OpenGL gl, ExtShaderProgram esp, vec3 lp)
        {
            var prms = esp.Parameters as ISingleLightParameters;
            var p = esp.Program;

            // Set the light position.
            if (prms.LightPositionId != null)
                p.SetUniform3(gl, prms.LightPositionId, lp.x, lp.y, lp.z);
        }

        public static void ApplyTransformableParameters(OpenGL gl, ExtShaderProgram esp, mat4 m)
        {
            var prms = esp.Parameters as ITransformableParameters;
            var p = esp.Program;

            // Set the transformation matrix.
            if (prms.TransformationMatrixId != null)
                p.SetUniformMatrix4(gl, prms.TransformationMatrixId, m.to_array());
        }

        public static void ApplyTransArrayParameters(OpenGL gl, ExtShaderProgram esp, mat4[] mats)
        {
            var prms = esp.Parameters as ITransformableParameters;
            var p = esp.Program;



            // Set the transformation matrix.
            if (prms.TransformationMatrixId != null)
            {
                var m = new float[mats.Length][];

                for (int i = 0; i < mats.Length; i++)
                {
                    var mat = mats[i];
                    m[i] = new float[16];
                    m[i] = mat.to_array();
                }

                throw new NotImplementedException();
                //p.set
                //p.SetUniformMatrix4(gl, prms.TransformationMatrixId, m);
            }
        }
    }
}
