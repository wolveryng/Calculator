using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _KeyPressedString;
        public string KeyPressedString
        {
            get { return _KeyPressedString; }
            set
            {
                _KeyPressedString = value;
                OnPropertyChanged("KeyPressedString");
            }
        }

        private string _Entered_Number;
        public string Entered_Number
        {
            get { return _Entered_Number; }
            set
            {
                _Entered_Number = value;
                OnPropertyChanged("Entered_Number");
            }
        }
    }
}
