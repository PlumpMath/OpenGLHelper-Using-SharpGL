using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders.ShaderGenerator.Primitives
{
    public class Variable
    {
        #region fields
        string _propertyName;
        GLSLQualifier _qualifier;
        

        GLSLType _propertyType;
        #endregion fields

        #region properties
        public GLSLQualifier Qualifier
        {
            get { return _qualifier; }
            set { _qualifier = value; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public GLSLType PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; }
        }
        #endregion properties

        #region constructor
        public Variable(string stringVarInfo)
        {
            var sections = stringVarInfo.Split(' ');

            if (sections.Length < 2)
                return;

            PropertyName = sections.Last();
            PropertyType = new GLSLType(sections[sections.Length - 2]);

            if (sections.Length < 3)
                return;

            Qualifier = new GLSLQualifier(sections[sections.Length - 3]);
        }

        public Variable(string name, string property, string qualifier = null)
            :this(name, new GLSLType(property), qualifier == null ? null : new GLSLQualifier(qualifier))
        { }

        public Variable(string name, GLSLType property, GLSLQualifier qualifier = null)
        {
            PropertyName = name;
            PropertyType = property;
            Qualifier = qualifier;
        }
        #endregion constructor

        public override string ToString()
        {
            return  _qualifier + " " + _propertyType + " " + _propertyName;
        }
    }
}
