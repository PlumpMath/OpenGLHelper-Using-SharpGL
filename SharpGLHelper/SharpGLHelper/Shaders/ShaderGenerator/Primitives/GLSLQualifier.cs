using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.Primitives
{
    public class GLSLQualifier
    {
        string _qualifier = "";

        public string Qualifier
        {
            get { return _qualifier; }
            private set { _qualifier = value; }
        }

        public GLSLQualifier(string qualifier, bool check = true)
        {
            if (check && !Resources.GlslKeywords.Types.Contains(qualifier))
                throw new Exception("Type does not exist in Resources/GlslKeywords.");
            _qualifier = qualifier;
        }

        public override string ToString()
        {
            return _qualifier;    
        }
    }
}
