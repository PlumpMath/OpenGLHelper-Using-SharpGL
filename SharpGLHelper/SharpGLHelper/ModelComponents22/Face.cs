using GlmNet;
using SharpGLHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents2
{
    /// <summary>
    /// Contains the data that makes up a simple 3D face. Includes it's vertices, face normal and face material (if available).
    /// </summary>
    public class Face: MaterialContainableBase
    {
        #region fields
        ObservableLinkedSet<Vertex> _vertices = new ObservableLinkedSet<Vertex>();
        ObservableLinkedSet<Edge> _edges = new ObservableLinkedSet<Edge>();
        vec3 _normal;
        Material _material = null;
        #endregion fields

        #region properties

        public ObservableLinkedSet<Edge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }
        /// <summary>
        /// The vertices that this face is build from.
        /// </summary>
        public ObservableLinkedSet<Vertex> Vertices
        {
            get { return _vertices; }
        }
        /// <summary>
        /// The face normal.
        /// </summary>
        public vec3 Normal
        {
            get { return _normal; }
            set { _normal = value; }
        }
        /// <summary>
        /// The material for this face.
        /// </summary>
        public Material Material
        {
            get { return _material; }
            set { _material = value; }
        }
        #endregion properties

        #region events
        public FaceRemovedEvent FaceRemoved;

        public void OnFaceRemovedEvent()
        {
            if (FaceRemoved != null)
                FaceRemoved(this, new FaceRemovedEventArgs());
        }
        #endregion events

        public Face(Edge[] edges)
        {
            if (edges.Length < 3)
                throw new Exception("A face must have 3 edges.");

            #region validate for equal edges
            #endregion validate for equal edges
            

            Edge[] sequencedEdges = new Edge[edges.Length];

            #region validate if edges form a closed face.

            var linkedListEdges = new LinkedList<Edge>(); // Buffer to keep unused edges in.

            foreach (var edge in edges)
            {
                linkedListEdges.AddLast(edge);
            }

            sequencedEdges[0] = edges[0]; // Set the first edge.
            var lastNode1 = sequencedEdges[0]; // first direction
            var lastNode2 = sequencedEdges[0]; // second direction
            
            var lastIdx1 = 0;
            var lastIdx2 = sequencedEdges.Length;
            var lastCount = linkedListEdges.Count;

            // While there still are edges in the buffer.
            while (linkedListEdges.Count > 0)
            {
                foreach (var edge in linkedListEdges)
                {
                    // Check for a equal vertex between edge and lastNode1
                    if (edge.Vertex1 == lastNode1.Vertex1 
                        || edge.Vertex2 == lastNode1.Vertex1
                        || edge.Vertex1 == lastNode1.Vertex2
                        || edge.Vertex2 == lastNode1.Vertex2)
                    {
                        sequencedEdges[++lastIdx1] = edge;
                        lastNode1 = edge;
                        linkedListEdges.Remove(edge);
                        break;
                    }
                    // Check for a equal vertex between edge and lastNode2
                    else if (edge.Vertex1 == lastNode2.Vertex1
                        || edge.Vertex2 == lastNode2.Vertex1
                        || edge.Vertex1 == lastNode2.Vertex2
                        || edge.Vertex2 == lastNode2.Vertex2)
                    {
                        sequencedEdges[--lastIdx2] = edge;
                        lastNode2 = edge;
                        linkedListEdges.Remove(edge);
                        break;
                    }
                }

                // Test if we're not going into a infinite loop, and increment lastCount afterwards.
                if (lastCount++ == linkedListEdges.Count)
                {
                    throw new Exception("Face cannot be constructed. Edges don't form a closed shape.");
                }
            }
            #endregion validate if edges form a closed face.


            #region let the edges and vertices know which faces are using them
            foreach (var edge in sequencedEdges)
            {
                edge.Faces.AddLast(this);
            }
            foreach (var vert in Vertices)
            {
                vert.Faces.AddLast(this);
            }
            #endregion let the edges and vertices know which faces are using them

            _edges.AddLast(sequencedEdges);
        }

        /// <summary>
        /// A simple constructor which sets it's vertices and adds itself as a parent for the vertices.
        /// </summary>
        /// <param name="vertices">The vertices. </param>
        //public Face(Vertex[] vertices)
        //{
        //    _vertices = vertices;

        //    // Let the vertices know which faces are using them.
        //    foreach (var vertex in vertices)
        //    {
        //        vertex.Faces.AddLast(this); // Establish many-to-many relationship.   
        //    }

        //    // Let the edges know which faces are using them.
        //}

        /// <summary>
        /// Calculates the face normal. This method assumes that this face is a triangle.
        /// </summary>
        public void CalculateNormal()
        {
            var v1 = _vertices[0];
            var v2 = _vertices[1];
            var v3 = _vertices[2];
            var edge1 = v2.Vec3-v1.Vec3;
            var edge2 = v3.Vec3-v1.Vec3;
            var normal = glm.cross(edge1, edge2);

            //for (int i = 0; i < _vertices.Length - 1; i++)
            //{
            //    vec3 curVert = _vertices[i].Vec3;
            //    vec3 nextVert = _vertices[(i + 1) % _vertices.Length].Vec3;

            //    normal.x += (curVert.y - nextVert.y) * (curVert.z + nextVert.z);
            //    normal.y += (curVert.z - nextVert.z) * (curVert.x + nextVert.x);
            //    normal.z += (curVert.x - nextVert.x) * (curVert.y + nextVert.y);
            //}

            
            if (normal.x == 0 && normal.y == 0 && normal.z ==0)
            {
                Normal = new vec3(0, 0, 0);
            }
            else
            {
                normal.z = -normal.z;
                normal.x = -normal.x;
                normal.y = -normal.y;
                Normal = glm.normalize(normal);
            }
        }

        /// <summary>
        /// Returns an array of triangle faces that are generated from this face.
        /// </summary>
        /// <returns></returns>
        public Face[] TriangulateFace()
        {
            var vertexCount = _vertices.Count;

            // It's already a triangle, so no need to throw an algorithm at it.
            if (vertexCount <= 3) 
                return null;
            else if (vertexCount == 4)
            {
                var startVert = _vertices[0];

                foreach (var v in startVert.Edges)
                {
                    
                }

                Face f1, f2;
            }



            throw new NotImplementedException();
        }
    }
}
