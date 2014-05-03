using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpGLHelper.ModelComponents;

namespace UnitTests
{
    [TestClass]
    public class ModelComponentsMeshTest
    {
        private static Vertex[] _verts = new Vertex[]
        {
            new Vertex(1,1,1), new Vertex(1,1,0), new Vertex(1,0,1), 
            new Vertex(1,0,0), new Vertex(0,1,1), new Vertex(0,1,0)
        };

        private static Edge[] _edges;

        static ModelComponentsMeshTest()
        {
            _edges = new Edge[]
            {
                new Edge(_verts[0], _verts[1]),
                new Edge(_verts[1], _verts[2]),
                new Edge(_verts[4], _verts[5]),
                new Edge(_verts[5], _verts[0]),
                new Edge(_verts[2], _verts[3]),
                new Edge(_verts[3], _verts[4]),
            };
        }

        [TestMethod]
        public void TestEdges()
        {
            #region tests that should throw exceptions
            try
            {
                var edge = new Edge(new Vertex(0, 0, 1, 0.1f), new Vertex(0, 0, 1));
                Assert.Fail("Edge can't have 2 vertices on the same position.");
            }
            catch (Exception ex)
            {
            }
            #endregion tests that should throw exceptions

            var edge2 = new Edge(new Vertex(0, 1, 0), new Vertex(-1, 0, 1));
            Assert.IsNotNull(edge2);
        }

        [TestMethod]
        public void TestSameEdgeInFace()
        {
            var face = new Face(new Edge[] { _edges[0], _edges[1], _edges[2], _edges[3], _edges[3] });

            Assert.AreEqual(4, face.Edges.Count);
        }

        [TestMethod]
        public void TestFaceValidation()
        {
            var face = new Face(_edges);
            face.ValidateFace();
            Assert.AreEqual(_edges.Length, face.Edges.Count);
        }

        private Mesh GetSimpleTriangleMesh()
        {
            var vertices = new Vertex[]
            {
                new Vertex(1,1,1), new Vertex(0,1,1), new Vertex(0,0,1), new Vertex(0,0,1)
            };

            var edges = new Edge[]{
                new Edge(vertices[0], vertices[1]),
                new Edge(vertices[1], vertices[2]),
                new Edge(vertices[2], vertices[0])
            };

            var faces = new Face[] {
                new Face(new Edge[] {edges[0], edges[1], edges[2]})
            };

            return new Mesh(faces);
        }
    }
}
