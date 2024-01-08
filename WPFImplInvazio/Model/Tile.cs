namespace Model;

public class Tile
{
    public TileVariant Variant { get; set; }

    public Tile(TileVariant variant)
    {
        Variant = variant;
    }

    public Tile() : this(TileVariant.Empty) { }
}

public enum TileVariant
{
    Empty = 0,
    Enemy = 1,
    Soldier = 2,
}
