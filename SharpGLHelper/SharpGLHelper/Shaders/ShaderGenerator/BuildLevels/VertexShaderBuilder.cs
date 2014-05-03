using SharpGLHelper.Shaders.ShaderGenerator.BuildOptions;
using SharpGLHelper.Shaders.ShaderGenerator.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.BuildLevels
{
    public class VertexShaderBuilder:IGenericBuilder<string>
    {
        #region fields
        VertexNormalBuilder _vertexNormalBuilder;
        VertexPositionBuilder _vertexPositionBuilder;

        string _version = "";
        HashSet<Variable> _globalVariables = new HashSet<Variable>();
        Mat4Loop _mat4Loop = new Mat4Loop();
        MainFunctionBuilder _main = new MainFunctionBuilder();
        #endregion fields

        #region properties

        public Mat4Loop Mat4Loop
        {
            get { return _mat4Loop; }
            set { _mat4Loop = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public HashSet<Variable> GlobalVariables
        {
            get { return _globalVariables; }
            set { _globalVariables = value; }
        }
        public MainFunctionBuilder Main
        {
            get { return _main; }
            set { _main = value; }
        }
        #region settings

        public VertexNormalBuilder VertexNormalBuilder
        {
            get { return _vertexNormalBuilder; }
            set { _vertexNormalBuilder = value; }
        }

        public VertexPositionBuilder VertexPositionBuilder
        {
            get { return _vertexPositionBuilder; }
            set { _vertexPositionBuilder = value; }
        }
        #endregion settings
        #endregion properties

        #region events
        #endregion events

        #region constructors
        public VertexShaderBuilder(string version)
        {
            Version = version;

            VertexNormalBuilder = new VertexNormalBuilder();
            VertexPositionBuilder = new VertexPositionBuilder(GlobalVariables, Main, Mat4Loop);
        }
        #endregion constructors

        public string Build()
        {

            GlobalVariables.Clear();
            Main.Content = "";



            StringBuilder result = new StringBuilder("#version " + Version + "\r\n");

            #region prepare data
            VertexPositionBuilder.Build();
            VertexNormalBuilder.Build();


            Main.Build();
            #endregion prepare data


            // Add global variables.
            foreach (var gVar in GlobalVariables)
            {
                result.Append(gVar.ToString() + ";");
            }

            // Add Main function.
            result.Append(Main.Result);

            // return result
            return result.ToString();
        }
    }
}
