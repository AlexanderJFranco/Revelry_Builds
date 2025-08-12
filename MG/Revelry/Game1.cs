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
    private static bool debug_Mode = false;





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

        base.LoadContent();
        //Create Script Manager
        _scriptManager = new ScriptManager();

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

        debug_Mode = GameController.DebugToggle();


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _player1.Update(gameTime);
        collisionChecks();
        //_obj.Interact(); //would run test lua interact script, keeping in case I forget something
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.CornflowerBlue);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);


        // TODO: Add your drawing code here
        _tilemap.Draw(Core.SpriteBatch);
        _obj.Draw();
        _player1.Draw(Core.SpriteBatch, debug_Mode);


        //_player1._interactBox.Draw(Core.SpriteBatch, _player1.getPosition(), Core._pixel, true );
        base.Draw(gameTime);
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();


    }

    private void collisionChecks()
    {
        Circle obj_bounds = _obj.GetBounds();
        Circle player1_bounds = _player1.GetBounds();

        if (player1_bounds.Intersects(obj_bounds))
        {
            Console.WriteLine("Touching");
        }
    }
}
