using GlmNet;
using SharpGL;
using SharpGL.SceneGraph.Core;
using SharpGLHelper.Common;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.SceneElements;
using SharpGLHelper.Shaders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Primitives
{
    public class Axis
    {
        
        #region fields
        private float _axisLength = 4, 
            _lineThickness = 2;
        private LinesBase[] _lines = new LinesBase[3];
        #endregion fields

        #region properties
        /// <summary>
        /// The length of each axis.
        /// </summary>
        public float AxisLength
        {
            get { return _axisLength; }
            set { _axisLength = value; }
        }
        /// <summary>
        /// The thickness of each axis.
        /// </summary>
        public float LineThickness
        {
            get { return _lineThickness; }
            set { _lineThickness = value; }
        }
        /// <summary>
        /// The material of the the X-axis
        /// </summary>
        public Material MaterialX
        {
            get { return Lines[0].Material; }
            set { Lines[0].Material = value; }
        }
        /// <summary>
        /// The material of the the Y-axis
        /// </summary>
        public Material MaterialY
        {
            get { return Lines[1].Material; }
            set { Lines[1].Material = value; }
        }

        /// <summary>
        /// The material of the the Z-axis
        /// </summary>
        public Material MaterialZ
        {
            get { return Lines[2].Material; }
            set { Lines[2].Material = value; }
        }

        /// <summary>
        /// The line axis pointing towards the X-direction.
        /// </summary>
        public LinesBase LineX
        {
            get { return Lines[0]; }
            set { Lines[0] = value; }
        }
        /// <summary>
        /// The line axis pointing towards the Y-direction.
        /// </summary>
        public LinesBase LineY
        {
            get { return Lines[1]; }
            set { Lines[1] = value; }
        }
        /// <summary>
        /// The line axis pointing towards the Z-direction.
        /// </summary>
        public LinesBase LineZ
        {
            get { return Lines[2]; }
            set { Lines[2] = value; }
        }
        /// <summary>
        /// All lines
        /// </summary>
        public LinesBase[] Lines
        {
            get { return _lines; }
        }
        
        #endregion properties

        #region constructors
        public Axis(OpenGL gl)
        {
            RecalculateShape(gl);

            LineX.Material = new Material();
            LineX.Material.Ambient = new ColorF(1f, 1f, 0, 0);

            LineY.Material = new Material();
            LineY.Material.Ambient = new ColorF(1f, 0, 1f, 0);

            LineZ.Material = new Material();
            LineZ.Material.Ambient = new ColorF(1f, 0, 0, 1f);

            var lineWidth = 5f;

            LineX.LineWidth = lineWidth;
            LineY.LineWidth = lineWidth;
            LineZ.LineWidth = lineWidth;
        }

        public Axis(OpenGL gl, Material matX, Material matY, Material matZ, float lineWidth)
        {
            RecalculateShape(gl);

            LineX.Material = matX;
            LineY.Material = matY;
            LineZ.Material = matZ;

            LineX.LineWidth = lineWidth;
            LineY.LineWidth = lineWidth;
            LineZ.LineWidth = lineWidth;
        }
        #endregion constructors


        //public void Render(OpenGL gl, ShaderManagerS1 shader)
        //{
        //    ValidateBeforeRender();

        //    shader.Material = MaterialX;
        //    shader.ApplyChangedProperties(gl);
        //    _lineX.Render(gl, shader);

        //    shader.Material = MaterialY;
        //    shader.ApplyChangedProperties(gl);
        //    _lineY.Render(gl, shader);

        //    shader.Material = MaterialZ;
        //    shader.ApplyChangedProperties(gl);
        //    _lineZ.Render(gl, shader);
        //}



        /// <summary>
        /// Calculates the lines using the current axis properties.
        /// </summary>
        public virtual void RecalculateShape(OpenGL gl)
        {
            var riseAxisValue = 0f;// .0001f; /

            var lineX = new List<Tuple<vec3, vec3>>();
            var lineY = new List<Tuple<vec3, vec3>>();
            var lineZ = new List<Tuple<vec3, vec3>>();

            lineX.Add(
                new Tuple<vec3, vec3>(
                    new vec3(0, riseAxisValue, 0),
                    new vec3(_axisLength, riseAxisValue, 0))
                );

            lineY.Add(
                new Tuple<vec3, vec3>(
                    new vec3(0, riseAxisValue, 0),
                    new vec3(0, _axisLength, 0))
                );

            lineZ.Add(
                new Tuple<vec3, vec3>(
                    new vec3(0, riseAxisValue, 0),
                    new vec3(0, riseAxisValue, _axisLength))
                );


            LineX = new LinesBase(gl, lineX, null);
            LineY = new LinesBase(gl, lineY, null);
            LineZ = new LinesBase(gl, lineZ, null);
        }

        /// <summary>
        /// Ensures that all required properties are acceptable.
        /// </summary>
        private void ValidateBeforeRender()
        {
            if (LineX == null || LineY == null || LineZ == null)
            {
                throw new Exception("Axis aren't calculated.");
            }
        }
    }
}
