using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using MauiImplInvazio.Utility;

using Model;

namespace WPFImplInvazio.ViewModel;

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

    public SolidColorBrush Color
    {
        get
        {
            switch (tile.Variant)
            {
                case TileVariant.Enemy:
                    return new SolidColorBrush(Colors.IndianRed);
                case TileVariant.Soldier:
                    return new SolidColorBrush(Colors.LawnGreen);
                default:
                    return new SolidColorBrush(Colors.White);
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
