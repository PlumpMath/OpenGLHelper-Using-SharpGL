using GlmNet;
using SharpGLHelper.Common;
using SharpGLHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    public enum TriangulationMethod {Strip, Fan, Delaunay }
    public class Mesh
    {
        #region events
        public event MeshPixelsChangedEvent MeshPixelsChanged;
        public void OnMeshPixelsChangedEvent()
        {
            if (MeshPixelsChanged != null)
                MeshPixelsChanged(this, new MeshPixelsChangedEventArgs());
        }
        #endregion events

        #region fields
        Face[] _faces;
        vec3[] _baseVertices, _vec3Vertices, _normals;
        ushort[] _indices;
        Vertex[] _vertices;
        Dictionary<uint, Vertex> _idxForVertex;
        #endregion fields

        #region properties
        /// <summary>
        /// The faces of the mesh.
        /// </summary>
        public Face[] Faces
        {
            get { return _faces; }
            set { _faces = value; }
        }
        /// <summary>
        /// The raw vec3 vertex data.
        /// </summary>
        public vec3[] Vec3Vertices
        {
            get { return _vec3Vertices; }
            set 
            {
                _vec3Vertices = value;
                UpdateFaceVertices();
            }
        }
        /// <summary>
        /// Each Vertex that's being used for building the faces of the mesh.
        /// </summary>
        public Vertex[] Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        public vec3[] Normals
        {
            get { return _normals; }
            set { _normals = value; }
        }
        public ushort[] Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }
        #endregion properties

        #region constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Mesh()
        {

        }

        /// <summary>
        /// Sets the faces
        /// </summary>
        /// <param name="faces">The faces.</param>
        public Mesh(Face[] faces, bool calculate)
        {
            _faces = faces;
        }

        /// <summary>
        /// Sets the Indices, Vec3Vertices and Normals. Also prepares for manipulation by virtually building each face.
        /// If normals are null, these will be automatically generated.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="normals">The normals.</param>
        public Mesh(ushort[] indices, vec3[] vertices, vec3[] normals, bool autoFillFaces = true)
        {
            _indices = indices;
            _normals = normals;
            _vec3Vertices = vertices;
            _baseVertices = vertices;

            if (autoFillFaces)
                CreateFaces();

            if (_normals == null)
            {
                CalculateNormals();
            }
        }

        #endregion constructors

        /// <summary>
        /// Fills Face[] using the available Vec3Vertices and Indices.
        /// </summary>
        public void CreateFaces()
        {
            var n = 3;
            var faces = new List<Face>();
            _idxForVertex = new Dictionary<uint, Vertex>();
            _vertices = new Vertex[_vec3Vertices.Length];

            #region getVertex(...) function
            Func<ushort, Vertex> getVertex = idx =>
            {
                if (_idxForVertex.ContainsKey(idx))
                {
                    return _idxForVertex[idx];
                }
                else
                {
                    var vert = new Vertex(idx, 
                        _vec3Vertices[idx],
                        _normals == null ?
                        new vec3()
                        : _normals[idx]);
                    _idxForVertex[idx] = vert;

                    _vertices[idx] = vert;
                    return vert;
                }
            };
            #endregion getVertex(...) function

            for (int i = 0; i < _indices.Length; i += n)
            {
                var idx0 = _indices[i];
                var idx1 = _indices[i + 1];
                var idx2 = _indices[i + 2];


                Vertex v0 = getVertex(idx0);
                Vertex v1 = getVertex(idx1);
                Vertex v2 = getVertex(idx2);


                faces.Add(new Face(new Vertex[] { v0, v1, v2 }));

            }

            _faces = faces.ToArray();
        }

        /// <summary>
        /// Calculate the normals of this Mesh.
        /// </summary>
        public void CalculateNormals()
        {
            var normals = new vec3[_vertices.Length];

            foreach (var face in _faces)
            {
                face.CalculateNormal();
            }

            foreach (var vert in _vertices)
            {
                vert.CalculateVertexNormal();
                normals[vert.Index.Value] = vert.Normal;
            }

            Normals = normals;
        }

        /// <summary>
        /// Update AllVertices by using Vec3Vertices.
        /// </summary>
        public void UpdateFaceVertices()
        {
            foreach (var idx in _indices)
            {
                Vertex vert = _idxForVertex[idx];
                vec3 vec3Vert = _vec3Vertices[idx];

                vert.Vec3 = vec3Vert;
            }
        }

        /// <summary>
        /// Update Vec3Vertices, Indices and Normals by using Vertices.
        /// </summary>
        public void UpdateRawData()
        {
            //TODO
            throw new NotImplementedException();
        }

        public void TriangulateMesh(TriangulationMethod method)
        {

        }

        public void ApplyTransformationMatrixToEachVertex(mat4 transformationMatrix)
        {
            vec3[] vertices = new vec3[Vertices.Length];
            // Apply transformations to each vertex
            for (int baseVertIdx = 0; baseVertIdx < _baseVertices.Length; baseVertIdx++)
            {
                vec3 vec3Vert = _baseVertices[baseVertIdx];
                vec4 vec4Vert = new vec4(vec3Vert.x, vec3Vert.y, vec3Vert.z, 1);

                // vec4 res = base.ResultMatrix * vec4Vert; // Apparently not supported in GlmNET, so we do it manually.
                vec4 res = new vec4();
                for (int i = 0; i < 4; i++) // i = collumn for mat4
                {
                    float val = 0;
                    for (int j = 0; j < 4; j++) // j = row for mat4 and col for vec4
                    {
                        val += vec4Vert[j] * transformationMatrix[j][i];
                    }
                    res[i] = val;
                }
                vertices[baseVertIdx] = new vec3(res[0], res[1], res[2]);
            }

            Vec3Vertices = vertices;
            //Vertices changed, so normals need to be recalculated
            CalculateNormals();

            // Trigger MeshTransformationChangedEvent
            OnMeshPixelsChangedEvent();
        }
    }
}
