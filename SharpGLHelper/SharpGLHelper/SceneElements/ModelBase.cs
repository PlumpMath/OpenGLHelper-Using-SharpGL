using GlmNet;
using SharpGLHelper.Common;
using SharpGLHelper.Extensions;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGL.VertexBuffers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.Shaders;

namespace SharpGLHelper.SceneElements
{
    /// <summary>
    /// Static: GPU stores model data => increases performance for models that don't change. 
    /// Dynamic: GPU expects new data during every frame => optimized for changing models.
    /// </summary>
    public enum OGLModelUsage : uint
    {
        StaticDraw = (uint)OpenGL.GL_STATIC_DRAW,
        StaticRead = (uint)OpenGL.GL_STATIC_READ,
        StaticCopy = (uint)OpenGL.GL_STATIC_COPY,
        DynamicDraw = (uint)OpenGL.GL_DYNAMIC_DRAW,
        DynamicRead = (uint)OpenGL.GL_DYNAMIC_READ,
        DynamicCopy = (uint)OpenGL.GL_DYNAMIC_COPY,
        StreamDraw = (uint)OpenGL.GL_STREAM_DRAW,
        StreamRead = (uint)OpenGL.GL_STREAM_READ,
        StreamCopy = (uint)OpenGL.GL_STREAM_COPY,
    }

    /// <summary>
    /// Provides a link between the Mesh and the OpenGL world.
    /// </summary>
    public abstract class ModelBase : OGLVisualSceneElementBase, IDisposable
    {
        #region fields
        //private ushort[] _indices;
        private Mesh _mesh;
        #endregion fields

        #region properties
        /// <summary>
        /// Directly access the mesh data.
        /// </summary>
        public Mesh Mesh
        {
            get { return _mesh; }
            set { _mesh = value; }
        }

        public override ushort[] Indices
        {
            get
            {
                return _mesh.Indices;
            }
            set
            {
                _mesh.Indices = value;
            }
        }

        public override vec3[] Normals
        {
            get
            {
                return _mesh.Normals;
            }
            set
            {
                _mesh.Normals = value;
            }
        }

        public override vec3[] Vertices
        {
            get
            {
                return _mesh.Vec3Vertices;
            }
            set
            {
                _mesh.Vec3Vertices = value;
            }
        }
        #endregion properties

        #region constructors
        public ModelBase()
        {

        }
        public ModelBase(vec3[] vertices, ushort[] indices, vec3[] normals = null)
        {
            CreateMesh(vertices, indices, normals);
        }
        #endregion constructors

        /// <summary>
        /// Creates the mesh for this model.
        /// </summary>
        /// <param name="vertices">The vertices of the mesh.</param>
        /// <param name="indices">The indices of the mesh.</param>
        /// <param name="normals">The normals of the mesh.</param>
        public void CreateMesh(vec3[] vertices, ushort[] indices, vec3[] normals = null)
        {
            _mesh = new Mesh(indices, vertices, normals);
            _mesh.MeshPixelsChanged += Mesh_MeshPixelsChanged;
        }

        /// <summary>
        /// Updates the buffers in GPU memory.
        /// </summary>
        /// <param name="gl">The GL.</param>
        public void UpdateGeometry(OpenGL gl)
        {
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBufferId.Value);
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, Vertices.SelectMany(v => v.to_array()).ToArray(), (uint)Usage);
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBufferId.Value);
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, Normals.SelectMany(v => v.to_array()).ToArray(), (uint)Usage);
        }


        /// <summary>
        /// Recalculates the normals.
        /// NOTE: this method is virtual.
        /// </summary>
        public override void CalculateNormals()
        {
            _mesh.CalculateNormals();
        }


        /// <summary>
        /// Cleans up OpenGL memory when this element is no longer needed.
        /// </summary>
        public void Dispose()
        {
            if (GL == null)
                return;

            List<uint> buffersToBeRemoved = new List<uint>();

            if (IndexBufferId != null)
                buffersToBeRemoved.Add(IndexBufferId.Value);
            if (VertexBufferId != null)
                buffersToBeRemoved.Add(VertexBufferId.Value);
            if (NormalBufferId != null)
                buffersToBeRemoved.Add(NormalBufferId.Value);

            GL.DeleteBuffers(buffersToBeRemoved.Count, buffersToBeRemoved.ToArray());
        }

        /// <summary>
        /// Converts an (uint)model.VertexBuffer.VertexBufferObject to a ARGB color.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A float[4] containing RGBA values.</returns>
        public ColorF GenerateColorFromId()
        {
            if (VertexBufferId == null)
            {
                return new ColorF(0,0,0,0);
            }

            // Get the integer ID
            var i = (int)VertexBufferId.Value;

            int b = (i >> 16) & 0xFF;
            int g = (i >> 8) & 0xFF;
            int r = i & 0xFF;

            return new ColorF(255, r, g, b);
        }


        /// <summary>
        /// Calls UpdateGeometry, whenever a transformation is applied to the mesh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mesh_MeshPixelsChanged(object sender, Events.MeshPixelsChangedEventArgs e)
        {
            if (GL != null)
                UpdateGeometry(GL);
        }
    }
}
