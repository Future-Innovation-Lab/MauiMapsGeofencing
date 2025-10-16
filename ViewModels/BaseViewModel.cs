using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiGeoJsonMapping.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private INavigation? _navigation;

        public INavigation? Navigation { get { return _navigation; } }

        public BaseViewModel(INavigation? navigation)
        {
            _navigation = navigation;
        }

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnAppearing()
        {
        }
    }
}