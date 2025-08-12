using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;


namespace Revelry.GameObjects;

public class Player1
{
    private static readonly TimeSpan s_movementTime = TimeSpan.FromMilliseconds(200);

    // The amount of time that has elapsed since the last movement update.
    private TimeSpan _movementTimer;

    // Normalized value (0-1) representing progress between movement ticks for visual interpolation
    private float _movementProgress;

    // The next direction to apply to the head of the slime chain during the
    // next movement update.
    private Vector2 _nextDirection;
    private Vector2 _direction = Vector2.Zero;

    // The AnimatedSprite used when drawing each slime segment
    private AnimatedSprite _sprite;



    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    private Vector2 _position;
    private Vector2 _nextPosition;
    private bool _showInteractBox = false;
    public RectZone _interactBox;



    public event EventHandler BodyCollision;

    /// <summary>
    /// Creates a new Slime using the specified animated sprite.
    /// </summary>
    /// <param name="sprite">The AnimatedSprite to use when drawing the slime.</param>
    public Player1(AnimatedSprite sprite)
    {
        _sprite = sprite;
    }


    public void Initialize(Vector2 startingPosition)
    {



        //Initialize where sprite will be initially drawn NOTE: This does not do the drawing, check Draw() function for that
        _position = startingPosition;

        // Zero out the movement timer.
        _movementTimer = TimeSpan.Zero;
        _interactBox = new RectZone((int)_position.X, (int)_position.Y, 76, 164, true);


    }


    private void HandleInput()
    {
        Vector2 potentialNextDirection = Vector2.Zero;

        if (GameController.MoveUp())
        {
            _position.Y -= MOVEMENT_SPEED;
            Console.WriteLine("Direction: " + potentialNextDirection);
        }
        if (GameController.MoveDown())
        {
            _position.Y += MOVEMENT_SPEED;
            Console.WriteLine("Direction: " + potentialNextDirection);
        }
        if (GameController.MoveLeft())
        {
            _position.X -= MOVEMENT_SPEED;
            Console.WriteLine("Direction: " + potentialNextDirection + "Movement Timer: " + _movementTimer);
        }
        if (GameController.MoveRight())
        {
            _position.X += MOVEMENT_SPEED;
            Console.WriteLine("Direction: " + potentialNextDirection);
        }
        if (GameController.Action())
        {

        }
        


    }


    public void Update(GameTime gameTime)
    {
        // Update the animated sprite.
        _sprite.Update(gameTime);

        // Handle any player input
        HandleInput();

        // Increment the movement timer by the frame elapsed time.
        _movementTimer += gameTime.ElapsedGameTime;


        // Update the movement lerp offset amount
        _movementProgress = (float)(_movementTimer.TotalSeconds / s_movementTime.TotalSeconds);
    }


    public void Draw(SpriteBatch spriteBatch, Boolean debug_Mode)
    {
        // Iterate through each segment and draw it

        // Calculate the visual position of the segment at the moment by
        // lerping between its "at" and "to" position by the movement
        // offset lerp amount
        Vector2 pos = Vector2.Lerp(_position, _nextPosition, _movementProgress);

        _sprite.Draw(Core.SpriteBatch, _position);

        
        _interactBox.Draw(Core.SpriteBatch, _position, Core._pixel, debug_Mode);


    }

    public Circle GetBounds()
    {
        int x = (int)(_position.X + _sprite.Width * 0.5f);
        int y = (int)(_position.Y + _sprite.Height * 0.5f);
        int radius = (int)(_sprite.Width * 0.25f);

        return new Circle(x, y, radius);
    }

 

    public Vector2 getPosition() => _position;

}