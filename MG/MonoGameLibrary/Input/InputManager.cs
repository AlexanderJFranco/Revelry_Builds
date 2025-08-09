using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Input;

public class InputManager
{

    
    /// Creates a new InputManager.
    
    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }
    }

    
    /// Updates the state information for the keyboard, mouse, and gamepad inputs.
    
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }



    /// Gets the state information of keyboard input.

    public KeyboardInfo Keyboard { get; private set; }

    
    /// Gets the state information of mouse input.
    
    public MouseInfo Mouse { get; private set; }

    
    /// Gets the state information of a gamepad.
    
    public GamePadInfo[] GamePads { get; private set; }


 }

