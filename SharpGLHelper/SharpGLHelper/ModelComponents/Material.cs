using SharpGLHelper.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class Material
    {
        #region fields
        static ColorF _defaultColor = new ColorF(0, 0, 0, 0);
        float shininess;
        ColorF _ambient, diffuse, specular, emission;
        ObservableLinkedList<MaterialContainableBase> _users = new ObservableLinkedList<MaterialContainableBase>();
        #endregion fields

        #region properties
        public ColorF Ambient
        {
            get { return _ambient; }
            set { _ambient = value; }
        }

        public ColorF Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        public ColorF Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        public ColorF Emission
        {
            get { return emission; }
            set { emission = value; }
        }

        public float Shininess
        {
            get { return shininess; }
            set { shininess = value; }
        }
        // A list of objects that are currently using this material.
        public ObservableLinkedList<MaterialContainableBase> Users
        {
            get 
            { 
                return _users; 
            }
        } 
        #endregion properties

        #region constructors
        /// <summary>
        /// Default constructor. No users will be recorded.
        /// </summary>
        public Material()
        {
            _users.CollectionChanged += Users_CollectionChanged;

            Ambient = _defaultColor;
            Diffuse = _defaultColor;
            Specular = _defaultColor;
            Emission = _defaultColor;
        }
        /// <summary>
        /// Create a material and add users of this material.
        /// </summary>
        /// <param name="users"></param>
        public Material(IEnumerable<MaterialContainableBase> users)
            : this(users, _defaultColor, _defaultColor, _defaultColor, _defaultColor, 0)
        {
        }
        public Material(ColorF ambient, ColorF diffuse, ColorF specular, ColorF emission, float shininess)
            : this(null, ambient, diffuse, specular, emission, shininess)
        {}

        public Material(IEnumerable<MaterialContainableBase> users, ColorF ambient, ColorF diffuse, ColorF specular, ColorF emission, float shininess)
            : this()
        {
            if (users != null)
            {
                foreach (var user in users)
                {
                    _users.AddLast(user);
                }
            }

            Ambient = ambient == null? _defaultColor : ambient;
            Diffuse = diffuse == null? _defaultColor : diffuse;
            Specular = specular == null? _defaultColor : specular;
            Emission = emission == null? _defaultColor : emission;
            Shininess = shininess;
        }
        #endregion constructors



        void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        _users.AddLast(item as MaterialContainableBase);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        _users.Remove(item as MaterialContainableBase);
                    }
                    break;
                // Replace is not needed, since it remains an item anyway.
            }
        }
    }
}
