using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources;
using SharpFont.PostScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.UI
{
    public class ComboBox : Control
    {
        private Texture2D _texture;
        private SpriteSheet _sheets;

        private RenderTarget2D _mainTexture;
        private RenderTarget2D _optionsTexture;

        public bool IsDroped { get => _isDroped; }
        private bool _isDroped = false;

        public new bool IsEnable 
        { 
            get => _isEnable;
            set
            {
                if (_isEnable == value) return;
                _isEnable = value;
                _needRenderMain = true;
            }
        }

        private int _size = 1;
        private int _optionSize = 1;
        private bool _needCompute = true;
        private bool _needRenderMain = true;
        private bool _needRenderOptions = true;

        //Scroll
        private int _maxItemsWithoutScroll = 5;
        private bool _scrollEnable = false;
        private Rectangle _scrollSource;
        private Rectangle[] _scrollSources;
        private Rectangle _scrollBarZone;
        private Rectangle _virtualScrollBarZone;
        private Rectangle _scrollDataSource;
        private float _scrollValue = 0f;
        private bool _scrollDragging = false;
        private int _scrollDragPos = 0;

        public new bool NeedRender { get => _needRenderMain || _needRenderOptions; }

        private Zones _zones;

        public string[] Options
        {
            get => _options;
            set
            {
                _options = value;
                _selectedOption = -1;
                _needCompute = true;
                if (_isDroped) Close();
            }
        }
        private string[] _options = Array.Empty<string>();

        private Rectangle _mainButtonZone;
        public new Point Position
        {
            get => _mainButtonZone.Location;
            set
            {
                _mainButtonZone.Location = value;
                _needCompute = true;
            }
        }
        public new int X
        {
            get => _mainButtonZone.X;
            set
            {
                _mainButtonZone.X = value;
                _needCompute = true;
            }
        }
        public new int Y
        {
            get => _mainButtonZone.Y;
            set
            {
                _mainButtonZone.Y = value;
                _needCompute = true;
            }
        }

        public int Width { get => _zones.Global.Width; }
        public int Height { get => _zones.Global.Height; }
        public int Size 
        { 
            get => _size; 
            set
            {
                if (_size == value) return;
                _size = value;
                _needCompute = true;
            }
        }
        public int OptionSize
        {
            get => _optionSize;
            set
            {
                if (_optionSize == value) return;
                _optionSize = value;
                _needCompute = true;
            }
        }
        public int MaxItemsWithoutScroll
        {
            get => _maxItemsWithoutScroll;
            set
            {
                if (_maxItemsWithoutScroll == value) return;
                _maxItemsWithoutScroll = value;
                _needCompute = true;
            }
        }


        private Point _virtualPosition;
        public new Point VirtualPosition
        {
            get => _virtualPosition;
            set
            {
                _virtualPosition = value;
                _needCompute = true;
            }
        }
        public new int VirtualX
        {
            get => _virtualPosition.X;
            set
            {
                _virtualPosition.X = value;
                _needCompute = true;
            }
        }
        public new int VirtualY
        {
            get => _virtualPosition.Y;
            set
            {
                _virtualPosition.Y = value;
                _needCompute = true;
            }
        }

        private HorizontalAlignament _optionAlignament = HorizontalAlignament.Left;
        public HorizontalAlignament OptionAlignament
        {
            get => _optionAlignament;
            set
            {
                if (_optionAlignament == value) return;
                _optionAlignament = value;
                _needCompute = true;
            }
        }

        private Label _mainLabel;
        private Label[] _optionLabels = Array.Empty<Label>();

        public event EventHandler DropEvent;
        public event EventHandler CloseEvent;
        public event EventHandler ClickEvent;
        public event EventHandler<int> OptionClickEvent;

        private int _overOption = -1; //todo: property
        private int _selectedOption = -1;//todo: property
        public int SelectedOption { get => _selectedOption; }

        public ComboBox()
        {
            _mainButtonZone = new(0, 0, 32, 16);
            _virtualPosition = Point.Zero;
        }
        public override void LoadContent()
        {
            _texture = Textures.Get(TextureType.UIControls);
            _sheets = SpriteSheets.Get(SpriteSheetType.UIControls);

            _mainLabel = new();
            _mainLabel.Alignament = TextAlignament.MiddleCenter;
            _mainLabel.Color = Color.Black;

            _scrollSources = new Rectangle[2] { _sheets.Get("ScrollBar_Enable"), _sheets.Get("ScrollBar_Pressed") };
            _scrollSource = _scrollSources[0];

            _needCompute = _needRenderMain = _needRenderOptions = true;
            Compute();
        }
        public override void Update()
        {
            if (_needCompute)
                Compute();

            UpdateScroll();
            UpdateOptions();

            _mainLabel.Update();
        }
        private void UpdateScroll()
        {
            if (!_scrollEnable) return;
            if (!IsDroped) return;
            if (!_isEnable) return;
            if (!_isVisible) return;

            Rectangle scrollBarZone = _useVirtualZone ? _virtualScrollBarZone : _scrollBarZone;
            Rectangle scrolBarLimitZone = _useVirtualZone ? _zones.ScrollVirtualZone : _zones.ScrollZone;

#if DESKTOP || PHONE
            if (_scrollDragging)
            {
                if (!Core.Controls.ClickPressed)
                {
                    _scrollDragging = false;
                    _scrollSource = _scrollSources[0];
                }
            }
            else
            {
                if (Core.Controls.ClickPressed)
                {
                    if (scrollBarZone.Contains(Core.Controls.ClickPositionUI))
                    {
                        _scrollDragging = true;
                        _scrollDragPos = Core.Controls.ClickPositionUI.Y - scrollBarZone.Y;
                        _scrollSource = _scrollSources[1];
                    }
                }
            }

            if (_scrollDragging)
            {
                float sv = (Core.Controls.ClickPositionUI.Y - scrolBarLimitZone.Y - _scrollDragPos) /
                    ((float)scrolBarLimitZone.Height - _scrollBarZone.Height);
                float scrollVal = MathHelper.Clamp(sv, 0, 1);

                if (_scrollValue != scrollVal)
                {
                    _scrollValue = scrollVal;
                    int scrollPaddY = Convert.ToInt32((scrolBarLimitZone.Height - _scrollBarZone.Height) * _scrollValue);
                    _scrollBarZone.Y = Convert.ToInt32(_zones.ScrollZone.Y + scrollPaddY);
                    _virtualScrollBarZone.Y = Convert.ToInt32(_zones.ScrollVirtualZone.Y + scrollPaddY);
                    _scrollDataSource.Y = Convert.ToInt32(-(_zones.OptionZone.Height - _zones.OptionRenderZone.Height) * _scrollValue); //todo: check this line
                }
            }
#endif
        }
        private void UpdateOptions()
        {
            if (!_isEnable) return;
            if (!_isVisible) return;
            if (_scrollDragging) return;

            Rectangle mainClickZone = _useVirtualZone ? _zones.VirtualMainZone : _zones.MainZone;
            Rectangle optionsClickZone = _useVirtualZone ? _zones.OptionZone : _zones.OptionVirtualZone;

            //Main
#if DESKTOP || PHONE
            if (Core.Controls.Click)
            {
                bool clickIn = false;
                if (mainClickZone.Contains(Core.Controls.ClickPositionUI))
                {
                    if (_isDroped) Close();
                    else Open();
                    FireFocus();
                    clickIn = true;
                }

                if (_isDroped)
                {
                    if (_zones.OptionVirtualZone.Contains(Core.Controls.ClickPositionUI))
                    {
                        for (int i = 0; i < _options.Length; i++)
                        {
                            Rectangle optionClickZone = _useVirtualZone ? _zones.OptionsVirtualZone[i] : _zones.OptionsZone[i];
                            if (_scrollEnable)
                                optionClickZone.Y -= _scrollDataSource.Y;

                            if (optionClickZone.Contains(Core.Controls.ClickPositionUI))
                            {
                                if (_selectedOption == i) continue;
                                SelectOption(i);
                                OptionClickEvent?.Invoke(this, i); //todo: ?
                                Close();
                                clickIn = true;
                            }
                        }
                    }
                }

                if (!clickIn) Close();
            }
#endif
            //todo: otros controles
        }

        public override void PreDraw()
        {
            if (!_isVisible) return;
            if (_needCompute) return;
            if (_needRenderMain) RenderMain();
            if (_needRenderOptions) RenderOptions();
        }
        public override void Draw()
        {
            if (!_isVisible) return;

            Core.SpriteBatch.Draw(_mainTexture, _zones.MainZone, Color.White);
            if (_isDroped)
            {
                Core.SpriteBatch.Draw(_optionsTexture, _zones.OptionZone, _scrollDataSource, Color.White);
                if (_scrollEnable)
                    Core.SpriteBatch.Draw(_texture, _scrollBarZone, _scrollSource, Color.White);
            }
        }
        public override void Dispose()
        {
            if (_mainTexture != null && _mainTexture.IsDisposed)
                _mainTexture.Dispose();

            if (_optionsTexture != null && _optionsTexture.IsDisposed)
                _optionsTexture.Dispose();
        }

        private void Compute()
        {
            _needCompute = false;

            _scrollEnable = _options.Length > _maxItemsWithoutScroll;

            //Zones
            _zones = GetZones();

            //Textures
            if (_mainTexture == null || _mainTexture.IsDisposed || _mainTexture.Width != _zones.MainZone.Width || _mainTexture.Height != _zones.MainZone.Height)
            {
                if (_mainTexture != null && _mainTexture.IsDisposed)
                    _mainTexture.Dispose();
                _mainTexture = new(Core.GraphicsDevice, _zones.MainZone.Width, _zones.MainZone.Height);
            }
            if (_optionsTexture == null || _optionsTexture.IsDisposed || _optionsTexture.Width != _zones.OptionRenderZone.Width || _mainTexture.Height != _zones.OptionRenderZone.Height)
            {
                if (_optionsTexture != null && _optionsTexture.IsDisposed)
                    _optionsTexture.Dispose();
                _optionsTexture = new(Core.GraphicsDevice, _zones.OptionRenderZone.Width, _zones.OptionRenderZone.Height);
            }

            //Labels
            _mainLabel.Position = _zones.MainZone.Size / new Point(2);

            _optionLabels = new Label[_options.Length];
            for (int i = 0; i < _options.Length; i++)
            {
                Label label = new();
                label.Text = _options[i];
                label.Position = _zones.OptionLabelPositions[i];
                label.Alignament = TextAlignament.MiddleCenter;
                label.Update();

                _optionLabels[i] = label;
            }

            //Scroll
            _scrollValue = 0f;
            _scrollDataSource = new(Point.Zero, _zones.OptionZone.Size);
            _scrollBarZone = new(_zones.ScrollZone.X, _zones.ScrollZone.Y, _zones.ScrollBarSize.X, _zones.ScrollBarSize.Y);
            _virtualScrollBarZone = new(_zones.ScrollVirtualZone.X, _zones.ScrollVirtualZone.Y, _zones.ScrollBarSize.X, _zones.ScrollBarSize.Y);
            
            _needRenderMain = _needRenderOptions = true;
        }

        private void RenderMain()
        {
            _needRenderMain = false;

            Core.GraphicsDevice.SetRenderTarget(_mainTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);

            Core.SpriteBatch.Begin();

            string sheet = "Button";
            if (_isDroped || !_isEnable) sheet += "_Disable";
            else sheet += "_Enable";

            //Base
            Core.SpriteBatch.Draw(_texture, _zones.RenderMainZone[0], _sheets.Get($"{sheet}_Left"), Color.White);
            Core.SpriteBatch.Draw(_texture, _zones.RenderMainZone[1], _sheets.Get($"{sheet}_Middle"), Color.White);
            Core.SpriteBatch.Draw(_texture, _zones.RenderMainZone[2], _sheets.Get($"{sheet}_Right"), Color.White);

            //Label
            _mainLabel.Draw();

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderOptions()
        {
            _needRenderOptions = false;

            Core.GraphicsDevice.SetRenderTarget(_optionsTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);

            Core.SpriteBatch.Begin();

            for (int i = 0; i < _options.Length; i++)
            {
                //Bases
                string sheetName = "ComboBox_List";
                if (_overOption == i || _selectedOption == i) sheetName += "_Pressed";
                if (i == _options.Length - 1) sheetName += "_Bottom";

                Core.SpriteBatch.Draw(_texture, _zones.RenderOptionsZones[i][0], _sheets.Get($"{sheetName}_Left"), Color.White);
                Core.SpriteBatch.Draw(_texture, _zones.RenderOptionsZones[i][1], _sheets.Get($"{sheetName}_Middle"), Color.White);
                Core.SpriteBatch.Draw(_texture, _zones.RenderOptionsZones[i][2], _sheets.Get($"{sheetName}_Right"), Color.White);

                //Label
                _optionLabels[i].Draw();

                //Scrol bar zone
                if (_scrollEnable)
                {
                    sheetName = "ScrollBarZone";
                    if (i == _options.Length - 1) sheetName += "_Bottom";
                    Core.SpriteBatch.Draw(_texture, _zones.ScrollRenderSections[i], _sheets.Get(sheetName), Color.White);
                }
            }



            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }

        #region EXTERNAL
        public void Open()
        {
            if (_isDroped) return;
            _isDroped = true;
            _needRenderMain = _needRenderOptions = true;
            DropEvent?.Invoke(this, EventArgs.Empty);
        }
        public void Close()
        {
            if (!_isDroped) return;
            _isDroped = false;
            _needRenderMain = _needRenderOptions = true;
            CloseEvent?.Invoke(this, EventArgs.Empty);
        }
        public void SelectOption(int index)
        {
            if (_selectedOption == index) return;
            _selectedOption = index;

            if (_selectedOption == -1) _mainLabel.Text = string.Empty;
            else _mainLabel.Text = _options[_selectedOption];
            _needRenderMain = _needRenderOptions = true;
        }
        #endregion

        private Zones GetZones()
        {
            int mainSize = 32 + _size * 16;
            int optionBlockSize = 32 + _optionSize * 16;
            int optionSize = _scrollEnable ? optionBlockSize - 8 : optionBlockSize;

            int xOptionPadd = _optionAlignament switch
            {
                HorizontalAlignament.Left => 0,
                HorizontalAlignament.Center => (mainSize - optionBlockSize) / 2,
                HorizontalAlignament.Right => mainSize - optionBlockSize,
                _ => throw new NotImplementedException("Not implemented alignament"),
            };
            int optionsX = X + xOptionPadd;
            int optionsVirtualX = VirtualX + xOptionPadd;
            int optionsRenderHeight = _scrollEnable ? _maxItemsWithoutScroll * 16 : _options.Length * 16;

            Rectangle[] renderMainZones = new Rectangle[3];
            renderMainZones[0] = new(0, 0, 16, 16);
            renderMainZones[1] = new(16, 0, mainSize - 32, 16);
            renderMainZones[2] = new(mainSize - 16, 0, 16, 16);


            //Options
            Rectangle[][] renderOptionsZones = new Rectangle[_options.Length][];
            Rectangle[] optionsClickZone = new Rectangle[_options.Length];
            Rectangle[] optionsVirtualClickZone = new Rectangle[_options.Length];
            Point[] optionsLabelPositions = new Point[_options.Length];

            for (int i = 0; i < _options.Length; i++)
            {
                int paddY = 16 * i;

                renderOptionsZones[i] = new Rectangle[3];
                renderOptionsZones[i][0] = new(0, paddY, 16, 16);
                renderOptionsZones[i][1] = new(16, paddY, optionSize - 32, 16);
                renderOptionsZones[i][2] = new(optionSize - 16, paddY, 16, 16);
                optionsLabelPositions[i] = new(optionSize / 2, paddY + 8);

                optionsClickZone[i] = new(optionsX, Y + 16 + paddY, optionSize, 16);
                optionsVirtualClickZone[i] = new(optionsVirtualX, VirtualY + 16 + paddY, optionSize, 16);
            }

            //Scroll
            Rectangle[] scrollRenderSections = new Rectangle[_options.Length];
            if (_scrollEnable)
            {
                int scrollRenderZoneX = optionBlockSize - 8;
                for (int i = 0; i < _options.Length; i++)
                    scrollRenderSections[i] = new(scrollRenderZoneX, 16 * i, 8, 16);
            }

            //Final zones
            Rectangle mainZone = new(X, Y, mainSize, 16);
            Rectangle virtualMainZone = new(VirtualX, VirtualY, mainZone.Width, mainZone.Height);
            Rectangle optionZone = new(optionsX, Y + 16, optionBlockSize, optionsRenderHeight);
            Rectangle optionVirtualZone = new(optionsVirtualX, VirtualY + 16, optionBlockSize, optionsRenderHeight);
            Rectangle optionRenderZone = new(0, 0, optionBlockSize, 16 * _options.Length);

            Rectangle scrollZone = Rectangle.Empty;
            Rectangle scrollVirtualZone = Rectangle.Empty;
            Point scrollBarSize = Point.Zero;
            if (_scrollEnable)
            {
                scrollZone = new(optionZone.Right - 8, optionZone.Y, 8, optionsRenderHeight);
                scrollVirtualZone = new(optionVirtualZone.Right - 8, optionVirtualZone.Y, 8, optionsRenderHeight);
                scrollBarSize = new(8, Convert.ToInt32(1f / (optionRenderZone.Height / optionZone.Height) * optionZone.Height)); 
            }

            //Global
            int realX = Math.Min(mainZone.X, optionZone.X);
            int realY = Y;
            int realWidth = (Math.Min(mainZone.Right, optionZone.Right) - realX);
            int realHeight = mainZone.Height + optionZone.Height;
            Rectangle global = new(realX, realY, realWidth, realHeight);

            return new(global, renderMainZones, renderOptionsZones, mainZone, virtualMainZone, 
                optionZone, optionVirtualZone, optionRenderZone, optionsClickZone, optionsVirtualClickZone, optionsLabelPositions, 
                scrollZone, scrollVirtualZone, scrollRenderSections, scrollBarSize);
        }
        private readonly struct Zones
        {
            public readonly Rectangle Global;
            public readonly Rectangle[] RenderMainZone;
            public readonly Rectangle[][] RenderOptionsZones;

            public readonly Rectangle MainZone;
            public readonly Rectangle VirtualMainZone;
            public readonly Rectangle OptionZone;
            public readonly Rectangle OptionVirtualZone;
            public readonly Rectangle OptionRenderZone;
            public readonly Rectangle[] OptionsZone;
            public readonly Rectangle[] OptionsVirtualZone;
            public readonly Point[] OptionLabelPositions;

            public readonly Rectangle ScrollZone;
            public readonly Rectangle ScrollVirtualZone;
            public readonly Rectangle[] ScrollRenderSections;
            public readonly Point ScrollBarSize;

            public Zones(Rectangle global, Rectangle[] renderMainZone, Rectangle[][] renderOptionsZones, 
                Rectangle mainZone, Rectangle virtualMainZone, Rectangle optionZone, Rectangle optionVirtualZone, Rectangle optionRenderZone, 
                Rectangle[] optionsZone, Rectangle[] optionsVirtualZone, Point[] optionLabelPositions, 
                Rectangle scrollZone, Rectangle scrollVirtualZone, Rectangle[] scrollRenderSections, Point scrollBarSize)
            {
                Global = global;
                RenderMainZone = renderMainZone;
                RenderOptionsZones = renderOptionsZones;
                MainZone = mainZone;
                VirtualMainZone = virtualMainZone;
                OptionZone = optionZone;
                OptionVirtualZone = optionVirtualZone;
                OptionRenderZone = optionRenderZone;
                OptionsZone = optionsZone;
                OptionsVirtualZone = optionsVirtualZone;
                OptionLabelPositions = optionLabelPositions;
                ScrollZone = scrollZone;
                ScrollVirtualZone = scrollVirtualZone;
                ScrollRenderSections = scrollRenderSections;
                ScrollBarSize = scrollBarSize;
            }
        }
    }
}
