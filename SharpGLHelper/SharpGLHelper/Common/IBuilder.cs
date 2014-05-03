using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper
{
    public interface IBuilder
    {
    }
    public interface IVoidBuilder : IBuilder
    {
        void Build();
    }

    public interface IGenericBuilder<T> : IBuilder
    {
        new T Build();
    }
}
