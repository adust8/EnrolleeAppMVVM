using EnrolleeAppMVVM.Services.Commands;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class SettingsVM : BaseViewModel
    {
        public BaseViewModel CurrentViewModel { get; private set; }

        #region Commands

        #region Change password command
        public ICommand ChangePasswordCommand { get; set; }

        public void OnChangePasswordCommandExecute(object param)
        {
            CurrentViewModel = new ChangePasswordVM();
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private bool CanChangePasswordCommandExecuted(object param) => true;
        #endregion

        #region Delete account command
        public ICommand DeleteAccountCommand { get; set; }

        public void OnDeleteAccountCommandExecute(object param)
        {
            CurrentViewModel = new DeleteAccountVM();
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private bool CanDeleteAccountCommandExecuted(object param) => true;
        #endregion

        #endregion

        #region Constructor
        public SettingsVM()
        {
            DeleteAccountCommand = new DelegateCommand(OnDeleteAccountCommandExecute, CanDeleteAccountCommandExecuted);
            ChangePasswordCommand = new DelegateCommand(OnChangePasswordCommandExecute, CanChangePasswordCommandExecuted);
        }
        #endregion

    }
}
