using SharpGLHelper.Scene;
using SharpGL;
using SharpGL.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGLHelper.Shaders.Parameters;
using GlmNet;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.Shaders.ParameterInterfaces;

namespace SharpGLHelper.Shaders
{
    /// <summary>
    /// An extended ShaderProgram.
    /// </summary>
    public class ExtShaderProgram : IDisposable
    {
        #region fields
        OpenGL _gl = null;
        #endregion fields

        #region properties
        /// <summary>
        /// The shader program.
        /// </summary>
        public ShaderProgram Program { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IShaderParameterIds Parameters { get; private set; }

        #endregion properties

        #region constructor
        public ExtShaderProgram(ShaderProgram program, IShaderParameterIds parameters)
        {
            Program = program;
            Parameters = parameters;
        }

        #endregion constructor

        #region apply parameters
        /// <summary>
        /// Applies the material to the shader if the "Parameters" are an implementation of "IMaterialShaderParameters".
        /// </summary>
        /// <param name="gl">The GL.</param>
        /// <param name="m">The material.</param>
        /// <param name="throwException">Property that sets whether it should throw an exception or just return when this shaderprogram doesn't implement the required interface.</param>
        public void ApplyMaterial(OpenGL gl, Material m, bool throwException = true)
        {
            // Test if the parameters implementing the required interface.
            if (!TypeTest(throwException, typeof(IMaterialShaderParameters)))
                return;

            var prms = Parameters as IMaterialShaderParameters;

            prms.ApplyMaterialParameters(gl, this, m);
        }

        /// <summary>
        /// Applies the Modelview-, projection- and normal matrix to the shaders if the "Parameters" are an implementation of "IMVPNParameters".
        /// </summary>
        /// <param name="gl">The GL.</param>
        /// <param name="modelview">The modelview class that contains the matrix.</param>
        /// <param name="projection">The projection class that contains the matrix.</param>
        /// <param name="normal">The normal class that contains the matrix.</param>
        /// <param name="throwException">Property that sets whether it should throw an exception or just return when this shaderprogram doesn't implement the required interface.</param>
        public void ApplyMVPNMatrices(OpenGL gl, ModelView modelview, Projection projection, Normal normal, bool throwException = true)
        {
            // Test if the parameters implementing the required interface.
            if (!TypeTest(throwException, typeof(IMVPNParameters)))
                return;

            var prms = Parameters as IMVPNParameters;

            prms.ApplyMVPNParameters(gl, this, projection, modelview, normal);
        }

        /// <summary>
        /// Sets the light position to the shader if the "Parameters" are an implementation of "ISingleLightParameters"
        /// </summary>
        /// <param name="gl">The GL.</param>
        /// <param name="lightPosition">The light position.</param>
        /// <param name="throwException">Property that sets whether it should throw an exception or just return when this shaderprogram doesn't implement the required interface.</param>
        public void ApplyLighting(OpenGL gl, vec3 lightPosition, bool throwException = true)
        {
            // Test if the parameters implementing the required interface.
            if (!TypeTest(throwException, typeof(ISingleLightParameters)))
                return;

            var prms = Parameters as ISingleLightParameters;

            prms.ApplySingleLightParameters(gl, this, lightPosition);
        }

        /// <summary>
        /// Sets the transformation matrices that should be used on the next objects if the "Parameters" are an implementation of "ITransformableParameters"
        /// </summary>
        /// <param name="gl">The GL.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <param name="throwException">Property that sets whether it should throw an exception or just return when this shaderprogram doesn't implement the required interface.</param>
        public void ApplyTransformationMatrix(OpenGL gl, mat4 matrix, bool throwException = true)
        {
            // Test if the parameters implementing the required interface.
            if (!TypeTest(throwException, typeof(ITransformableParameters)))
                return;

            var prms = Parameters as ITransformableParameters;

            prms.ApplyTransformableParameters(gl, this, matrix);
        }

        #endregion apply parameters


        /// <summary>
        /// This method binds the shaderprogram to the GL, invokes the function and unbinds the shader.
        /// The function should contain everything that has to be loaded with this shader's settings.
        /// </summary>
        /// <param name="gl">The GL.</param>
        /// <param name="func">The code that has to be executing in this ShaderProgram. </param>
        public void UseProgram(OpenGL gl, Action func)
        {
            _gl = gl;

            // Bind the shader.
            Program.Bind(gl);
            

            // Invoke logic.
            func.Invoke();

            // Unbind the shader.
            Program.Unbind(gl);
        }

        /// <summary>
        /// Deletes the shaderprogram van the GL and clears the parameters of this object.
        /// </summary>
        public void Dispose()
        {
            Program.Delete(_gl);
            Program = null;
            Parameters = null;
        }


        private bool TypeTest(bool throwException, Type type)
        {
            // if the parameters are an extention from the given type, return true.
            if (type.IsAssignableFrom(Parameters.GetType()))
                return true;


            if (!throwException)
                return false;

            throw new InvalidCastException("'Parameters' does not implement '"+ type.ToString() +"'." +
                "This has to be implemented for this method to know how the shader expects it's parameter names.");
            
        }
    }
}
