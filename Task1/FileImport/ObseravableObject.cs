using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileImport
{
    /// <summary>
    /// Класс реаилзующий интерфейс INotifyPropertyChanged необходимый для отслеживания изменения 
    /// в полях класса с последующим оповещением об этом отображения
    /// </summary>
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
