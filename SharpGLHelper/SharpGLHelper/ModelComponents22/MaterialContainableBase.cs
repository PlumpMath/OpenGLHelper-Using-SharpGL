using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.ModelComponents2
{
    public abstract class MaterialContainableBase
    {
        private Material _material = null;

        /// <summary>
        /// Contains the material for the object. Settings this automatically applies the changes to the Material.Users.
        /// </summary>
        public Material Material
        {
            get { return _material; }
            set
            {
                if (value == null)
                {
                    // Remove the binding.
                    Material.Users.Remove(this);
                }
                else if (_material != null)
                {
                    // Remove old binding and add the new one.
                    Material.Users.Remove(this);
                    Material.Users.AddLast(this);
                }
                else
                {
                    // Add new binding.
                    Material.Users.AddLast(this);
                }

                _material = value;
            }
        }
    }
}
