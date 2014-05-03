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
        bool _changesHandled = false;

        OGLVisualSceneElementBase _sceneElement;
        List<TransformationMatrix> _transformations = new List<TransformationMatrix>();
        #endregion fields


        #region properties
        public bool ChangesHandled
        {
            get { return _changesHandled || Transformations.Any(x=>x.ChangesHandled == false); }
            set 
            { 
                _changesHandled = value;
                foreach (var item in Transformations)
                {
                    item.ChangesHandled = value;
                }
            }
        }
        public OGLVisualSceneElementBase SceneElement
        {
            get { return _sceneElement; }
            set 
            { 
                _sceneElement = value;
                ChangesHandled = false;
            }
        }
        public List<TransformationMatrix> Transformations
        {
            get 
            { 
                return _transformations; 
            }
            set 
            { 
                _transformations = value;
                _changesHandled = false;
            }
        }
        #endregion properties
    }
}
