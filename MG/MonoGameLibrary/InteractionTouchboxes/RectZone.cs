using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Graphics;
public readonly struct RectZone : IEquatable<RectZone>
{

    //Class variables/Operator declarations
    public static bool operator ==(RectZone lhs, RectZone rhs) => lhs.Equals(rhs);
    public static bool operator !=(RectZone lhs, RectZone rhs) => !lhs.Equals(rhs);
    public readonly int _width;
    public readonly int _height;
    /// Returns the hash code for this rectangle.
    /// <returns>The hash code for this rectangle as a 32-bit signed integer.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, _width, _height);
    private static readonly RectZone s_empty = new RectZone();
    /// The x-coordinate of the center of this RectZone.
    public readonly int X;
    /// The y-coordinate of the center of this RectZone.
    public readonly int Y;
    public readonly bool DebugMode { get; }
    public readonly Color DebugColor { get; }

    public readonly Point Location => new Point(X, Y);
    /// Gets a RectZone with X=0, Y=0, and Radius=0.
    public static RectZone Empty => s_empty;
    /// Gets a value that indicates whether this RectZone has a radius of 0 and a location of (0, 0).
    ///     /// Returns a value that indicates whether this circle and the specified object are equal
    /// <param name="obj">The object to compare with this circle.</param>
    /// <returns>true if this circle and the specified object are equal; otherwise, false.</returns>
    public override readonly bool Equals(object obj) => obj is RectZone other && Equals(other);
    /// Returns a value that indicates whether this circle and the specified circle are equal.
    /// <param name="other">The circle to compare with this circle.</param>
    /// <returns>true if this circle and the specified circle are equal; otherwise, false.</returns>
    public readonly bool Equals(RectZone other) => this.X == other.X &&
                                                    this.Y == other.Y &&
                                                    this._width == other._width &&
                                                    this._height == other._height;
    public readonly bool IsEmpty => X == 0 && Y == 0 && _width == 0 && _height == 0;
    /// Gets the y-coordinate of the highest point on this RectZone.
    public readonly int Top => Y - (int)Math.Floor((double)_height / 2);
    /// Gets the y-coordinate of the lowest point on this RectZone.
    public readonly int Bottom => Y + (int)Math.Floor((double)_height / 2);
    /// Gets the x-coordinate of the leftmost point on this RectZone.
    public readonly int Left => X - (int)Math.Floor((double)_width / 2);
    /// Gets the x-coordinate of the rightmost point on this RectZone.
    public readonly int Right => X + (int)Math.Floor((double)_width / 2);
    // Static shared resources for drawing
    public static SpriteBatch SharedSpriteBatch { get; set; }
    public static Texture2D SharedPixelTexture { get; set; }

    //Constructor RectZone(x origin, y origin ...)
    public RectZone(int x, int y, int width, int height, bool debug_Mode = false, Color? debug_Color = null)
    {
        X = x;
        Y = y;
        _width = width;
        _height = height;
        DebugMode = debug_Mode;
        DebugColor = debug_Color ?? new Color(255, 0, 0, 128); // default semi-transparent red

    }
    //Constructor accepting Point
    public RectZone(Point location, int width, int height)
    {
        X = location.X;
        Y = location.Y;
        _width = width;
        _height = height;
    }
    
    
    //Draw to SpriteBatch
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D pixel, bool debug_Mode)
    {

        //Check if debug mode is enabled
        if (!debug_Mode)
            return;

        //Create Rectangle with current root position + width and height of instantiated object
        var rect = new Rectangle(
            (int)(position.X),
            (int)(position.Y),
            _width,
            _height);


        Color debugColor = new Color(Color.Green, 0.02f); // set debug shader

        //Draw rectangle to current spritesheet
        spriteBatch.Draw(pixel, rect, debugColor);


    }

}
