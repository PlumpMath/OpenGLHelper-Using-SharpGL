using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    public class Edge : IEquatable<Edge>
    {
        #region fields
        ObservableLinkedSet<Face> _faces = new ObservableLinkedSet<Face>(); // Edge can be part of multiple faces.
        Vertex _vertex1, _vertex2; // Edge needs 2 vertices.
        vec3? _normal;
        #endregion fields

        #region properties
        public vec3 Normal
        {
            get
            {
                if (_normal == null)
                    CalculateEdgeNormal();
                return _normal.Value;
            }
            set
            {
                _normal = value;
            }
        }
        public Vertex Vertex1
        {
            get { return _vertex1; }
            set
            {
                ValidateEdge(value, _vertex2);
                _vertex1 = value; 
            }
        }

        public Vertex Vertex2
        {
            get { return _vertex2; }
            set
            {
                ValidateEdge(value, _vertex1);
                _vertex2 = value; 
            }
        }

        public ObservableLinkedSet<Face> Faces
        {
            get { return _faces; }
            set 
            { 
                if (value != null) 
                    _faces = value; 
                else _faces.Clear(); 
            }
        }
        #endregion properties

        #region constructors
        public Edge(Vertex v1, Vertex v2, IEnumerable<Face> users = null)
        {
            ValidateEdge(v1, v2);

            _vertex1 = v1;
            _vertex2 = v2;

            v1.Edges.AddLast(this);
            v2.Edges.AddLast(this);

            if(users != null)
                Faces.AddLast(users);
        }
        #endregion constructors

        public static void ValidateEdge(Vertex v1, Vertex v2)
        {
            if (v1.Equals(v2))
                throw new Exception("The vertices are on the same location in the 3D space, an edge can't be formed.");

            if (v1 == null || v2 == null)
                throw new ArgumentNullException("Vertex of edge cannot be null.");
        }

        /// <summary>
        /// Checks if both edges contain the same vertices.
        /// </summary>
        /// <param name="other">The other Edge.</param>
        /// <returns>(this.Vertex1 == other.Vertex1 && this.Vertex2 == other.Vertex2) || (this.Vertex1 == other.Vertex2 && this.Vertex2 == other.Vertex1)</returns>
        public bool Equals(Edge other)
        {
            return (Vertex1 == other.Vertex1 && Vertex2 == other.Vertex2) || (Vertex1 == other.Vertex2 && Vertex2 == other.Vertex1);
        }

        public void SwapVertices()
        {
            var tempV1 = Vertex1;
            _vertex1 = Vertex2;
            _vertex2 = tempV1;
        }

        public Vertex HasEqualVertex(Edge other)
        {
            return Vertex1 == other.Vertex1 || Vertex1 == other.Vertex2 ? Vertex1 : 
                Vertex2 == other.Vertex1 || Vertex2 == other.Vertex2 ? Vertex2 : null;
        }

        public void CalculateEdgeNormal()
        {
            var normal = new vec3();

            normal.x -= (Vertex1.Vec3.y - Vertex2.Vec3.y) * (Vertex1.Vec3.z + Vertex2.Vec3.z);
            normal.y -= (Vertex1.Vec3.z - Vertex2.Vec3.z) * (Vertex1.Vec3.x + Vertex2.Vec3.x);
            normal.z -= (Vertex1.Vec3.x - Vertex2.Vec3.x) * (Vertex1.Vec3.y + Vertex2.Vec3.y);

            Normal = normal;// glm.normalize(normal);
        }


        public static Edge CreateThirdTriangleEdge(Edge e1, Edge e2)
        {
            if (e1.Vertex1 == e2.Vertex1)
            {
                return new Edge(e1.Vertex2, e2.Vertex2);
            }
            else if (e1.Vertex1 == e2.Vertex2)
            {
                return new Edge(e1.Vertex2, e2.Vertex1);
            }
            else if (e1.Vertex2 == e2.Vertex2)
            {
                return new Edge(e1.Vertex1, e2.Vertex1);
            }
            else if (e1.Vertex2 == e2.Vertex1)
            {
                return new Edge(e1.Vertex1, e2.Vertex2);
            }

            // No equal vertices found.
            return null;
        }
    }
}
