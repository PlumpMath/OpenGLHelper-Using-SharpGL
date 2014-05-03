using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Buffers
{
    public class OGLBufferId
    {
        #region fields
        uint? _bufferObjectId;
        #endregion fields

        #region properties
        public uint Value
        {
            get { return _bufferObjectId.Value; }
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors

        private OGLBufferId (uint bufferObjectId)
        {
            _bufferObjectId = bufferObjectId;
        }

        public static OGLBufferId CreateBufferId(OpenGL gl)
        {
            return CreateBufferIds(gl, 1)[0];
        }

        public static OGLBufferId[] CreateBufferIds(OpenGL gl, int amount)
        {
            uint[] ids = new uint[amount];
            OGLBufferId[] bIds = new OGLBufferId[amount];
            gl.GenBuffers(amount, ids);

            for (int i = 0; i < amount; i++)
            {
                bIds[i] = new OGLBufferId(ids[i]);
            }

            return bIds;
        }
        #endregion constructors


        public static void DeleteBuffers(OpenGL gl, IEnumerable<OGLBufferId> bufferIds)
        {
            var ids = bufferIds.Select<OGLBufferId, uint>(x => x.Value).ToArray();
            gl.DeleteBuffers(ids.Length, ids);

        }

        public static void DeleteBuffers(OpenGL gl, IEnumerable<OGLBufferObject> bufferObjecs)
        {
            var ids = bufferObjecs.Select<OGLBufferObject, uint>(x => x.BufferId.Value).ToArray();
            gl.DeleteBuffers(ids.Length, ids);

        }

    }
}
