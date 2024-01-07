namespace Matrix;

/// <summary>
/// A simple, coordinate with casting and overlfowing capabilities
/// </summary>
public struct Position
{
    public int x;
    public int y;

    #region Casting
    public static implicit operator (int, int)(Position p) => (p.y, p.x);
    public static explicit operator Position((int, int) c) => new Position { x = c.Item2, y = c.Item1 };
    #endregion

    #region Comparison
    public static bool operator ==(Position lhs, Position rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }
    public static bool operator !=(Position lhs, Position rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y;
    }

    public override bool Equals(object? obj)
    {
        return obj != null && obj is Position pos && pos == this;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }
    #endregion

    #region Operations
    public static Position operator +(Position lhs, Position rhs)
    {
        return new Position { x = lhs.x + rhs.x, y = lhs.y + rhs.y };
    }
    public static Position operator -(Position lhs, Position rhs)
    {
        return new Position { x = lhs.x - rhs.x, y = lhs.y - rhs.y };
    }

    public static Position operator *(Position lhs, int rhs)
    {
        return new Position { x = lhs.x * rhs, y = lhs.y * rhs };
    }
    public static Position operator /(Position lhs, int rhs)
    {
        return new Position { x = lhs.x / rhs, y = lhs.y / rhs };
    }
    /// <summary>
    /// Performs an overflow operation whitch ensures that the first coordinate will be between (y:0,x:0) and the given second position
    /// <br/>
    /// The method makes an overflowing element to return
    /// <br/>
    /// Works with negative indexes.
    /// </summary>
    /// <param name="coordinate">The indexing position</param>
    /// <param name="bounds">The boundary, which is not included</param>
    /// <returns></returns>
    public static Position operator %(Position coordinate, Position bounds)
    {
        int x = coordinate.x;
        if (x < 0)
        {
            x = bounds.x - (-x % bounds.x);
        }
        else
        {
            x = x % bounds.x;
        }
        int y = coordinate.y;
        if (y < 0)
        {
            y = bounds.y - (-y % bounds.y);
        }
        else
        {
            y = y % bounds.y;
        }
        return new Position { x = x, y = y };
    }
    #endregion

}
