using EnrolleeAppMVVM.Services.Commands;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    class CounterVM : BaseViewModel
    {

        public ICommand IncrementCommand { get; set; }
        public ICommand DecrementCommand { get; set; }

        public bool CanIncrementCommandExecuted(object param) => true;

        public bool CanDecrementCommandExecuted(object param) => true;

        public void OnIncrementCommandExecuted(object param)
        {
            CurrentNumber++;
            OnPropertyChanged(nameof(CurrentNumber));
            //OnPropertyChanged(nameof(ResultString));
        }

        public void OnDecrementCommandExecuted(object param)
        {
            CurrentNumber--;
            OnPropertyChanged(nameof(CurrentNumber));
            //OnPropertyChanged(nameof(ResultString));
        }

        private int _currentNumber;

        public int CurrentNumber
        {
            get => _currentNumber;
            set
            {
                if (value > 0 && value <= 11)
                {
                    Set(ref _currentNumber, value);
                    OnPropertyChanged(nameof(ResultString));
                }

            }
        }

        public string ResultString => CurrentNumber.ToString();

        public CounterVM()
        {
            _currentNumber = 1;
            IncrementCommand = new DelegateCommand(OnIncrementCommandExecuted, CanIncrementCommandExecuted);
            DecrementCommand = new DelegateCommand(OnDecrementCommandExecuted, CanDecrementCommandExecuted);
        }

    }
}
