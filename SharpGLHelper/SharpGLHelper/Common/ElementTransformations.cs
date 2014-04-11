using SharpGLHelper.SceneElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Common
{
    public class ElementTransformations
    {
        #region fields
        OGLSceneElementBase _sceneElement;
        List<TransformationMatrix> _transformations = new List<TransformationMatrix>();
        #endregion fields


        #region properties
        public OGLSceneElementBase SceneElement
        {
            get { return _sceneElement; }
            set { _sceneElement = value; }
        }
        public List<TransformationMatrix> Transformations
        {
            get { return _transformations; }
            set { _transformations = value; }
        }
        #endregion properties
    }
}
