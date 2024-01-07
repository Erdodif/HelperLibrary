using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MauiImplInvazio.Utility;

using Model;

namespace MauiImplInvazio.ViewModel;

public class TileViewModel : PropertyNotifier
{
    private Tile tile;
    private Position position;

    public int X
    {
        get => position.x;
    }

    public int Y
    {
        get => position.y;
    }

    public string Text
    {
        get
        {
            switch (tile.Variant)
            {
                case TileVariant.Enemy:
                    return "E";
                case TileVariant.Soldier:
                    return "S";
                default:
                    return "";
            }
        }
    }

    public Color Color
    {
        get
        {
            switch (tile.Variant)
            {
                case TileVariant.Enemy:
                    return Colors.IndianRed;
                case TileVariant.Soldier:
                    return Colors.LawnGreen;
                default:
                    return Colors.White;
            }
        }
    }

    public Tile Tile
    {
        get => tile; set
        {
            if (tile == null || tile.Variant != value.Variant)
            {
                this.tile = value;
            }
            NotifyPropertiesChanged(nameof(Tile), nameof(Text), nameof(Color),nameof(X),nameof(Y));
        }
    }

    public ICommand HandleClick { get; protected set; }

    public TileViewModel(Position p, Tile tile, Action handleClick)
    {
        position = p;
        Tile = tile;
        HandleClick = new TileClickHandler(handleClick);
        //TODO
    }
}

public class TileClickHandler : ICommand
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

    public TileClickHandler(Action action)
    {
        this.action = action;
    }
}
