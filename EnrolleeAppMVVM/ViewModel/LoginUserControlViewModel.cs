using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services;
using EnrolleeAppMVVM.Services.Commands;
using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using System.Windows;
using EnrolleeAppMVVM.View;
using EFDataAccessLibrary.DataAccess;
using System.Linq;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class LoginUserControlViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Properties
        private readonly ErrorsViewModel _errorsViewModel;

        public bool CanSignIn => !HasErrors && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                if (Set(ref _login, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Login));
                    if (string.IsNullOrEmpty(Login))
                    {
                        _errorsViewModel.AddError(nameof(Login), "Заполните данное поле");
                    }
                }
                OnPropertyChanged(nameof(CanSignIn));
            }
        }

        private string _password;

        public string Password
        {
            get => _password;

            set
            {
                if (Set(ref _password, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Password));
                    if (string.IsNullOrEmpty(Password))
                    {
                        _errorsViewModel.AddError(nameof(Password), "Заполните данное поле");
                    }
                }
                OnPropertyChanged(nameof(CanSignIn));
            }
        }

        #region RadioButtons

        public enum Roles
        {
            Enrollee = 1,
            Administrator = 2
        }

        private Roles _role = Roles.Enrollee;

        public Roles Role
        {
            get => _role;
            set
            {
                Set(ref _role, value);
                OnPropertyChanged(nameof(IsEnrolleeRole));
                OnPropertyChanged(nameof(IsAdminRole));
            }
        }

        public bool IsEnrolleeRole
        {
            get => Role == Roles.Enrollee;
            set => Role = value ? Roles.Enrollee : Role;
        }

        public bool IsAdminRole
        {
            get => Role == Roles.Administrator;
            set => Role = value ? Roles.Administrator : Role;
        }

        public int GetResultRoleId
        {
            get
            {
                if (IsEnrolleeRole)
                    return 1;
                else if (IsAdminRole)
                    return 2;

                return 0;
            }
        }

        #endregion RadioButtons

        private string _wrongLoginOrPasswordString;

        /// <summary>
        /// String that appeared if login or password is wrong
        /// </summary>
        public string WrongLoginOrPasswordString
        {
            get => _wrongLoginOrPasswordString;
            set => Set(ref _wrongLoginOrPasswordString, value);
        }

        #endregion Properties

        #region Commands

        #region SignInCommand

        public ICommand SignInCommand { get; }

        public void OnSignInCommandExecuted(object param)
        {
            using (AppDbContext db = new())
            {
                if (db.Users.Any(p => p.Login == Login && p.Password == AccountManager.GetHashCode(Password) && p.RoleId == GetResultRoleId))
                {
                    AccountManager.CurrentUser = db.Users.Where(p => p.Login == Login && p.Password == AccountManager.GetHashCode(Password)).FirstOrDefault();
                    AccountManager.CurrentEnrollee = db.Enrollees.Where(p => p.User == AccountManager.CurrentUser).FirstOrDefault();
                    WindowService.OpenNewWindow(new MainWindowViewModel());
                }
                else
                {
                    WrongLoginOrPasswordString = "Неверный логин или пароль";
                    OnPropertyChanged(nameof(WrongLoginOrPasswordString));
                }

            }

        }
        public bool CanSignInCommandExecuted(object param) => true;

        #endregion SignInCommand

        #region SignUpCommand

        public ICommand SignUpCommand { get; }

        public void OnSignUpCommand(object param)
        {
            WindowService.OpenNewWindow(new SignUpWindowViewModel());
        }

        public bool CanSignUpCommand(object param) => true;

        #endregion SignUpCommand

        #endregion Commands

        #region INotifyDataErrorInfo

        public bool HasErrors => _errorsViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanSignIn));
        }

        #endregion INotifyDataErrorInfo

        #region Constructor

        public LoginUserControlViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            SignInCommand = new DelegateCommand(OnSignInCommandExecuted, CanSignInCommandExecuted);
            SignUpCommand = new DelegateCommand(OnSignUpCommand, CanSignUpCommand);
        }

        #endregion Constructor
    }
}