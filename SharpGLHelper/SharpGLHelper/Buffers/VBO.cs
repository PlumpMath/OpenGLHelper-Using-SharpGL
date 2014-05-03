using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Buffers
{
    public class VBO : OGLBufferObject
    {

        public VBO(OGLBufferId bufferId)
            :base(bufferId)
        { }

        public virtual void BindBuffer(OpenGL gl)
        {
            base.BindBuffer(gl, OGLBindBufferTarget.ArrayBuffer);
        }
    }
}
