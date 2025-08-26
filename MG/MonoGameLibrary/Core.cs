using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Utilities;
using MonoGameLibrary.Physics;
using System.IO;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace MonoGameLibrary;

public class Core : Game
{
    internal static Core s_instance;


    /// Gets a reference to the Core instance.

    public static Core Instance => s_instance;

    private static Scene s_activeScene;
    private static Scene s_nextScene;

    public static Texture2D _pixel;

    public static string scriptsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "scripts");

    /// Gets the graphics device manager to control the presentation of graphics.

    public static GraphicsDeviceManager Graphics { get; private set; }


    /// Gets the graphics device used to create graphical resources and perform primitive rendering.

    public static new GraphicsDevice GraphicsDevice { get; private set; }


    /// Gets the sprite batch used for all 2D rendering.

    public static SpriteBatch SpriteBatch { get; private set; }


    /// Gets the content manager used to load global assets.

    public static new ContentManager Content { get; private set; }


    /// Gets a reference to the input management system.

    public static InputManager Input { get; private set; }


    /// Gets or Sets a value that indicates if the game should exit when the esc key on the keyboard is pressed.

    public static bool ExitOnEscape { get; set; }

    /// <summary>
    /// Gets a reference to the audio control system.
    /// </summary>
    public static AudioController Audio { get; private set; }
    
    public static DialogueManager DialogueManager { get; private set; }

    public static ZoneManager ZoneManager { get; private set; }
    public static List<DialogueManager> DialogueManagers { get; private set; }
    public static ScriptManager ScriptManager { get; private set; }




    /// Creates a new Core instance.

    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public Core(string title, int width, int height, bool fullScreen)
    {
        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Set the graphics defaults.
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();

        // Set the window title.
        Window.Title = title;

        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "Content";

        // Mouse is visible by default.
        IsMouseVisible = true;


    }

    protected override void Initialize()
    {

        base.Initialize();

        // Set the core's graphics device to a reference of the base Game's
        // graphics device.
        GraphicsDevice = base.GraphicsDevice;

        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a new input manager.
        Input = new InputManager();

        // Create a new audio controller.
        Audio = new AudioController();

        //Set Debug Pixel Value to White
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        ZoneManager = new ZoneManager();
        DialogueManagers = new List<DialogueManager>();

        foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
        {
            DialogueManagers.Add(new DialogueManager());
        }
        

        ScriptManager = new ScriptManager();
    }

    protected override void UnloadContent()
    {
        // Dispose of the audio controller.
        Audio.Dispose();

        base.UnloadContent();
    }
    protected override void Update(GameTime gameTime)
    {
        // Update the input manager.
        Input.Update(gameTime);

        // Update the audio controller.
        Audio.Update();

        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // if there is a next scene waiting to be switch to, then transition
        // to that scene.
        if (s_nextScene != null)
        {
            TransitionScene();
        }

        // If there is an active scene, update it.
        if (s_activeScene != null)
        {
            s_activeScene.Update(gameTime);
        }

        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        // If there is an active scene, draw it.
        if (s_activeScene != null)
        {
            s_activeScene.Draw(gameTime);
        }

        base.Draw(gameTime);
    }

    public static void ChangeScene(Scene next)
    {
        // Only set the next scene value if it is not the same
        // instance as the currently active scene.
        if (s_activeScene != next)
        {
            s_nextScene = next;
        }
    }

    private static void TransitionScene()
    {
        // If there is an active scene, dispose of it.
        if (s_activeScene != null)
        {
            s_activeScene.Dispose();
        }

        // Force the garbage collector to collect to ensure memory is cleared.
        GC.Collect();

        // Change the currently active scene to the new scene.
        s_activeScene = s_nextScene;

        // Null out the next scene value so it does not trigger a change over and over.
        s_nextScene = null;

        // If the active scene now is not null, initialize it.
        // Remember, just like with Game, the Initialize call also calls the
        // Scene.LoadContent
        if (s_activeScene != null)
        {
            s_activeScene.Initialize();
        }
    }

   public static Color HexToColor(string hex)
{
    if (string.IsNullOrWhiteSpace(hex))
        return Color.White;

    // Remove "#" if present
    hex = hex.Replace("#", "");

    byte r, g, b, a = 255;

    if (hex.Length == 6)
    {
        r = Convert.ToByte(hex.Substring(0, 2), 16);
        g = Convert.ToByte(hex.Substring(2, 2), 16);
        b = Convert.ToByte(hex.Substring(4, 2), 16);
    }
    else if (hex.Length == 8)
    {
        r = Convert.ToByte(hex.Substring(0, 2), 16);
        g = Convert.ToByte(hex.Substring(2, 2), 16);
        b = Convert.ToByte(hex.Substring(4, 2), 16);
        a = Convert.ToByte(hex.Substring(6, 2), 16);
    }
    else
    {
        throw new ArgumentException("Invalid hex color length. Must be 6 or 8 characters.");
    }

    return new Color(r, g, b, a);
}


}
