using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MoonSharp.Interpreter;


namespace Revelry.GameObjects;

public class Interactable
{
    private Vector2 _position;

    private Script _script;
    private AnimatedSprite _sprite;
    private TextureAtlas _textureAtlas;
    public event EventHandler BodyCollision;


    public Interactable(Script script)
    {

        _script = script;

    }


    public void Initialize()
    {

        var objTable = _script.Globals.Get("object").Table;

        if (objTable == null)
        {
            throw new Exception("Lua script does not define an 'object' table");
        }

        // Read position data from Lua object table
        float x = (float)objTable.Get("x").Number;
        float y = (float)objTable.Get("y").Number;
        _position = new Vector2(x, y);

        // Read sprite name or other static data from Lua
        string _atlaspath = objTable.Get("atlas").String;
        string _spritePath = objTable.Get("sprite").String;

        _textureAtlas = TextureAtlas.FromFile(Core.Content, _atlaspath);
        _sprite = _textureAtlas.CreateAnimatedSprite(_spritePath);
        _sprite.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);



    }
    //TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/dev_assets/t1_atlas.xml");
    //AnimatedSprite player_animation = atlas.CreateAnimatedSprite("player1-animation");
    //player_animation.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);
    //_player1 = new Player1(player_animation);


    public void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);
    }


    public void Draw()
    {

        _sprite.Draw(Core.SpriteBatch, _position);

    }

    public void Interact()
    {
        var interactFunc = _script.Globals.Get("onInteract");
        if (interactFunc.Type == DataType.Function)
        {
            _script.Call(interactFunc);
        }
    }

  public Circle GetBounds()
    {
        Console.WriteLine(_sprite.Width);
        int x = (int)(_position.X + _sprite.Width * 0.5f);
        int y = (int)(_position.Y + _sprite.Height * 0.5f);
        int radius = (int)(_sprite.Width * 0.25f);

        return new Circle(x, y, radius);
    }

}