using GlmNet;
using SharpGLHelper.Events;
using SharpGLHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents2
{
    public class Vertex
    {
        #region fields
        ObservableLinkedSet<Face> _faces = new ObservableLinkedSet<Face>();
        ObservableLinkedSet<Edge> _edges = new ObservableLinkedSet<Edge>();
        ushort? _index = null;
        vec3 _vertex;
        vec3 _normal;
        #endregion fields

        #region properties
        /// <summary>
        /// The edges that use this vertex.
        /// </summary>
        public ObservableLinkedSet<Edge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }
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
        public ObservableLinkedSet<Face> Faces
        {
            get { return _faces; }
            set { _faces = value; }
        }
        #endregion properties

        #region events
        public VertexRemovedEvent VertexRemoved;

        public void OnVertexRemovedEvent()
        {
            if (VertexRemoved != null)
                VertexRemoved(this, new VertexRemovedEventArgs());
        }
        #endregion events

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
            foreach (var plane in _faces)
            {
                normalSum += plane.Normal;
            }

            Normal = glm.normalize(normalSum);
        }

        public override string ToString()
        {
            return _vertex.ToValueString();
        }

        /// <summary>
        /// Remove this vertex from every edge and face that uses it.
        /// </summary>
        public void Remove()
        {
            OnVertexRemovedEvent();
        }
    }
}
