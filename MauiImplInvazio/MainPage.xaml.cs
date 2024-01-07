using MauiImplInvazio.ViewModel;

namespace MauiImplInvazio
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Game.BindingContext = new GameViewModel();
            Application.Current.MainPage.Window.Deactivated += (_, _) => (Game.BindingContext as GameViewModel).PauseCommand.Execute(null);
        }
    }

}
