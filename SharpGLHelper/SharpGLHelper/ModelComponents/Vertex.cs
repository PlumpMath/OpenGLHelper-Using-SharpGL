using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    public class Vertex : IEquatable<Vertex>
    {
        #region fields
        ObservableLinkedSet<Edge> _edges = new ObservableLinkedSet<Edge>();
        uint? _index = null;
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
            set
            {
                if (value != null)
                    _edges = value;
                else _edges.Clear();
            }
        }
        /// <summary>
        /// The index (= null if not set).
        /// </summary>
        public uint? Index
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
        #endregion properties

        #region constructors
        public Vertex(uint? idx = null, vec3 normal = new vec3())
        {
            Index = idx;
            Normal = normal;
        }

        public Vertex(vec4 v, uint? idx = null, vec3 normal = new vec3())
            : this(new vec3(v.x, v.y, v.z),idx, normal)
        {
        }

        public Vertex(vec3 v, uint? idx = null, vec3 normal = new vec3())
            : this(idx, normal)
        {
            Vec3 = v;
        }

        public Vertex(float x, float y, float z, uint? idx = null, vec3 normal = new vec3())
            : this(new vec3(x, y, z), idx, normal) { }
        public Vertex(float x, float y, float z, float w, uint? idx = null, vec3 normal = new vec3())
            : this(new vec4(x, y, z, w), idx, normal) { }


        public static Vertex[] GenerateFrom(IEnumerable<vec4> vertices)
        {
            var newVerts = new Vertex[vertices.Count()];

            for (int i = 0; i < vertices.Count(); i++)
            {
                newVerts[i] = new Vertex(vertices.ElementAt(i));
            }

            return newVerts;
        }
        public static Vertex[] GenerateFrom(IEnumerable<vec3> vertices)
        {
            var newVerts = new Vertex[vertices.Count()];

            for (int i = 0; i < vertices.Count(); i++)
            {
                newVerts[i] = new Vertex(vertices.ElementAt(i));
            }

            return newVerts;
        }
        #endregion constructors


        /// <summary>
        /// Calculates the vertex normal. (= the normalized sum of all of its parents their face normals.)
        /// </summary>
        public void CalculateVertexNormal()
        {
            vec3 normalSum = new vec3();

            var faces = GetParentFaces();

            foreach (var plane in faces)
            {
                normalSum += plane.Normal;
            }

            Normal = glm.normalize(normalSum);
        }

        public IEnumerable<Face> GetParentFaces()
        {
            var faces = new HashSet<Face>();

            foreach (var edge in Edges)
            {
                foreach (var face in edge.Faces)
                {
                    faces.Add(face);
                }
            }

            return faces;
        }


        public bool Equals(Vertex other)
        {
        //    if (Index != null)
        //        return Index == other.Index;

            vec3 vx = this.Vec3,
                vy = other.Vec3;
            return vx.x == vy.x && vx.y == vy.y && vx.z == vy.z;
        }
    }
}
