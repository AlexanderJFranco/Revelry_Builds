using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Revelry.GameObjects;
using System;
using System.IO;


namespace Revelry;

public class Game1 : Core
{


    const int DEFAULT_WINDOW_WIDTH = 1280;
    const int DEFAULT_WINDOW_HEIGHT = 720;
    const bool DEFAULT_FULLSCREEN = false;
    private Player1 _player1;
    private Tilemap _tilemap;
    private Vendor _vendor;
    private ScriptManager _scriptManager;
    public ZoneManager _zoneManager;

    public Game1() : base("Revelry", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, DEFAULT_FULLSCREEN)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        _vendor.Initialize();
        _player1.Initialize(Microsoft.Xna.Framework.Vector2.Zero);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        //Create Script Manager
        _scriptManager = new ScriptManager();

        //Create Zone Manager
        _zoneManager = new ZoneManager();

        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/dev_assets/t1_atlas.xml");

        // Create the animated sprite for the slime from the atlas.
        AnimatedSprite player_animation = atlas.CreateAnimatedSprite("player1-animation");
        player_animation.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);

        //Create interactable object
       _vendor = new Vendor(_scriptManager.GetScript(Core.scriptsFolder, "vendor.lua"), _zoneManager);

        _player1 = new Player1(player_animation, _zoneManager);
        _tilemap = Tilemap.FromFile(Content, "images/dev_assets/tilesets/dev_grass-definition.xml");
        _tilemap.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _player1.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //Level Drawing Start
        _tilemap.Draw(Core.SpriteBatch);
        _vendor.Draw();
        _player1.Draw(Core.SpriteBatch);
        base.Draw(gameTime);
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }

    
   
}
