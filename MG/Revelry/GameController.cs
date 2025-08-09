using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace Revelry;

/// <summary>
/// Provides a game-specific input abstraction that maps physical inputs
/// to game actions, bridging our input system with game-specific functionality.
/// </summary>
public static class GameController
{
    private static KeyboardInfo s_keyboard => Core.Input.Keyboard;
    private static GamePadInfo s_gamePad => Core.Input.GamePads[(int)PlayerIndex.One];

    /// <summary>
    /// Returns true if the player has triggered the "move up" action.
    /// </summary>
    public static bool MoveUp()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Up) ||
               Keyboard.GetState().IsKeyDown(Keys.W) ||
               GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.5f;
    }

    /// <summary>
    /// Returns true if the player has triggered the "move down" action.
    /// </summary>
    public static bool MoveDown()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Down) ||
           Keyboard.GetState().IsKeyDown(Keys.S) ||
           GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed ||
           GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.5f;
    }

    /// <summary>
    /// Returns true if the player has triggered the "move left" action.
    /// </summary>
    public static bool MoveLeft()
    {
        return Keyboard.GetState().IsKeyDown(Keys.Left) ||
               Keyboard.GetState().IsKeyDown(Keys.A) ||
               GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.5f;
    }

    /// <summary>
    /// Returns true if the player has triggered the "move right" action.
    /// </summary>
    public static bool MoveRight()
    {
       return Keyboard.GetState().IsKeyDown(Keys.Right) ||
               Keyboard.GetState().IsKeyDown(Keys.D) ||
               GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.5f;
    }

    /// <summary>
    /// Returns true if the player has triggered the "pause" action.
    /// </summary>
    public static bool Pause()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Escape) ||
               s_gamePad.WasButtonJustPressed(Buttons.Start);
    }

    /// <summary>
    /// Returns true if the player has triggered the "action" button,
    /// typically used for menu confirmation.
    /// </summary>
    public static bool Action()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Enter) ||
               s_gamePad.WasButtonJustPressed(Buttons.A);
    }
}
