using SharpGLHelper.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Events
{
    // A delegate type for hooking up change notifications.
    public delegate void EdgeRemovedEvent(Edge sender, EdgeRemovedEventArgs e);
    public class EdgeRemovedEventArgs : EventArgs
    {

    }
}
