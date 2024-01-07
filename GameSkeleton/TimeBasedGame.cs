namespace GameSkeleton;

using Matrix;

public delegate void UpdateHandler(List<int> indexes);
public delegate void GameOverHandler(bool win);


public class TimeBasedGame
{
    #region Properties
    private Matrix2D<Tile> map;

    public int Width { get => map.Width; }
    public int Height { get => map.Height; }

    public readonly int RoundDelayTime = 500;
    public GameState State { get; protected set; }
    #endregion

    #region Indexers
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
        map = new Matrix2D<Tile>(width, height);
        for (int i = 0; i < map.Area; i++)
        {
            map[i] = new Tile(); //TODO
        }
    }
    #endregion

    #region Methods
    #region GameFlow
    private List<int> Update()
    {
        List<(int, Tile)> positions = new List<(int, Tile)>();
        //TODO - No Game logic oc
        foreach ((int pos, Tile tile) in positions)
        {
            map[pos] = tile;
        }
        return positions.Select(dual => dual.Item1).ToList();
    }

    private bool IsGameOver()
    {
        //TODO - No end condition
        return false;
    }
    #endregion
    public void ResetGame()
    {
        if (State == GameState.Ongoing) return;
        InitializeGame(Width,Height);
    }

    public void PauseGame()
    {
        if (State != GameState.Ongoing) return;
        State = GameState.Paused;
    }

    public void StartGame()
    {
        if (State == GameState.Ongoing || State == GameState.Ended) return;
        State = GameState.Ongoing;
        Task.Run(() =>
        {
            while (State == GameState.Ongoing)
            {
                List<int> indexes = Update();
                //TODO
                UpdateEvent?.Invoke(indexes);
                if (IsGameOver())
                {
                    GameOverEvent?.Invoke(true);
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
