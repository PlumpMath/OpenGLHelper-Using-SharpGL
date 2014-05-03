using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Events
{
    // A delegate type for hooking up change notifications.
    public delegate void MeshChangedEvent(object sender, MeshChangedEventArgs e);
    public class MeshChangedEventArgs : EventArgs
    {
        public MeshChangedEventArgs()
        {

        }
    }
}
