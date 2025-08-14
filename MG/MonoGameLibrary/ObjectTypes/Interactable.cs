using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*Summary
The Interactable class is to be inherited by all objects the player can interact with: Chests, Doors, NPC's, etc
Includes:
- Hitbox for interaction/collision detection
- ZoneManager to pass all instance's RectZones to Core Zone Manager
*/
public abstract class Interactable
{
    public RectZone Hitbox { get; protected set; }
    public ZoneManager ZoneManager { get; protected set; }
    public abstract void Interact();
    public abstract void Initialize();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch? SpriteBatch);
}
