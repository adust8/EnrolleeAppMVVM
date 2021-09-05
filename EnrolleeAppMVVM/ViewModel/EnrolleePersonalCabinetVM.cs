using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Data.Models;
using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services;
using EnrolleeAppMVVM.Services.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class EnrolleePersonalCabinetVM : BaseViewModel, INotifyDataErrorInfo
    {
        #region Properties
        /// <summary>
        /// Коллекция для заполнения ComboBox'a
        /// </summary>
        public List<Education> Education { get; private set; }

        /// <summary>
        /// Вспомогательный ViewModel для реализации INotifyDataErrorInfo и кастномной валидации
        /// </summary>
        private readonly ErrorsViewModel _errorsViewModel;

        /// <summary>
        /// Можно ли сохранить изменения
        /// </summary>
        public bool CanSaveChanges => !HasErrors;

        private readonly Enrollee _enrollee;

        #region Selected specialities
        private ObservableCollection<Speciality> _specialities;
        /// <summary>
        /// Коллекция всех выбранных направлений
        /// </summary>
        public ObservableCollection<Speciality> Specialities
        {
            get => _specialities;
            set => _specialities = value;
        }
        #endregion

        #region Education

        private Education _selectedEducation;

        /// <summary>
        /// Полученное образование
        /// </summary>
        public Education SelectedEducation
        {
            get => _selectedEducation;
            set => Set(ref _selectedEducation, value);
        }


        public bool IsEducationEnabled => _selectedEducation == null;

        #endregion Education

        #region Fullname

        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (Set(ref _firstName, value))
                {
                    _errorsViewModel.ClearErrors(nameof(FirstName));

                    #region Digits,punctuattion chars,length validation

                    if (string.IsNullOrEmpty(FirstName))
                    {
                        _errorsViewModel.AddError(nameof(FirstName), "Заполните данное поле");
                    }
                    else if (!FirstName.All(x => char.IsLetter(x)) || FirstName.Length > 20)
                    {
                        _errorsViewModel.AddError(nameof(FirstName), "Поле заполнено некорректно");
                    }

                    #endregion Digits,punctuattion chars,length validation                   
                }
                OnPropertyChanged(nameof(CanSaveChanges));
            }
        }
        private string _secondName;

        public string SecondName
        {
            get => _secondName;
            set
            {
                if(Set(ref _secondName,value))
                {
                    #region Digits,punctuattion chars,length validation

                    if (string.IsNullOrEmpty(SecondName))
                    {
                        _errorsViewModel.AddError(nameof(SecondName), "Заполните данное поле");
                    }
                    else if (!SecondName.All(x => char.IsLetter(x)) || SecondName.Length > 20)
                    {
                        _errorsViewModel.AddError(nameof(SecondName), "Поле заполнено некорректно");
                    }
                    #endregion Digits,punctuattion chars,length validation
                }
                OnPropertyChanged(nameof(CanSaveChanges));
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
                }
                OnPropertyChanged(nameof(CanSaveChanges));
            }
        }




        #endregion Fullname

        #region BirthDate

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate => _enrollee.BirthDate;

        #endregion BirthDate

        #region Email
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
                    else if (!regex.IsMatch(_email))
                    {
                        _errorsViewModel.AddError(nameof(Email), "Укажите верный формат электронной почты");
                    }
                }
                OnPropertyChanged(nameof(CanSaveChanges));
            }
        }

        #endregion

        #region Address

        private string _address;

        /// <summary>
        /// Адрес абитуриента
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                if (Set(ref _address, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Address));
                    if (string.IsNullOrEmpty(Address))
                    {
                        _errorsViewModel.AddError(nameof(Address), "Заполните поле");
                    }
                    OnPropertyChanged(nameof(CanSaveChanges));
                }
            }
        }

        #endregion Address

        #region GPA

        private string _gpa;

        /// <summary>
        /// Средний балл аттестата
        /// </summary>
        public string GPA
        {
            get => _gpa;
            set
            {
                if (Set(ref _gpa, value))
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                    _errorsViewModel.ClearErrors(nameof(GPA));

                    if (!double.TryParse(GPA, out double gpa) || double.Parse(GPA) > 5.0 || double.Parse(GPA) < 0 || GPA.Contains('.'))
                    {
                        _errorsViewModel.AddError(nameof(GPA), "Некорректно заполнено поле");
                    }

                    OnPropertyChanged(nameof(CanSaveChanges));
                }
            }
        }

        #endregion GPA

        #region Passport

        private string _passport;

        /// <summary>
        /// Номер и серия паспорта
        /// </summary>
        public string Passport
        {
            get => _passport;
            set
            {
                if (Set(ref _passport, value))
                {
                    _errorsViewModel.ClearErrors(nameof(Passport));
                    if (Passport.All(x => char.IsDigit(x)) || Passport.Length != 11 || Passport.ToCharArray()[4] != ' ')
                    {
                        _errorsViewModel.AddError(nameof(Passport), "Некорректно заполнено поле");
                    }
                    else if (string.IsNullOrEmpty(Passport))
                    {
                        _errorsViewModel.AddError(nameof(Passport), "Незаполненное поле");
                    }
                    OnPropertyChanged(nameof(CanSaveChanges));
                }
            }
        }

        #endregion Passport

        #region Certificate number

        private string _certificateNumber;

        /// <summary>
        /// Средний балл аттестата
        /// </summary>
        public string CertificateNumber
        {
            get => _certificateNumber;
            set
            {
                if (Set(ref _certificateNumber, value))
                {
                    _errorsViewModel.ClearErrors(nameof(CertificateNumber));
                    if (!CertificateNumber.All(x => char.IsDigit(x)))
                    {
                        _errorsViewModel.AddError(nameof(CertificateNumber), "Некорректно заполнено поле");
                    }
                    else if (string.IsNullOrEmpty(CertificateNumber))
                    {
                        _errorsViewModel.AddError(nameof(CertificateNumber), "Незаполненное поле");
                    }
                    OnPropertyChanged(nameof(CanSaveChanges));
                }
            }
        }

        #endregion Certificate number

        #endregion Properties

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
            OnPropertyChanged(nameof(CanSaveChanges));
        }

        #endregion INotifyDataErrorInfo

        #region Commands

        #region Save changes command
        public ICommand SaveChangesCommand { get; set; }

        public void OnSaveChangesCommandExecute(object param)
        {
            using (AppDbContext db = new())
            {
                Enrollee enrolle = db.Enrollees.FirstOrDefault(x => x.UserGUID == AccountManager.CurrentUser.UserGUID);
                enrolle.FirstName = FirstName;
                enrolle.SecondName = SecondName;
                enrolle.Patronymic = this.Patronymic;
                enrolle.Email = Email;
                enrolle.Address = Address;
                enrolle.GPA = double.Parse(GPA);
                
                enrolle.PassportId = Passport;
                enrolle.CertificateNumber = CertificateNumber;
                enrolle.EducationId = SelectedEducation?.EducationId;
                db.SaveChanges();
            }

            MessageBox.Show("Данные успешно изменены");
        }

        public bool CanSaveChangesExecuted(object param) => true;

        #endregion

        #region Exit command
        public ICommand ExitCommand { get; set; }

        private void OnExitCommandExecute(object param)
        {
            WindowService.OpenNewWindow(new LoginUserControlViewModel());
            AccountManager.ClearCurrentGuids();
        }

        private bool CanExitCommandExecuted(object param) => true;

        #endregion

        #endregion Commands

        #region Constructor

        public EnrolleePersonalCabinetVM()
        {
            SaveChangesCommand = new DelegateCommand(OnSaveChangesCommandExecute, CanSaveChangesExecuted);
            ExitCommand = new DelegateCommand(OnExitCommandExecute, CanExitCommandExecuted);
            _errorsViewModel = new ErrorsViewModel();

            using (AppDbContext db = new())
            {
                Education = db.Educations.ToList();
                _enrollee = db.Enrollees.FirstOrDefault(x => x.EnrolleeGUID == AccountManager.CurrentEnrollee.EnrolleeGUID);
                _specialities = new ObservableCollection<Speciality>(db.Enrollees_Specialities.Where(x => x.EnrolleeGUID == AccountManager.CurrentEnrollee.EnrolleeGUID).Include(x => x.Speciality).ThenInclude(x => x.SpecialityName).Include(x => x.Speciality).ThenInclude(x => x.Financing).Select(x => x.Speciality).ToList());
            }
            _firstName = _enrollee.FirstName;
            _secondName = _enrollee.SecondName;
            _patronymic = _enrollee.Patronymic;
            _address = _enrollee.Address;
            _email = _enrollee.Email;
            _gpa = _enrollee.GPA.ToString();
            _certificateNumber = _enrollee.CertificateNumber;
            _passport = _enrollee.PassportId;
            _selectedEducation = _enrollee.Education;
        }

        #endregion Constructor
    }
}