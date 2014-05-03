using SharpGLHelper.Shaders.ShaderGenerator.BuildLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.BuildOptions
{
    public class VertexNormalBuilder:IBuilder
    {
        #region fields
        bool _applyNormals,
            _applyTransformations;
        #endregion fields

        #region properties
        public bool ApplyNormals
        {
            get { return _applyNormals; }
            set { _applyNormals = value; }
        }

        public bool ApplyTransformations
        {
            get { return _applyTransformations; }
            set 
            {
                if (value)
                    ApplyNormals = true;
                _applyTransformations = value; 
            }
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors
        #endregion constructors


        public void Build()
        {
        }
    }
}
