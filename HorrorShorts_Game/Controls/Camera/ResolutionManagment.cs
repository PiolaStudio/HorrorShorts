using Assimp;
using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Controls.UI.Questions;
using Microsoft.Xna.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HorrorShorts_Game.Settings;

namespace HorrorShorts_Game.Controls.Camera
{
    public class ResolutionManagment
    {
        public float AjustOriginY
        {
            get => _ajustOriginY;
            set
            {
                if (_ajustOriginY == value) return;
                _ajustOriginY = value;
                CalculateResolution();
            }
        }
        private float _ajustOriginY = 1f;

        private Matrix _matrix;
        private Rectangle _bounds;
        private Rectangle _clickZone;

        private const float MIN_ASPECT_RAIO = 320f / 160f;

        public Matrix Matrix { get => _matrix; }
        public Rectangle Bounds { get => _bounds; }
        public Rectangle ClickZone { get => _clickZone; }

        internal void SetResolution(int width, int height, ResizeModes resizeMode, bool resizable)
        {
            CheckResolution(ref width, ref height);

            Settings settings = Core.Settings;

            settings.ResolutionWidth = width;
            settings.ResolutionHeight = height;
#if DESKTOP
            settings.ResizeMode = resizeMode;
#endif

            Core.GraphicsDeviceManager.PreferredBackBufferWidth = settings.ResolutionWidth;
            Core.GraphicsDeviceManager.PreferredBackBufferHeight = settings.ResolutionHeight;

            bool fullscreen = resizeMode == ResizeModes.FullScreen;

#if CONSOLE || PHONE
            Core.GraphicsDeviceManager.IsFullScreen = fullscreen;
#endif
#if DESKTOP
            if (fullscreen != Core.GraphicsDeviceManager.IsFullScreen)
            {
                //Core.GraphicsDeviceManager.IsFullScreen = fullscreen;
                Core.GraphicsDeviceManager.ToggleFullScreen();
            }
#endif

#if DESKTOP
            Core.Window.IsBorderless = resizeMode == ResizeModes.Bordeless;
            Core.Window.AllowUserResizing = resizeMode == ResizeModes.Window && resizable;

            if (resizeMode == ResizeModes.Bordeless)
            {
                Core.Window.IsBorderless = true;
                Core.Window.Position = Point.Zero;
            }
            else Core.Window.IsBorderless = false;
#endif


            CalculateResolution();
        }
        private void CheckResolution(ref int width, ref int height)
        {
            if (width < Settings.NativeResolution.Width)
                width = Settings.NativeResolution.Width;

            if (width / (float)height > MIN_ASPECT_RAIO)
                height = Convert.ToInt32(width / MIN_ASPECT_RAIO);
        }
        private void CalculateResolution()
        {
            //Calculate Resoultion Camera
            Settings settings = Core.Settings;
            int width = settings.ResolutionWidth ;
            int height = settings.ResolutionHeight;
            Rectangle nativeResolution = Settings.NativeResolution;

            float ratioAspect = width / (float)height;
            float nativeRatioAspect = nativeResolution.Width / (float)nativeResolution.Height;

            int baseHeight = (int)Math.Ceiling(nativeResolution.Width / ratioAspect);

            float scale = width / (float)nativeResolution.Width;

            int heightDif = nativeResolution.Height - baseHeight;
            float posY;
            if (ratioAspect < nativeRatioAspect)
            {
                posY = (-heightDif * scale) / 2;
                _clickZone = new(0, (int)posY, width, Convert.ToInt32(height - posY * 2));
            }
            else
            {
                posY = -heightDif * _ajustOriginY * scale;
                _clickZone = new(0, 0, width, height);
            }

            _bounds = new(0,
                          Math.Max(Convert.ToInt32(heightDif * _ajustOriginY), 0),
                          nativeResolution.Width,
                          Math.Min(nativeResolution.Height, baseHeight));

            _matrix = Matrix.CreateScale(scale) * Matrix.CreateTranslation(0, posY, 0);
        }
    }
}
