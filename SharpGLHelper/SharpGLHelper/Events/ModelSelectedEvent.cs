using SharpGLHelper.Common;
using SharpGLHelper.Primitives;
using SharpGLHelper.SceneElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Events
{
    // A delegate type for hooking up change notifications.
    public delegate void ModelSelectedEvent(object sender, ModelSelectedEventArgs e);
    public class ModelSelectedEventArgs : EventArgs
    {
        public Point Point { get; set; }
        //public int IdxModel { get; set; }
        //public int IdxUsedTransformation { get; set; }
        //public IEnumerable<ElementTransformations> InputModels { get; set; }

        public IEnumerable<ElementAndTransformation> InputModels { get; set; }

        public int InputModelsIndex { get; set; }

        public ElementAndTransformation SelectedModel { get; set; }

        //public ModelSelectedEventArgs(Point p, IEnumerable<ElementTransformations> models, int idxModel, int idxUsedTransformation)
        //{
        //    Point = p;
        //    InputModels = models;
        //    IdxModel = idxModel;
        //    IdxUsedTransformation = IdxUsedTransformation;
        //}

        public ModelSelectedEventArgs (Point p, IEnumerable<ElementAndTransformation> models, int inputModelsIndex)
        {
            Point = p;
            InputModels = models;
            InputModelsIndex = inputModelsIndex;
            if (inputModelsIndex > 0)
                SelectedModel = InputModels.ElementAt(inputModelsIndex);
	    }
    }
}
