using GlmNet;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGLHelper.Buffers;
using SharpGLHelper.Common;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.Shaders;
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
        private uint[] _indices;
        private VBO _vertexBuffer, _normalBuffer;
        private IBO _indexBuffer;
        private OpenGL _gl;
        private bool _visible = true;
        private Material _material = null;
        private OGLModelUsage _usage;
        private bool _autoCalculateNormals = true;
        private uint _glDrawMode = OpenGL.GL_TRIANGLES;

        private long _verticesCount, _indicesCount, _normalsCount;
        #endregion fields

        #region properties
        /// <summary>
        /// The amount of vertices that are contained in the buffer.
        /// </summary>
        public virtual long VerticesCount
        {
            get { return _verticesCount; }
            set { _verticesCount = value; }
        }

        /// <summary>
        /// The amount of indices that are contained in the buffer.
        /// </summary>
        public virtual long IndicesCount
        {
            get { return _indicesCount; }
            set { _indicesCount = value; }
        }

        /// <summary>
        /// The amount of normals that are contained in the buffer.
        /// </summary>
        public virtual long NormalsCount
        {
            get { return _normalsCount; }
            set { _normalsCount = value; }
        }

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
        public virtual VBO NormalBuffer
        {
            get { return _normalBuffer; }
            set { _normalBuffer = value; }
        }
        /// <summary>
        /// The vertex buffer for binding with the GL.
        /// </summary>
        public VBO VertexBuffer
        {
            get { return _vertexBuffer; }
            set { _vertexBuffer = value; }
        }
        /// <summary>
        /// The index buffer for binding with the GL.
        /// </summary>
        public IBO IndexBuffer
        {
            get { return _indexBuffer; }
            set { _indexBuffer = value; }
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
        public virtual uint[] Indices
        {
            get { return _indices; }
            set 
            { 
                _indices = value;
                if (value != null)
                    _indicesCount = value.Length;
            }
        }
        /// <summary>
        /// The normals for this model. Primarily used for lighting calculations by telling the GL in which direction the surface is facing.
        /// </summary>
        public virtual vec3[] Normals
        {
            get { return _normals; }
            set
            {
                _normals = value;
                if (value != null)
                    _normalsCount = value.Length;
            }
        }
        /// <summary>
        /// The vertices for this model. 
        /// </summary>
        public virtual vec3[] Vertices
        {
            get { return _vertices; }
            set
            {
                _vertices = value;
                if (value != null)
                    _verticesCount = value.Length;
            }
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

        //public abstract void Render(OpenGL gl, Shaders.ShaderManagerBase shader = null);

        /// <summary>
        /// Clear all static data that can be stored by the GPU. All static data-dependant methods will throw exceptions after this.
        /// It's recommended to call this after a static element has been created and all it's raw data is sent to the GPU. 
        /// </summary>
        public virtual void ClearStaticData()
        {
        }

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
            var buffers = OGLBufferId.CreateBufferIds(gl, 3);
            IndexBuffer = new IBO(buffers[0]);
            NormalBuffer = new VBO(buffers[1]);
            VertexBuffer = new VBO(buffers[2]);

            if (AutoCalculateNormals)
            {
                CalculateNormals();
            }

            var vertData = Vertices.SelectMany(v => v.to_array()).ToArray();
            VertexBuffer.BindBuffer(gl); // GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBuffer.Buffer.Value);
            VertexBuffer.SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, vertData, usage, 3); // GL.BufferData(OpenGL.GL_ARRAY_BUFFER, vertData, (uint)usage);

            var normData = Normals.SelectMany(v => v.to_array()).ToArray();
            NormalBuffer.BindBuffer(gl); //GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBuffer.Buffer.Value);
            NormalBuffer.SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, normData, usage, 3); // GL.BufferData(OpenGL.GL_ARRAY_BUFFER, normData, (uint)usage);

            IndexBuffer.BindBuffer(gl); // GL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, IndexBuffer.Buffer.Value);
            IndexBuffer.SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, Indices, usage, 1); // GL.BufferData(OpenGL.GL_ARRAY_BUFFER, Indices, (uint)usage);

            if (new OGLModelUsage[]{OGLModelUsage.StaticCopy, OGLModelUsage.StaticDraw, OGLModelUsage.StaticRead}.Contains(usage))
            {
                ClearStaticData();
            }
        }

        /// <summary>
        /// Recalculates the normals.
        /// NOTE: this method is virtual and does nothing by default.
        /// </summary>
        public virtual void CalculateNormals()
        {
        }

        /// <summary>
        /// Calls VertexBuffer.Bind(gl), IndexBuffer.Bind(gl) and Material.Bind(gl). 
        /// </summary>
        /// <param name="gl">The OpenGL</param>
        public void Bind(OpenGL gl)
        {
            //if (gl == null)
            //{
            //    throw new ArgumentNullException("OpenGL parameter cannot be null. Call 'GenerateGeometry(...)' before attempting to bind.");
            //}

            // Bind the vertex, normal and index buffers.
            if (VertexBuffer != null)
            {
                //Bind
                //VertexBuffer.BindBuffer(gl); // 
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBuffer.BufferId.Value);
                gl.VertexAttribPointer(VertexAttributes.Position, BufferStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
                gl.EnableVertexAttribArray(VertexAttributes.Position);
            }

            if (NormalBuffer != null)
            {
                //Bind
                //NormalBuffer.BindBuffer(gl); // 
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBuffer.BufferId.Value);
                gl.VertexAttribPointer(VertexAttributes.Normal, BufferStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
                gl.EnableVertexAttribArray(VertexAttributes.Normal);
            }

            if (IndexBuffer != null)
            {
                //IndexBuffer.BindBuffer(gl); // 
                gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, IndexBuffer.BufferId.Value);
            }
        }



        ///// <summary>
        ///// Generates and draws the model from scratch for the given GL. Do NOT use this method for each draw-call. 
        ///// This method should only be used for drawing a model once.
        ///// </summary>
        ///// <param name="gl"></param>
        ///// <param name="model"></param>
        //public static List<uint> GenerateAndDrawOnce(OpenGL gl, OGLVisualSceneElementBase model)
        //{
        //    var verts = model.Vertices.SelectMany(v => v.to_array()).ToArray();
        //    var normals = model.Normals.SelectMany(v => v.to_array()).ToArray();
        //    var indices = model.Indices;
        //    var drawMode = model.GlDrawMode;

        //    var usage = OGLModelUsage.StaticRead;

        //    // Create the data buffers.
        //    var indexBufferId = CreateBufferId(gl);
        //    var vertexBufferId = CreateBufferId(gl);
        //    var normalBufferId = CreateBufferId(gl);

        //    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vertexBufferId);
        //    gl.BufferData(OpenGL.GL_ARRAY_BUFFER, model.Vertices.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
        //    gl.VertexAttribPointer(VertexAttributes.Position, model.BufferStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
        //    gl.EnableVertexAttribArray(VertexAttributes.Position);

        //    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, normalBufferId);
        //    gl.BufferData(OpenGL.GL_ARRAY_BUFFER, model.Normals.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
        //    gl.VertexAttribPointer(VertexAttributes.Normal, model.BufferStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
        //    gl.EnableVertexAttribArray(VertexAttributes.Normal);

        //    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, indexBufferId);
        //    gl.BufferData(OpenGL.GL_ARRAY_BUFFER, model.Indices, (uint)usage);
        //    gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, indexBufferId);

        //    gl.FrontFace(OpenGL.GL_CW);

        //    // Draw the elements.
        //    gl.DrawElements(drawMode, indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);


        //    // Clean up
        //    List<uint> buffersToBeRemoved = new List<uint>();

        //    if (model.IndexBufferId != null)
        //        buffersToBeRemoved.Add(model.IndexBufferId.Value);
        //    if (model.VertexBufferId != null)
        //        buffersToBeRemoved.Add(model.VertexBufferId.Value);
        //    if (model.NormalBufferId != null)
        //        buffersToBeRemoved.Add(model.NormalBufferId.Value);

        //    return buffersToBeRemoved;
        //}


    }
}
