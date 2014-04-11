using GlmNet;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGLHelper.Common;
using SharpGLHelper.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.SceneElements
{
    /// <summary>
    /// A class that provides a base for every visible element.
    /// </summary>
    public abstract class OGLVisualSceneElementBase : OGLSceneElementBase
    {
        #region fields
        private int _bufferStride = 3;
        private vec3[] _vertices, _normals;
        private ushort[] _indices;
        private uint? _vertexBufferId, _normalBufferId, _indexBufferId;
        private OpenGL _gl;
        private bool _visible = true;
        private Material _material = null;
        private OGLModelUsage _usage;
        private bool _autoCalculateNormals = true;
        private uint _glDrawMode = OpenGL.GL_TRIANGLES;
        #endregion fields

        #region properties

        /// <summary>
        /// The stride between 
        /// </summary>
        public int BufferStride
        {
            get { return _bufferStride; }
            set { _bufferStride = value; }
        }

        /// <summary>
        /// The mode that this model should be drawn with (GL_LINES, GL_TRIANGLES, GL_QUADS,...).
        /// </summary>
        public uint GlDrawMode
        {
            get { return _glDrawMode; }
            set { _glDrawMode = value; }
        }
        /// <summary>
        /// Checks whether this model should be drawn during the next draw-call. Implement this in your Render-method.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// The material that has to be applied to this model.
        /// </summary>
        public Material Material
        {
            get { return _material; }
            set { _material = value; }
        }
        /// <summary>
        /// The normal buffer for binding with the GL.
        /// </summary>
        public virtual uint? NormalBufferId
        {
            get { return _normalBufferId; }
            set { _normalBufferId = value; }
        }
        /// <summary>
        /// The vertex buffer for binding with the GL.
        /// </summary>
        public uint? VertexBufferId
        {
            get { return _vertexBufferId; }
            set { _vertexBufferId = value; }
        }
        /// <summary>
        /// The index buffer for binding with the GL.
        /// </summary>
        public uint? IndexBufferId
        {
            get { return _indexBufferId; }
            set { _indexBufferId = value; }
        }
        /// <summary>
        /// The opengl instance.
        /// </summary>
        public OpenGL GL
        {
            get { return _gl; }
            private set { _gl = value; }
        }

        /// <summary>
        /// This indices for this model. Used for pointing to a position in the Normals- and Vertices arrays.
        /// </summary>
        public virtual ushort[] Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }
        /// <summary>
        /// The normals for this model. Primarily used for lighting calculations by telling the GL in which direction the surface is facing.
        /// </summary>
        public virtual vec3[] Normals
        {
            get { return _normals; }
            set { _normals = value; }
        }
        /// <summary>
        /// The vertices for this model. 
        /// </summary>
        public virtual vec3[] Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }
        /// <summary>
        /// Checks if the normals can be automatically calculated if they're unavailable or after a transformation.
        /// </summary>
        public bool AutoCalculateNormals
        {
            get { return _autoCalculateNormals; }
            set { _autoCalculateNormals = value; }
        }
        /// <summary>
        /// Defines how the model will be used. (Frequency of updating vertex data)
        /// </summary>
        public OGLModelUsage Usage
        {
            get { return _usage; }
            protected set { _usage = value; }
        }
        #endregion properties

        public abstract void Render(OpenGL gl, RenderMode renderMode, Shaders.ExtShaderProgram shader = null);

        /// <summary>
        /// Generates the vertices, normals and indices and creates them for the OpenGL.
        /// This method has to be called once before drawing. 
        /// </summary>
        /// <param name="gl"></param>
        public void GenerateGeometry(OpenGL gl, OGLModelUsage usage)
        {
            GL = gl;
            _usage = usage;

            // Create the data buffers.
            IndexBufferId = CreateBufferId(gl);
            VertexBufferId = CreateBufferId(gl);
            NormalBufferId = CreateBufferId(gl);

            if (AutoCalculateNormals)
            {
                CalculateNormals();
            }

            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBufferId.Value);
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, Vertices.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBufferId.Value);
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, Normals.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
            GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, IndexBufferId.Value);
            GL.BufferData(OpenGL.GL_ARRAY_BUFFER, Indices, (uint)usage);
        }

        /// <summary>
        /// Recalculates the normals.
        /// NOTE: this method is virtual and does nothing by default.
        /// </summary>
        public virtual void CalculateNormals()
        {
        }


        public static uint CreateBufferId(OpenGL gl)
        {
            //  Generate the vertex array.
            uint[] ids = new uint[1];
            gl.GenBuffers(1, ids);
            return ids[0];
        }

        /// <summary>
        /// Calls VertexBuffer.Bind(gl), IndexBuffer.Bind(gl) and Material.Bind(gl). 
        /// </summary>
        /// <param name="gl">The OpenGL</param>
        public void Bind()
        {
            if (GL == null)
            {
                throw new ArgumentNullException("OpenGL parameter cannot be null. Call 'GenerateGeomerty(...)' before attempting to bind.");
            }

            // Bind the vertex, normal and index buffers.
            if (VertexBufferId != null)
            {
                //Bind
                GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBufferId.Value);
                GL.VertexAttribPointer(VertexAttributes.Position, BufferStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
                GL.EnableVertexAttribArray(VertexAttributes.Position);
            }

            if (NormalBufferId != null)
            {
                //Bind
                GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBufferId.Value);
                GL.VertexAttribPointer(VertexAttributes.Normal, BufferStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
                GL.EnableVertexAttribArray(VertexAttributes.Normal);
            }

            if (IndexBufferId != null)
            {
                GL.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, IndexBufferId.Value);
            }
        }



        /// <summary>
        /// Generates and draws the model from scratch for the given GL. Do NOT use this method for each draw-call. 
        /// This method should only be used for drawing a model once.
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="model"></param>
        public static void GenerateAndDrawOnce(OpenGL gl, OGLVisualSceneElementBase model)
        {
            var verts = model.Vertices.SelectMany(v => v.to_array()).ToArray();
            var normals = model.Normals.SelectMany(v => v.to_array()).ToArray();
            var indices = model.Indices;
            var drawMode = model.GlDrawMode;

            var usage = OGLModelUsage.StaticRead;

            // Create the data buffers.
            var indexBufferId = CreateBufferId(gl);
            var vertexBufferId = CreateBufferId(gl);
            var normalBufferId = CreateBufferId(gl);

            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vertexBufferId);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, model.Vertices.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, normalBufferId);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, model.Normals.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, indexBufferId);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, model.Indices, (uint)usage);

            gl.FrontFace(OpenGL.GL_CW);

            // Draw the elements.
            gl.DrawElements(drawMode, indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);


            // Clean up
            List<uint> buffersToBeRemoved = new List<uint>();

            if (model.IndexBufferId != null)
                buffersToBeRemoved.Add(model.IndexBufferId.Value);
            if (model.VertexBufferId != null)
                buffersToBeRemoved.Add(model.VertexBufferId.Value);
            if (model.NormalBufferId != null)
                buffersToBeRemoved.Add(model.NormalBufferId.Value);

            gl.DeleteBuffers(buffersToBeRemoved.Count, buffersToBeRemoved.ToArray());
        }


    }
}
