using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Common
{
    public class TransformationMatrix : TransformableBase
    {
        #region fields
        private bool _changesHandled;
        private ulong _uniqueId;
        #endregion fields

        #region properties

        /// <summary>
        /// This holds the next used tranformationId and will increment by 1 for each new Transformation matrix. 
        /// You may change it, but usually there should be no reason to do so. Please note that you may lose the uniqueness of a matrix by changing this value.
        /// </summary>
        public static ulong NextTransformationId { get; set; }

        /// <summary>
        /// The unique id for this matrix during the lifetime of this process.
        /// </summary>
        public ulong UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        public bool ChangesHandled
        {
            get { return _changesHandled; }
            set { _changesHandled = value; }
        }
        #endregion properties

        public TransformationMatrix()
        {
            _uniqueId = NextTransformationId++; // set unique id and increment NextId by 1.
        }

        public override TransformableBase RecalculateResultMatrix()
        {
            _changesHandled = false;
            return base.RecalculateResultMatrix();
        }
    }
}
