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
        public DynamicOGLModel(vec3[] verts, uint[] indices, vec3[] normals)
        {
            CreateMesh(verts, indices, normals);
        }

        public DynamicOGLModel(Mesh mesh) 
        {
            Mesh = mesh;
            mesh.RefreshRawData(false, false);
            VerticesCount = mesh.VerticesVec3.Length;
            IndicesCount = mesh.Indices.Length;
            NormalsCount = mesh.Normals.Length;
        }

        //public override void Render(SharpGL.OpenGL gl, Shaders.ShaderManagerBase shader = null)
        //{
        //    // Load our cube data ClockWise.
        //    gl.FrontFace(OpenGL.GL_CW);

        //    // Binds buffers.
        //    base.Bind(gl);

        //    // Draw the elements.
        //    gl.DrawElements(OpenGL.GL_TRIANGLES, (int)IndicesCount, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);
        
        //}
    }
}
