using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MoonSharp.Interpreter;

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
    private Script _script;


    public DialogueBox()
    {
        Script _script = Core.ScriptManager.GetScript(Core.scriptsFolder, "vendor.lua");
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "Dialogue/dialogue_atlas.xml");
        _dialogueBox = atlas.CreateAnimatedSprite("box-animation");
        _dialogueBox.Scale = new Microsoft.Xna.Framework.Vector2(1.0f, 1.0f);
        _position = new Vector2(100,450);

    }

    public void Open(string text)
    {
        this._isOpen = true;
        _isFinished = false;
        _fullText = text;
        _visibleText = "";
    }

    public void Close()
    {
        _isOpen = false;
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (this._isOpen)
        {
            Console.WriteLine("Open Dialogue");
            _dialogueBox.Draw(Core.SpriteBatch, _position);
        }

    }
}
