using SharpGLHelper.Shaders;
using SharpGL;
using SharpGL.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGLHelper.Shaders.Parameters;
using SharpGLHelper.Shaders.ParameterInterfaces;

namespace SharpGLHelper.Scene
{
    /// <summary>
    /// 
    /// </summary>
    public static class Shaders
    {
        #region fields
        static Dictionary<uint, string> _attributeLocations;
        #endregion fields

        #region properties
        #endregion properties

        #region constructor
        /// <summary>
        /// Static constructor.
        /// </summary>
        static Shaders()
        {
            //  We're going to specify the attribute locations for the position and normal, 
            //  so that we can force both shaders to explicitly have the same locations.
            _attributeLocations = new Dictionary<uint, string>
            {
                {0, "Position"},
                {1, "Normal"},
            };
        }
        #endregion constructor

        #region LoadShader methods
        public static ExtShaderProgram LoadPerPixelShader(OpenGL gl)
        {
            return CreateShader(gl, new PerPixelParameters(), "PerPixel");
        }
        public static ExtShaderProgram LoadToonShader(OpenGL gl)
        {
            return CreateShader(gl, new ToonParameters(), "Toon");
        }
        public static ExtShaderProgram LoadSimpleShader(OpenGL gl)
        {
            return CreateShader(gl, new SimpleShaderParameters(),"SimpleShader");
        }
        public static ExtShaderProgram LoadPerPixelTransformableShader(OpenGL gl)
        {
            return CreateShader(gl, new PerPixelTransformableParameters(), "PerPixelTransformable");
        }
        #endregion LoadShader methods

        #region Create Shader
        public static ExtShaderProgram CreateShader(OpenGL gl, IShaderParameterIds parameters, string nameOfShaderFiles)
        {
            string vertexShaderPath = ShaderProperties.ShaderPath + nameOfShaderFiles + ShaderProperties.VertexShaderExtension;
            string fragmentShaderPath = ShaderProperties.ShaderPath + nameOfShaderFiles + ShaderProperties.FragmentShaderExtension;
            return CreateShader(gl, parameters, vertexShaderPath, fragmentShaderPath);
        }

        /// <summary>
        /// Create and add a new ShaderProgram from the given sources. 
        /// Call this function if you decide to add your own shaders.
        /// </summary>
        /// <param name="gl">The OpenGL</param>
        /// <param name="vertexShaderSource">The path to the vertex shader code.</param>
        /// <param name="fragmentShaderSource">The path to the fragment shader code.</param>
        /// <param name="shaderName">The name for the shader.</param>
        public static ExtShaderProgram CreateShader(OpenGL gl, IShaderParameterIds parameters, string vertexShaderSource, string fragmentShaderSource)
        {
            //  Create the per pixel shader.
            ShaderProgram shader = new ShaderProgram();
            shader.Create(gl,
                ManifestResourceLoader.LoadTextFile(vertexShaderSource),
                ManifestResourceLoader.LoadTextFile(fragmentShaderSource), _attributeLocations);

            return new ExtShaderProgram(shader, parameters);
        }

        #endregion Create Shader
    }
}
