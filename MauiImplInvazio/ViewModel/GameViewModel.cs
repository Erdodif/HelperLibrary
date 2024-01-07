
using System.Collections.ObjectModel;
using System.Windows.Input;

using MauiImplInvazio.Utility;

using Model;

namespace MauiImplInvazio.ViewModel;

public class GameViewModel : PropertyNotifier
{

    private TimeBasedGame game;

    public string Money
    {
        get => game.Money.ToString();
    }

    public string TimePassed
    {
        get => game.SecondsPassed.ToString();
    }

    public string Health
    {
        get => game.Health.ToString();
    }

    public RowDefinitionCollection RowDefs { get; private set; }
    public ColumnDefinitionCollection ColumnDefs { get; private set; }

    public ObservableCollection<TileViewModel> Map { get; private set; }

    public StartCommand StartCommand { get; private set; }
    public PauseCommand PauseCommand { get; private set; }

    public GameViewModel()
    {
        game = new TimeBasedGame(12, 6);
        RowDefinition[] rDefs = new RowDefinition[game.Height];
        ColumnDefinition[] cDefs = new ColumnDefinition[game.Width];
        for (int i = 0; i < rDefs.Length; i++)
        {
            rDefs[i] = new RowDefinition(GridLength.Star);
        }
        for (int i = 0; i < cDefs.Length; i++)
        {
            cDefs[i] = new ColumnDefinition(GridLength.Star);
        }
        RowDefs = new RowDefinitionCollection(rDefs);
        ColumnDefs = new ColumnDefinitionCollection(cDefs);
        NotifyPropertiesChanged(
            nameof(RowDefs),
            nameof(ColumnDefs));
        Map = new ObservableCollection<TileViewModel>();
        for (int i = 0; i < game.Width * game.Height; i++)
        {
            Position p = game.PositionFromIndex(i);
            Map.Add(new TileViewModel(p, game[i], () =>
            {
                game.PlaceSoldier(p);
            }));
        }
        NotifyPropertiesChanged(
            nameof(Health),
            nameof(Money),
            nameof(TimePassed),
            nameof(Map)
            );
        game.UpdateEvent += positions =>
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                foreach (int pos in positions)
                {
                    Map[pos].Tile = game[pos];
                }
            });
            NotifyPropertiesChanged(
                nameof(Health),
                nameof(Money),
                nameof(TimePassed)
                );
        };
        game.GameOverEvent += gameOver =>
        {
            Application.Current.Dispatcher.Dispatch(() =>
            Application.Current.MainPage.DisplayAlert("Game over", $"Time passed: {game.SecondsPassed}", "okay"));
        };
        StartCommand = new StartCommand(() => {
            if(game.State == GameState.Ended)
            {
                foreach(TileViewModel tvm in Map)
                {
                    tvm.Tile = new Tile();
                }
            }
            game.StartGame();
        });
        PauseCommand = new PauseCommand(() => game.PauseGame());

    }
}

public class StartCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    private Action action;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        action();
    }

    public StartCommand(Action action)
    {
        this.action = action;
    }
}


public class PauseCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    private Action action;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        action();
    }

    public PauseCommand(Action action)
    {
        this.action = action;
    }
}

