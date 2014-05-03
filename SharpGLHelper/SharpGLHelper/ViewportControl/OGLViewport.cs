using SharpGL;
using SharpGL.Version;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SharpGL.RenderContextProviders;
using System.Windows.Media.Imaging;
using SharpGL.WPF;
using System.Windows.Media;

namespace SharpGLHelper.ViewportControl
{
    public class OGLViewport
    {
        #region fields
        int _viewportWidth;
        int _viewportHeight;
        OpenGL _gl = new OpenGL();
        OpenGLVersion _oglVersion;
        RenderContextType _renderContextType;
        #endregion fields

        #region properties
        public int ViewportWidth
        {
            get { return _viewportWidth; }
            private set { _viewportWidth = value; }
        }

        public int ViewportHeight
        {
            get { return _viewportHeight; }
            private set { _viewportHeight = value; }
        }
        public OpenGL Gl
        {
            get { return _gl; }
            set { _gl = value; }
        }

        public OpenGLVersion OglVersion
        {
            get { return _oglVersion; }
            set { _oglVersion = value; }
        }

        public RenderContextType RenderContextType
        {
            get { return _renderContextType; }
            set { _renderContextType = value; }
        }
        #endregion properties

        #region events
        #endregion events

        #region constructors
        public OGLViewport(OpenGLVersion version = OpenGLVersion.OpenGL2_1, RenderContextType contextType = SharpGL.RenderContextType.FBO)
        {
            OglVersion = version;
            RenderContextType = contextType;

            //  Lock on OpenGL.
            lock (Gl)
            {
                //  Create OpenGL.
                Gl.Create(OglVersion, RenderContextType, 1, 1, 32, null);
            }

            Gl.Enable(OpenGL.GL_DEPTH_TEST);
        }
        #endregion constructors

        public FormatConvertedBitmap GetFrame()
        {
            lock (Gl)
            {
                //  Render.
                Gl.Blit(IntPtr.Zero);

                IntPtr hBitmap = IntPtr.Zero;
                FormatConvertedBitmap newFormatedBitmapSource = null;
                switch (RenderContextType)
                {
                    case RenderContextType.DIBSection:
                    {
                        var provider = Gl.RenderContextProvider as DIBSectionRenderContextProvider;
                        //hBitmap = provider.DIBSection.HBitmap;
                        //break;
                        //  TODO: We have to remove the alpha channel - for some reason it comes out as 0.0 
                        //  meaning the drawing comes out transparent.
                        newFormatedBitmapSource = new FormatConvertedBitmap();
                        newFormatedBitmapSource.BeginInit();
                        newFormatedBitmapSource.Source = BitmapConversion.HBitmapToBitmapSource(provider.DIBSection.HBitmap);
                        newFormatedBitmapSource.DestinationFormat = PixelFormats.Rgb24;
                        newFormatedBitmapSource.EndInit();
                        break;
                    }
                    case RenderContextType.NativeWindow:
                        break;
                    case RenderContextType.HiddenWindow:
                        break;
                    case RenderContextType.FBO:
                    {
                        FBORenderContextProvider provider = Gl.RenderContextProvider as FBORenderContextProvider;
                        //hBitmap = provider.InternalDIBSection.HBitmap;
                        //break;
                        //  TODO: We have to remove the alpha channel - for some reason it comes out as 0.0 
                        //  meaning the drawing comes out transparent.
                        newFormatedBitmapSource = new FormatConvertedBitmap();
                        newFormatedBitmapSource.BeginInit();
                        newFormatedBitmapSource.Source = BitmapConversion.HBitmapToBitmapSource(provider.InternalDIBSection.HBitmap);
                        newFormatedBitmapSource.DestinationFormat = PixelFormats.Rgb24;
                        newFormatedBitmapSource.EndInit();
                        break;
                    }
                }

                //Bitmap res = hBitmap != null ? Bitmap.FromHbitmap(hBitmap) : null;
                //return res;

                return newFormatedBitmapSource ;
            }
        }

        public void Resize(int width, int height)
        {
            //  Lock on OpenGL.
            lock (Gl)
            {
                Gl.SetDimensions(width, height);

                //	Set the viewport.
                Gl.Viewport(0, 0, width, height);

                ViewportHeight = height;
                ViewportWidth = width;
            }
        }
    }
}
