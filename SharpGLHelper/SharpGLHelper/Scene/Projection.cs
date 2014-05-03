using GlmNet;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Scene
{
    /// <summary>
    /// This class contains all functionality related to the projection matrix.
    /// </summary>
    public class Projection : EqualityComparer<Projection>
    {
        #region fields
        mat4 _projectionMatrix = mat4.identity();
        float _left, _right, _bottom, _top,
            _nearVal = 2f, 
            _farVal = 100;//-1.2f;
        int screenWidth, screenHeight;

        vec3 _translationVector = new vec3();

        #endregion fields

        #region properties
        /// <summary>
        /// The Projection Matrix.
        /// </summary>
        public mat4 ProjectionMatrix
        {
            get { return _projectionMatrix; }
            set { _projectionMatrix = value; }
        }
        
        /// <summary>
        /// The Left.
        /// Used in GlmNET.glm.frustum(...);
        /// </summary>
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// The Right.
        /// Used in GlmNET.glm.frustum(...);
        /// </summary>
        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// The Bottom.
        /// Used in GlmNET.glm.frustum(...);
        /// </summary>
        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        /// The Top.
        /// Used in GlmNET.glm.frustum(...);
        /// </summary>
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// The Near Val.
        /// Used in GlmNET.glm.frustum(...);
        /// </summary>
        public float NearVal
        {
            get { return _nearVal; }
            set { _nearVal = value; }
        }

        /// <summary>
        /// The Far val.
        /// Used in GlmNET.glm.frustum(...);
        /// </summary>
        public float FarVal
        {
            get { return _farVal; }
            set { _farVal = value; }
        }
        public int ScreenWidth
        {
            get { return screenWidth; }
            set { screenWidth = value; }
        }

        public int ScreenHeight
        {
            get { return screenHeight; }
            set { screenHeight = value; }
        }


        #endregion properties


        /// <summary>
        /// Resets the frustum using the current values for NearVal and FarVal.
        /// </summary>
        /// <param name="screenWidth">Width of the screen.</param>
        /// <param name="screenHeight">Height of the screen.</param>
        public void SetFrustum(float screenWidth, float screenHeight)
        {
            SetFrustum(screenWidth, screenHeight, _nearVal, _farVal);
        }

        /// <summary>
        /// Resets the frustum.
        /// </summary>
        /// <param name="screenWidth">Width of the screen.</param>
        /// <param name="screenHeight">Height of the screen.</param>
        /// <param name="nearVal"></param>
        /// <param name="farVal"></param>
        public void SetFrustum(float screenWidth, float screenHeight, float nearVal = 1, float farVal = 0)
        {
            float scale = 1 / screenWidth;
            ScreenWidth = (int)screenWidth;
            ScreenHeight = (int)screenHeight;

            screenWidth *= scale;
            screenHeight *= scale;

            Left = -screenWidth;
            Right = -Left;
            Bottom = -screenHeight;
            Top = -Bottom;
            NearVal = nearVal;
            FarVal = farVal;
            CalculateFrustum();
        }

        /// <summary>
        /// Recalculate the frustum from the available properties.
        /// </summary>
        public void CalculateFrustum()
        {
            ProjectionMatrix = CalculateFOVProjection(ScreenWidth, ScreenHeight, 0.1f, NearVal, FarVal);
            //ProjectionMatrix = glm.frustum(Left, Right , Bottom , Top , NearVal, FarVal);

            glm.translate(ProjectionMatrix, _translationVector);
        }

        public mat4 CalculateFOVProjection(int sceneWidth, int sceneHeight, float nearHeight, float nearDist, float farDist, bool leftHanded = true)
        {
            float fov = (float)Math.Atan(nearHeight / (2 * nearDist)) * 0.3f;
            float aspect = sceneWidth / (float)sceneHeight;

            //
            // General form of the Projection Matrix
            //
            // uh = Cot( fov/2 ) == 1/Tan(fov/2)
            // uw / uh = 1/aspect
            // 
            //   uw         0       0       0
            //    0        uh       0       0
            //    0         0      f/(f-n)  1
            //    0         0    -fn/(f-n)  0
            //
            // Make result to be identity first

            // check for bad parameters to avoid divide by zero:
            if ( fov <= 0)
            {
                throw new Exception("Field Of View (FOV) must be more than 0.");
            }
            else if(aspect == 0 )
            {
                throw new Exception("Aspect cannot be equal to 0.");
            }



            float frustumDepth = farDist - nearDist;
            float oneOverDepth = 1 / frustumDepth;
            var vecs = new vec4[] { new vec4(), new vec4(), new vec4(), new vec4() };

            vecs[1][1] = 1 / (float)Math.Tan(0.5f * fov);
            vecs[0][0] = (leftHanded ? 1 : -1) * vecs[1][1] / aspect;
            vecs[2][2] = farDist * oneOverDepth;
            vecs[3][2] = (-farDist * nearDist) * oneOverDepth;
            vecs[2][3] = 1;
            vecs[3][3] = 0;

            return new mat4(vecs);
        }

        /// <summary>
        /// Changes the z values from the TranslationVector, resulting in a zoom effect.
        /// </summary>
        /// <param name="distance">Zooming distance (positive => zoom in, negative => zoom out)</param>
        public void Zoom(float distance)
        {
            _translationVector.z += distance;
            CalculateFrustum();
        }

        /// <summary>
        /// Converts ProjectionMatrix to a float[].
        /// </summary>
        /// <returns>A float[] containing all the values from the initial matrix.</returns>
        public float[] ToArray()
        {
            return ProjectionMatrix.to_array();
        }

        public override bool Equals(Projection x, Projection y)
        {
            return x.Left == y.Left && x.Right == y.Right && x.Bottom == y.Bottom && x.Top == y.Top && 
                x.NearVal == y.NearVal && x.FarVal == y.FarVal && x.ScreenWidth == y.ScreenWidth && x.ScreenHeight == y.ScreenHeight;
        }

        public override int GetHashCode(Projection obj)
        {
            throw new NotImplementedException();
        }
    }
}
