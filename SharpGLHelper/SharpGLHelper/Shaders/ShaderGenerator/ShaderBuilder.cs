using SharpGLHelper.Shaders.ShaderGenerator.BuildLevels;
using SharpGLHelper.Shaders.ShaderGenerator.BuildOptions;
using SharpGLHelper.Shaders.ShaderGenerator.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator
{
    enum Shader { Vertex, Fragment}
    public class ShaderBuilder
    {
        #region fields
        Shader _curShader = Shader.Vertex;
        VertexShaderBuilder _vertexShaderBuilder = new VertexShaderBuilder("130");
        FragmentShaderBuilder _fragmentShaderBuilder = new FragmentShaderBuilder("130");
        string _version = "",
            _vertexShader = "",
            _fragmentShader = "";
        #endregion fields

        #region properties

        public VertexShaderBuilder VertexShaderBuilder
        {
            get { return _vertexShaderBuilder; }
            set { _vertexShaderBuilder = value; }
        }

        public FragmentShaderBuilder FragmentShaderBuilder
        {
            get { return _fragmentShaderBuilder; }
            set { _fragmentShaderBuilder = value; }
        }

        #region default
        public string Version
        {
            get { return _version; }
            set 
            {
                if (ResourceLoader.GetGLSLVersions().Contains(value))
                    _version = value;
                VertexShaderBuilder.Version = _version;
                FragmentShaderBuilder.Version = _version;
            }
        }

        public string VertexShader
        {
            get { return _vertexShader; }
            set { _vertexShader = value; }
        }

        public string FragmentShader
        {
            get { return _fragmentShader; }
            set { _fragmentShader = value; }
        }

        #endregion default

        #endregion properties

        #region events
        #endregion events

        #region constructors
        public ShaderBuilder(string version)
        {
            Version = version;
            VertexShaderBuilder = new VertexShaderBuilder(Version);
            FragmentShaderBuilder = new FragmentShaderBuilder(Version);
        }
        #endregion constructors

        public void Build()
        {
            VertexShader = new CodeFormatter(VertexShaderBuilder.Build()).Format();
            FragmentShader = new CodeFormatter(FragmentShaderBuilder.Build()).Format();
        }
    }
}
