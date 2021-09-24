using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AvaRaspberry.Models.Torrent
{
    public class BaseResponce : INotifyPropertyChanged
    {

        public bool Result { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected void RaiseAndSetIfChanged<TObj>(ref TObj backingField,
            TObj newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (backingField is not null && !backingField.Equals(newValue))
            {
                backingField = newValue;
                OnPropertyChanged(propertyName);
            }
        }
    }
}