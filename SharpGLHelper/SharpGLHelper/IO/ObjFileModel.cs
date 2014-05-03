using GlmNet;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.Primitives;
using SharpGLHelper.SceneElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpGLHelper.IO
{
    class ObjFileModel : IFileModel
    {
        #region fields;
        float[] _vertices;
        uint[] _indices;
        float[] _normals;
        Material[] _materials;
        ModelBase _visualModel;
        Mesh _mesh = null;
        #endregion fields;

        #region properties
        public float[] Vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                _vertices = value;
            }
        }

        public uint[] Indices
        {
            get
            {
                return _indices;
            }
            set
            {
                _indices = value;
            }
        }

        public float[] Normals
        {
            get
            {
                return _normals;
            }
            set
            {
                _normals = value;
            }
        }

        public Material[] Materials
        {
            get
            {
                return _materials;
            }
            set
            {
                _materials = value;
            }
        }

        /// <summary>
        /// Gets the loaded DynamicOGLModel. Automatically calls GenerateVisualModel(...) when load succeeded and _visualModel was still null.
        /// </summary>
        public ModelBase VisualModel
        {
            get
            {
                if (_visualModel == null && _mesh != null)
                    GenerateVisualModel();
                return _visualModel;
            }
            set
            {
                _visualModel = value;
            }
        }
        #endregion properties

        /// <summary>
        /// Generates the DynamicOGLModel from the data that was retrieved from LoadObjFile.
        /// </summary>
        public void GenerateVisualModel()
        {
            VisualModel = new DynamicOGLModel(_mesh);
        }

        public void LoadObjFile(string path)
        {
            var v = new List<vec4>(); // Vertices coordinates
            var vt = new List<vec3>(); // Texture coordinates
            var vn = new List<vec3>(); // Normals
            var vp = new List<vec3>(); // Parameter space vertices
            var f = new List<List<uint>>(); // Face definitions

            #region read data
            using (var sr = new StreamReader(path))
            {
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    {
                        // # : comments
                        continue; 
                    }
                    else if (line.StartsWith("v"))
                    {
                        // v : vertex coordinates
                        #region load vertex coordinates
                        var pattern = "/d+";
                        var matches = new Regex(pattern).Matches(line);
                        var matchCount = matches.Count;
                        if (matchCount < 3) continue; // Invalid vertex.
                        var vec = new vec4();
                        vec.x = float.Parse(matches[0].Value);
                        vec.y = float.Parse(matches[1].Value);
                        vec.z = float.Parse(matches[2].Value);
                        vec.w = matchCount > 3 ? float.Parse(matches[3].Value) : 1;

                        v.Add(vec);
                        #endregion load vertex coordinates
                    }
                    else if (line.StartsWith("vt"))
                    {
                        // vt : texture coordinate
                        #region load texture coordinate
                        var pattern = "/d+";
                        var matches = new Regex(pattern).Matches(line);
                        var matchCount = matches.Count;
                        if (matchCount < 2) continue; // Invalid texture coordinate.
                        var vec = new vec3();
                        vec.x = float.Parse(matches[0].Value);
                        vec.y = float.Parse(matches[1].Value);
                        vec.z = matchCount > 2 ? float.Parse(matches[2].Value) : 0;

                        vt.Add(vec);
                        #endregion load texture coordinate
                    }
                    else if (line.StartsWith("vn"))
                    {
                        // vn : vertex normal
                        #region load vertex normal
                        var pattern = "/d+";
                        var matches = new Regex(pattern).Matches(line);
                        var matchCount = matches.Count;
                        if (matchCount < 2) continue; // Invalid.
                        var vec = new vec3();
                        vec.x = float.Parse(matches[0].Value);
                        vec.y = float.Parse(matches[1].Value);
                        vec.z = float.Parse(matches[2].Value);

                        vn.Add(vec);
                        #endregion load vertex normal
                    }
                    else if (line.StartsWith("vp"))
                    {
                        // vp : parameter space vertices
                        #region load parameter space vertices
                        var pattern = "/d+";
                        var matches = new Regex(pattern).Matches(line);
                        var matchCount = matches.Count;
                        if (matchCount < 1) continue; // Invalid vertex.
                        var vec = new vec3();
                        vec.x = float.Parse(matches[0].Value);
                        vec.y = matchCount > 1 ? float.Parse(matches[1].Value) : float.MaxValue;
                        vec.z = matchCount > 2 ? float.Parse(matches[2].Value) : float.MaxValue;

                        vp.Add(vec);
                        #endregion load parameter space vertices
                    }
                    else if (line.StartsWith("f"))
                    {
                        // vt : face definitions
                        #region load face definitions
                        var pattern = "/d+";
                        var matches = new Regex(pattern).Matches(line);
                        var matchCount = matches.Count;
                        if (matchCount < 3) continue; // Invalid vertex.

                        List<uint> points = new List<uint>();
                        foreach (var match in matches)
                        {
                            points.Add(uint.Parse(match.ToString()));
                        }

                        f.Add(points);
                        #endregion load face definitions
                    }
                }
            }
            #endregion read data

            var vertices = new Vertex[v.Count];

            #region create Vertex- objects
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vertex( 
                    new vec4(v[i].x, v[i].y, v[i].z, v[i].w), // vertex
                    (uint)i, // index
                    vn.Count > i ? new vec3(vn[i].x, vn[i].y, vn[i].z) : new vec3()); // normal
            }
            #endregion create Vertex- objects

            var faces = new Face[f.Count];
            #region create Face- objects
            for (int i = 0; i < faces.Length; i++)
            {
                var face = f[i];
                var verts = new Vertex[face.Count];
                for (int j = 0; j < verts.Length; j++)
                {
                    verts[j] = vertices[face[j]];
                }

                //faces[i] = new Face(verts);
            }
            #endregion create Face- objects

            // Create mesh
            var mesh = new Mesh(faces);

            mesh.Triangulate();


            mesh.RefreshRawData(true, true, true);


            Indices = mesh.Indices;
            Vertices = mesh.VerticesVec3.SelectMany<vec3, float>(x=> new float[]{x.x, x.y, x.z}).ToArray();
            Normals = mesh.Normals.SelectMany<vec3, float>(x => new float[] { x.x, x.y, x.z }).ToArray();
            _mesh = mesh;
        }
    }
}
