using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IDrawableEntity
{
    Vector2 Position { get; }   // always reflects current position
    float Depth { get; }        // computed from Position.Y
    void Draw(SpriteBatch spriteBatch);
}
