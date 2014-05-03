using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator
{
    public class FragmentShaderBuilder:IGenericBuilder<string>
    {

        #region fields

        string _version = "";
        #endregion fields

        #region properties

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors
        #region constructors
        public FragmentShaderBuilder(string version)
        {
            
        }
        #endregion constructors
        #endregion constructors


        public string Build()
        {
            return "throw new NotImplementedException();";
        }
    }
}
