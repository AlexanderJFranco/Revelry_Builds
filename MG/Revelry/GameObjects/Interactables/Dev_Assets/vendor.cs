using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MoonSharp.Interpreter;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.ObjecTypes;

namespace Revelry.GameObjects;

public class Vendor : Interactable
{
    //Declar object parameters
    private Vector2 _position;
    private Script _script;
    private AnimatedSprite _sprite;
    private TextureAtlas _textureAtlas;
    private Color _debugShader;
    private bool _debugStatus;
    public ZoneType _physics;
    public bool _isTalking = false;



    //Constructor
    public Vendor(Script script)
    {

        _script = script;

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
        int _hitboxy = (int)_position.Y + (int)_sprite.Height * (2/3);
        Hitbox = new RectZone(_hitboxx, _hitboxy, (int)_sprite.Width, (int)(_sprite.Height)/3, ZoneType.Solid, _debugStatus, _debugShader);
        
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
        
        this.Hitbox.DisjointDraw(SpriteBatch,new Vector2(_position.X, _position.Y +  (int)_sprite.Height * 2/3), Core._pixel,  _debugStatus, _debugShader);
        //Zone Manager accepts Zone and object to determine which areas are interactive
        Core.ZoneManager.RegisterZone(Hitbox, this);    

    }

    //Object Function on Player Interaction
    public override void Interact(PlayerIndex playerIndex)
    {
        var interactFunc = _script.Globals.Get("onInteract");
        if (interactFunc.Type == DataType.Function)
        {
            _script.Call(interactFunc);
        }


        Core.DialogueManagers[(int)playerIndex].LoadDialogue(_script.Globals.Get("dialogue").Table);
        
            
        
    }

}