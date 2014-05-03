using GlmNet;
using SharpGLHelper.SceneElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Primitives
{
    public class BackgroundPlane : ModelBase
    {
        vec3[] _vertices = new vec3[]
        {
            new vec3(-1,-1,0),
            new vec3(1,-1,0),
            new vec3(1,1,0),
            new vec3(-1,1,0),
        };

        uint[] _indices = new uint[]
        {
            0,1,2,
            1,2,3
        };

        public BackgroundPlane()
        {
            AutoCalculateNormals = false;
            CreateMesh(_vertices, _indices, null);
        }

    }
}
