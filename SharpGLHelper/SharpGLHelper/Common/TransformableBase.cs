using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGLHelper.Extensions;

namespace SharpGLHelper.Common
{
    /// <summary>
    /// Contains the rotation, scaling and translation matrices, together with the result produced from multiplying them.
    /// Override RecalculateResultMatrix(...) in order to multiply the result matrix to your vectors/matrices whenever a transformation has been done.
    /// </summary>
    public abstract class TransformableBase
    {
        #region fields
        private mat4 _translationMatrix = mat4.identity();
        private mat4 _rotationMatrix = mat4.identity();
        private mat4 _scalingMatrix = mat4.identity();
        private mat4 _resultMatrix = mat4.identity();

        #endregion fields

        #region properties
        /// <summary>
        /// Contains the result of all transformations.
        /// </summary>
        public mat4 ResultMatrix
        {
            get { return _resultMatrix; }
            set { _resultMatrix = value; }
        }
        /// <summary>
        /// Represents all translations.
        /// </summary>
        public mat4 TranslationMatrix
        {
            get { return _translationMatrix; }
            set { _translationMatrix = value; }
        }
        /// <summary>
        /// Represents all rotations.
        /// </summary>
        public mat4 RotationMatrix
        {
            get { return _rotationMatrix; }
            set { _rotationMatrix = value; }
        }
        /// <summary>
        /// Represents all scalings.
        /// </summary>
        public mat4 ScalingMatrix
        {
            get { return _scalingMatrix; }
            set { _scalingMatrix = value; }
        }
        #endregion properties

        #region absolute rotation
        /// <summary>
        /// Rotates by angleRadians in the direction(s) v and forces a recalculation of the result matrix.
        /// </summary>
        /// <param name="angleRadians">The angle of the rotation in radians.</param>
        /// <param name="v">The vector that represents the axis around which should be rotated.</param>
        public virtual TransformableBase RotateAbsolute(float angleRadians, vec3 v)
        {
            if (angleRadians == 0)
                return this;

            _rotationMatrix = GlmNet.glm.rotate(_rotationMatrix, angleRadians, v);

            return RecalculateResultMatrix();
        }
        /// <summary>
        /// Calls RotateAbsolute(angleRadians, new vec3(1, 0, 0));
        /// </summary>
        /// <param name="angleRadians"></param>
        public TransformableBase RotateAbsoluteX(float angleRadians)
        {
            return RotateAbsolute(angleRadians, new vec3(1, 0, 0));
        }
        /// <summary>
        /// Calls RotateAbsolute(angleRadians, new vec3(0, 1, 0));
        /// </summary>
        /// <param name="angleRadians"></param>
        public TransformableBase RotateAbsoluteY(float angleRadians)
        {
            return RotateAbsolute(angleRadians, new vec3(0, 1, 0));
        }
        
        /// <summary>
        /// RotateAbsolute(angleRadians, new vec3(0, 0, 1));
        /// </summary>
        /// <param name="angleRadians"></param>
        public TransformableBase RotateAbsoluteZ(float angleRadians)
        {
            return RotateAbsolute(angleRadians, new vec3(0, 0, 1));
        }

        #endregion absolute rotation

        #region absolute translation
        /// <summary>
        /// Translates by the values contained in 'vec'.
        /// </summary>
        /// <param name="vec">Translation vector.</param>
        public TransformableBase TranslateAbsolute(vec3 vec)
        {
            _translationMatrix = GlmNet.glm.translate(_translationMatrix, vec);
            return RecalculateResultMatrix();
        }
        /// <summary>
        /// Calls TranslateAbsolute(new vec3(distance, 0, 0));
        /// </summary>
        /// <param name="distance">The distance.</param>
        public TransformableBase TranslateAbsoluteX(float distance)
        {
            return TranslateAbsolute(new vec3(distance, 0, 0));
        }
        /// <summary>
        /// Calls TranslateAbsolute(new vec3(0, distance, 0));
        /// </summary>
        /// <param name="distance">The distance.</param>
        public TransformableBase TranslateAbsoluteY(float distance)
        {
            return TranslateAbsolute(new vec3(0, distance, 0));
        }
        /// <summary>
        /// Calls TranslateAbsolute(new vec3(0, 0, distance));
        /// </summary>
        /// <param name="distance">The distance.</param>
        public TransformableBase TranslateAbsoluteZ(float distance)
        {
            return TranslateAbsolute(new vec3(0, 0, distance));
        }
        #endregion absolute translation

