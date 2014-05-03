using SharpGL;
using SharpGLHelper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.SceneElements
{
    /// <summary>
    /// A base class for all elements that can be added to the GL scene
    /// </summary>
    public abstract class OGLSceneElementBase
    {
        #region fields
        private ulong _uniqueId;
        #endregion fields

        #region properties
        public static ulong NextTransformationId { get; set; }
        /// <summary>
        /// The unique id for this matrix during the lifetime of this process.
        /// </summary>
        public ulong UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }


        #endregion properties

        #region events
        #endregion events

        #region constructors

        public OGLSceneElementBase()
        {
            _uniqueId = NextTransformationId++; // set unique id and increment NextId by 1.
        }
        #endregion constructors

    }
}
