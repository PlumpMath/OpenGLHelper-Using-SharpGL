using SharpGLHelper.Shaders.ShaderGenerator.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.BuildOptions
{
    public class VertexPositionBuilder:IVoidBuilder
    {
        #region fields
        HashSet<Variable> _globalVars;
        Mat4Loop _mat4Loop;
        MainFunctionBuilder _main;
        #endregion fields

        #region properties
        #endregion properties

        #region events
        #endregion events

        #region constructors
        public VertexPositionBuilder(HashSet<Variable> globalVars, MainFunctionBuilder main, Mat4Loop mat4Loop)
        {
            _globalVars = globalVars;
            _mat4Loop = mat4Loop;
            _main = main;
        }
        #endregion constructors


        public void Build()
        {
            _globalVars.Add(new Variable("in vec4 Position"));

        }
    }
}
