using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileImport
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T propertyFiled, T newValue, [CallerMemberName] string propertyName = null)
        {
            bool notEquals = !Equals(propertyFiled, newValue);
            if (notEquals)
            {
                T oldValue = propertyFiled;
                propertyFiled = newValue;
                RaisePropertyChanged(propertyName);

                OnPropertyChanged(propertyName, oldValue, newValue);
            }
            return notEquals;
        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) { }
    }
}
