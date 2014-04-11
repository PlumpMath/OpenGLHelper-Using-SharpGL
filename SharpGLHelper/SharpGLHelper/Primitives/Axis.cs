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
        private LinesBase _lineX, _lineY, _lineZ;
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
            get { return _lineX.Material; }
            set { _lineX.Material = value; }
        }
        /// <summary>
        /// The material of the the Y-axis
        /// </summary>
        public Material MaterialY
        {
            get { return _lineY.Material; }
            set { _lineY.Material = value; }
        }

        /// <summary>
        /// The material of the the Z-axis
        /// </summary>
        public Material MaterialZ
        {
            get { return _lineZ.Material; }
            set { _lineZ.Material = value; }
        }
        #endregion properties

        #region constructors
        public Axis(OpenGL gl)
        {
            RecalculateShape(gl);

            _lineX.Material = new Material();
            _lineX.Material.Ambient = new ColorF(1f, 1f, 0, 0);

            _lineY.Material = new Material();
            _lineY.Material.Ambient = new ColorF(1f, 0, 1f, 0);

            _lineZ.Material = new Material();
            _lineZ.Material.Ambient = new ColorF(1f, 0, 0, 1f);

            var lineWidth = 5f;

            _lineX.LineWidth = lineWidth;
            _lineY.LineWidth = lineWidth;
            _lineZ.LineWidth = lineWidth;
        }

        public Axis(OpenGL gl, Material matX, Material matY, Material matZ, float lineWidth)
        {
            RecalculateShape(gl);

            _lineX.Material = matX;
            _lineY.Material = matY;
            _lineZ.Material = matZ;

            _lineX.LineWidth = lineWidth;
            _lineY.LineWidth = lineWidth;
            _lineZ.LineWidth = lineWidth;
        }
        #endregion constructors


        public void Render(OpenGL gl, RenderMode renderMode, ExtShaderProgram shader)
        {
            // Ensure that we're in design mode (we don't want axis during render)
            if (renderMode != RenderMode.Design)
                return;

            ValidateBeforeRender();

            shader.ApplyMaterial(gl, MaterialX);
            _lineX.Render(gl, renderMode, shader);
                
            shader.ApplyMaterial(gl, MaterialY);
            _lineY.Render(gl, renderMode, shader);

            shader.ApplyMaterial(gl, MaterialZ);
            _lineZ.Render(gl, renderMode, shader);
        }



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


            _lineX = new LinesBase(gl, lineX, null);
            _lineY = new LinesBase(gl, lineY, null);
            _lineZ = new LinesBase(gl, lineZ, null);
        }

        /// <summary>
        /// Ensures that all required properties are acceptable.
        /// </summary>
        private void ValidateBeforeRender()
        {
            if (_lineX == null || _lineY == null || _lineZ == null)
            {
                throw new Exception("Axis aren't calculated.");
            }
        }
    }
}
