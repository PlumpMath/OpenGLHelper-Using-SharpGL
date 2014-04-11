using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Events
{
    // A delegate type for hooking up change notifications.
    public delegate void MeshPixelsChangedEvent(object sender, MeshPixelsChangedEventArgs e);
    public class MeshPixelsChangedEventArgs : EventArgs
    {
        public MeshPixelsChangedEventArgs()
        {

        }
    }
}
