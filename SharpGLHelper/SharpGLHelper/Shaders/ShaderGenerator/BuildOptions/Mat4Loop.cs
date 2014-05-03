using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.BuildOptions
{
    public class Mat4Loop:IGenericBuilder<string>
    {

        #region fields
        List<string> _beforeLoop1 = new List<string>();
        List<string> _beforeLoop2 = new List<string>();
        List<string> _afterLoop1 = new List<string>();
        List<string> _afterLoop2 = new List<string>();
        List<string> _inLoop2 = new List<string>();
        string _loop1VarName = "i",
            _loop2VarName = "j";
        #endregion fields

        #region properties
        #endregion properties

        #region events
        #endregion events

        #region constructors

        /// <summary>
        /// for (int i = 0; i &lt; 4; i++) {
        ///     --beforeLoop2--
        ///     for (int j = 0; j &lt; 4; j++){
        ///         --inLoop2--
        ///     }
        ///     --afterLoop2--
        /// }
        /// </summary>
        /// <param name="loop1VarName">The first loop variable.</param>
        /// <param name="loop2VarName">The second loop variable.</param>
        /// <returns></returns>
        public Mat4Loop(string loop1VarName = "i", string loop2VarName = "j")
        {
            _loop1VarName = loop1VarName;
            _loop2VarName = loop2VarName;
        }
        #endregion constructors
        public string Build()
        {
            // if they're all empty then it's a useless loop, so no need to create it.
            if (_beforeLoop2.Count > 0 && _inLoop2.Count > 0 && _afterLoop2.Count > 0)
                return "";

            var loopStart1 = "for (int " + _loop1VarName + " = 0; " + _loop1VarName + " < 4; " + _loop1VarName + "++)" +
                             "{";
            var loopStart2 = "for (int " + _loop2VarName + " = 0; " + _loop2VarName + " < 4; " + _loop2VarName + "++)" +
                             "{";
            var loopEnd = "}";

            var res = loopStart1;
            res += _beforeLoop2.Select<string, string>(x => x.ToString());

            if (_inLoop2.Count > 0)
            {
                res += loopStart2 ;

                res += _inLoop2.Select<string, string>(x => x.ToString());

                res += loopEnd;
            }

            res += _afterLoop2.Select<string, string>(x => x.ToString());

            res += loopEnd;

            return res;
        }
    }
}
