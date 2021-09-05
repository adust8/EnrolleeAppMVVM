using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class ChangePasswordVM : BaseViewModel,INotifyDataErrorInfo
    {

        #region Properties

        public Brush ColorBrush { get; set; }

        private ErrorsViewModel _errorsViewModel { get; set; }
        public string ResultString { get; set; }

        #region Current password
        private string _currentPassword;

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            { 
                if(Set(ref _currentPassword, value))
                {
                    _errorsViewModel.ClearErrors(nameof(CurrentPassword));
                    if(string.IsNullOrWhiteSpace(CurrentPassword))
                    {
                        _errorsViewModel.AddError(nameof(CurrentPassword), "Заполните пустое поле");
                    }
                    else if (AccountManager.GetHashCode(CurrentPassword) != AccountManager.CurrentUser.Password)
                    {
                        _errorsViewModel.AddError(nameof(CurrentPassword), "Вы указали неверный пароль");
                    }
                    OnPropertyChanged(nameof(CanChangePassword));
                    _errorsViewModel.AddError(nameof(NewPassword), "Error for not fill check");
                }
            }
        }
        #endregion

        #region New password
        private string _newPassword;

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if(Set(ref _newPassword,value))
                {
                    _errorsViewModel.ClearErrors(nameof(NewPassword));
                    if(string.IsNullOrEmpty(_newPassword))
                    {
                        _errorsViewModel.AddError(nameof(NewPassword),"Заполните пустое поле");
                    }
                    else if(NewPassword.Length < 8)
                    {
                        _errorsViewModel.AddError(nameof(NewPassword), "Слишком короткий пароль");
                    }
                    else if(!ErrorsViewModel.IsValidPassword(NewPassword))
                    {
                        _errorsViewModel.AddError(nameof(NewPassword), "Слабый пароль");
                    }
                    else if (AccountManager.GetHashCode(NewPassword) == AccountManager.CurrentUser.Password)
                    {
                        _errorsViewModel.AddError(nameof(NewPassword), "Ваш старый и новый пароли совпадают");
                    }
                    OnPropertyChanged(nameof(CanChangePassword));
                    _errorsViewModel.AddError(nameof(RepeatNewPassword),"Error for not fill check");
                }
            }
        }

        #endregion

        #region Repeat new password
        private string _repeatNewPassword;

        public string RepeatNewPassword
        {
            get => _repeatNewPassword;
            set
            {
                if (Set(ref _repeatNewPassword, value))
                {
                    _errorsViewModel.ClearErrors(nameof(RepeatNewPassword));
                    if (string.IsNullOrEmpty(RepeatNewPassword))
                    {
                        _errorsViewModel.AddError(nameof(RepeatNewPassword), "Заполните пустое поле");
                    }
                    else if (NewPassword != RepeatNewPassword)
                    {
                        _errorsViewModel.AddError(nameof(RepeatNewPassword), "Пароли не совпадают");
                    }

                    OnPropertyChanged(nameof(CanChangePassword));
                }
            }
        }
        #endregion

        public bool IsChangePasswordLimit
        {
            get => ErrorsViewModel.ChangingPasswordLimit();
        }

        public bool CanChangePassword => !HasErrors;

        #endregion


        #region Commands

        #region Change password command

        public ICommand ChangePasswordCommand { get; set; }

        private void OnChangePasswordCommandExecute(object param)
        {
            using (AppDbContext db = new())
            {
                if (!IsChangePasswordLimit)
                {
                    User user = db.Users.Where(x => x.UserGUID == AccountManager.CurrentUser.UserGUID).FirstOrDefault();
                    user.Password = AccountManager.GetHashCode(NewPassword);
                    user.LastPasswordChangeDate = DateTime.Now;
                    db.SaveChanges();
                    ResultString = "Пароль успешно изменён";
                    ColorBrush = Brushes.Green;
                    OnPropertyChanged(nameof(ResultString));
                    OnPropertyChanged(nameof(ColorBrush));
                    OnPropertyChanged(nameof(IsChangePasswordLimit));
                }
                else
                {
                    ColorBrush = Brushes.DarkRed;
                    ResultString = "Вы не можете менять пароль чаще 1 раза в день";
                    OnPropertyChanged(nameof(ResultString));
                    OnPropertyChanged(nameof(ColorBrush));
                }
                

            }
        }

        private bool CanChangePasswordCommandExecuted(object param) => true;

        #endregion

        #endregion

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
            OnPropertyChanged(nameof(CanChangePassword));
        }
        #endregion

        #region Constructor
        public ChangePasswordVM()
        {
            ChangePasswordCommand = new DelegateCommand(OnChangePasswordCommandExecute,CanChangePasswordCommandExecuted);
            _errorsViewModel = new ErrorsViewModel();

            _errorsViewModel.AddError(nameof(CurrentPassword), "Заполните пустое поле");
        }
        #endregion


    }
}
