using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Buffers
{
    public class VAO
    {

        #region fields
        OGLBufferId _bufferId;
        int _size = -1;
        #endregion fields

        #region properties
        /// <summary>
        /// 
        /// </summary>
        public OGLBufferId BufferId
        {
            get { return _bufferId; }
            set { _bufferId = value; }
        }

        /// <summary>
        /// The size of the content array. 
        /// If it returns -1 means that SetBufferData(...) hasn't been called.
        /// </summary>
        public int Size
        {
            get { return _size; }
            protected set { _size = value; }
        }
        #endregion properties

        public VAO()
        {
            throw new NotImplementedException("Not supported by SharpGL on 30/04/2014");
            /*
             * Tutorial regarding VAO's
             * http://ogldev.atspace.co.uk/www/tutorial32/tutorial32.html
             * 
             * Missing required methods:
             * http://www.opengl.org/sdk/docs/man/html/glDrawElementsBaseVertex.xhtml
             * 
             * 
             */
        }

        public void BindBuffer(SharpGL.OpenGL gl, Action executeWhileBinded)
        {
            gl.BindVertexArray(BufferId.Value);

            executeWhileBinded.Invoke();


            gl.BindVertexArray(0); // Unbind.
        }
    }
}
