using System;

namespace EnrolleeAppMVVM.Services.Commands
{
    internal class DelegateCommand : BaseCommand
    {
        public Action<object> _Execute { get; set; }
        public Func<object, bool> _CanExecute { get; set; }

        public DelegateCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentException(nameof(Execute));
            _CanExecute = CanExecute;
        }

        public override void Execute(object parameter) => _Execute(parameter);
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
    }
}
