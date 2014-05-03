using SharpGL;
using SharpGLHelper.OGLOverloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpGLHelper.Buffers
{
    public class OGLBufferObject
    {
        #region fields
        OGLBufferId _bufferId;
        OGLType? _bufferDataType = null;
        int _size = -1;
        int _stride = -1;
        VertexAttribPointer _vertexAttribPointer;
        #endregion fields

        #region properties
        /// <summary>
        /// 
        /// </summary>
        public OGLBufferId BufferId
        {
            get { return _bufferId; }
            set { _bufferId = value; }
        }

        /// <summary>
        /// The size of the content array. 
        /// If it returns -1 means that SetBufferData(...) hasn't been called.
        /// </summary>
        public int Size
        {
            get { return _size; }
            protected set { _size = value; }
        }

        /// <summary>
        /// The vertex attribute pointer.
        /// </summary>
        public VertexAttribPointer VertexAttribPointer
        {
            get { return _vertexAttribPointer; }
            set { _vertexAttribPointer = value; }
        }
        /// <summary>
        /// The size of a section from the data that forms a single attribute.
        /// </summary>
        public int Stride
        {
            get { return _stride; }
            protected set { _stride = value; }
        }

        /// <summary>
        /// The datatype specified when setting the buffer data.
        /// </summary>
        public OGLType? BufferDataType
        {
            get { return _bufferDataType; }
            protected set { _bufferDataType = value; }
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pointer">Usually used to provide information about how the shader accepts this buffer as input parameter. </param>
        public OGLBufferObject(OGLBufferId buffer, VertexAttribPointer pointer = null)
        {
            BufferId = buffer;
            VertexAttribPointer = pointer;
        }
        #endregion constructors

        /// <summary>
        /// Calls the glBufferData(...). The data.Length will be used as value for Size property.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="target">The buffer data target.</param>
        /// <param name="data">The data as float.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="stride">The amount of floats that form one input variable.</param>
        public virtual void SetBufferData(OpenGL gl, OGLBufferDataTarget target, float[] data, OGLModelUsage usage, int stride)
        {
            Size = data.Length / stride;
            Stride = stride;

            BufferDataType = OGLType.Float;
            
            gl.BufferData((uint)target, data, (uint)usage);
        }
        /// <summary>
        /// Calls the glBufferData(...). The data.Length will be used as value for Size property.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="target">The buffer data target.</param>
        /// <param name="data">The data as uint.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="stride">The amount of uint that form one input variable.</param>
        public virtual void SetBufferData(OpenGL gl, OGLBufferDataTarget target, byte[] data, OGLModelUsage usage, int stride)
        {
            Size = data.Length / stride;
            Stride = stride;

            BufferDataType = OGLType.UnsignedByte;

            var dataSize = data.Length * sizeof(byte);
            IntPtr newDataPtr = Marshal.AllocHGlobal(dataSize);
            var byteData = new byte[data.Length];
            Buffer.BlockCopy(data, 0, byteData, 0, dataSize);
            Marshal.Copy(byteData, 0, newDataPtr, data.Length);

            gl.BufferData((uint)target, dataSize, newDataPtr, (uint)usage);
        }
        /// <summary>
        /// Calls the glBufferData(...). The data.Length will be used as value for Size property.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="target">The buffer data target.</param>
        /// <param name="data">The data as ushort.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="stride">The amount of ushorts that form one input variable.</param>
        public virtual void SetBufferData(OpenGL gl, OGLBufferDataTarget target, ushort[] data, OGLModelUsage usage, int stride)
        {
            Size = data.Length / stride;
            Stride = stride;

            BufferDataType = OGLType.UnsignedShort;

            gl.BufferData((uint)target, data, (uint)usage);
        }
        /// <summary>
        /// Calls the glBufferData(...). The data.Length will be used as value for Size property.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="target">The buffer data target.</param>
        /// <param name="data">The data as uint.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="stride">The amount of uint that form one input variable.</param>
        public virtual void SetBufferData(OpenGL gl, OGLBufferDataTarget target, uint[] data, OGLModelUsage usage, int stride)
        {
            Size = data.Length / stride;
            Stride = stride;

            BufferDataType = OGLType.UnsignedInt;

            var dataSize = data.Length * sizeof(uint);
            IntPtr newDataPtr = Marshal.AllocHGlobal(dataSize);
            var intData = new int[data.Length];
            Buffer.BlockCopy(data, 0, intData, 0, dataSize);
            Marshal.Copy(intData, 0, newDataPtr, data.Length);

            gl.BufferData((uint)target, dataSize, newDataPtr, (uint)usage);
        }
        /// <summary>
        /// Calls the glBufferData(...). The size will be used as value for Size property.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="target">The buffer data target.</param>
        /// <param name="size">The size of the data buffer.</param>
        /// <param name="data">The pointer to the data in memory.</param>
        /// <param name="usage">The usage.</param>
        public virtual void SetBufferData(OpenGL gl, OGLBufferDataTarget target, int size, IntPtr data, OGLModelUsage usage, int stride, OGLType bufferDataType,
            bool bind = false, OGLBindBufferTarget bindTarget = OGLBindBufferTarget.ArrayBuffer)
        {
            Size = size / stride;
            Stride = stride;


            if(bind)
                BindBuffer(gl, bindTarget);
            gl.BufferData((uint)target, size, data, (uint)usage);
        }

        public virtual void ReplaceBufferData(OpenGL gl, OGLBufferDataTarget target, int offset, float[] newData,
            bool bind = false, OGLBindBufferTarget bindTarget = OGLBindBufferTarget.ArrayBuffer)
        {
            IntPtr newDataPtr;
            // Unsafe code.
            unsafe
            {
                // Fix the pointer to the array. 
                fixed (float* pArray = newData)
                {
                    // pArray now has the pointer to the array. You can get an IntPtr by casting to void, and passing that in.
                    newDataPtr = new IntPtr((void*)pArray);
                }
            }

            ReplaceBufferData(gl, target, offset, newData.Length, newDataPtr, bind, bindTarget);
        }

        public virtual void ReplaceBufferData(OpenGL gl, OGLBufferDataTarget target, int offset, ushort[] newData,
            bool bind = false, OGLBindBufferTarget bindTarget = OGLBindBufferTarget.ArrayBuffer)
        {
            IntPtr newDataPtr;
            // Unsafe code.
            unsafe
            {
                // Fix the pointer to the array. 
                fixed (ushort* pArray = newData)
                {
                    // pArray now has the pointer to the array. You can get an IntPtr by casting to void, and passing that in.
                    newDataPtr = new IntPtr((void*)pArray);
                }
            }

            ReplaceBufferData(gl, target, offset, newData.Length, newDataPtr, bind, bindTarget);
        }
        public virtual void ReplaceBufferData(OpenGL gl, OGLBufferDataTarget target, int offset, uint[] newData,
           bool bind = false, OGLBindBufferTarget bindTarget = OGLBindBufferTarget.ArrayBuffer)
        {
            IntPtr newDataPtr;
            // Unsafe code.
            unsafe
            {
                // Fix the pointer to the array. 
                fixed (uint* pArray = newData)
                {
                    // pArray now has the pointer to the array. You can get an IntPtr by casting to void, and passing that in.
                    newDataPtr = new IntPtr((void*)pArray);
                }
            }

            ReplaceBufferData(gl, target, offset, newData.Length, newDataPtr, bind, bindTarget);
        }

        public virtual void ReplaceBufferData(OpenGL gl, OGLBufferDataTarget target, int offset, int size, IntPtr newData,
            bool bind = false, OGLBindBufferTarget bindTarget = OGLBindBufferTarget.ArrayBuffer)
        {
            if (bind)
                BindBuffer(gl, bindTarget);
            gl.BufferSubData((uint)target, offset, size, newData);
        }

        /// <summary>
        /// Calls glBindBuffer.
        /// </summary>
        /// <param name="gl"></param>
        public virtual void BindBuffer(OpenGL gl, OGLBindBufferTarget target, VertexAttribPointer pointer = null)
        {
            gl.BindBuffer((uint)target, BufferId.Value);
            VertexAttribPointer pointerToUse = null;

            if (pointer != null)
                pointerToUse = pointer;
            else if (VertexAttribPointer != null)
                pointerToUse = VertexAttribPointer;

            if (pointerToUse != null)
            {
                pointerToUse.Invoke(gl);
                gl.EnableVertexAttribArray(pointerToUse.Index);
            }
        }

    }
}
