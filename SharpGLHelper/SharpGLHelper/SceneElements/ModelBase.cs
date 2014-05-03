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
using SharpGLHelper.Buffers;

namespace SharpGLHelper.SceneElements
{

    /// <summary>
    /// Provides a link between the Mesh and the OpenGL world.
    /// </summary>
    public abstract class ModelBase : OGLVisualSceneElementBase, IDisposable
    {
        
        #region fields
        private Mesh _mesh;
        private uint[] _indices;
        private vec3[] _vertices, _normals;
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

        public override uint[] Indices
        {
            get
            {
                if (_mesh == null)
                    return _indices;
                return _mesh.Indices;
            }
        }

        public override vec3[] Normals
        {
            get
            {
                if (_mesh == null)
                    return _normals;
                return _mesh.Normals;
            }
        }

        public override vec3[] Vertices
        {
            get
            {
                if (_mesh == null)
                    return _vertices;
                return _mesh.VerticesVec3;
            }
        }

        #endregion properties

        #region constructors
        public ModelBase()
        {

        }
        public ModelBase(vec3[] vertices, uint[] indices, vec3[] normals = null)
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
        public void CreateMesh(vec3[] vertices, uint[] indices, vec3[] normals = null)
        {
            _mesh = Mesh.BuildMesh(vertices, indices, normals);
            _mesh.RefreshRawData(false, false, true);
            _mesh.MeshChanged += Mesh_MeshChanged;
            VerticesCount = _mesh.VerticesVec3.Length;
            IndicesCount = _mesh.Indices.Length;
            NormalsCount = _mesh.Normals.Length;
        }

        /// <summary>
        /// Updates the buffers in GPU memory.
        /// </summary>
        /// <param name="gl">The GL.</param>
        public void UpdateGeometry(OpenGL gl)
        {
            var verts = Vertices.SelectMany(v => v.to_array()).ToArray();
            var norms = Normals.SelectMany(v => v.to_array()).ToArray();
            VertexBuffer.BindBuffer(gl);// gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBufferId.Value);
            VertexBuffer.SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, verts, Usage, 3); // gl.BufferData(OpenGL.GL_ARRAY_BUFFER, verts, (uint)Usage);
            NormalBuffer.BindBuffer(gl); // gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBufferId.Value);
            NormalBuffer.SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, norms, Usage, 3); // gl.BufferData(OpenGL.GL_ARRAY_BUFFER, norms, (uint)Usage);
        }


        /// <summary>
        /// Recalculates the normals.
        /// NOTE: this method is virtual.
        /// </summary>
        public override void CalculateNormals()
        {
            _mesh.RefreshRawData(false, true, true, false);
        }


        /// <summary>
        /// Cleans up OpenGL memory when this element is no longer needed.
        /// </summary>
        public void Dispose()
        {
            if (GL == null)
                return;

            var buffersToBeRemoved = new List<OGLBufferObject>();

            if (IndexBuffer != null)
                buffersToBeRemoved.Add(IndexBuffer);
            if (VertexBuffer != null)
                buffersToBeRemoved.Add(VertexBuffer);
            if (NormalBuffer != null)
                buffersToBeRemoved.Add(NormalBuffer);

            OGLBufferId.DeleteBuffers(GL, buffersToBeRemoved);
            //GL.DeleteBuffers(buffersToBeRemoved.Count, buffersToBeRemoved.ToArray());
        }

        /// <summary>
        /// Converts an (uint)model.VertexBuffer.VertexBufferObject to a ARGB color.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A float[4] containing RGBA values.</returns>
        public ColorF GenerateColorFromId()
        {
            if (VertexBuffer == null)
            {
                return new ColorF(0,0,0,0);
            }

            // Get the integer ID
            var i = (int)VertexBuffer.BufferId.Value;

            int b = (i >> 16) & 0xFF;
            int g = (i >> 8) & 0xFF;
            int r = i & 0xFF;

            return new ColorF(255, r, g, b);
        }

        public override void ClearStaticData()
        {
            base.ClearStaticData();

            _indices = Mesh.Indices;
            _vertices = Mesh.VerticesVec3;
            _normals = Mesh.Normals;
            Mesh = null;
        }

        /// <summary>
        /// Calls UpdateGeometry, whenever a transformation is applied to the mesh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mesh_MeshChanged(object sender, Events.MeshChangedEventArgs e)
        {
            if (GL != null)
            {
                Mesh.RefreshRawData(false, true);
                UpdateGeometry(GL);
            }
        }
    }
}
