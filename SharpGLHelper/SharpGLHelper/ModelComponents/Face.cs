using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    public class Face : IEquatable<Face>
    {
        #region fields
        bool _flipFace = false;
        Mesh _mesh;
        ObservableLinkedSet<Edge> _edges = new ObservableLinkedSet<Edge>();
        vec3? _normal;
        #endregion fields

        #region properties
        /// <summary>
        /// Changing this value will reverse the order of the current edge list. This will also affect ChainEdges(...).
        /// </summary>
        public bool FlipFace
        {
            get { return _flipFace; }
            set 
            { 
                if (value != _flipFace)
                {
                    _flipFace = value;
                    var reversedEdges = _edges.Reverse();
                    _edges.Clear(false);
                    _edges.AddLast(reversedEdges, false);
                }
            }
        }
        public vec3 Normal
        {
            get
            {
                if (_normal == null)
                    CalculateFaceNormal(); 
                return _normal.Value;
            }
            set 
            {
                _normal = value; 
            }
        }
        public Mesh Mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }
        public ObservableLinkedSet<Edge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        #endregion properties

        #region events
        #endregion events

        #region constructors
        private Face(Mesh parentMesh = null)
        {
            Mesh = parentMesh;
            Edges.CollectionChanged += Edges_CollectionChanged;
        }
        public Face(IEnumerable<Edge> edges,Mesh parentMesh = null)
            :this(parentMesh)
        {
            Edges.AddLast(edges);

            if (Edges.Count < 3)
                throw new ArgumentOutOfRangeException("A face must contain at least 3 different edges.");

        }

        public static List<Face> GenerateFrom(vec3[] vertices, uint[] indices, vec3[] normals, byte verticesPerFace = 3, bool removeUnusedVertices = false)
        {
            var newVertices = Vertex.GenerateFrom(vertices);
            return GenerateFrom(newVertices, indices, normals, verticesPerFace, removeUnusedVertices);
        }

        /// <summary>
        /// Converts and the returns the data as a Face[]. 
        /// You can only use this method for one face type, e.g. triangles/quads/...
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="indices">The indices</param>
        /// <param name="normals">The Normals</param>
        /// <param name="verticesPerFace">The amount of vertices that each face has.</param>
        /// <param name="removeUnusedVertices">If there are any vertices that aren't being used in the indices array, remove them.</param>
        /// <returns>The faces that were created from this data.</returns>
        public static List<Face> GenerateFrom(vec4[] vertices, uint[] indices, vec3[] normals, byte verticesPerFace = 3, bool removeUnusedVertices = false)
        {
            var newVertices = Vertex.GenerateFrom(vertices);
            return GenerateFrom(newVertices, indices, normals, verticesPerFace, removeUnusedVertices);
        }
        public static List<Face> GenerateFrom(Vertex[] vertices, uint[] indices, vec3[] normals, byte verticesPerFace = 3, bool removeUnusedVertices = false)
        {
            #region convert input data to faces
            List<Face> faces = new List<Face>();
            var newVertices = vertices.ToList();
            //try
            //{
                var tempEdges = new Edge[verticesPerFace];
                var idxPos = 0;

                // This action was to created to prevent an extra "first-loop" test and to prevent double code.
                #region idxIteration-Action
                var idxIteration = new Action<uint, Vertex, int>((idx, vert, tempEdgePos) => 
                {
                    if (tempEdgePos == verticesPerFace - 1)
                    {
                        // Final vertex for face => use first and last vertex for final edge.
                        var nextIdx = indices[idxPos - verticesPerFace + 1];
                        var firstVert = newVertices[Convert.ToInt32(nextIdx)];
                        tempEdges[tempEdgePos] = new Edge(vert, firstVert);
                    }
                    else
                    {
                        // Create edge from this and the next vertex.
                        var nextIdx = indices[idxPos + 1];
                        var nextVert = newVertices[Convert.ToInt32(nextIdx)];
                        tempEdges[tempEdgePos] = new Edge(vert, nextVert);
                    }

                    idxPos++;

                    // If index is set, then this vertex has already been handled.
                    if (vert.Index == null)
                    {
                        vert.Index = idx;
                        vert.Normal = normals != null ? normals[idx] : new vec3();
                    }

                });
                #endregion idxIteration-Action

                uint index = indices[0];
                var vertex = newVertices[Convert.ToInt32(index)];
                idxIteration(index, vertex, 0);

                foreach (var idx in indices.Skip(1))
                {
                    var vert = newVertices[Convert.ToInt32(idx)];

                    var tempEdgePos = idxPos % verticesPerFace;

                    if (tempEdgePos == 0)
                    {
                        // Create face from tempEdges.
                        faces.Add(new Face(tempEdges));
                    }

                    idxIteration(idx, vert, tempEdgePos);
                }

                // Create face from tempEdges.
                faces.Add(new Face(tempEdges));

                if (removeUnusedVertices)
                {
                    newVertices.RemoveAll(x => x.Index == null);
                }
            //}
            //catch (Exception ex)
            //{
            //    if (ex.GetType() == typeof(IndexOutOfRangeException))
            //    {
            //        var maxIdx = indices.Max();
            //        var vertLength = vertices.Length;
            //        var normalsLength = normals!= null ? normals.Length : 0;
            //        if (maxIdx >= vertices.Length)
            //            throw new IndexOutOfRangeException("Data is corrupted, " +
            //                "there seems to be indices that point to a vertex beyond the bounds of the vertex vector. \n" +
            //                "Highest index value: " + maxIdx + "\nIndex vector length: " + vertices.Length, ex);
            //    }
            //}
            #endregion convert input data to faces

            return faces;
        }
        #endregion constructors

        public Edge[] ChainEdges()
        {
            Edge[] sequencedEdges = new Edge[Edges.Count];

            #region validate if edges form a closed face.

            var linkedListEdges = new LinkedList<Edge>(); // Buffer to keep unused edges in.

            foreach (var edge in Edges)
            {
                linkedListEdges.AddLast(edge);
            }

            sequencedEdges[0] = Edges[0]; // Set the first edge.
            var lastNode1 = sequencedEdges[0]; // first direction
            var lastNode2 = sequencedEdges[0]; // second direction
            linkedListEdges.Remove(sequencedEdges[0]);

            var lastIdx1 = 0;
            var lastIdx2 = sequencedEdges.Length;
            var lastCount = linkedListEdges.Count;

            // While there still are edges in the buffer.
            while (linkedListEdges.Count > 0)
            {
                foreach (var edge in linkedListEdges)
                {
                    // Check for a equal vertex between edge and lastNode1
                    if (edge.HasEqualVertex(lastNode1) != null)
                    {
                        sequencedEdges[++lastIdx1] = edge;
                        lastNode1 = edge;
                        linkedListEdges.Remove(edge);
                        break;
                    }
                    // Check for a equal vertex between edge and lastNode2
                    else if (edge.HasEqualVertex(lastNode2) != null)
                    {
                        sequencedEdges[--lastIdx2] = edge;
                        lastNode2 = edge;
                        linkedListEdges.Remove(edge);
                        break;
                    }
                }

                // Test if we're not going into a infinite loop, and increment lastCount afterwards.
                if (lastCount-- == linkedListEdges.Count)
                {
                    throw new Exception("Face cannot be constructed. Using all edges won't form one closed shape.");
                }
            }

            // If lastNode1 and lastNode2 don't have an equal vertex, means they don't close.
            if (lastNode1.HasEqualVertex(lastNode2) == null)
            {
                throw new Exception("Face cannot be constructed. Using all edges won't form one closed shape.");
            }


            #endregion validate if edges form a closed face.


            Edges.Clear(false);
            if (FlipFace)
            {
                var reversed = sequencedEdges.Reverse(); 
                Edges.AddLast(reversed, false);
                return reversed.ToArray();
            }
            else
            {
                Edges.AddLast(sequencedEdges, false);
                return sequencedEdges;
            }
        }

        public void ValidateFace()
        {
            if (Edges.Count < 3)
                throw new Exception("Face needs to have at least 3 edges. Current edge count: " + Edges.Count);

            ChainEdges();
            
        }

        /// <summary>
        /// Calculates the face normal. This method assumes that this face is a triangle.
        /// </summary>
        public void CalculateFaceNormal()
        {
            //var v1 = _vertices[0];
            //var v2 = _vertices[1];
            //var v3 = _vertices[2];
            //var edge1 = v2.Vec3 - v1.Vec3;
            //var edge2 = v3.Vec3 - v1.Vec3;
            //var normal = glm.cross(edge1, edge2);

            var normal = new vec3();

            foreach (var edge in Edges)
            {
                edge.CalculateEdgeNormal();
                normal.x += edge.Normal.x;
                normal.y += edge.Normal.y;
                normal.z += edge.Normal.z;
            }


            if (normal.x == 0 && normal.y == 0 && normal.z == 0)
            {
                Normal = new vec3(0, 0, 0);
            }
            else
            {
                Normal = glm.normalize(normal);
            }
        }

        public HashSet<Vertex> GetUsedVertices()
        {
            var vertices = new HashSet<Vertex>();

            foreach (var edge in Edges)
            {
                vertices.Add(edge.Vertex1);
                vertices.Add(edge.Vertex2);
            }

            return vertices;
        }

        public IEnumerable<Face> Triangulate()
        {
            if (Edges.Count <= 3)
                return new Face[] { this };

            var newFaces = new List<Face>();

            var chainedEdges = ChainEdges().ToList();

            int idxUp = 0;
            int idxDown = Edges.Count - 1;

            #region local func: createAddFace
            var createAddFace = new Func<Edge, Edge, Edge>((e1, e2) =>
            {
                var newEdge = Edge.CreateThirdTriangleEdge(e1, e2);
                newFaces.Add(new Face(new Edge[] { e1, e2, newEdge }));

                chainedEdges.Remove(e1);

                return newEdge;
            });
            #endregion local func: createAddFace

            var lastNewEdge = createAddFace(chainedEdges[idxUp], chainedEdges[idxDown]);
            chainedEdges.Remove(chainedEdges[idxDown]);

            idxUp++;
            idxDown--;

            while (chainedEdges.Count > 2)
            {
                var eUp = chainedEdges[idxUp];
                lastNewEdge = createAddFace(eUp, lastNewEdge);

                if (lastNewEdge != null)
                    idxUp++;


                if (chainedEdges.Count <= 2)
                    break;


                var eDown = chainedEdges[idxDown];
                lastNewEdge = createAddFace(eDown, lastNewEdge);

                if (lastNewEdge != null)
                    idxDown--;
            }

            // Use the last 3 remaining edges to form the final face.
            newFaces.Add(new Face(new Edge[] { chainedEdges[idxUp], chainedEdges[idxDown], lastNewEdge }));
            chainedEdges.Remove(chainedEdges[idxDown]);
            chainedEdges.Remove(chainedEdges[idxUp]);

            if (chainedEdges.Count != null)
                throw new Exception("Not all chained edges were used, something went wrong.");

            return newFaces;
        }

        void Edges_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                // If edge is removed from this face, break the link between this face and the edge and it's vertices.
                foreach (Edge edge in e.OldItems)
                {
                    edge.Faces.Remove(this);
                }

            if (e.NewItems != null)
                // If edge is added to this face, let the edge and vertices know of this face.
                foreach (Edge edge in e.NewItems)
                {
                    edge.Faces.AddLast(this);
                }
        }

        /// <summary>
        /// Faces equal eachother when they contain the same edges. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Face other)
        {
            if (other == null|| other.Edges.Count != Edges.Count)
                return false;

            var edgeCount = Edges.Count;
            HashSet<Edge> edges = new HashSet<Edge>();

            foreach (var edge in Edges)
            {
                edges.Add(edge);
            }
            foreach (var edge in other.Edges)
            {
                edges.Add(edge);
            }

            return edges.Count == edgeCount;
        }

    }
}