        #region relative translation
        public TransformableBase TranslateOnRotationMatrix(vec3 vec)
        {
            throw new NotImplementedException();
            ////var tMatrix = GlmNet.glm.translate(mat4.identity(), vec);
            ////var invRotMatrix = _rotationMatrix.Inverse();
            ////var resMat = (tMatrix * _rotationMatrix).Transpose(); // 
            ////var translateVector = new vec3(resMat[3].x, resMat[3].y, resMat[3].z);


            //float largestRot;
            //float totRot = RotatedRadians.x + RotatedRadians.y + RotatedRadians.z;
            ////if (RotatedRadians.x > RotatedRadians.y)
            ////{
            ////    if (RotatedRadians.x > RotatedRadians.z)
            ////    {
            ////        // x is largest
            ////        largestRot = RotatedRadians.x;
            ////    }
            ////    else 
            ////    {
            ////        // z is largest
            ////        largestRot = RotatedRadians.z;
            ////    }
            ////}
            ////else {
            ////    if (RotatedRadians.y > RotatedRadians.z)
            ////    {
            ////        // y is largest
            ////        largestRot = RotatedRadians.y;
            ////    }
            ////    else 
            ////    {
            ////        // z is largest
            ////        largestRot = RotatedRadians.z;
            ////    }
            ////}

            //var totMoveDistance = (float)Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);

            //float addX;
            //float addY = 0;
            //float addZ;
            //if (totRot == 0)
            //{
            //    addX = vec.x;
            //    addY = vec.y;
            //    addZ = vec.z;
            //}
            //else
            //{
            //    var scaleX = RotatedRadians.x / totRot;
            //    var scaleY = RotatedRadians.y / totRot;
            //    var scaleZ = RotatedRadians.z / totRot;

            //    addX = vec.x * scaleX;
            //    //addY = vec.x * scaleY;
            //    addZ = vec.x * scaleZ;
            //}
            //vec3 translateVector = new vec3(addX, addY, addZ);




            ////ResMatDeleteMeLater = resMat;
            ////TranslationVecDeleteMeLater = vec;
            ////TranslateAbsolute(translateVector);

            //var t = _translationMatrix[3];
            //t.x += translateVector.x;
            //t.y += translateVector.y;
            //t.z += translateVector.z;
            //_translationMatrix[3] = t;

            //RecalculateResultMatrix();
        }
        #endregion relative translation

        #region absolute scaling
        /// <summary>
        /// Scales by the values contained in 'vec'.
        /// </summary>
        /// <param name="vec">Scaling vector.</param>
        public TransformableBase Scale(vec3 vec)
        {
            _scalingMatrix = GlmNet.glm.scale(_scalingMatrix, vec);
            return RecalculateResultMatrix();
        }
        /// <summary>
        /// Calls Scale(new vec3(scale, 1, 1));
        /// </summary>
        /// <param name="scale"></param>
        public TransformableBase ScaleX(float scale)
        {
            return Scale(new vec3(scale, 1, 1));
        }
        /// <summary>
        /// Calls Scale(new vec3(1, scale, 1));
        /// </summary>
        /// <param name="scale"></param>
        public TransformableBase ScaleY(float scale)
        {
            return Scale(new vec3(1, scale, 1));
        }
        /// <summary>
        /// Calls Scale(new vec3(1, 1, scale));
        /// </summary>
        /// <param name="scale"></param>
        public TransformableBase ScaleZ(float scale)
        {
            return Scale(new vec3(1, 1, scale));
        }
        #endregion absolute scaling

        /// <summary>
        /// Multiplies all transformations into the ResultMatrix in folowing sequence 'ResultMatrix = TranslationMatrix * RotationMatrix * ScalingMatrix'. 
        /// </summary>
        public virtual TransformableBase RecalculateResultMatrix()
        {
            _resultMatrix = _translationMatrix * _rotationMatrix * _scalingMatrix;
            return this;
        }
    }
}
