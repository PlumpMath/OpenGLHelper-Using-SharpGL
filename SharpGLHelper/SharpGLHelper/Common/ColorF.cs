using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper.Common
{
    public class ColorF
    {
        #region fields
        float _a, _r, _g, _b;
        #endregion fields

        #region properties
        public float A
        {
            get { return _a; }
            set { _a = value; }
        }

        public float R
        {
            get { return _r; }
            set { _r = value; }
        }

        public float G
        {
            get { return _g; }
            set { _g = value; }
        }

        public float B
        {
            get { return _b; }
            set { _b = value; }
        }
        #endregion properties

        #region constructors
        public ColorF()
        { }

        /// <summary>
        /// Converts uint to ColorF. Second byte = blue, Thirth byte = green, Last byte = red.
        /// </summary>
        /// <param name="colorRGB"></param>
        public ColorF(uint colorRGB)
        {
            // Get the integer ID
            var i = colorRGB;

            int b = (int)(i >> 16) & 0xFF;
            int g = (int)(i >> 8) & 0xFF;
            int r = (int)i & 0xFF;

            IntToFloatColor(255, r, g, b);
        }

        public ColorF(int a, int r, int g, int b)
        {
            IntToFloatColor(a, r, g, b);
        }

        public ColorF(float a, float r, float g, float b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public ColorF(System.Drawing.Color color)
            : this(color.A, color.R, color.G, color.B)
        { }
        public ColorF(System.Windows.Media.Color color)
            : this(color.A, color.R, color.G, color.B)
        { }


        #endregion constructors

        private void IntToFloatColor(int a, int r, int g, int b)
        {
            A = a / 255.0f;
            R = r / 255.0f;
            G = g / 255.0f;
            B = b / 255.0f;
        }

        public uint ToUint()
        {
            // Get color id from pixel data.
            return (uint)(R * 255 + G * 65025 + B * 16581375); // r * 255 + g * 255² + b * 255³.
        }
    }
}
