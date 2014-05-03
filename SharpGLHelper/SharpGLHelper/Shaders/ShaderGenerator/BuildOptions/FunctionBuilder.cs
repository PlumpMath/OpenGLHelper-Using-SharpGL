using SharpGLHelper.Shaders.ShaderGenerator.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.BuildOptions
{
    public class FunctionBuilder : IBuilder
    {

        #region fields
        HashSet<Variable> _parameters = new HashSet<Variable>();
        GLSLType _returnType;
        string _functionName;

        #endregion fields

        #region properties
        public string FunctionName
        {
            get { return _functionName; }
            set { _functionName = value; }
        }

        public GLSLType ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        public string Content { get; set; }

        public HashSet<Variable> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public string Result;
        #endregion properties

        #region events
        #endregion events

        #region constructors
        public FunctionBuilder(GLSLType returnType, string functionName, HashSet<Variable> parameters = null)
        {
            ReturnType = returnType;
            FunctionName = functionName;

            if (parameters != null)
                Parameters = parameters;
        }
        #endregion constructors

        public virtual void Build()
        {
            Result = ReturnType + " " + FunctionName + "(";

            foreach (var prm in Parameters)
            {
                Result += prm.ToString() + ",";
            }

            if (Parameters.Count > 0)
                Result = Result.Substring(0, Result.LastIndexOf(','));


            Result += "){" + Content + "}";
        }
    }
}
