using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper
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

    public enum OGLBufferDataTarget : uint
    {
        ArrayBuffer = (uint)OpenGL.GL_ARRAY_BUFFER,
        CopyReadBuffer = (uint)36662,//OpenGL.GL_COPY_READ_BUFFER, 
        CopyWriteBuffer = (uint)36663,//OpenGL.GL_COPY_WRITE_BUFFER, 
        ElementArrayBuffer = (uint)OpenGL.GL_ELEMENT_ARRAY_BUFFER,
        PixelPackBuffer = (uint)OpenGL.GL_PIXEL_PACK_BUFFER,
        PixelUnpackBuffer = (uint)OpenGL.GL_PIXEL_UNPACK_BUFFER,
        TextureBuffer = (uint)OpenGL.GL_TEXTURE_BUFFER,
        TransformFeedbackBuffer = (uint)OpenGL.GL_TRANSFORM_FEEDBACK_BUFFER,
        UniformBuffer = (uint)35345//OpenGL.GL_UNIFORM_BUFFER
    }

    public enum OGLBindBufferTarget : uint
    {
        ArrayBuffer = (uint)OpenGL.GL_ARRAY_BUFFER,
        ElementArrayBuffer = (uint)OpenGL.GL_ELEMENT_ARRAY_BUFFER,
        PixelPackBuffer = (uint)OpenGL.GL_PIXEL_PACK_BUFFER,
        PixelUnpackBuffer = (uint)OpenGL.GL_PIXEL_UNPACK_BUFFER
    }

    public enum OGLType : uint
    {
        Byte = OpenGL.GL_BYTE,
        UnsignedByte = OpenGL.GL_UNSIGNED_BYTE,
        Short = OpenGL.GL_SHORT,
        UnsignedShort = OpenGL.GL_UNSIGNED_SHORT,
        Int = OpenGL.GL_INT,
        UnsignedInt = OpenGL.GL_UNSIGNED_INT,
        HalfFloat = (uint)5131, //OpenGL.GL_HALF_FLOAT,
        Float = OpenGL.GL_FLOAT,
        Double = OpenGL.GL_DOUBLE,
        //Fixed = OpenGL.GL_FIXED,
        Int_2_10_10_10_Rev = 36255, // OpenGL.GL_INT_2_10_10_10_REV,
        UnsignedInt_2_10_10_10_Rev = OpenGL.GL_UNSIGNED_INT_2_10_10_10_REV,
        UnsignedInt_10F_11F_11F_Rev = OpenGL.GL_UNSIGNED_INT_10F_11F_11F_REV,
    }
}
