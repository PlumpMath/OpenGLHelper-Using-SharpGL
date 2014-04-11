using SharpGLHelper.SceneElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Common
{
    public class ElementAndTransformation
    {
        #region fields
        OGLSceneElementBase _sceneElement;
        TransformationMatrix _transformation;
        #endregion fields


        #region properties
        public OGLSceneElementBase SceneElement
        {
            get { return _sceneElement; }
            set { _sceneElement = value; }
        }
        public TransformationMatrix Transformation
        {
            get { return _transformation; }
            set { _transformation= value; }
        }
        #endregion properties

        public ElementAndTransformation(OGLSceneElementBase sceneElement, TransformationMatrix transformation)
        {
            SceneElement = sceneElement;
            Transformation = transformation;
        }
    }
}
