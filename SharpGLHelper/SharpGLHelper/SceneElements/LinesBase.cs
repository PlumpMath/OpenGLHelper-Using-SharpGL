using GlmNet;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Core;
using SharpGL.VertexBuffers;
using SharpGLHelper.Common;
using SharpGLHelper.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.SceneElements
{
    public class LinesBase : OGLVisualSceneElementBase
    {
        #region fields
        //private uint _glDrawMode = SharpGL.OpenGL.GL_LINE;
        //vec3[] _vertices, _normals;
        //ushort[] _indices;
        //Material _material;
        //OpenGL _openGL;
        //private VertexBuffer _vertexBuffer, _normalBuffer;
        //private IndexBuffer _indexBuffer;
        float _lineWidth = 1f;
        #endregion fields


        #region properties
        //public vec3[] Vertices
        //{
        //    get { return _vertices; }
        //    set { _vertices = value; }
        //}

        //public vec3[] Normals
        //{
        //    get { return _normals; }
        //    set { _normals = value; }
        //}

        //public ushort[] Indices
        //{
        //    get { return _indices; }
        //    set { _indices = value; }
        //}

        //public uint GlDrawMode
        //{
        //    get { return _glDrawMode; }
        //    set { _glDrawMode = value; }
        //}

        //public Material Material
        //{
        //    get { return _material; }
        //    set { _material = value; }
        //}

        //public OpenGL GL
        //{
        //    get { return _openGL; }
        //    set { _openGL = value; }
        //}

        //public VertexBuffer VertexBuffer
        //{
        //    get { return _vertexBuffer; }
        //    set { _vertexBuffer = value; }
        //}

        //public VertexBuffer NormalBuffer
        //{
        //    get { return _normalBuffer; }
        //    set { _normalBuffer = value; }
        //}
        //public IndexBuffer IndexBuffer
        //{
        //    get { return _indexBuffer; }
        //    set { _indexBuffer = value; }
        //}

        public float LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        #endregion properties

        public LinesBase(OpenGL gl, List<Tuple<vec3, vec3>> lines, Material material = null, OGLModelUsage usage = OGLModelUsage.StaticRead)
        {
            var verts = new vec3[lines.Count * 2];
            var normals = new vec3[lines.Count * 2];
            var indices = new uint[lines.Count * 2];

            for (int i = 0; i < lines.Count; i++)
            {
                var i2 = i * 2;
                verts[i2] = lines[i].Item1;
                verts[i2 + 1] = lines[i].Item2;

                normals[i2] = new vec3(1, 1, 1);
                normals[i2 + 1] = new vec3(1, 1, 1);

                indices[i2] = (uint)i2;
                indices[i2 + 1] = (uint)(i2 + 1);
            }

            if (material != null)
                Material = material;

            Vertices = verts;
            Normals = normals;
            Indices = indices;
            GlDrawMode = OpenGL.GL_LINES;


            if (gl != null)
                GenerateGeometry(gl, usage);
        }

        /// <summary>
        /// Generates the vertices, normals and indices and creates them for the OpenGL.
        /// This method has to be called once before drawing. 
        /// </summary>
        /// <param name="gl"></param>
        //public void GenerateGeometry(OpenGL gl)
        //{
        //    _openGL = gl;

        //    // Create the index data buffer.
        //    _indexBuffer = new IndexBuffer();
        //    _indexBuffer.Create(_openGL);

        //    // Create the vertex data buffer.
        //    _vertexBuffer = new VertexBuffer();
        //    _vertexBuffer.Create(_openGL);

        //    _normalBuffer = new VertexBuffer();
        //    _normalBuffer.Create(_openGL);

        //    _openGL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VertexBuffer.VertexBufferObject);
        //    _openGL.BufferData(OpenGL.GL_ARRAY_BUFFER, TransformedVertices.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
        //    _openGL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, NormalBuffer.VertexBufferObject);
        //    _openGL.BufferData(OpenGL.GL_ARRAY_BUFFER, Normals.SelectMany(v => v.to_array()).ToArray(), (uint)usage);
        //}

        //public override void Render(OpenGL gl, Shaders.ShaderManagerBase shader = null)
        //{
        //    // Sets the linewidth.
        //    gl.LineWidth(LineWidth);

        //    // Binds buffers.
        //    Bind();

        //    // Draw the elements.
        //    gl.DrawArrays(GlDrawMode, 0, (int)VerticesCount);
        //}



        /// <summary>
        /// Calls VertexBuffer.Bind(gl), IndexBuffer.Bind(gl) and Material.Bind(gl). 
        /// </summary>
        /// <param name="gl">The OpenGL</param>
        //public void Bind()
        //{
        //    if (_openGL == null)
        //    {
        //        throw new ArgumentNullException("OpenGL parameter cannot be null. Call 'GenerateGeomerty(...)' before attempting to bind.");
        //    }

        //    // Bind the vertex, normal and index buffers.
        //    if (_vertexBuffer != null)
        //    {
        //        _vertexBuffer.Bind(_openGL);
        //        _vertexBuffer.SetData(_openGL, VertexAttributes.Position, Vertices.SelectMany(v => v.to_array()).ToArray(), false, 3);
        //    }

        //    if (_normalBuffer != null)
        //    {
        //        _normalBuffer.Bind(_openGL);
        //        _normalBuffer.SetData(_openGL, VertexAttributes.Normal, Normals.SelectMany(v => v.to_array()).ToArray(), false, 3);
        //    }

        //    if (_indexBuffer != null)
        //    {
        //        _indexBuffer.Bind(_openGL);
        //        _indexBuffer.SetData(_openGL, Indices);
        //    }
        //}
    }
}
