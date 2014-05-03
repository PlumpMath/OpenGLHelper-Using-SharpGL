using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator
{
    public class ResourceLoader
    {
        public static char SplitChar = ';';
        public static string[] ToArray(string value)
        {
            return value.Split(SplitChar);
        }

        public static string[] GetGLSLVersions()
        {
            return ToArray(Resources.ShaderValues.GLSL_VERSIONS);
        }
    }
}
