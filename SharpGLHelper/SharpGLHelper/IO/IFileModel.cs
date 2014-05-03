using SharpGLHelper.Primitives;
using SharpGLHelper.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGLHelper.SceneElements;

namespace SharpGLHelper.IO
{
    public interface IFileModel
    {
        /// <summary>
        /// The vertices.
        /// </summary>
        float[] Vertices { get; set; }
        /// <summary>
        /// The indices.
        /// </summary>
        uint[] Indices { get; set; }
        /// <summary>
        /// The normals.
        /// </summary>
        float[] Normals { get; set; }
        /// <summary>
        /// The materials.
        /// </summary>
        Material[] Materials { get; set; }

        /// <summary>
        /// The model that can be displayed in the scene.
        /// </summary>
        ModelBase VisualModel { get; set; }

        /// <summary>
        /// Loads all the data available in the file.
        /// </summary>
        /// <param name="path">The path on the file system.</param>
        /// <returns></returns>
        //IFileModel LoadModelFromFile(string path);

        /// <summary>
        /// Generates a Model3DBase from this model and saves it to the "VisualModel" property.
        /// </summary>
        void GenerateVisualModel();
    }
}
