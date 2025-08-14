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

    // The next direction to apply to the head of the slime chain during the
    // next movement update.
    private Vector2 _direction = Vector2.Zero;

    // The AnimatedSprite used when drawing each slime segment
    private AnimatedSprite _sprite;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    //Location on screen
    private Vector2 _position;
    //Collision hitbox
    public RectZone _hitbox;
    //Object Interaction zone
    public RectZone _interactBox;
    //Tracks players current direct - Up, Down, Left, Right
    public Vector2 currentDirection;

    public Vector2 nextPosition = Vector2.Zero;
    public ZoneManager _zoneManager;




    /// <summary>
    /// Creates a new Slime using the specified animated sprite.
    /// </summary>
    /// <param name="sprite">The AnimatedSprite to use when drawing the slime.</param>
    public Player1(AnimatedSprite sprite, ZoneManager zoneManager)
    {
        _sprite = sprite;
        _zoneManager = zoneManager;
    }

    public void Initialize(Vector2 startingPosition)
    {

        //Initialize where sprite will be initially drawn NOTE: This does not do the drawing, check Draw() function for that
        _position = startingPosition;
        
        // Zero out the movement timer.
        _movementTimer = TimeSpan.Zero;
        _hitbox = new RectZone((int)_position.X, (int)_position.Y + (int)_sprite.Height/2 , (int)_sprite.Width, (int)_sprite.Height/2, ZoneType.Path, true);
        _interactBox = new RectZone((int)_position.X, (int)_position.Y, (int)_sprite.Width, (int)_sprite.Height, ZoneType.Path, true);
        
    }


    private void HandleInput()
    {

        //START - PLAYER MOVEMENT CONTROLS
        if (GameController.MoveUp())
        {

            nextPosition.Y = _position.Y - MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X), (_position.Y - 60));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, (int)_sprite.Width, 60, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - UP
        }
        if (GameController.MoveDown())
        {
            nextPosition.Y = _position.Y + MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X), (_position.Y + _sprite.Height));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, (int)_sprite.Width, 60, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - DOWN

        }
        if (GameController.MoveLeft())
        {
            nextPosition.X = _position.X - MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X - 60), (_position.Y));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, 60, (int)_sprite.Height, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - LEFT

        }
        if (GameController.MoveRight())
        {
            nextPosition.X = _position.X + MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X + _sprite.Width), (_position.Y));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, 60, (int)_sprite.Height, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - RIGHT

        }
        if (GameController.Action())
        {
            var interactable = _zoneManager.CheckInteractions(_interactBox);
            if(interactable != null) { interactable.Interact(); }
        }

       







        // Keep the horizontal test at the player's next X, but Y starts halfway down the sprite
        Vector2 horizontalTest = new Vector2(nextPosition.X, _position.Y + (int)(_sprite.Height / 2));

        // Width covers the full sprite, height is half the sprite
        RectZone horizontalZone = new RectZone(
            (int)horizontalTest.X,
            (int)horizontalTest.Y,
            (int)_sprite.Width,
            (int)(_sprite.Height / 2),
            ZoneType.Solid,
            true);

// Only update X if no collision
if (!_zoneManager.CheckCollision(horizontalZone, out _))
{
    _position.X = nextPosition.X; // <- use nextPosition.X, not horizontalTest.X
}


        Vector2 verticalTest = new Vector2(_position.X, nextPosition.Y + (int)(_sprite.Height / 2));
        RectZone verticalZone = new RectZone(
            (int)verticalTest.X,
            (int)verticalTest.Y,
            (int)_sprite.Width,
            (int)(_sprite.Height / 2),
            ZoneType.Solid,
            true);

if (!_zoneManager.CheckCollision(verticalZone, out _))
{
    _position.Y = nextPosition.Y;
}
        
    }

    public void Update(GameTime gameTime)
    {
        // Update the animated sprite.
        _sprite.Update(gameTime);

        // Handle any player input
        HandleInput();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Render Player1 sprite, hitbox, and interaction zone
        _sprite.Draw(Core.SpriteBatch, _position);
        _hitbox.DisjointDraw(Core.SpriteBatch,new Vector2(_position.X, (int)_position.Y + (int)_sprite.Height/2), Core._pixel, GameController.DebugToggle(),new Color(255, 0, 0, 128) );//Hitbox for collision detection
        _interactBox.DisjointDraw(Core.SpriteBatch, currentDirection, Core._pixel, GameController.DebugToggle(), new Color(0, 255, 0, 128));//Interaction zone used for player-object interactions

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