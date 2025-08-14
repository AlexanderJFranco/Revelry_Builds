using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MoonSharp.Interpreter;
using System.Globalization;
using Microsoft.Xna.Framework.Graphics;


namespace Revelry.GameObjects;

public class Vendor : Interactable
{
    //Declar object parameters
    private Vector2 _position;
    private Script _script;
    private AnimatedSprite _sprite;
    private TextureAtlas _textureAtlas;
    private Color _debugShader;
    private Boolean _debugStatus;
    public ZoneType _physics;

    //Constructor
    public Vendor(Script script, ZoneManager zoneManager)
    {

        _script = script;
        ZoneManager = zoneManager;

    }


    public override void Initialize()
    {
        //START// Pull object data from Lua Script

        var objTable = _script.Globals.Get("object").Table;

        if (objTable == null)
        {
            throw new Exception("Lua script does not define an 'object' table");
        }

        // Read position data from Lua object table
        float x = (float)objTable.Get("x").Number;
        float y = (float)objTable.Get("y").Number;
        _position = new Vector2(x, y);

        // Read sprite name and file path from lua script
        string _atlaspath = objTable.Get("atlas").String;
        string _spritePath = objTable.Get("sprite").String;

        //Parse Debug shader & if Debug Mode is enabled from lua script
        string _debugHexCode = objTable.Get("hitbox_color").String;
        _debugShader = Core.HexToColor(_debugHexCode);
        _debugStatus = objTable.Get("debug_enabled").Boolean;
        _physics = Enum.Parse<ZoneType>(objTable.Get("physics_type").String);
        //END

        //Assign object attributes for Texture Atlas and Animated Sprite
        _textureAtlas = TextureAtlas.FromFile(Core.Content, _atlaspath);
        _sprite = _textureAtlas.CreateAnimatedSprite(_spritePath);
        _sprite.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);

        //Assign inherited HitBox (Interactable Class)
        int _hitboxx = (int)_position.X;
        int _hitboxy = (int)_position.Y + (int)_sprite.Height/2;
        Hitbox = new RectZone(_hitboxx, _hitboxy, (int)_sprite.Width, (int)(_sprite.Height/2), ZoneType.Solid, _debugStatus, _debugShader);
        

    }


    public override void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);
        
    }


    public override void Draw(SpriteBatch SpriteBatch)
    {
        //Draw object sprite and Hitbox (DebugStatus tracks if object will be visible in-game)
        _sprite.Draw(SpriteBatch, _position);
        //HitBox Dimensions
        
        this.Hitbox.DisjointDraw(SpriteBatch,new Vector2(_position.X, _position.Y +  (int)(_sprite.Height/2)), Core._pixel,  _debugStatus, _debugShader);
        ZoneManager.RegisterZone(Hitbox, this);    //Zone Manager accepts Zone and object to determine which areas are interactive

    }

    //Object Function on Player Interaction
    public override void Interact()
    {
        ZoneManager.OutputZones();
        var interactFunc = _script.Globals.Get("onInteract");
        if (interactFunc.Type == DataType.Function)
        {
            _script.Call(interactFunc);
        }
    }

}