
namespace Model;

public delegate void UpdateHandler(List<int> indexes);
public delegate void GameOverHandler(bool win);


public class TimeBasedGame
{
    #region Properties
    public Random random = new Random();
    private int Enemydead = 0;
    public int Health { get; protected set; } = 3;
    public int Money { get; protected set; } = 2;
    public int SecondsPassed { get; protected set; }


    private Matrix2D<Tile> map;

    public int Width { get => map.Width; }
    public int Height { get => map.Height; }

    public readonly int RoundDelayTime = 500;
    public GameState State { get; protected set; }
    #endregion

    #region Indexers
    public bool IsInBounds(int index) => map.IsInBounds(index);
    public bool IsInBounds(Position p) => map.IsInBounds(p);

    public bool IsInBounds(int y, int x) => map.IsInBounds(y, x);
    public bool IsInBounds((int, int) coordinates) => map.IsInBounds(coordinates);
    public int IndexFromPosition(Position p) => map.IndexFromPosition(p);
    public Position PositionFromIndex(int index) => map.PositionFromIndex(index);
    public int IndexFromCoordinates(int y, int x) => map.IndexFromCoordinates(y, x);

    public Tile this[int index]
    {
        get => map[index];
    }

    public Tile this[Position p]
    {
        get => map[p];
    }

    public Tile this[int y, int x]
    {
        get => map[y, x];
    }
    #endregion

    #region Constructor

#pragma warning disable CS8618 // Misses init, 'cause it cannot read the implementation
    public TimeBasedGame(int width, int height)
#pragma warning restore CS8618
    {
        InitializeGame(width, height);
    }
    private void InitializeGame(int width, int height)
    {
        State = GameState.Fresh;
        Health = 3;
        Money = 2;
        SecondsPassed = 0;
        map = new Matrix2D<Tile>(width, height);
        for (int i = 0; i < map.Area; i++)
        {
            map[i] = new Tile();
        }
    }
    #endregion

    #region Methods
    #region GameFlow
    public bool PlaceSoldier(Position p)
    {
        if (map[p].Variant != TileVariant.Empty || Money < 1)
        {
            return false;
        }
        map[p] = new Tile(TileVariant.Soldier);
        Money--;
        UpdateEvent.Invoke(new List<int> { map.IndexFromPosition(p) });
        return true;
    }

    private List<int> Update()
    {
        List<(int, Tile)> positions = new List<(int, Tile)>();

        for (int i = 0; i < map.Area; i++)
        {
            Position p = PositionFromIndex(i);
            if (map[i].Variant == TileVariant.Enemy)
            {
                positions.Add((i, new Tile()));
                if (p.x == 0)
                {
                    Health--;
                }
                else
                {
                    bool dies = false;
                    if (IsInBounds(p - (0, 1)) && map[p - (0, 1)].Variant == TileVariant.Soldier)
                    {
                        dies = true;
                        positions.Add((IndexFromCoordinates(p.y, p.x - 1), new Tile()));
                    }
                    if (IsInBounds(p + (0, 1)) && map[p + (0, 1)].Variant == TileVariant.Soldier)
                    {
                        dies = true;
                    }
                    if (IsInBounds(p - (1, 0)) && map[p - (1, 0)].Variant == TileVariant.Soldier)
                    {
                        dies = true;
                    }
                    if (IsInBounds(p + (1, 0)) && map[p + (1, 0)].Variant == TileVariant.Soldier)
                    {
                        dies = true;
                    }
                    if (!dies)
                    {
                        positions.Add((IndexFromCoordinates(p.y, p.x - 1), new Tile(TileVariant.Enemy)));
                    }
                    else
                    {
                        Enemydead++;
                        if (Enemydead == 3)
                        {
                            Money++;
                            Enemydead = 0;
                        }
                    }
                }
            }
        }
        foreach ((int pos, Tile tile) in positions)
        {
            map[pos] = tile;
        }
        return positions.Select(dual => dual.Item1).ToList();
    }

    private bool IsGameOver()
    {
        return Health < 1;
    }
    #endregion
    public void ResetGame()
    {
        if (State == GameState.Ongoing) return;
        InitializeGame(Width, Height);
    }

    public void PauseGame()
    {
        if (State != GameState.Ongoing) return;
        State = GameState.Paused;
    }

    public void StartGame()
    {
        if (State == GameState.Ongoing) return;
        if (State == GameState.Ended) ResetGame();
        State = GameState.Ongoing;
        Task.Run(async () =>
        {
            while (State == GameState.Ongoing)
            {
                await Task.Delay(1000);
                List<int> indexes = Update();
                UpdateEvent?.Invoke(indexes);
                SecondsPassed++;
                if (IsGameOver())
                {
                    State = GameState.Ended;
                    GameOverEvent?.Invoke(true);
                }
                else
                {
                    if (random.Next(10) > 3)
                    {
                        int h = random.Next(Height);
                        map[h, Width - 1] = new Tile(TileVariant.Enemy);
                        UpdateEvent?.Invoke(new List<int> { IndexFromCoordinates(h, Width - 1) });
                    }
                }
            }
        });
    }

    #endregion

    #region Events
    public event UpdateHandler? UpdateEvent = null;
    public event GameOverHandler? GameOverEvent = null;
    #endregion
}

public enum GameState
{
    Paused = 0,
    Ongoing = 1,
    Ended = 2,
    Fresh = 3,
}
