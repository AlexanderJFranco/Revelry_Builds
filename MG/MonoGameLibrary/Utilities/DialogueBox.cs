using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Graphics;
using MoonSharp.Interpreter;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Utilities;

public class DialogueBox
{
    private SpriteFont _font;

    private string _fullText;
    private string _visibleText;
    private float _charTimer;
    private int _charIndex;

    private bool _isOpen;
    private bool _isFinished;

    public bool IsOpen => _isOpen;
    public bool IsFinished => _isFinished;
    private Vector2 _position;
    private AnimatedSprite _dialogueBox;

    private double _timePerChar = .05; // seconds per letter

    



    public DialogueBox()
    {
        Script _script = Core.ScriptManager.GetScript(Core.scriptsFolder, "vendor.lua");
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "Dialogue/dialogue_atlas.xml");
        _dialogueBox = atlas.CreateAnimatedSprite("box-animation");
        _dialogueBox.Scale = new Microsoft.Xna.Framework.Vector2(1.0f, 1.0f);
        _position = new Vector2(100, 450);

    }

    public void Open(string text)
    {
        this._isOpen = true;
        _isFinished = false;
        _fullText = text;
        _visibleText = "";
        _charTimer = 0;
        _charIndex = 0;
    }

    public void Close()
    {
        _isOpen = false;
    }

    public void Update(GameTime gameTime, bool skip)
    {
        if (!_isOpen) return;

        if (skip && _charIndex < _fullText.Length)
        {
            _visibleText = _fullText;
            _charIndex = _fullText.Length;
            this._isFinished = true;
        }
        
        // accumulate precise time
        _charTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
          if (_charIndex < _fullText.Length && _charTimer >= _timePerChar)
            {
                _charIndex++;
                string nextText = _fullText.Substring(0, _charIndex);

                // Measure the text up to this character
                Vector2 size = _font.MeasureString(nextText);
                _visibleText = _fullText.Substring(0, _charIndex);
                _charTimer = 0;
            }
        
        if (_visibleText.Equals(_fullText))
            this._isFinished = true;
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (this._isOpen)
        {
            _dialogueBox.Draw(Core.SpriteBatch, _position);
            spriteBatch.DrawString(_font, _visibleText, _position + new Vector2(20, 20), Color.White);
        }

    }

    public void LoadContent()
    {
        _font = Core.Content.Load<SpriteFont>("Fonts/PeaberryMono");
    }
    

}
