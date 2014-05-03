using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders
{
    public class ParameterIds
    {
        #region shader parameter id's
        public virtual string AmbientId { get { return "AmbientMaterial"; } }
        public virtual string DiffuseId { get { return "DiffuseMaterial"; } }
        public virtual string SpecularId { get { return "SpecularMaterial"; } }
        public virtual string EmissionId { get { return "Emission"; } }
        public virtual string ShininessId { get { return "Shininess"; } }

        public virtual string TransformationMatrixId { get { return "TransformationMatrix"; } }

        public virtual string ProjectionMatrixId { get { return "Projection"; } }
        public virtual string ModelviewMatrixId { get { return "Modelview"; } }
        public virtual string NormalMatrixId { get { return "NormalMatrix"; } }

        public virtual string LightPositionId { get { return "LightPosition"; } }
        #endregion shader parameter id's
    }
}
