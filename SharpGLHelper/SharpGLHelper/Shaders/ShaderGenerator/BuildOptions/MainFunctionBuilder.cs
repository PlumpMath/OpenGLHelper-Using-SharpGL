using SharpGLHelper.Shaders.ShaderGenerator.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.BuildOptions
{
    public class MainFunctionBuilder : FunctionBuilder
    {
        #region fields
        #endregion fields

        #region properties
        #endregion properties

        #region events
        #endregion events

        #region constructors
        public MainFunctionBuilder()
            :base(new GLSLType("void"), "main")
        {
        }
        #endregion constructors

        public override void Build()
        {
            base.Build();
        }
    }
}
