using GlmNet;
using SharpGLHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    public class Vertex
    {
        #region fields
        ushort? _index = null;
        List<Face> _parentFaces = new List<Face>();
        vec3 _vertex;
        vec3 _normal;
        #endregion fields

        #region properties
        /// <summary>
        /// The index (= null if not set).
        /// </summary>
        public ushort? Index
        {
            get { return _index; }
            set { _index = value; }
        }
        /// <summary>
        /// The vertex normal.
        /// </summary>
        public vec3 Normal
        {
            get { return _normal; }
            set { _normal = value; }
        }
        /// <summary>
        /// The vertex.
        /// </summary>
        public vec3 Vec3
        {
            get { return _vertex; }
            set { _vertex = value; }
        }
        /// <summary>
        /// The faces that make use of this vertex.
        /// </summary>
        public List<Face> ParentFaces
        {
            get { return _parentFaces; }
            set { _parentFaces = value; }
        }
        #endregion properties

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Vertex() { }

        /// <summary>
        /// A constructor which simple sets the index, normal and vertex.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="normal">The normal.</param>
        /// <param name="vertex">The vertex.</param>
        public Vertex(ushort index, vec3 vertex, vec3 normal)
        {
            _index = index;
            _normal = normal;
            _vertex = vertex;
        }

        /// <summary>
        /// Calculates the vertex normal. (= the normalized sum of all of its parents their face normals.)
        /// </summary>
        public void CalculateVertexNormal()
        {
            vec3 normalSum = new vec3();
            foreach (var plane in _parentFaces)
            {
                normalSum += plane.Normal;
            }

            Normal = glm.normalize(normalSum);
        }

        public override string ToString()
        {
            return _vertex.ToValueString();
        }
    }
}
