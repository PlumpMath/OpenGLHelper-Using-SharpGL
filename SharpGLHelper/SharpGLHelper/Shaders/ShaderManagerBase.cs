using SharpGL;
using SharpGL.Shaders;
using SharpGLHelper.Buffers;
using SharpGLHelper.SceneElements;
using SharpGLHelper.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpGLHelper.Shaders
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ShaderManagerBase:ParameterIds,IDisposable
    {
        #region fields
        bool _programIsBound = false;


        List<BufferGroup> _bufferGroups = new List<BufferGroup>();
        List<Action<OpenGL>> _updateActions = new List<Action<OpenGL>>();
        ShaderProgram _shaderProgram;
        OpenGL _gl;
        static Dictionary<uint, string> _attributeLocations;
        string _vertexShaderCode, _fragmentShaderCode;
        #endregion fields

        #region properties

        public List<Action<OpenGL>> UpdateActions
        {
            get { return _updateActions; }
            set { _updateActions = value; }
        }
        /// <summary>
        /// Is true when "UseProgram" is executing.
        /// </summary>
        public bool ProgramIsBound
        {
            get { return _programIsBound; }
            private set { _programIsBound = value; }
        }
        public ShaderProgram ShaderProgram
        {
            get { return _shaderProgram; }
            set { _shaderProgram = value; }
        }

        protected virtual Dictionary<uint, string> AttributeLocations
        {
            get { return _attributeLocations; }
            set { _attributeLocations = value; }
        }

        public string VertexShaderCode
        {
            get { return _vertexShaderCode; }
            protected set { _vertexShaderCode = value; }
        }

        public string FragmentShaderCode
        {
            get { return _fragmentShaderCode; }
            protected set { _fragmentShaderCode = value; }
        }

        public abstract bool HasChanges { get; }

        public virtual List<BufferGroup> BufferGroups
        {
            get { return _bufferGroups; }
            set { _bufferGroups = value; }
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors
        /// <summary>
        /// Static constructor.
        /// </summary>
        static ShaderManagerBase()
        {
            //  We're going to specify the attribute locations for the position and normal, 
            //  so that we can force both shaders to explicitly have the same locations.
            _attributeLocations = new Dictionary<uint, string>
            {
                {0, "Position"},
                {1, "Normal"},
            };
        }

        /// <summary>
        /// Compile the code for the shaderprogram.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="nameOfShaderFiles">The name of the shader files. Extensions will be added automatically.</param>
        public ShaderManagerBase(OpenGL gl, string nameOfShaderFiles)
            : this(gl,
                   ShaderProperties.ShaderPath + nameOfShaderFiles + ShaderProperties.VertexShaderExtension,
                   ShaderProperties.ShaderPath + nameOfShaderFiles + ShaderProperties.FragmentShaderExtension) { }

        /// <summary>
        /// Compile the code for the shaderprogram.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="vertexShaderSource">The path to the vertex shader code.</param>
        /// <param name="fragmentShaderSource">The path to the fragment shader code.</param>
        public ShaderManagerBase(OpenGL gl, string vertexShaderSource, string fragmentShaderSource, Assembly executingAssembly = null, bool autoAttachAssemblyName = true)
            : this(
                   ManifestResourceLoader.LoadTextFile(vertexShaderSource, executingAssembly, autoAttachAssemblyName),
                   ManifestResourceLoader.LoadTextFile(fragmentShaderSource, executingAssembly, autoAttachAssemblyName), gl) { }

        /// <summary>
        /// Compile the code for the shaderprogram.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="vertexShaderCode">The code for the vertex shader.</param>
        /// <param name="fragmentShaderCode">The code for the fragment shader.</param>
        public ShaderManagerBase(string vertexShaderCode, string fragmentShaderCode, OpenGL gl)
        {
            // Remember the GL, so it's available for dispose.
            _gl = gl;

            _vertexShaderCode = vertexShaderCode;
            _fragmentShaderCode = fragmentShaderCode;

            //  Create the shader program.
            ShaderProgram = new ShaderProgram();
            ShaderProgram.Create(gl, vertexShaderCode, fragmentShaderCode, AttributeLocations);

        }
        #endregion constructors


        /// <summary>
        /// This method binds the shaderprogram to the GL, invokes the function and unbinds the shader.
        /// The function should contain everything that has to be loaded with this shader's settings.
        /// </summary>
        /// <param name="gl">The GL.</param>
        /// <param name="func">The code that has to be executing in this ShaderProgram. </param>
        protected void UseProgram(OpenGL gl, Action func)
        {
            // Bind the shader.
            ShaderProgram.Bind(gl);
            ProgramIsBound = true;


            try
            {
                // Invoke logic.
                func.Invoke();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Unbind the shader.
                ShaderProgram.Unbind(gl);
                ProgramIsBound = false;
            }
        }

        public abstract void RenderAll(OpenGL gl);

        public virtual void ApplyChangedProperties(OpenGL gl)
        {

            if (!ProgramIsBound)
                throw new Exception("This method cannot be called outside a program. Consider using this inside RenderAll(OpenGL, Action).");

            foreach (var act in UpdateActions)
            {
                act.Invoke(gl);
            }

            UpdateActions.Clear();
        }


        /// <summary>
        /// Deletes the shaderprogram of the GL and clears the parameters of this object.
        /// </summary>
        public void Dispose()
        {
            ShaderProgram.Delete(_gl);
        }

        public void Dispose(OpenGL gl)
        {
            ShaderProgram.Delete(gl);
        }

    }
}
