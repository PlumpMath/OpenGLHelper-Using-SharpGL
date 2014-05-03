using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.OGLOverloads
{
    public class VertexAttribPointer
    {
        #region fields
        uint _index;
        int _size = 4;
        OGLType _type;
        bool _normalized = false;
        int _stride;
        IntPtr _offsetPointer = IntPtr.Zero;
        uint _divisorValue = 0;
        //Action<OpenGL> _currentVariant;

        int[] _permittedSizes = new int[] { 1, 2, 3, 4, (int)OpenGL.GL_BGRA };
        #endregion fields

        #region properties
        /// <summary>
        /// The position of this attribute inside the shader.
        /// </summary>
        public uint Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public int Size
        {
            get { return _size; }
            set 
            {
                // Validate.
                if (!_permittedSizes.Contains(value))
                    throw new Exception("Size has to be either 1,2,3,4 or OpenGL.GL_BGRA(=32993).");
                    
                _size = value; 
            }
        }

        public OGLType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool Normalized
        {
            get { return _normalized; }
            set { _normalized = value; }
        }

        public int Stride
        {
            get { return _stride; }
            set { _stride = value; }
        }

        public IntPtr OffsetPointer
        {
            get { return _offsetPointer; }
            set { _offsetPointer = value; }
        }

        /// <summary>
        /// The amount of steps through the vertex shader for which this attribute will be reused.
        /// 0 and 1 = change every step, else change once every x steps.
        /// </summary>
        public uint DivisorValue
        {
            get { return _divisorValue; }
            set { _divisorValue = value; }
        } 


        //private Action<OpenGL> CurrentVariant
        //{
        //    get { return _currentVariant; }
        //    set { _currentVariant = value; }
        //}
        #endregion properties

        #region events
        #endregion events

        #region constructors
        /// <summary>
        /// Prepares the settings required for a vertex attribute.
        /// </summary>
        /// <param name="index">The line number of this attribute in the shader.</param>
        /// <param name="size">Specifies the number of components per generic vertex attribute.</param>
        /// <param name="type">Specifies the data type of each component in the array.</param>
        /// <param name="stride">Specifies the byte offset between consecutive generic vertex attributes. If stride is 0, the generic vertex attributes are understood to be tightly packed in the array.</param>
        /// <param name="offsetPointer">Specifies a offset of the first component of the first generic vertex attribute in the array in the data store of the buffer currently bound to the GL_ARRAY_BUFFER target.</param>
        /// <param name="normalized">Specifies whether fixed-point data values should be normalized or converted directly as fixed-point values when they are accessed.</param>
        /// <param name="divisor">Specify the number of instances that will pass between updates of the generic attribute at slot index.</param>
        public VertexAttribPointer(uint index, int size, OGLType type, int stride, IntPtr offsetPointer, bool normalized = false, uint divisor = 0)
        {
            Index = index;
            Size = size;
            Type = type;
            Stride = stride;
            OffsetPointer = offsetPointer;
            Normalized = normalized;
            DivisorValue = divisor;
            //CurrentVariant = new Action<OpenGL>(InvokeDefaultVariant);
        }
        //public VertexAttribPointer(uint index, int size, OGLType type, int stride, IntPtr pointer)
        //{
        //    Index = index;
        //    Size = size;
        //    Type = type;
        //    Stride = stride;
        //    Pointer = pointer;
        //    CurrentVariant = new Action<OpenGL>(InvokeIVariant);
        //}
        #endregion constructors

        public void Invoke(OpenGL gl)
        {
            //CurrentVariant.Invoke(gl);
            gl.VertexAttribPointer(Index, Size, (uint)Type, Normalized, Stride, OffsetPointer);
            if (DivisorValue > 0)
            {
                gl.VertexAttribDivisor(Index, DivisorValue);
            }
        }

        /// <summary>
        /// Initiates the VertexAttribIPointer. Note that the Type must be one of the folowing:
        /// GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, GL_UNSIGNED_INT, GL_HALF_FLOAT, GL_FLOAT, 
        /// GL_DOUBLE, GL_FIXED, GL_INT_2_10_10_10_REV, GL_UNSIGNED_INT_2_10_10_10_REV or GL_UNSIGNED_INT_10F_11F_11F_REV
        /// </summary>
        /// <param name="gl"></param>
        //public void InvokeDefaultVariant(OpenGL gl)
        //{
        //    gl.VertexAttribPointer(Index, Size, (uint)Type, Normalized, Stride, Pointer);
        //}
        //private void LVariant(OpenGL gl)
        //{
        //    gl.VertexAttribLPointer(Index, Size, (uint)Type, Stride, Pointer);

        //}

        /// <summary>
        /// Initiates the VertexAttribIPointer. Note that the Type must be one of the folowing:
        /// GL_BYTE, GL_UNSIGNED_BYTE, GL_SHORT, GL_UNSIGNED_SHORT, GL_INT, or GL_UNSIGNED_INT.
        /// </summary>
        /// <param name="gl"></param>
        //public void InvokeIVariant(OpenGL gl)
        //{
        //    gl.VertexAttribIPointer(Index, Size, (uint)Type, Stride, Pointer);
        //}
    }
}
