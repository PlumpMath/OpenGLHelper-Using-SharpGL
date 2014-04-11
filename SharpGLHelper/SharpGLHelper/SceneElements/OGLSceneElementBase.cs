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
        //#region fields
        //List<TransformationMatrix> _transformationMatrices = new List<TransformationMatrix>();
        //#endregion fields

        //#region properties
        ///// <summary>
        ///// The transformations that can be applied to this scene element.
        ///// </summary>
        //public List<TransformationMatrix> TransformationMatrices
        //{
        //    get { return _transformationMatrices; }
        //    set { _transformationMatrices = value; }
        //}
        //#endregion properties

        //public TransformationMatrix AddNewTransformationMatrix()
        //{
        //    var tm = new TransformationMatrix();
        //    TransformationMatrices.Add(tm);

        //    return tm;

        //}



        private ulong _uniqueId;

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
        #endregion properties


        public OGLSceneElementBase()
        {
            _uniqueId = NextTransformationId++; // set unique id and increment NextId by 1.
        }
    }
}
