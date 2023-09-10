using Assimp;
using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Controls.UI.Questions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Camera
{
    public class ResolutionManagment
    {
        private float _ajustOriginY = 1f;
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

        private Matrix _matrix;
        private Rectangle _bounds;
        private Rectangle _clickZone;

        public Matrix Matrix { get => _matrix; }
        public Rectangle Bounds { get => _bounds; }
        public Rectangle ClickZone { get => _clickZone; }

        internal void SetResolution(int width, int height, bool fullScreen)
        {
            Settings settings = Core.Settings;

            settings.ResolutionWidth = width;
            settings.ResolutionHeight = height;
            settings.FullScreen = fullScreen;

            Core.GraphicsDeviceManager.PreferredBackBufferWidth = settings.ResolutionWidth;
            Core.GraphicsDeviceManager.PreferredBackBufferHeight = settings.ResolutionHeight;
            Core.GraphicsDeviceManager.IsFullScreen = settings.FullScreen;

            CalculateResolution();
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
