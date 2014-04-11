using GlmNet;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.SceneElements;
using SharpGLHelper.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Primitives
{
    /// <summary>
    /// A model of which it's data can be set on runtime.
    /// </summary>
    public class DynamicOGLModel: ModelBase
    {
        public DynamicOGLModel(vec3[] verts, ushort[] indices, vec3[] normals)
        {
            CreateMesh(verts, indices, normals);
        }

        public DynamicOGLModel(Mesh mesh) 
        {
            Mesh = mesh;
            mesh.UpdateRawData();
        }


        public override void Render(OpenGL gl, RenderMode renderMode, ExtShaderProgram shader = null)
        {
            // Load our cube data ClockWise.
            gl.FrontFace(OpenGL.GL_CW);

            // Binds buffers.
            base.Bind();

            // Draw the elements.
            gl.DrawElements(OpenGL.GL_TRIANGLES, Indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);
        }
    }
}
