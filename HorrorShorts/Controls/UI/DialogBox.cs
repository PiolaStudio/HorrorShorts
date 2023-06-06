using HorrorShorts.Data;
using HorrorShorts.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HorrorShorts.Controls.UI
{
    public class DialogBox
    {
        private RenderTarget2D _texture;
        private Rectangle _zone;
        private SpriteFont _font;

        private static readonly Vector2 _characterNamePos = new();
        private string _characterName = null;

        private readonly Rectangle _backgroundNarratorSource;
        private readonly Rectangle _backgroundCharacterSource;
        private Texture2D _backgroundsTextures;
        private Rectangle _backgroundSource;

        private Texture2D _characterTexture;
        private Rectangle _characterFaceSource;
        private Vector2 _characterFacePos;

        private bool _needRender = true;
        private bool _isVisible = false;

        private Locations _location = Locations.Bottom;

        public EventHandler<int> DialogEvent;

        public enum Locations : byte
        {
            Top,
            Middle,
            Bottom
        }
        public enum SpeakType : byte
        {
            None = 0,
            Default = 1,
            Grave
        }

        public void LoadContent()
        {
            _texture = new RenderTarget2D(Core.GraphicsDevice, 640, 144);
        }
        public void Update()
        {
            if (Core.KeyState.IsKeyDown(Keys.Space))
            {

            }
        }
        public void PreDraw()
        {
            if (!_needRender) return;

            Core.GraphicsDevice.SetRenderTarget(_texture);
            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Draw(_backgroundsTextures, Vector2.Zero, _backgroundSource, Color.White);
            Core.SpriteBatch.Draw(_characterTexture, _characterFacePos, _characterFaceSource, Color.White);
            Core.SpriteBatch.DrawString(_font, _characterName, _characterNamePos, Color.White);
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        public void Draw()
        {
            if (!_isVisible) return;
            Core.SpriteBatch.Draw(_texture, _zone, Color.White);
        }

        public void Show(Dialog dialog)
        {
            //<Dialog ID="0" Character="Pepe" FaceID="1">Hola, yo soy {c:1}Pepe{c:0}</Dialog>

            //d = Do a delay for x milis
            //c = Color on RGB bytes
            //p = Progressive appear speed
            //s = Do a sound effect
            //e = Call a event with a index.
            //v = Vibrate dialogbox
            //f = change the face index
            //h = scale (height)
            //j = jump-line
            //d = Change the character sound
            //a = Change the character sound pitch
            //string simbs = "{d:400} {c:2} {p:100} {s:Noise} {e:1} {v:4} {f:3} {h:2} {j} {d:CharacterSound4} {a:-50to10}";


            if (dialog.Character == Characters.None)
            {
                _backgroundSource = _backgroundNarratorSource;

                _characterName = null;
                _characterTexture = null;
                _characterFaceSource = Rectangle.Empty;
            }
            else
            {
                _backgroundSource = _backgroundCharacterSource;

                switch (dialog.Character)
                {
                    case Characters.Girl1:
                        _characterTexture = Textures.Mario;
                        _characterFaceSource = SpriteSheets.Mario.Get($"DialogFace_{dialog.Face}");
                        _characterName = dialog.Character.ToString();
                        break;
                    default:
                        throw new NotImplementedException("Not implemented character dialog");
                }
            }
        }
    }
}
