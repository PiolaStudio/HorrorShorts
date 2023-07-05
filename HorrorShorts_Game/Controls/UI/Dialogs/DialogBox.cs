using HorrorShorts_Game.Controls.Sprites;
using HorrorShorts_Game.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Resources;
using Resources.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace HorrorShorts_Game.Controls.UI.Dialogs
{
    public class DialogBox
    {
        #region VARIABLES & PROPERTIES
        //Textures
        private RenderTarget2D _baseTexture;
        private RenderTarget2D _textTexture;
        private RenderTarget2D _textTextureTemp;

        private Texture2D _backgroundsTextures;

        private Texture2D _characterTexture;
        private SpriteSheet _characterSheet = null;
        private Rectangle _characterFaceSource;

        private SpriteFont _textFont;
        private SpriteFont _characterNameFont;

        private CharacterAttribute defaultsValues;

        private TextBoxLocation _location = TextBoxLocation.BottomLeft;
        private TextAlignament _textAlign;
        //Flags
        [Flags()]
        private enum Dirtys : byte
        {
            None = 0,
            NeedRenderBackground = 1,
            NeedClearText = 2,
            NeedRenderText = 4,
        }
        private Dirtys _isDirty = Dirtys.None;

        //State
        private bool _isVisible = false;
        public bool Finished { get => _finished; }
        private bool _finished;
        public bool Closed { get => _closed; }
        private bool _closed = true;

        //Events
        public EventHandler<int> DialogEvent;
        public EventHandler<int> ResponseEvent;
        public EventHandler Finish;
        public EventHandler Close; //todo

        //Processing Variables
        private string _fullText = string.Empty;
        private List<CommandData> _allCommands = new();
        private List<CommandData> _currentCommands = new();

        private int _currentCommandIndex = 0;
        private int _currentCharIndex = 0;
        private int _characterCount;
        private int _prevAudioChar = 0;
        private float _nextCharElapsed = 0f;

        private float _delay = 0;
        private float _delayElapsed = 0;

        private bool showFace;
        private bool showCharacterName;
        private FaceType _faceType;
        private Characters _characterType;

        private string _characterName = null;
        private Vector2 _textPos;
        private int _lineCount;
        private Color _textColor;
        private float _textScale;
        private float _baseTextScale;
        private readonly Vector2 _textOrigin = new(0, 3);
        private readonly List<Vector2> _linesPosition = new List<Vector2>();

        private SoundEffect _speakSound = null;
        private int _speakSoundSpeed;
        private int _speakPitchBase;
        private int _speakPitchVariation;
        private int _speakPitchMin;
        private int _speakPitchMax;

        private float _speed = 1f;

        private Zones _zones;

        //Constants
        private static readonly Rectangle _baseBackgroundSource = new(0, 0, 640, 128);
        private static readonly Rectangle _baseCharacterNameBackgroundSource = new(640, 0, 208, 32);
        private static readonly Rectangle _baseCharacterFaceBackgroundSource = new(640, 32, 80, 80);

        private const int COMMA_DELAY = 500;
        private const int POINT_DELAY = 2500;
        private const int EXCLAMATION_DELAY = 2500;
        private const int QUESTION_DELAY = 2500;
        private const int LINE_HEIGHT = 12;
        #endregion

        #region XNA
        public void LoadContent()
        {
            _baseTexture = new RenderTarget2D(Core.GraphicsDevice, 640, 144);
            _textTexture = new RenderTarget2D(Core.GraphicsDevice, 640, 144);
            _textTextureTemp = new RenderTarget2D(Core.GraphicsDevice, 640, 144);
            //_zone = new Rectangle(0, 0, 640, 144);
            _backgroundsTextures = Textures.DialogMenu;
            _characterNameFont = Fonts.Arial;
        }
        public void Update()
        {
            if (!_isVisible) return;
            if (_closed) return;

#if DESKTOP
            if (Core.KeyState.IsKeyDown(Keys.Space)) //todo: cambiar por los controles usados
            {
                if (_finished) _closed = true;
            }
#elif PHONE
            //todo
            if (TouchPanel.GetState().Count > 0)
            {
                if (_finished) _closed = true;
            }   
#endif

            UpdateNextCharacters();
        }
        public void PreDraw()
        {
            if (!_isVisible) return;
            if (_closed) return;

            //Render background (background, face, character name)
            if (_isDirty.HasFlag(Dirtys.NeedRenderBackground))
            {
                RenderBase();
                _isDirty &= ~Dirtys.NeedRenderBackground;
            }

            //Render Text and Commands
            if (_isDirty != Dirtys.None)
            {
                RenderText();
                _isDirty = Dirtys.None;
            }
        }
        public void Draw()
        {
            if (!_isVisible) return;
            if (_closed) return;

            //Render Full
            Core.SpriteBatch.Draw(_baseTexture, _zones.Zone, Color.White);
            Core.SpriteBatch.Draw(_textTexture, _zones.Zone, Color.White);
        }
        public void Dispose()
        {
            if (_baseTexture != null && !_baseTexture.IsDisposed) _baseTexture.Dispose();
            if (_textTexture != null && !_textTexture.IsDisposed) _textTexture.Dispose();
            if (_textTextureTemp != null && !_textTextureTemp.IsDisposed) _textTextureTemp.Dispose();
        }
        #endregion

        #region COMPUTE
        private static void ComputeTextCommands(string fullText, out List<CommandData> commands)
        {
            bool readingCommand = false;

            commands = new List<CommandData>();

            string buffer = string.Empty;

            for (int i = 0; i < fullText.Length; i++)
            {
                char character = fullText[i];

                if (readingCommand)
                {
                    buffer += character;

                    if (character == '}' && buffer != "{sc:") //Command Complete
                    {
                        CommandData commandData = ProcessCommand(buffer);
                        if (commandData.Type == Commands.EscapeChar)
                            buffer = (string)commandData.Data1; //Scape Character
                        else
                        {
                            commands.Add(commandData); //Add Command
                            buffer = string.Empty;
                        }

                        readingCommand = false;
                    }
                }
                else
                {
                    if (character == '{') //Start Command
                    {
                        readingCommand = true;

                        if (buffer.Length > 0)
                        {
                            commands.Add(new(Commands.PlainText, buffer));
                            buffer = string.Empty;
                        }
                    }

                    buffer += character;
                }
            }

            if (buffer.Length > 0)
                commands.Add(new(Commands.PlainText, buffer));
        }
        private static CommandData ProcessCommand(string commandText)
        {
            string cmdTypeTXT = commandText[1..3];
            Commands cmdType = (Commands)BitConverter.ToInt16(Encoding.UTF8.GetBytes(cmdTypeTXT));

            if (!Enum.IsDefined(typeof(Commands), cmdType))
                throw new NotImplementedException("Not implemented Dialog Command: " + cmdTypeTXT);

            string[] cmdDataTXT = new string[0];
            if (commandText.Contains(':'))
                cmdDataTXT = commandText[4..^1].Split(',');

            return cmdType switch
            {
                Commands.Delay => new CommandData(Commands.Delay, Convert.ToInt32(cmdDataTXT[0])),
                Commands.Color => new CommandData(Commands.Color, cmdDataTXT[0].HexToColor()),
                Commands.Font => new CommandData(Commands.Font, (FontType)Enum.Parse(typeof(FontType), cmdDataTXT[0])),
                Commands.FontSize => new CommandData(Commands.FontSize, Convert.ToInt32(cmdDataTXT[0])),
                Commands.Event => new CommandData(Commands.Event, Convert.ToInt32(cmdDataTXT[0])),
                Commands.VibrateBox => new CommandData(Commands.VibrateBox, Convert.ToInt32(cmdDataTXT[0]), Convert.ToInt32(cmdDataTXT[1])),
                Commands.Face => new CommandData(Commands.Face, Enum.Parse(typeof(FaceType), cmdDataTXT[0])),
                Commands.LineBreak => new CommandData(Commands.LineBreak),
                Commands.SoundEffect => new CommandData(Commands.SoundEffect, cmdDataTXT[0]),
                Commands.SpeakType => new CommandData(Commands.SpeakType, Enum.Parse(typeof(SpeakType), cmdDataTXT[0])),
                Commands.SpeakPitch => new CommandData(Commands.SpeakPitch, Convert.ToInt32(cmdDataTXT[0])),
                Commands.SpeakPitchVariation => new CommandData(Commands.SpeakPitchVariation, Convert.ToInt32(cmdDataTXT[0])),
                Commands.TextSpeed => new CommandData(Commands.TextSpeed, Convert.ToByte(cmdDataTXT[0])),
                Commands.EscapeChar => new CommandData(Commands.EscapeChar, cmdDataTXT[0]),
                _ => throw new ArgumentException("Not valid Dialog Argument " + commandText)
            };

        }
        private void ComputeBreakLines(List<CommandData> commands)
        {
            float fontScale = _textScale;
            SpriteFont font = _textFont;
            float x = 0;
            int maxX = _zones.TextZone.Width;

            for (int i = 0; i < commands.Count; i++)
            {
                int lgh = 0;
                switch (commands[i].Type)
                {
                    case Commands.PlainText:
                        string[] words = (commands[i].Data1 as string).Split(' ');
                        for (int j = 0; j < words.Length; j++)
                        {
                            string word = words[j] + " ";
                            x += font.MeasureString(word).X * fontScale;
                            if (x > maxX)
                            {
                                //Add a breakLine
                                string txt = (string)commands[i].Data1;

                                if (lgh == 0)
                                {
                                    commands.Insert(i, new(Commands.LineBreak));
                                    x = 0;
                                }
                                else
                                {
                                    string t1 = txt.Substring(0, lgh - 1);
                                    string t2 = txt.Substring(lgh);

                                    commands[i] = new(Commands.PlainText, t1);
                                    commands.Insert(i + 1, new(Commands.LineBreak));
                                    commands.Insert(i + 2, new(Commands.PlainText, t2));

                                    x = 0;
                                }
                                break;
                            }
                            else lgh += word.Length;
                        }
                        break;
                    case Commands.LineBreak:
                        x = 0;
                        //currentText = string.Empty;
                        break;
                    case Commands.Font:
                        font = Fonts.GetFont((FontType)commands[i].Data1);
                        break;
                    case Commands.FontSize:
                        fontScale = (int)commands[i].Data1;
                        break;
                }
            }
        }
        private void ComputeTextAlign(List<CommandData> commands)
        {
            _linesPosition.Clear();

            float fontScale = _textScale;
            SpriteFont font = _textFont;

            List<float> xPos = new List<float>();

            float lenght = 0;
            int linesCount = 0;
            for (int i = 0; i < commands.Count; i++)
            {
                switch (commands[i].Type)
                {
                    case Commands.PlainText:
                        lenght += font.MeasureString(commands[i].GetData1<string>()).X * fontScale;
                        break;
                    case Commands.Font:
                        font = Fonts.GetFont((FontType)commands[i].Data1);
                        break;
                    case Commands.FontSize:
                        fontScale = commands[i].GetData1<int>();
                        break;
                    case Commands.LineBreak:
                        xPos.Add(SetAlignX(lenght));

                        lenght = 0;
                        linesCount++;
                        break;
                }
            }

            if (lenght > 0)
            {
                xPos.Add(SetAlignX(lenght));
                linesCount++;
            }

            float halfHeight = LINE_HEIGHT / 2;
            float singleHeight = LINE_HEIGHT * _textScale;
            float totalHeight = singleHeight * linesCount;

            for (int i = 0; i < linesCount; i++)
            {
                switch (_textAlign)
                {
                    case TextAlignament.TopLeft:
                    case TextAlignament.TopCenter:
                    case TextAlignament.TopRight:
                        _linesPosition.Add(new(xPos[i], _zones.TextZone.Y + singleHeight * i));
                        break;
                    case TextAlignament.MiddleLeft:
                    case TextAlignament.MiddleCenter:
                    case TextAlignament.MiddleRight:
                        _linesPosition.Add(new(xPos[i], (_zones.TextZone.Center.Y - totalHeight / 2 + halfHeight) + singleHeight * i));
                        break;
                    case TextAlignament.BottomLeft:
                    case TextAlignament.BottomCenter:
                    case TextAlignament.BottomRight:
                        _linesPosition.Add(new(xPos[i], (_zones.TextZone.Bottom - totalHeight) + singleHeight * i));
                        break;
                    default:
                        throw new NotImplementedException("Not implemented Text Alignament: " + _textAlign);
                }
            }
        }
        private float SetAlignX(float lenght)
        {
            switch (_textAlign)
            {
                case TextAlignament.TopLeft:
                case TextAlignament.MiddleLeft:
                case TextAlignament.BottomLeft:
                    return _zones.TextZone.X;
                case TextAlignament.TopCenter:
                case TextAlignament.MiddleCenter:
                case TextAlignament.BottomCenter:
                    return _zones.TextZone.Center.X - lenght / 2;
                case TextAlignament.TopRight:
                case TextAlignament.MiddleRight:
                case TextAlignament.BottomRight:
                    return _zones.TextZone.Right - lenght;
                default:
                    throw new NotImplementedException("Not implemented Text Alignament: " + _textAlign);
            }
        }

        private void ComputeTextPauses(List<CommandData> commands)
        {
            float speed = _speed;

            for (int i = 0; i < commands.Count; i++)
            {
                switch (commands[i].Type)
                {
                    case Commands.PlainText:
                        string text = (string)commands[i].Data1;
                        for (int j = 0; j < text.Length; j++)
                        {
                            char character = text[j];
                            int delay;

                            switch (character)
                            {
                                case '.':
                                    delay = POINT_DELAY;
                                    break;
                                case ',':
                                    delay = COMMA_DELAY;
                                    break;
                                case '!':
                                    //todo
                                    if (j + 1 < text.Length - 1 && text[j + 1] != '!')
                                        delay = EXCLAMATION_DELAY;
                                    else continue;
                                    break;
                                case '?':
                                    //todo
                                    if (j + 1 < text.Length - 1 && text[j + 1] != '?')
                                        delay = QUESTION_DELAY;
                                    else continue;
                                    break;
                                default:
                                    continue;
                            }

                            string t1 = text.Substring(0, j + 1);
                            commands[i] = new(Commands.PlainText, t1);
                            commands.Insert(i + 1, new(Commands.Delay, Convert.ToInt32(delay / speed)));

                            if (j < text.Length - 1)
                            {
                                string t2 = text.Substring(j + 1);
                                commands.Insert(i + 2, new(Commands.PlainText, t2));
                            }
                            break;
                        }
                        break;
                    case Commands.TextSpeed:
                        speed = (byte)commands[i].Data1;
                        break;
                }
            }
        }
        private void GetZone()
        {
            string charName = _characterName ?? string.Empty;
            _zones = _location switch
            {
                TextBoxLocation.TopLeft => new Zones(
                        new(0, Core.ResolutionBounds.Top, 640, 144),    //Zone
                        new(0, 0, 640, 128),    //Background Zone 
                        new(16, 16, 80, 80),    //Face Zone 
                        new(0, 112, 208, 32),   //Name Zone
                        new(10, 118),           //Name Pos
                        _faceType != FaceType.None ? new(112, 20, 512, 80) : new(16, 20, 608, 80),   //Text Zone
                        SpriteEffects.FlipVertically),
                TextBoxLocation.TopRight => new Zones(
                        new(0, Core.ResolutionBounds.Top, 640, 144),    //Zone
                        new(0, 0, 640, 128),    //Background Zone 
                        new(544, 16, 80, 80),   //Face Zone 
                        new(432, 112, 208, 32), //Name Zone
                        new(630 - _characterNameFont.MeasureString(charName).X * 2, 118),   //Name Pos
                        _faceType != FaceType.None ? new(16, 20, 512, 80) : new(16, 20, 608, 80),               //Text Zone
                        SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally),
                
                TextBoxLocation.MiddleLeft => new Zones(
                        new(0, Core.ResolutionBounds.Center.Y - 72, 640, 144),  //Zone   //todo: ajustar a la resolución
                        new(0, 16, 640, 128),   //Background Zone 
                        new(16, 48, 80, 80),    //Face Zone 
                        new(0, 0, 208, 32),     //Name Zone
                        new(10, 14),            //Name Pos
                        _faceType != FaceType.None ? new(112, 48, 512, 80) : new(16, 48, 608, 80),               //Text Zone
                        SpriteEffects.None),
                TextBoxLocation.MiddleRight => new Zones(
                        new(0, Core.ResolutionBounds.Center.Y - 72, 640, 144),  //Zone   //todo: ajustar a la resolución
                        new(0, 16, 640, 128),   //Background Zone 
                        new(544, 48, 80, 80),   //Face Zone 
                        new(432, 0, 208, 32),   //Name Zone
                        new(630 - _characterNameFont.MeasureString(charName).X * 2, 14),           //Name Pos
                        _faceType != FaceType.None ? new(16, 48, 512, 80) : new(16, 48, 608, 80),               //Text Zone
                        SpriteEffects.FlipHorizontally),

                TextBoxLocation.BottomLeft => new Zones(
                        new(0, Core.ResolutionBounds.Bottom - 144, 640, 144),  //Zone   //todo: ajustar a la resolución
                        new(0, 16, 640, 128),   //Background Zone 
                        new(16, 48, 80, 80),    //Face Zone 
                        new(0, 0, 208, 32),     //Name Zone
                        new(10, 14),            //Name Pos
                        _faceType != FaceType.None ? new(112, 48, 512, 80) : new(16, 48, 608, 80),               //Text Zone
                        SpriteEffects.None),
                TextBoxLocation.BottomRight => new Zones(
                        new(0, Core.ResolutionBounds.Bottom - 144, 640, 144),  //Zone   //todo: ajustar a la resolución
                        new(0, 16, 640, 128),   //Background Zone 
                        new(544, 48, 80, 80),   //Face Zone 
                        new(432, 0, 208, 32),   //Name Zone
                        new(630 - _characterNameFont.MeasureString(charName).X * 2, 14),           //Name Pos
                        _faceType != FaceType.None ? new(16, 48, 512, 80) : new(16, 48, 608, 80),               //Text Zone
                        SpriteEffects.FlipHorizontally),

                _ => throw new NotImplementedException("Not implemented Dialog Location: " + _location)
            };
        }

        private void UpdateNextCharacters()
        {
            if (_finished) return;

            //Delay
            //todo: creo que se puede mejorar el como se gestiona el delay...
            _delayElapsed += (float)Core.GameTime.ElapsedGameTime.TotalMilliseconds;
            if (_delayElapsed < _delay) return;
            _delayElapsed = 0;
            _delay = 0;

            _nextCharElapsed += _speed / 40 * Core.DeltaTime;
            int characters2Add = (int)Math.Floor(_nextCharElapsed);
            _nextCharElapsed -= characters2Add;

            if (characters2Add == 0) return;

            string miniTextBuffer = string.Empty;
            int charactersAddedFull = 0;

            while (charactersAddedFull < characters2Add && _currentCommandIndex < _allCommands.Count)
            {
                _isDirty |= Dirtys.NeedRenderText;

                CommandData command = _allCommands[_currentCommandIndex];
                Commands commandType = command.Type;

                switch (commandType)
                {
                    case Commands.PlainText:
                        string str = command.GetData1<string>();
                        char currentChar = str[_currentCharIndex];
                        miniTextBuffer += currentChar;
                        charactersAddedFull++;
                        _characterCount++;
                        _currentCharIndex++;

                        if (_currentCharIndex >= str.Length)
                        {
                            if (_currentCommands.Count > 0 && _currentCommands.Last().Type == Commands.PlainText)
                                _currentCommands[^1] = new CommandData(Commands.PlainText, _currentCommands.Last().Data1 + miniTextBuffer);
                            else _currentCommands.Add(new CommandData(Commands.PlainText, miniTextBuffer));

                            miniTextBuffer = string.Empty;
                            _currentCharIndex = 0;
                            _currentCommandIndex++;
                        }
                        break;
                    case Commands.Delay:
                        _delay += command.GetData1<int>();
                        _currentCommandIndex++;
                        break;
                    case Commands.Color:
                    case Commands.Font:
                    case Commands.FontSize:
                    case Commands.LineBreak:
                        _currentCommands.Add(command);
                        _currentCommandIndex++;
                        break;
                    case Commands.Face:
                        _characterFaceSource = _characterSheet.Get($"DialogFace_{command.GetData1<FaceType>()}");
                        _isDirty |= Dirtys.NeedRenderBackground;
                        _currentCommandIndex++;
                        break;
                    case Commands.Event:
                        DialogEvent?.Invoke(this, command.GetData1<int>());
                        _currentCommandIndex++;
                        break;
                    case Commands.VibrateBox:
                        _currentCommandIndex++; //todo
                        break;
                    case Commands.SoundEffect:
                        _currentCommandIndex++;
                        //todo: play sound effect
                        break;
                    case Commands.SpeakType:
                        _speakSound = GetSpeak(command.GetData1<SpeakType>());
                        _currentCommandIndex++;
                        break;
                    case Commands.SpeakPitch:
                        _speakPitchBase = command.GetData1<int>();
                        GetPitch(_speakPitchBase, _speakPitchVariation, out _speakPitchMin, out _speakPitchMax);
                        _currentCommandIndex++;
                        break;
                    case Commands.SpeakPitchVariation:
                        _speakPitchVariation = command.GetData1<int>();
                        GetPitch(_speakPitchBase, _speakPitchVariation, out _speakPitchMin, out _speakPitchMax);
                        _currentCommandIndex++;
                        break;
                    case Commands.TextSpeed:
                        _speed = command.GetData1<byte>();
                        _currentCommandIndex++;
                        break;
                    default:
                        throw new NotImplementedException("Not implemented Dialog Command: " + commandType);
                }

                if (commandType == Commands.Delay) break;
            }

            if (miniTextBuffer.Length > 0)
            {
                if (_currentCommands.Count > 0 && _currentCommands.Last().Type == Commands.PlainText)
                    _currentCommands[^1] = new CommandData(Commands.PlainText, _currentCommands.Last().Data1 + miniTextBuffer);
                else _currentCommands.Add(new CommandData(Commands.PlainText, miniTextBuffer));
            }

            //Speak Sound
            if (_characterCount - _prevAudioChar >= _speakSoundSpeed)
            {
                _prevAudioChar += _speakSoundSpeed;
                if (_speakSound != null)
                {
                    float pitch;
                    if (_speakPitchMin == _speakPitchMax)
                        pitch = _speakPitchMin / 100f;
                    else pitch = Random.Shared.Next(_speakPitchMin, _speakPitchMax) / 100f;

                    _speakSound?.Play(0.5f, pitch, 0f); //todo: usar el AudioManager
                }
            }

            if (_currentCommandIndex >= _allCommands.Count)
            {
                _finished = true;
                Finish?.Invoke(this, EventArgs.Empty);
            }
        }

        private static SoundEffect GetSpeak(SpeakType speak)
        {
            return speak switch
            {
                SpeakType.None => null,
                SpeakType.Speak1 => Sounds.Speak1,
                SpeakType.Speak2 => Sounds.Speak2,
                _ => throw new NotImplementedException("No implemented Speak Sound" + speak)
            };
        }
        private static void GetPitch(int basePitch, int variation, out int speakPitchMin, out int speakPitchMax)
        {
            speakPitchMin = MathHelper.Clamp(basePitch - variation, -100, 100);
            speakPitchMax = MathHelper.Clamp(basePitch + variation, -100, 100);
        }
        #endregion

        #region RENDER
        private void RenderBase()
        {
            Core.GraphicsDevice.SetRenderTarget(_baseTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Render background
            Core.SpriteBatch.Draw(_backgroundsTextures, _zones.BackgroundZone, _baseBackgroundSource, Color.White, 0f, Vector2.Zero, _zones.Flip, 1f);

            //Render Face
            if (showFace)
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.FaceZone, _baseCharacterFaceBackgroundSource, Color.White);
                Core.SpriteBatch.Draw(_characterTexture, _zones.FaceZone, _characterFaceSource, Color.White);
            }

            //Render Name
            if (showCharacterName)
            {
                Core.SpriteBatch.Draw(_backgroundsTextures, _zones.NameZone, _baseCharacterNameBackgroundSource, Color.White, 0f, Vector2.Zero, _zones.Flip, 1f);
                Core.SpriteBatch.DrawString(_characterNameFont, _characterName, _zones.NamePos, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
            }

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);
        }
        private void RenderText()
        {
            Core.GraphicsDevice.SetRenderTarget(_textTextureTemp);
            Core.GraphicsDevice.Clear(Color.Transparent);

            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (_isDirty.HasFlag(Dirtys.NeedClearText))
            {
                //No render the buffer texture...
                _isDirty |= Dirtys.NeedRenderText;
            }
            else
            {
                //Render buffer texture...
                Core.SpriteBatch.Draw(_textTexture, Vector2.Zero, Color.White);
            }

            if (_isDirty.HasFlag(Dirtys.NeedRenderText))
            {
                //Render Text and Apply (some) Commands
                for (int i = 0; i < _currentCommands.Count; i++)
                {
                    Commands commandType = _currentCommands[i].Type;
                    switch (commandType)
                    {
                        case Commands.PlainText: //Render Text
                            string text = (string)_currentCommands[i].Data1;
                            Core.SpriteBatch.DrawString(_textFont, text, _textPos, _textColor, 0f, _textOrigin, _textScale, SpriteEffects.None, 1f);
                            _textPos.X += _textFont.MeasureString(text).X * _textScale;
                            break;
                        case Commands.LineBreak: //Do a break Line
                            _lineCount++;
                            _textPos = _linesPosition[_lineCount];
                            break;
                        case Commands.Color: //Change the Text Color
                            _textColor = (Color)_currentCommands[i].Data1;
                            break;
                        case Commands.Font: //Change the Text Font
                            _textFont = Fonts.GetFont((FontType)_currentCommands[i].Data1);
                            break;
                        case Commands.FontSize: //Cahnge the text Font Size
                            _textScale = (int)_currentCommands[i].Data1;
                            break;
                        default:
                            throw new NotImplementedException("Not implemented Dialog Command: " + _allCommands[_currentCommandIndex].Type);
                    }
                }
            }

            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);

            //Clone texture
            Core.GraphicsDevice.SetRenderTarget(_textTexture);
            Core.GraphicsDevice.Clear(Color.Transparent);
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Core.SpriteBatch.Draw(_textTextureTemp, Vector2.Zero, Color.White);
            Core.SpriteBatch.End();
            Core.GraphicsDevice.SetRenderTarget(null);

            _currentCommands.Clear();
        }
        #endregion

        #region EXTERNAL
        public void Show(Dialog dialog)
        {
            Characters character = dialog.Character;
            FaceType face = dialog.Face;
            FontType font = dialog.Font;
            int soundSpeed = dialog.SpeakSpeed;
            SpeakType speak = dialog.Speak;
            int pitchBase = dialog.SpeakPitch;

            var enumType = typeof(Characters);
            var memberInfos = enumType.GetMember(dialog.Character.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(CharacterAttribute), false);
            defaultsValues = (CharacterAttribute)valueAttributes[0];

            if (defaultsValues.TextureName == null || defaultsValues.SheetName == null)
                face = FaceType.None;
            if (face == FaceType.Default) face = defaultsValues.DefaultFace;

            if (font == FontType.Default) font = defaultsValues.DefaultFont;
            if (soundSpeed == -1) soundSpeed = defaultsValues.DefaultSpeakSpeed;
            if (speak == SpeakType.Default) speak = defaultsValues.DefaultSpeak;

            int speakPitchMin;
            int speakPitchMax;
            if (pitchBase == -1)
            {
                speakPitchMin = defaultsValues.DefaultSpeakPitchMin;
                speakPitchMax = defaultsValues.DefaultSpeakPitchMax;
            }
            else GetPitch(pitchBase, dialog.SpeakPitchVariation, out speakPitchMin, out speakPitchMax);

            _characterType = character;
            _characterName = defaultsValues.DefaultName;
            _faceType = face;
            _textFont = Fonts.GetFont(font);
            _textScale = dialog.FontSize;
            _baseTextScale = dialog.FontSize;
            _textColor = dialog.Color;
            _speed = dialog.Speed;
            _speakSound = GetSpeak(speak);
            _speakSoundSpeed = soundSpeed;
            _speakPitchBase = pitchBase;
            _speakPitchVariation = dialog.SpeakPitchVariation;
            _speakPitchMin = speakPitchMin;
            _speakPitchMax = speakPitchMax;

            if (defaultsValues.TextureName != null)
                Textures.GetTexture(defaultsValues.TextureName, out _characterTexture);
            if (defaultsValues.SheetName != null)
                SpriteSheets.GetSheet(defaultsValues.SheetName, out _characterSheet);

            if (string.IsNullOrEmpty(_characterName))
                showCharacterName = false;
            else showCharacterName = true;

            if (face != FaceType.None)
            {
                showFace = true;
                _characterFaceSource = _characterSheet.Get($"DialogFace_{face}");
            }
            else showFace = false;

            //Zones
            _location = dialog.Location;
            _textAlign = dialog.TextAlign;
            GetZone();

            //Process Text
            ComputeTextCommands(dialog.Text, out _allCommands);
            if (dialog.AjustEndLine) ComputeBreakLines(_allCommands);
            ComputeTextAlign(_allCommands); //todo crear método
            if (dialog.DoPauses) ComputeTextPauses(_allCommands);

            //Reset values
            _isDirty = Dirtys.NeedRenderBackground | Dirtys.NeedClearText | Dirtys.NeedClearText | Dirtys.NeedRenderText;
            _finished = false;
            _closed = false;
            _isVisible = true;
            _currentCommandIndex = 0;
            _currentCharIndex = 0;
            _characterCount = 0;
            _prevAudioChar = 0;
            _delay = 0;
            _textPos = _linesPosition.First();
            _lineCount = 0;
            _currentCommands.Clear();
        }
        public void Show(string text)
        {
            //todo
        }
        internal void ResetResolution()
        {
            GetZone();
        }
        #endregion

        #region SUB-CLASSES
        private struct Zones
        {
            public readonly Rectangle Zone;
            public readonly Rectangle BackgroundZone;
            public readonly Rectangle FaceZone;
            public readonly Rectangle NameZone;
            public readonly Vector2 NamePos;
            public readonly Rectangle TextZone;
            public readonly SpriteEffects Flip;

            public Zones(Rectangle zone, Rectangle backgroundZone, Rectangle faceZone, Rectangle nameZone, Vector2 namePos, Rectangle textZone, SpriteEffects flip)
            {
                Zone = zone;
                BackgroundZone = backgroundZone;
                FaceZone = faceZone;
                NameZone = nameZone;
                NamePos = namePos;
                TextZone = textZone;
                Flip = flip;
            }
        }

        public enum Commands : short
        {
            PlainText = 0,
            Delay = 0x7964,         //dy {dy:1000}
            Font = 0x7466,          //ft {ft:Arial}
            Color = 0x6366,         //co {co:FFFFFF}
            FontSize = 0x7366,      //fs {fs:2}
            Face = 0x6166,          //fa {fa:Happy1}
            LineBreak = 0x6C62,     //lb {lb}
            EscapeChar = 0x6365,    //ec {ec:}}
            SpeakType = 0x7473,     //st {st:Voice1}
            SpeakPitch = 0x7073,    //sp {sp:50}
            SpeakPitchVariation = 0x7673,//sv {sv:50}
            SpeakSpeed = 0x7373,    //ss {ss:4}
            TextSpeed = 0x7374,     //ts {ts:10}
            Event = 0x7665,         //ev {ev:0}
            SoundEffect = 0x6573,   //se {se:Note}
            VibrateBox = 0x6276,    //vb {vb:1}
        }

        [DebuggerDisplay("{DebugDisplay,nq}")]
        private readonly struct CommandData
        {
            public readonly Commands Type;
            public readonly object Data1;
            public readonly object Data2;

            [Browsable(false)]
            [Bindable(false)]
            internal string DebugDisplay
            {
                get
                {
                    string ret = Type.ToString();
                    if (Data1 != null) ret += $" - {Data1}";
                    if (Data2 != null) ret += $" - {Data2}";
                    return ret;
                }
            }

            public T GetData1<T>() => (T)Data1;
            public T GetData2<T>() => (T)Data2;

            public CommandData(Commands type)
            {
                Type = type;
                Data1 = null;
                Data2 = null;
            }
            public CommandData(Commands type, object data)
            {
                Type = type;
                Data1 = data;
                Data2 = null;
            }
            public CommandData(Commands type, object data1, object data2)
            {
                Type = type;
                Data1 = data1;
                Data2 = data2;
            }
        }
        #endregion
    }
}