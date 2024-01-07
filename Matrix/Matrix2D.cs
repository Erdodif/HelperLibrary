using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Matrix;

/// <summary>
/// A one dimentional array representing a 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Matrix2D<T>
{
    #region Properties
    /// <summary>
    /// The matrix's virtual width
    /// </summary>
    public int Width { get; protected set; }
    /// <summary>
    /// The matrix's virtual height
    /// </summary>
    public int Height { get; protected set; }

    /// <summary>
    /// The matrix's current area, to make linear enumeration possible
    /// </summary>
    public int Area { get => map.Length; }

    private T[] map;
    #endregion

    #region Indexers
    /// <summary>
    /// Determines whether the given index is in the bounds of the matrix
    /// </summary>
    /// <param name="index">The addressed index</param>
    /// <returns>True when the given index is valid</returns>
    public bool IsInBounds(int index) =>
        index >= 0 && index < map.Length;
    /// <summary>
    /// Determines whether the given position points to an existing location in the matrix
    /// </summary>
    /// <param name="p">The adressed point</param>
    /// <returns>True when the x is within the matrix's width and the y is within the matrix's height</returns>
    public bool IsInBounds(Position p) =>
        p.x >= 0 && p.y >= 0 && p.x < Width && p.y < Height;
    /// <summary>
    /// Determines whether the given position points to an existing location in the matrix
    /// </summary>
    /// <param name="p">The adressed point</param>
    /// <returns>True when the x is within the matrix's width and the y is within the matrix's height</returns>

    public bool IsInBounds(int y, int x) =>
        x >= 0 && y >= 0 && x < Width && y < Height;
    /// <summary>
    /// Determines whether the given position points to an existing location in the matrix
    /// </summary>
    /// <param name="p">The adressed point</param>
    /// <returns>
    /// True when the second value(Item2 as x) is within the matrix's width 
    /// and the first value(Item1 as y) is within the matrix's height
    /// </returns>
    public bool IsInBounds((int, int) coordinates) =>
        coordinates.Item2 >= 0 && coordinates.Item1 >= 0 && coordinates.Item2 < Width && coordinates.Item1 < Height;
    /// <summary>
    /// Returns an index to directly access this matrix's values
    /// </summary>
    /// <param name="p">A (y,x) based position that' y is within the matrix's height and x is within it's width</param>
    /// <returns>
    /// A single integer which is between the matrix's length
    /// </returns>
    /// <exception cref="MatrixOutOfBoundsException"/>
    public int IndexFromPosition(Position p)
    {
        if (!IsInBounds(p)) throw OutOfBounds(p.y, p.x);
        return (p.y * Width) + p.x;
    }
    /// <summary>
    /// Returns an index to directly access this matrix's values
    /// </summary>
    /// <param name="index">A single integer which is the linear index of the actual array</param>
    /// <returns>
    /// A (y,x) based Position which is between the matrix's width and height
    /// </returns>
    /// <exception cref="MatrixOutOfBoundsException"/>
    public Position PositionFromIndex(int index)
    {
        if (!IsInBounds(index)) throw OutOfBounds(index);
        return new Position { x = index % Width, y = index / Width };
    }
    /// <summary>
    /// Returns an index to directly access this matrix's values
    /// </summary>
    /// <param name="y">An integer within the matrix's height</param>
    /// <param name="x">An integer within the matrix's width</param>
    /// <returns>
    /// A single integer which is between the matrix's length
    /// </returns>
    /// <exception cref="MatrixOutOfBoundsException"/>
    public int IndexFromCoordinates(int y, int x)
    {
        if (!IsInBounds(y, x)) throw OutOfBounds(y, x);
        return (y * Width) + x;
    }

    public T this[int index]
    {
        get => map[index];
        set => map[index] = value;
    }
    public T this[Position p]
    {
        get => map[IndexFromPosition(p)];
        set => map[IndexFromPosition(p)] = value;
    }
    public T this[int y, int x]
    {
        get => map[IndexFromCoordinates(y, x)];
        set => map[IndexFromCoordinates(y, x)] = value;
    }
    #endregion

    #region Constructor
    public Matrix2D(int width, int height)
    {
        Width = width;
        Height = height;
        map = new T[Width * Height];
    }

    public Matrix2D(int size) : this(size, size) { }

    public Matrix2D(int width,int height,T initValue) : this(width, height)
    {
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = initValue;
        }
    }
    #endregion

    #region Exception
    private MatrixOutOfBoundsException OutOfBounds(int y, int x) => new MatrixOutOfBoundsException(this.Height, this.Width, y, x);
    private MatrixOutOfBoundsException OutOfBounds(int index) => new MatrixOutOfBoundsException(this.Area, index);
    #endregion

    #region Showing
    public override string ToString()
    {
        StringBuilder s = new StringBuilder($"Matrix<{typeof(T)}>; Width:{Width}, Height:{Height}, Values:\n");
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                s.Append(this[y, x]?.ToString() ?? "∅");
                s.Append("; ");
            }
            s.Append('\n');
        }
        return s.ToString();
    }

    public string ToString(bool invert)
    {
        if (!invert) return ToString();
        StringBuilder s = new StringBuilder($"Matrix<{typeof(T)}>; Width:{Width}, Height:{Height}, Values:\n");
        for (int y = Height - 1; y > -1; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                s.Append(this[y, x]?.ToString() ?? "∅");
                s.Append("; ");
            }
            s.Append('\n');
        }
        return s.ToString();
    }

    public string ToString(bool invert, bool format) => format ? ToString(s => s?.ToString() ?? "", invert) : ToString(invert);

    public string ToString(Func<T, string> converter, bool invert = false)
    {
        List<List<string>> positions = new List<List<string>>();
        int padwidth = 0;
        for (int y = 0; y < Height; y++)
        {
            positions.Add(new List<string>());
            for (int x = 0; x < Width; x++)
            {
                string text = converter(this[y, x]);
                positions[y].Add(text);
                if (padwidth < text.Length)
                {
                    padwidth = text.Length;
                }
            }
        }
        StringBuilder s = new StringBuilder($"Matrix<{typeof(T)}>; Width:{Width}, Height:{Height}, Values:\n");
        if (!invert)
        {
            for (int y = Height - 1; y > -1; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    s.Append(positions[y][x].PadLeft(padwidth));
                    s.Append(";");
                }
                s.Append('\n');
            }
        }
        else
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    s.Append(positions[y][x].PadLeft(padwidth));
                    s.Append("; ");
                }
                s.Append('\n');
            }
        }
        return s.ToString();
    }
    #endregion
}

public class MatrixOutOfBoundsException : ArgumentException
{
    public MatrixOutOfBoundsException() : base("Matrix accessed outside it's bounds!") { }
    public MatrixOutOfBoundsException(int length, int requestIndex) : base($"Matrix accessed outside it's bounds! Max index is {length}, yet {requestIndex} was requested.") { }
    public MatrixOutOfBoundsException(int boundY, int boundX, int requestY, int requestX) :
        base($"Matrix indexed outside of it's bounds! Requested (y: {requestY}, x: {requestX}), yet the bounds were (y: {boundY}, x: {boundX}).")
    { }

    public MatrixOutOfBoundsException(Position bounds, Position requestedPos) : this(bounds.y, bounds.x, requestedPos.y, requestedPos.x) { }
}
