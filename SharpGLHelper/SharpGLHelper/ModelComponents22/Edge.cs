using SharpGLHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents2
{
    public class Edge
    {
        #region fields
        Vertex _vertex1, _vertex2;

        ObservableLinkedList<Face> _faces = new ObservableLinkedList<Face>();
        #endregion fields

        #region properties

        public ObservableLinkedList<Face> Faces
        {
            get { return _faces; }
            set { _faces = value; }
        }

        public Vertex Vertex1
        {
            get { return _vertex1; }
            set { _vertex1 = value; }
        }

        public Vertex Vertex2
        {
            get { return _vertex2; }
            set { _vertex2 = value; }
        }
        #endregion properties


        #region events
        public EdgeRemovedEvent EdgeRemoved;

        public void OnEdgeRemovedEvent()
        {
            if (EdgeRemoved != null)
                EdgeRemoved(this, new EdgeRemovedEventArgs());
        }
        #endregion events


        public Edge(Vertex v1, Vertex v2)
        {
            if (v1 == null || v2 == null)
                throw new ArgumentNullException("One of the 2 vertices for this edge is null.");
            else if(v1 == v2)
                throw new Exception("The edge cannot exist from the same vertices.");

            // Let the vertices know that they're being used in this edge.
            v1.Edges.AddLast(this);
            v2.Edges.AddLast(this);

            _vertex1 = v1;
            _vertex2 = v2;
        }
    }
}
