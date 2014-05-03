using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.Primitives
{
    public class GLSLType
    {
        string _type = "";

        public string Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        public GLSLType(string type, bool check = true)
        {
            if (check && !Resources.GlslKeywords.Types.Contains(type))
                throw new Exception("Type does not exist in Resources/GlslKeywords.");
            _type = type;
        }

        public override string ToString()
        {
            return _type;    
        }
    }
}
