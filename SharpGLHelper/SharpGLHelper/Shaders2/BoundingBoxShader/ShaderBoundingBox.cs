using GlmNet;
using SharpGL;
using SharpGLHelper.Common;
using SharpGLHelper.SceneElements;
using SharpGLHelper.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders2.BoundingBoxShader
{
    public class ShaderBoundingBox: ShaderManagerBase
    {
        #region fields
        mat4 _projectionMatrix = mat4.identity();
        mat4 _modelviewMatrix = mat4.identity();
        IEnumerable<TransformationMatrix> _transformations;
        uint? _indicesBufferId;
        uint? _transformationsBufferId;
        uint? _colorsBufferId;

        #endregion fields

        #region properties
        public mat4 ModelviewMatrix
        {
            get { return _modelviewMatrix; }
            set
            {
                if (!value.Equals(_modelviewMatrix))
                {
                    UpdateActions.Add(ApplyModelviewMatrix);

                    _modelviewMatrix = value;
                }
            }
        }
        public mat4 ProjectionMatrix
        {
            get { return _projectionMatrix; }
            set
            {
                if (!value.Equals(_projectionMatrix))
                {
                    UpdateActions.Add(ApplyProjectionMatrix);

                    _projectionMatrix = value;
                }
            }
        }

        public IEnumerable<TransformationMatrix> Transformations
        {
            get { return _transformations; }
            set 
            {
                _transformations = value; 
            }
        }



        public override bool HasChanges
        {
            get
            {
                return UpdateActions.Count > 0;
            }
        }

        public bool ChangesHandled
        {
            get { return HasChanges; }
            set
            {
                UpdateActions.Clear();
            }
        }
        #endregion properties

        

        #region events
        #endregion events

        #region constructors

        public ShaderBoundingBox(OpenGL gl, IEnumerable<TransformationMatrix> transformations)
            : base(gl, "BoundingBox")
        {
            if (_indicesBufferId == null)
            {
                // Create buffers
                CreateBuffers(gl);

                // Set data.
                SetBufferData(gl, transformations);
            }

        }
        #endregion constructors

        public void CreateBuffers(OpenGL gl, bool cleanUp = true)
        {
            #region remove previous buffers
            if( cleanUp)
            {
                var bufferIds = new List<uint>();
                if (_indicesBufferId != null)
                    bufferIds.Add(_indicesBufferId.Value);
                if (_transformationsBufferId != null)
                    bufferIds.Add(_transformationsBufferId.Value);
                if (_colorsBufferId != null)
                    bufferIds.Add(_colorsBufferId.Value);

                if (bufferIds.Count > 0)
                    OGLSceneElementBase.DeleteBuffers(gl, bufferIds.ToArray());
            }
            #endregion remove previous buffers

            var buffers = OGLSceneElementBase.CreateBufferIds(gl, 3);
            _indicesBufferId = buffers[0];
            _transformationsBufferId = buffers[1];
            _colorsBufferId = buffers[2];
        }

        public void SetBufferData(OpenGL gl, IEnumerable<TransformationMatrix> transformations, OGLModelUsage usage = OGLModelUsage.StaticRead)
        {
            var transCount = transformations.Count();

            // Validation.
            if(transCount > Math.Pow(2, 24))
            {
                throw new OverflowException(
                    "This shader can't handle more than 2^24 transformations while "+
                    "using 24 bit colors.");
            }

            #region get indices
            var indices = new ushort[transCount];
            for (ushort i = 0; i < indices.Length; i++)
            {
                indices[i] = i; // Do all transformations once.
            }
            #endregion get indices

            #region get transformations array
            var stride = 16; // Transformation matrix is a 4x4 = 16.

            var transformationsArray = new float[transCount * stride];
            for (int i = 0; i < transCount; i++)
            {
                float[] transAsFloats = transformations.ElementAt(i).ResultMatrix.to_array();
                for (int j = 0; j < stride; j++)
                {
                    transformationsArray[i * stride + j] = transAsFloats[j];
                }
            }
            #endregion get transformations array

            #region get color array
            int colorStride = 3;
            var colorArray = new float[transCount * colorStride];

            for (int i = 0; i < transCount; i++)
            {
                ulong id = transformations.ElementAt(i).UniqueId;
                var color = new ColorF((uint) id);
                
                colorArray[i * stride] = color.R;
                colorArray[i * stride + 1] = color.G;
                colorArray[i * stride + 2] = color.B;
            }
            #endregion get color array



            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _indicesBufferId.Value);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, indices, (uint)usage);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _transformationsBufferId.Value);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, transformationsArray, (uint)usage);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _colorsBufferId.Value);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, colorArray, (uint)usage);
        }

        public void Bind(OpenGL gl)
        {

            // Bind the vertex, normal and index buffers.
            if (_transformationsBufferId != null)
            {
                var transStride = 16;

                //Bind
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _transformationsBufferId.Value);
                gl.VertexAttribPointer(VertexAttributes.Position, transStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
                gl.EnableVertexAttribArray(VertexAttributes.Position);
            }

            if (_colorsBufferId != null)
            {
                var colStride = 3;
                //Bind
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _colorsBufferId.Value);
                gl.VertexAttribPointer(VertexAttributes.Normal, colStride, OpenGL.GL_FLOAT, false, 0, IntPtr.Zero);
                gl.EnableVertexAttribArray(VertexAttributes.Normal);
            }

            if (_indicesBufferId != null)
            {
                gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, _indicesBufferId.Value);
            }
        }

        public override void RenderAll(OpenGL gl)
        {
            ChangesHandled = true;

            UseProgram(gl, () =>
            {
                ApplyChangedProperties(gl);

                Bind(gl);
            });

        }


        public override void RenderAll(OpenGL gl, Action executedSequence)
        {
            UseProgram(gl, executedSequence);
        }



        #region applying MVP - matrices
        public void ApplyModelviewMatrix(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = ModelviewMatrixId;
            var val = ModelviewMatrix;

            p.SetUniformMatrix4(gl, id, val.to_array());
        }
        public void ApplyProjectionMatrix(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = ProjectionMatrixId;
            var val = ProjectionMatrix;

            p.SetUniformMatrix4(gl, id, val.to_array());
        }
        #endregion applying MVP - matrices
    }
}
