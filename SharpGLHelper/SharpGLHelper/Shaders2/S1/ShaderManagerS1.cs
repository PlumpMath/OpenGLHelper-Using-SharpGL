using GlmNet;
using SharpGL;
using SharpGLHelper.Common;
using SharpGLHelper.ModelComponents;
using SharpGLHelper.SceneElements;
using SharpGLHelper.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Shaders2.S1
{
    /// <summary>
    /// Requires Modelview matrix, Projection Matrix, Normal Matrix, Material, TransformationMatrix
    /// </summary>
    public class ShaderManagerS1: ShaderManagerBase
    {
        #region fields
        List<ElementTransformations> _elementsInShader = new List<ElementTransformations>();

        Material _material = new Material();
        mat4 _transformationMatrix = mat4.identity();
        mat4 _projectionMatrix = mat4.identity();
        mat4 _modelviewMatrix = mat4.identity();
        mat3 _normalMatrix = mat3.identity();
        vec3 _lightPosition = new vec3();

        #endregion fields

        #region properties
        public List<ElementTransformations> ElementsInShader
        {
            get { return _elementsInShader; }
            set 
            {
                if (value == null)
                    _elementsInShader.Clear();
                else
                    _elementsInShader = value.OrderBy(x=>x.SceneElement.Material.ToString()).ToList(); 
            }
        }
        public Material Material
        {
            get { return _material; }
            set
            {
                if (value != _material)
                {
                    if (_material.Ambient != value.Ambient)
                        UpdateActions.Add(ApplyAmbient);
                    if (_material.Diffuse != value.Diffuse)
                        UpdateActions.Add(ApplyDiffuse);
                    if (_material.Emission != value.Emission)
                        UpdateActions.Add(ApplyEmission);
                    if (_material.Shininess != value.Shininess)
                        UpdateActions.Add(ApplyShininess);
                    if (_material.Specular != value.Specular)
                        UpdateActions.Add(ApplySpecular);

                    //UpdateActions.Add(ApplyAmbient);
                    //UpdateActions.Add(ApplyDiffuse);
                    //UpdateActions.Add(ApplyEmission);
                    //UpdateActions.Add(ApplyShininess);
                    //UpdateActions.Add(ApplySpecular);
                     _material = value;
                }
            }
        }

        public mat4 TransformationMatrix
        {
            get { return _transformationMatrix; }
            set
            {
                if (!value.Equals(_transformationMatrix))
                {
                    UpdateActions.Add(ApplyTransformationMatrix);

                    _transformationMatrix = value;
                }
            }
        }

        public mat4 ModelviewMatrix
        {
            get { return _modelviewMatrix; }
            set
            {
                if (!value.Equals(_modelviewMatrix))
                {
                    UpdateActions.Add(ApplyModelviewMatrix);

                    _modelviewMatrix = value;
                }
            }
        }
        public mat4 ProjectionMatrix
        {
            get { return _projectionMatrix; }
            set
            {
                if (!value.Equals(_projectionMatrix))
                {
                    UpdateActions.Add(ApplyProjectionMatrix);

                    _projectionMatrix = value;
                }
            }
        }
        public mat3 NormalMatrix
        {
            get { return _normalMatrix; }
            set
            {
                if (!value.Equals(_normalMatrix))
                {
                    UpdateActions.Add(ApplyNormalMatrix);

                    _normalMatrix = value;
                }
            }
        }
        public vec3 LightPosition
        {
            get { return _lightPosition; }
            set
            {
                if (!value.Equals(_lightPosition))
                {
                    UpdateActions.Add(ApplyLightPosition);

                    _lightPosition = value;
                }
            }
        }

        public override bool HasChanges
        {
            get
            {
                return UpdateActions.Count > 0 && ElementsInShader.Any(x => x.ChangesHandled == false);
            }
        }

        public bool ChangesHandled
        {
            get { return HasChanges; }
            set
            {
                UpdateActions.Clear();

                foreach (var elem in ElementsInShader)
                {
                    elem.ChangesHandled = true;
                }
            }
        }
        #endregion properties

        #region shader parameter id's
        public string AmbientId { get { return "AmbientMaterial"; } }
        public string DiffuseId { get { return "DiffuseMaterial"; } }
        public string SpecularId { get { return "SpecularMaterial"; } }
        public string EmissionId { get { return "Emission"; } }
        public string ShininessId { get { return "Shininess"; } }

        public string TransformationMatrixId { get { return "TransformationMatrix"; } }

        public string ProjectionMatrixId { get { return "Projection"; } }
        public string ModelviewMatrixId { get { return "Modelview"; } }
        public string NormalMatrixId { get { return "NormalMatrix"; } }

        public string LightPositionId { get { return "LightPosition"; } }
        #endregion shader parameter id's

        #region events
        #endregion events

        #region constructors

        public ShaderManagerS1(OpenGL gl)
            : base(gl, "S1")
        { }
        #endregion constructors

        public override void RenderAll(OpenGL gl)
        {
            ChangesHandled = true;

            UseProgram(gl, () =>
            {
                ApplyChangedProperties(gl);

                foreach (var item in _elementsInShader)
                {

                    foreach (var transformation in item.Transformations)
                    {
                        TransformationMatrix = transformation.ResultMatrix;
                        ApplyChangedProperties(gl);
                        (item.SceneElement as OGLVisualSceneElementBase).Bind(gl);
                    }
                }
            });

        }
        public override void RenderAll(OpenGL gl, Action executedSequence)
        {
            UseProgram(gl, executedSequence);
        }

        public override void ApplyChangedProperties(OpenGL gl)
        {
            if (!ProgramIsBound)
                throw new Exception("This method cannot be called outside a program. Consider using this inside RenderAll(OpenGL, Action).");

            foreach (var act in UpdateActions)
            {
                act.Invoke(gl);
            }

            UpdateActions.Clear();
        }


        #region applying material properties
        public void ApplyAmbient(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = AmbientId;
            var val = Material.Ambient;

            if (id != null && val != null)
                p.SetUniform3(gl, id, val.R, val.G, val.B);
        }
        public void ApplyDiffuse(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = DiffuseId;
            var val = Material.Diffuse;

            if (id != null && val != null)
                p.SetUniform3(gl, id, val.R, val.G, val.B);
        }
        public void ApplySpecular(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = SpecularId;
            var val = Material.Specular;

            if (id != null && val != null)
                p.SetUniform3(gl, id, val.R, val.G, val.B);
        }
        public void ApplyEmission(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = EmissionId;
            var val = Material.Emission;

            if (id != null && val != null)
                p.SetUniform3(gl, id, val.R, val.G, val.B);
        }
        public void ApplyShininess(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = ShininessId;
            var val = Material.Shininess;

            if (id != null)
                p.SetUniform1(gl, id, val);
        }

        #endregion applying material properties

        #region applying transformation matrix
        public void ApplyTransformationMatrix(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = TransformationMatrixId;
            var val = TransformationMatrix;

            p.SetUniformMatrix4(gl, id, val.to_array());
        }
        #endregion applying transformation matrix

        #region applying MVP and Normal- matrices
        public void ApplyModelviewMatrix(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = ModelviewMatrixId;
            var val = ModelviewMatrix;

            p.SetUniformMatrix4(gl, id, val.to_array());
        }
        public void ApplyProjectionMatrix(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = ProjectionMatrixId;
            var val = ProjectionMatrix;

            p.SetUniformMatrix4(gl, id, val.to_array());
        }
        public void ApplyNormalMatrix(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = NormalMatrixId;
            var val = NormalMatrix;

            p.SetUniformMatrix3(gl, id, val.to_array());
        }
        #endregion applying MVP and Normal- matrices
        public void ApplyLightPosition(OpenGL gl)
        {
            var p = ShaderProgram;
            var id = LightPositionId;
            var val = LightPosition;

            p.SetUniform3(gl, id, val.x, val.y, val.z);
        }
    }
}
