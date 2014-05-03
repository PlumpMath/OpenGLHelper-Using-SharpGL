using GlmNet;
using SharpGLHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    public class Mesh
    {
        #region fields
        ObservableLinkedList<Vertex> _vertices = new ObservableLinkedList<Vertex>(); // all verts
        ObservableLinkedList<Edge> _edges = new ObservableLinkedList<Edge>(); // all edges
        ObservableLinkedList<Face> _faces = new ObservableLinkedList<Face>(); // all faces
        #endregion fields

        #region properties
        public vec3[] VerticesVec3 { get; private set; }
        public vec3[] Normals { get; private set; }
        public uint[] Indices { get; private set; }
        public ObservableLinkedList<Vertex> Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        public ObservableLinkedList<Edge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        public ObservableLinkedList<Face> Faces
        {
            get { return _faces; }
            set 
            {
                if (value == null)
                    _faces.Clear();
                else _faces = value; 
            }
        }
        #endregion properties

        #region events
        public MeshChangedEvent MeshChanged;

        private void OnMeshChanged()
        {
            if (MeshChanged != null)
                MeshChanged(this, new MeshChangedEventArgs());
        }
        #endregion events

        #region constructors
        public Mesh(IEnumerable<Face> faces)
        {
            Faces.CollectionChanged += Faces_CollectionChanged;
            Faces.AddLast(faces);

            AddEdgesAndVerticesFrom(Faces);
        }

        /// <summary>
        /// Converts the data to a Face[] and creates- and returns a Mesh from these faces. 
        /// You can only use this method for one face type, e.g. triangles/quads/...
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="indices">The indices</param>
        /// <param name="normals">The Normals</param>
        /// <param name="verticesPerFace">The amount of vertices that each face has.</param>
        /// <param name="removeUnusedVertices">If there are any vertices that aren't being used in the indices array, remove them.</param>
        /// <returns>The mesh that was created from this data.</returns>
        public static Mesh BuildMesh(vec4[] vertices, uint[] indices, vec3[] normals, byte verticesPerFace = 3, bool removeUnusedVertices = false)
        {
            return new Mesh(Face.GenerateFrom(vertices, indices, normals, verticesPerFace, removeUnusedVertices));
        }
        public static Mesh BuildMesh(vec3[] vertices, uint[] indices, vec3[] normals, byte verticesPerFace = 3, bool removeUnusedVertices = false)
        {
            return new Mesh(Face.GenerateFrom(vertices, indices, normals, verticesPerFace, removeUnusedVertices));
        }
        #endregion constructors

        void Faces_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newFaces = new List<Face>();

            foreach (Face face in e.NewItems)
	        {
                newFaces.Add(face);
                face.Mesh = this;
	        }

            AddEdgesAndVerticesFrom(newFaces);
            OnMeshChanged();
        }

        private void AddEdgesAndVerticesFrom(IEnumerable<Face> faces)
        {
            var verticesSet = new HashSet<Vertex>();
            foreach (var face in faces)
            {
                Edges.AddLast(face.Edges);
                foreach (var edge in face.Edges)
                {
                    verticesSet.Add(edge.Vertex1);
                    verticesSet.Add(edge.Vertex2);
                }
            }
            Vertices.AddLast(verticesSet);
        }

        public void CalculateNormals(bool refreshEdgesVertices = true)
        {
            foreach (var face in Faces)
            {
                face.CalculateFaceNormal();
            }

            RefreshEdgesVertices();

            foreach (var vertex in Vertices)
            {
                vertex.CalculateVertexNormal();
            }



            OnMeshChanged();
        }

        public void RefreshEdgesVertices()
        {
            Vertices.Clear();
            Edges.Clear();
            AddEdgesAndVerticesFrom(Faces);
        }

        public void Triangulate()
        {
            var oldFaces = Faces;
            foreach (var face in oldFaces)
            {
                Faces.Remove(face);
                Faces.AddLast(face.Triangulate());
            }
            OnMeshChanged();
        }

        public void RefreshRawData(bool recalculateIndices, bool refreshEdgesVertices, bool recalculateNormals = false, bool clockwise = true)
        {
            #region refresh data
            if (refreshEdgesVertices)
            {
                RefreshEdgesVertices();
            }
            #endregion refresh data

            if (recalculateNormals)
                CalculateNormals();

            vec3[] verts;
            vec3[] normals;
            uint[] indices;
            if (recalculateIndices)
            {
                #region recalculate indices
                verts = new vec3[Vertices.Count];
                normals = new vec3[Vertices.Count];
                var tempIndices = new List<uint>();

                var vertIdxMap = new Dictionary<Vertex, uint>();

                for (uint i = 0; i < Vertices.Count; i++)
                {
                    var vert = Vertices[Convert.ToInt32(i)];
                    verts[i] = vert.Vec3;
                    normals[i] = vert.Normal;
                    vertIdxMap.Add(vert, i);
                }

                foreach (var face in Faces)
                {
                    var usedVerts = face.GetUsedVertices();
                    var idxs = new uint[]{
                        vertIdxMap[usedVerts.ElementAt(0)],
                        vertIdxMap[usedVerts.ElementAt(1)],
                        vertIdxMap[usedVerts.ElementAt(2)],
                    };

                    //TODO: clockwise, counterclockwise

                    tempIndices.AddRange(idxs);
                }

                indices = tempIndices.ToArray();
                #endregion recalculate indices
            }
            else
            {
                #region reuse current indices
                var highestIdx = Vertices.Max(x => x.Index);
                verts = new vec3[highestIdx.Value + 1];
                normals = new vec3[highestIdx.Value + 1];
                indices = new uint[Faces.Count * 3];

                var idxPos = 0;
                foreach (var face in Faces)
                {
                    var usedVerts = face.GetUsedVertices();
                    var vert1 = usedVerts.ElementAt(0);
                    var vert2 = usedVerts.ElementAt(1);
                    var vert3 = usedVerts.ElementAt(2);

                    var idx1 = vert1.Index.Value;
                    var idx2 = vert2.Index.Value;
                    var idx3 = vert3.Index.Value;

                    indices[idxPos] = idx1;
                    indices[idxPos + 1] = idx2;
                    indices[idxPos + 2] = idx3;

                    if (verts[idx1].Equals(new vec3()))
                    {
                        verts[idx1] = vert1.Vec3;
                        normals[idx1] = vert1.Normal;
                    }
                    if (verts[idx2].Equals(new vec3()))
                    {
                        verts[idx2] = vert2.Vec3;
                        normals[idx2] = vert2.Normal;
                    }
                    if (verts[idx3].Equals(new vec3()))
                    {
                        verts[idx3] = vert3.Vec3;
                        normals[idx3] = vert3.Normal;
                    }

                    idxPos += 3;
                }
                
                #endregion reuse current indices
            }

            VerticesVec3 = verts;
            Normals = normals;
            Indices = indices;
        }

        public static Mesh MergeData(Mesh m1, Mesh m2)
        {
            var deepCopy1 = Mesh.BuildMesh(m1.VerticesVec3, m1.Indices, m1.Normals);
            var deepCopy2 = Mesh.BuildMesh(m2.VerticesVec3, m2.Indices, m2.Normals);

            var faces1 = deepCopy1.Faces;
            var faces2 = deepCopy2.Faces;
            
            deepCopy1.RefreshEdgesVertices();
            deepCopy2.RefreshEdgesVertices();

            var copy1VertCount = deepCopy1.Vertices.Count;
            foreach (var Vertex in deepCopy2.Vertices)
            {
                Vertex.Index += (uint)copy1VertCount;
            }

            var newFaces = new List<Face>();
            newFaces.AddRange(faces1);
            newFaces.AddRange(faces2);

            var mesh = new Mesh(newFaces);
            mesh.RefreshRawData(false, true);

            return mesh;
        }
    }
}
