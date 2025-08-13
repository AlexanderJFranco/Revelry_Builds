using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

public  class RectZone : IEquatable<RectZone>
{

    //Class variables/Operator declarations
    public static bool operator ==(RectZone lhs, RectZone rhs) => lhs.Equals(rhs);
    public static bool operator !=(RectZone lhs, RectZone rhs) => !lhs.Equals(rhs);
    public  int _width;
    public  int _height;
    /// Returns the hash code for this rectangle.
    /// <returns>The hash code for this rectangle as a 32-bit signed integer.</returns>
    public override  int GetHashCode() => HashCode.Combine(X, Y, _width, _height);
    private static  RectZone s_empty = new RectZone(0, 0, 0, 0, ZoneType.Path, false);
    /// The x-coordinate of the center of this RectZone.
    public  int X;
    /// The y-coordinate of the center of this RectZone.
    public  int Y;
    public  bool DebugMode { get;  }
    public  Color DebugColor { get; set; }
   

    public Point Location => new Point(X, Y);
    /// Gets a RectZone with X=0, Y=0, and Radius=0.
    public static RectZone Empty => s_empty;
    /// Gets a value that indicates whether this RectZone has a radius of 0 and a location of (0, 0).
    ///     /// Returns a value that indicates whether this circle and the specified object are equal
    /// <param name="obj">The object to compare with this circle.</param>
    /// <returns>true if this circle and the specified object are equal; otherwise, false.</returns>
    public override  bool Equals(object obj) => obj is RectZone other && Equals(other);
    /// Returns a value that indicates whether this circle and the specified circle are equal.
    /// <param name="other">The circle to compare with this circle.</param>
    /// <returns>true if this circle and the specified circle are equal; otherwise, false.</returns>
    public  bool Equals(RectZone other) => this.X == other.X &&
                                                    this.Y == other.Y &&
                                                    this._width == other._width &&
                                                    this._height == other._height;
    public  bool IsEmpty => X == 0 && Y == 0 && _width == 0 && _height == 0;
    /// Gets the y-coordinate of the highest point on this RectZone.
    public  int Top => Y - (int)Math.Floor((double)_height / 2);
    /// Gets the y-coordinate of the lowest point on this RectZone.
    public  int Bottom => Y + (int)Math.Floor((double)_height / 2);
    /// Gets the x-coordinate of the leftmost point on this RectZone.
    public  int Left => X - (int)Math.Floor((double)_width / 2);
    /// Gets the x-coordinate of the rightmost point on this RectZone.
    public  int Right => X + (int)Math.Floor((double)_width / 2);
    // Static shared resources for drawing
    public static SpriteBatch SharedSpriteBatch { get; set; }
    public static Texture2D SharedPixelTexture { get; set; }
    public ZoneType _zoneType;

    //Constructor RectZone(x origin, y origin ...)
    public RectZone(int x, int y, int width, int height, ZoneType physics, bool debug_Mode = false, Color? debug_Color = null)
    {
        X = x;
        Y = y;
        _width = width;
        _height = height;
        _zoneType = physics;
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
    
  // Intersects method to check overlap with another RectZone
    public bool Intersects(RectZone other)
    {
        // Check for separation on the X axis
        bool noOverlapX = (this.X + this._width) < other.X || this.X > (other.X + other._width);

        // Check for separation on the Y axis
        bool noOverlapY = (this.Y + this._height) < other.Y || this.Y > ( other.Y + _height);
        // If there’s separation on either axis, no intersection
        return !(noOverlapX || noOverlapY);
    }
    
    //Draw to SpriteBatch
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Texture2D pixel, bool debug_Mode, Color? shader)
    {

        //Check if debug mode is enabled
        if (!debug_Mode)
            DebugColor = new Color(0, 0, 0, 0);
        else
            DebugColor = (Color)shader;
        X = (int) position.X;
        Y = (int)position.Y;



        //Create Rectangle with current root position + width and height of instantiated object
        var rect = new Rectangle(
        (int)(position.X),
        (int)(position.Y),
        _width,
        _height);



        //Draw rectangle to current spritesheet
        spriteBatch.Draw(pixel, rect, DebugColor);


    }

}
