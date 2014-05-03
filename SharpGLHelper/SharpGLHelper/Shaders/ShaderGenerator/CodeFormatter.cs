using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator
{
    public class CodeFormatter
    {

        #region fields
        string _input;
        string _result;

        string _lb = Resources.ShaderFileSettings.LineBreak.Replace("\\n", "\n").Replace("\\r", "\r");
        string _t = Resources.ShaderFileSettings.Tab.Replace("\\t", "\t");
        int _tabSteps = 0;
        #endregion fields

        #region properties
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private string Br
        {
            get
            {
                string br = _lb;
                for (int i = 0; i < _tabSteps; i++)
                {
                    br += _t;
                }
                return br;
            }
        }

        #endregion properties

        #region events
        #endregion events

        #region constructors
        public CodeFormatter(string input)
        {
            _input = input;
        }
        #endregion constructors

        public string Format()
        {
            var resultBuilder = new StringBuilder();
            foreach (var chr in _input)
            {
                switch (chr)
                {
                    case ';': resultBuilder.Append(chr + Br); break;
                    case '{':
                        _tabSteps++;
                        resultBuilder.Append(chr + Br);
                        break;
                    case '}': 
                        _tabSteps--; 
                        resultBuilder.Append(Br + chr + Br); 
                        break;
                    default: resultBuilder.Append(chr); break;
                }
            }

            Result = resultBuilder.ToString();

            return Result;
        }
    }
}
