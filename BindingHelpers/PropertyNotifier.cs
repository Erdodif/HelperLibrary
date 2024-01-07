using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BindingHelpers;

public class PropertyNotifier : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void NotifyPropertiesChanged(params string[] properties)
    {
        if (PropertyChanged == null) return; // There's nothing we can do...
        foreach (string property in properties)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

}
