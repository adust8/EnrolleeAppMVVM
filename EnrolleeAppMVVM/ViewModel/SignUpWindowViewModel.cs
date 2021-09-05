using EFDataAccessLibrary.DataAccess;
using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services;
using EnrolleeAppMVVM.Services.Commands;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class SignUpWindowViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Properties

        private readonly ErrorsViewModel _errorsViewModel;

        public static Guid GUID => Guid.NewGuid();

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (Set(ref _name, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Name));

                    #region Digits,punctuattion chars,length validation

                    if (string.IsNullOrEmpty(Name))
                    {
                        _errorsViewModel.AddError(nameof(Name), "Заполните данное поле");
                    }
                    else if (!Name.All(x => char.IsLetter(x)) || Name.Length > 20)
                    {
                        _errorsViewModel.AddError(nameof(Name), "Поле заполнено некорректно");
                    }

                    #endregion Digits,punctuattion chars,length validation                   
                }
                OnPropertyChanged(nameof(CanSignUp));
            }
        }

        private string _surname;

        public string Surname
        {
            get => _surname;
            set
            {
                if (Set(ref _surname, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Surname));

                    #region Digits,punctuattion chars,length validation

                    if (string.IsNullOrEmpty(Surname))
                    {
                        _errorsViewModel.AddError(nameof(Surname), "Заполните данное поле");
                    }
                    else if (!Surname.All(x => char.IsLetter(x)) || Surname.Length > 20)
                    {
                        _errorsViewModel.AddError(nameof(Surname), "Поле заполнено некорректно");
                    }
                    #endregion Digits,punctuattion chars,length validation

                    OnPropertyChanged(nameof(CanSignUp));
                }
            }
        }

        private string _patronymic;

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                if (Set(ref _patronymic, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Patronymic));

                    #region Digits,punctuattion chars,length validation

                    if (string.IsNullOrEmpty(Patronymic))
                    {
                        _errorsViewModel.AddError(nameof(Patronymic), "Заполните данное поле");
                    }
                    else if (!Patronymic.All(x => char.IsLetter(x)) || Patronymic.Length > 20)
                    {
                        _errorsViewModel.AddError(nameof(Patronymic), "Поле заполнено некорректно");
                    }

                    #endregion Digits,punctuattion chars,length validation

                    OnPropertyChanged(nameof(CanSignUp));
                }
            }
        }

        private DateTime _birthDate;

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (Set(ref _birthDate, value))
                {
                    _errorsViewModel.ClearErrors(nameof(BirthDate));
                    if (string.IsNullOrEmpty(BirthDate.ToString()))
                    {
                        _errorsViewModel.AddError(nameof(BirthDate), "Заполните данное поле");
                    }
                }
                OnPropertyChanged(nameof(CanSignUp));
            }
        }

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                if (Set(ref _email, value))
                {
                    string pattern = @"^[a-zA-Z0-9]+\@[a-zA-Z0-9]+\.[a-z]+$";
                    Regex regex = new Regex(pattern);

                    _errorsViewModel.ClearErrors(nameof(Email));
                    if (string.IsNullOrEmpty(Email))
                    {
                        _errorsViewModel.AddError(nameof(Email), "Заполните данное поле");
                    }
                    else if(!regex.IsMatch(Email))
                    {
                        _errorsViewModel.AddError(nameof(Email), "Укажите верный формат электронной почты");
                    }
                }
                OnPropertyChanged(nameof(CanSignUp));
            }
        }


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
                    else if (ErrorsViewModel.IsLoginAlreadyExist(Login))
                    {
                        _errorsViewModel.AddError(nameof(Login), "Такой логин уже существует");
                    }
                    else if (Login.Length < 5)
                    {
                        _errorsViewModel.AddError(nameof(Login), "Логин слишком короткий");
                    }
                    else if (Login.Length > 30)
                    {
                        _errorsViewModel.AddError(nameof(Login), "Логин слишком длинный");
                    }
                }
                OnPropertyChanged(nameof(CanSignUp));
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
                    else if (Password.Length < 8)
                    {
                        _errorsViewModel.AddError(nameof(Password), "Слишком короткий пароль");
                    }
                    else if (ErrorsViewModel.MostPopularPasswordsList.Contains(Password) || !ErrorsViewModel.IsValidPassword(Password))
                    {
                        _errorsViewModel.AddError(nameof(Password), "Слабый пароль");
                    }
                    else if (Login == Password)
                    {
                        _errorsViewModel.AddError(nameof(Password), "Логин и пароль совпадают");
                    }
                }
                OnPropertyChanged(nameof(CanSignUp));
            }
        }

        private string _repeatPassword;

        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (Set(ref _repeatPassword, value))
                {
                    _errorsViewModel.ClearErrors(nameof(RepeatPassword));
                    if (string.IsNullOrEmpty(RepeatPassword))
                    {
                        _errorsViewModel.AddError(nameof(RepeatPassword), "Заполните данное поле");
                    }
                    else if (Password != RepeatPassword)
                    {
                        _errorsViewModel.AddError(nameof(RepeatPassword), "Пароли не совпадают");
                    }
                }
                OnPropertyChanged(nameof(CanSignUp));
            }
        }

        public bool AreFieldsFilled() => !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Surname)
            && !string.IsNullOrWhiteSpace(Patronymic)
            && !string.IsNullOrWhiteSpace(BirthDate.ToString())
            && !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Login)
            && !string.IsNullOrWhiteSpace(Password)
            && !string.IsNullOrWhiteSpace(RepeatPassword);

        public bool CanSignUp => !HasErrors && AreFieldsFilled();

        #endregion Properties

        #region Commands

        public ICommand SignUpCommand { get; }
        public ICommand ExitCommand { get; }

        public bool CanSignUpCommandExecuted(object param) => true;

        public bool CanExitCommandExecuted(object param) => true;

        #region Sign up command

        public void OnSignUpCommandExecuted(object param)
        {
            if (CanSignUp && AreFieldsFilled())
            {
                Guid guidForUser = GUID;
                Guid guidForEnrollee = GUID;
                AccountManager.SignUpAccount(guidForUser, guidForEnrollee, Login, Password, Name, Surname, Patronymic, BirthDate,Email);
                MessageBox.Show("Вы успешно зарегистрировались");
                WindowService.OpenNewWindow(new LoginUserControlViewModel());
            }
            else
            {
                MessageBox.Show("Заполните все поля и попробуйте снова");
            }
        }

        #endregion Sign up command

        #region Exit command

        public void OnExitCommandExecuted(object param)
        {
            WindowService.OpenNewWindow(new LoginUserControlViewModel());
        }

        #endregion Exit command

        #endregion Commands

        #region Constructor

        public SignUpWindowViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _birthDate = DateTime.Today;


            SignUpCommand = new DelegateCommand(OnSignUpCommandExecuted, CanSignUpCommandExecuted);
            ExitCommand = new DelegateCommand(OnExitCommandExecuted, CanExitCommandExecuted);
        }

        #endregion Constructor

        #region INotifyDataErrorInfo


        public bool HasErrors => _errorsViewModel.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanSignUp));
        }

        #endregion INotifyDataErrorInfo
    }
}