using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.ObjecTypes;


namespace Revelry.GameObjects;

public class Player1 : Players
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
      
    public Player1(AnimatedSprite sprite, PlayerIndex playerIndex)
    {
        _sprite = sprite;
        _playerIndex = playerIndex;
    }

    public void Initialize(Vector2 startingPosition)
    {

        //Initialize where sprite will be initially drawn NOTE: This does not do the drawing, check Draw() function for that
        _position = startingPosition;
        //Initialize player Dialogue Manager at player index
        Core.DialogueManagers[(int)_playerIndex].Initialize();
        // Zero out the movement timer.
        _movementTimer = TimeSpan.Zero;

        //Build area to serve as player object hitbox
        _hitbox = new RectZone((int)_position.X, (int)_position.Y + ((int)_sprite.Height), (int)_sprite.Width, (int)_sprite.Height/3, ZoneType.Path, true);
        //Build area to serve as player object interaction range
        _interactBox = new RectZone((int)_position.X, (int)_position.Y, (int)_sprite.Width, (int)_sprite.Height, ZoneType.Path, true);
        
    }


    private void HandleInput()
    {

        //START - PLAYER MOVEMENT CONTROLS - If Player inputs command and is not in dialogue
        if (GameController.MoveUp() && !Core.DialogueManagers[(int)_playerIndex].IsOpen)
        {

            nextPosition.Y = _position.Y - MOVEMENT_SPEED;
            //Calculate origin (top-left) for interaction box depending on direction player is facing
            currentDirection = new Vector2((_position.X), (_position.Y - 50));
            //Build interact box at new origin
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, (int)_sprite.Width, 120, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - UP
        }
        if (GameController.MoveDown()  && !Core.DialogueManagers[(int)_playerIndex].IsOpen)
        {
            nextPosition.Y = _position.Y + MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X), (_position.Y + _sprite.Height));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, (int)_sprite.Width, 90, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - DOWN

        }
        if (GameController.MoveLeft()  && !Core.DialogueManagers[(int)_playerIndex].IsOpen)
        {
            nextPosition.X = _position.X - MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X - 60), (_position.Y));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, 60, (int)_sprite.Height, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - LEFT

        }
        if (GameController.MoveRight()  && !Core.DialogueManagers[(int)_playerIndex].IsOpen)
        {
            nextPosition.X = _position.X + MOVEMENT_SPEED;
            currentDirection = new Vector2((_position.X + _sprite.Width), (_position.Y));
            _interactBox = new RectZone((int)currentDirection.X, (int)currentDirection.Y, 60, (int)_sprite.Height, ZoneType.Path, true);//Reposition zone for object Interaction to match player orientation - RIGHT

        }
        //When player presses action, check if interactable object is within range
        if (GameController.Action()  && !Core.DialogueManagers[(int)_playerIndex].IsOpen)
        {
            //Check zone manager if interactable object is overlapping with Player Interact Box
            var interactable = Core.ZoneManager.CheckInteractions(_interactBox);
            //Run interactable interact function
            if (interactable != null) { interactable.Interact(_playerIndex); }
        }
        if (GameController.Action()  && !Core.DialogueManagers[(int)_playerIndex].IsOpen)
        {
            //Check zone manager if interactable object is overlapping with Player Interact Box
            var interactable = Core.ZoneManager.CheckInteractions(_interactBox);
            //Run interactable interact function
            if (interactable != null) { interactable.Interact(_playerIndex); }
        }  


        // Keep the horizontal test at the player's next X, but Y starts halfway down the sprite
        Vector2 horizontalTest = new Vector2(nextPosition.X, _position.Y + (int)(_sprite.Height * 2/3));

        // Width covers the full sprite, height is half the sprite
        RectZone horizontalZone = new RectZone(
            (int)horizontalTest.X,
            (int)horizontalTest.Y,
            (int)_sprite.Width,
            (int)(_sprite.Height / 3),
            ZoneType.Solid,
            true);

    // Only update X if no collision
    if (!Core.ZoneManager.CheckCollision(horizontalZone, out _))
    {
        _position.X = nextPosition.X; // <- use nextPosition.X, not horizontalTest.X
    }


            Vector2 verticalTest = new Vector2(_position.X, nextPosition.Y + (int)(_sprite.Height*2/3));
            RectZone verticalZone = new RectZone(
                (int)verticalTest.X,
                (int)verticalTest.Y,
                (int)_sprite.Width,
                (int)(_sprite.Height / 3),
                ZoneType.Solid,
                true);

    if (!Core.ZoneManager.CheckCollision(verticalZone, out _))
    {
        _position.Y = nextPosition.Y;
    }
        
    }

    public void Update(GameTime gameTime)
    {
        // Update the animated sprite.
        _sprite.Update(gameTime);

        // Handle any player input
        if(!Core.DialogueManagers[(int)_playerIndex].IsOpen)
            HandleInput();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Render Player1 sprite, hitbox, and interaction zone
        _sprite.Draw(Core.SpriteBatch, _position);
        
        //Use Disjoint Draw method of RectZone so draw hitbox and interact zone at dynamic positions
        _hitbox.DisjointDraw(Core.SpriteBatch,new Vector2(_position.X, _position.Y + (int)_sprite.Height * 2/3), Core._pixel, GameController.DebugToggle(),new Color(255, 0, 0, 128) );//Hitbox for collision detection
        _interactBox.DisjointDraw(Core.SpriteBatch, currentDirection, Core._pixel, GameController.DebugToggle(), new Color(0, 255, 0, 128));//Interaction zone used for player-object interactions

    }

    public Vector2 getPosition() => _position;

}