using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Buffers
{
    public class BufferGroup
    {

        #region fields
        IBO _ibo; //TODO: change to IBO list in order to overcome the 65k limit that .
        List<VBO> _vbos = new List<VBO>();
        #endregion fields

        #region properties
        public IBO Ibo
        {
            get { return _ibo; }
            set 
            { 
                _ibo = value; 
            }
        }

        public List<VBO> Vbos
        {
            get { return _vbos; }
            set { _vbos = value; }
        }


        /// <summary>
        /// This property can be changed to hold the count for instanced element drawing.
        /// </summary>
        public int ParticleCount
        {
            get;
            set;
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors
        public BufferGroup(IBO ibo, IEnumerable<VBO> vbos)
        {
            Ibo = ibo;
            Vbos.AddRange(vbos);
        }
        #endregion constructors
    }
}
