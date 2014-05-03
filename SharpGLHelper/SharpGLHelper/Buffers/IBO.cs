using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Buffers
{

    public class IBO : OGLBufferObject
    {
        public IBO(OGLBufferId bufferId)
            :base(bufferId)
        { }

        public void SetBufferData(OpenGL gl, OGLBufferDataTarget target, uint[] indices, OGLModelUsage usage, bool compress)
        {
            Size = indices.Length;
            Stride = 1;


            if(compress)
                if (indices.Max() < byte.MaxValue)
                {
                    // use a byte buffer instead in order to decrease the buffer size.
                    var newIdxs = indices.Select(x => (byte)x).ToArray();
                    SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, newIdxs, OGLModelUsage.StaticCopy, Stride);
                    return;
                }
                else if (indices.Max() < ushort.MaxValue)
                {
                    // use a ushort buffer instead in order to decrease the buffer size.
                    var newIdxs = indices.Select(x => (ushort)x).ToArray();
                    SetBufferData(gl, OGLBufferDataTarget.ArrayBuffer, newIdxs, OGLModelUsage.StaticCopy, Stride);
                    return;
                }


            SetBufferData(gl, SharpGLHelper.OGLBufferDataTarget.ArrayBuffer, indices, SharpGLHelper.OGLModelUsage.StaticCopy, Stride);
            
        }

        public virtual void BindBuffer(OpenGL gl)
        {
            base.BindBuffer(gl, OGLBindBufferTarget.ElementArrayBuffer);
        }
    }
}
