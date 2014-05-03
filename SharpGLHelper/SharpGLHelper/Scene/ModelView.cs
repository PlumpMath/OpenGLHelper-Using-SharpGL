using GlmNet;
using SharpGLHelper.Common;
using SharpGLHelper.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Scene
{
    /// <summary>
    /// The sequence in which the rotation matrix calculation occurs. SRT = Look around eye, TRS = Look around center.
    /// </summary>
    public enum CameraRotationSequence { SRT, TRS }
    public enum RotationMethod { TurnTableYZ, TurnTableXZ, TurnTableXY, TrackBall, Simple }


    /// <summary>
    /// This class contains all functionality related to the ModelView matrix.
    /// </summary>
    public class ModelView : TransformableBase,INotifyPropertyChanged
    {
        #region events

        #region propertychanged event
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion propertychanged event

        #endregion events

        #region fields
        CameraRotationSequence _cameraHandling = CameraRotationSequence.SRT;
        private RotationMethod _rotationMethod = RotationMethod.Simple;
        private vec3 _rotatedRadians = new vec3(0, 0, 0);
        #endregion fields

        #region properties
        /// <summary>
        /// The ModelView Matrix.
        /// </summary>
        public mat4 ModelviewMatrix
        {
            get { return ResultMatrix; }
        }

        /// <summary>
        /// Get or set the sequence of the modelview rotation transformation.
        /// </summary>
        public CameraRotationSequence CameraHandling
        {
            get { return _cameraHandling; }
            set { _cameraHandling = value; }
        }
        /// <summary>
        /// Defines the way of rotating, TurnTable is usually more intuitive for camera control. TrackBall provides more freedom.
        /// </summary>
        public RotationMethod RotationMethod
        {
            get { return _rotationMethod; }
            set { _rotationMethod = value; }
        }
        /// <summary>
        /// Gets the value of rotation (radians) in each direction.
        /// </summary>
        public vec3 RotatedRadians
        {
            get { return _rotatedRadians; }
            protected set { _rotatedRadians = value; }
        }
        #endregion properties

        /// <summary>
        /// Converts ModelviewMatrix to a float[].
        /// </summary>
        /// <returns>A float[] containing all the values from the initial matrix.</returns>
        public float[] ToArray()
        {
            return ModelviewMatrix.to_array();
        }
       
        /// <summary>
        /// Converst the this.ToArray() from a float[] to a double[]
        /// </summary>
        /// <returns></returns>
        public double[] ToDoubleArray()
        {
            float[] arr = ToArray();
            var newArr = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = (double)arr[i];
            }

            return newArr;
        }

        /// <summary>
        /// Modelview needs different sequences of transformations.
        /// </summary>
        public override TransformableBase RecalculateResultMatrix()
        {
            if (CameraHandling == Scene.CameraRotationSequence.TRS)
                ResultMatrix = TranslationMatrix * RotationMatrix * ScalingMatrix; // rotate around center
            else if (CameraHandling == Scene.CameraRotationSequence.SRT)
                ResultMatrix = ScalingMatrix * RotationMatrix * TranslationMatrix; // rotate around camera center

            //ResultMatrix = ScalingMatrix * RotationMatrix * TranslationMatrix; // rotate around camera center
            //ResultMatrix = TranslationMatrix * ScalingMatrix * RotationMatrix; // rotate around camera center (swap axis)
            //ResultMatrix = TranslationMatrix * RotationMatrix * ScalingMatrix; // rotate around own center
            //ResultMatrix = RotationMatrix * ScalingMatrix * TranslationMatrix; // rotate around own center (swap axis)
            //ResultMatrix = RotationMatrix * TranslationMatrix * ScalingMatrix; // stretches shape (swap axis)
            //ResultMatrix = ScalingMatrix * TranslationMatrix * RotationMatrix; // stretches shape (swap axis)
            //ResultMatrix = RotationMatrix * TranslationMatrix.Transpose() * ScalingMatrix; // stretches shape (swap axis)
            //ResultMatrix = ScalingMatrix * TranslationMatrix.Transpose() * RotationMatrix; // stretches shape (swap axis)

            OnPropertyChanged("ModelviewMatrix");

            return this;
        }

        public override TransformableBase RotateAbsolute(float angleRadians, vec3 v)
        {
            if (angleRadians == 0)
                return this;

            mat4 tempRotationMatrix = RotationMatrix;
            switch (_rotationMethod)
            {
                case RotationMethod.TurnTableXY:
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, -RotatedRadians.x, new vec3(1, 0, 0));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, -RotatedRadians.y, new vec3(0, 1, 0));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, angleRadians, v);
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, RotatedRadians.y, new vec3(0, 1, 0));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, RotatedRadians.x, new vec3(1, 0, 0));
                    break;
                case RotationMethod.TurnTableXZ:
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, -RotatedRadians.x, new vec3(1, 0, 0));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, -RotatedRadians.z, new vec3(0, 0, 1));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, angleRadians, v);
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, RotatedRadians.z, new vec3(0, 0, 1));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, RotatedRadians.x, new vec3(1, 0, 0));
                    break;
                case RotationMethod.TurnTableYZ:
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, -RotatedRadians.y, new vec3(0, 1, 0));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, -RotatedRadians.z, new vec3(0, 0, 1));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, angleRadians, v);
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, RotatedRadians.z, new vec3(0, 0, 1));
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, RotatedRadians.y, new vec3(0, 1, 0));
                    break;
                case RotationMethod.TrackBall:
                    break;
                default: // Simple
                    tempRotationMatrix = GlmNet.glm.rotate(tempRotationMatrix, angleRadians, v);
                    break;
            }

            // Set the result as the current rotation matrix.
            RotationMatrix = tempRotationMatrix;

            // Keep count of the rotated radians in each direction.
            _rotatedRadians.x += v.x * angleRadians;
            _rotatedRadians.y += v.y * angleRadians;
            _rotatedRadians.z += v.z * angleRadians;

            return RecalculateResultMatrix();
        }

        /// <summary>
        /// Allow direct array access to the ModelviewMatrix.
        /// </summary>
        /// <param name="col">Collumn index.</param>
        /// <returns>The vec4 at this position.</returns>
        public vec4 this[int col]{
            get
            {
                return ModelviewMatrix[col];
            }
        }
    }
}
