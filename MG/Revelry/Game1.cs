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
    private Interactable _obj;
    private ScriptManager _scriptManager;

    public Game1() : base("Revelry", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, DEFAULT_FULLSCREEN)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        _obj.Initialize();
        _player1.Initialize(Microsoft.Xna.Framework.Vector2.Zero);
        
    }

    protected override void LoadContent()
    {
        //Create Script Manager
        _scriptManager = new ScriptManager();
        

        // TODO: use this.Content to load your game content here
        // Create the texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/dev_assets/t1_atlas.xml");

        // Create the animated sprite for the slime from the atlas.
        AnimatedSprite player_animation = atlas.CreateAnimatedSprite("player1-animation");
        player_animation.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);

        //Create interactable object
        _obj = new Interactable(_scriptManager.GetScript(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../scripts", "audio_test.lua")));
        

        _player1 = new Player1(player_animation);
        _tilemap = Tilemap.FromFile(Content, "images/dev_assets/tilesets/dev_grass-definition.xml");
        _tilemap.Scale = new Microsoft.Xna.Framework.Vector2(4.0f, 4.0f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _player1.Update(gameTime);
        _obj.Interact();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        
        // TODO: Add your drawing code here
        _tilemap.Draw(Core.SpriteBatch);
        _obj.Draw();
        _player1.Draw();
        base.Draw(gameTime);
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    
    }
}
