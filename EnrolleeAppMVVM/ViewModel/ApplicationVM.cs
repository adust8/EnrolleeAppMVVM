using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Data.Models;
using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class ApplicationVM : BaseViewModel, INotifyDataErrorInfo
    {
        #region Properties

        /// <summary>
        /// Available specialities
        /// </summary>
        public ObservableCollection<Speciality> AllSpecialities { get; set; }

        public ObservableCollection<Financing> AllFinancings { get; set; }

        private readonly ErrorsViewModel _errorsViewModel;

        private readonly EnrolleePersonalCabinetVM _enrolleePersonalCabinetVM;

        private Speciality _selectedSpeciality;

        /// <summary>
        /// Selected speciality(from combobox)
        /// </summary>
        public Speciality SelectedSpeciality
        {
            get => _selectedSpeciality;
            set
            {
                if (Set(ref _selectedSpeciality, value))
                {
                    _errorsViewModel.ClearErrors(nameof(SelectedSpeciality));

                    using (AppDbContext db = new())
                    {
                        if (ErrorsViewModel.IsApplicationAlreadyExists(AccountManager.CurrentEnrollee.EnrolleeGUID, SelectedSpeciality))
                        {
                            _errorsViewModel.AddError(nameof(SelectedSpeciality), "Вы уже подавали заявление на данное направление");
                        }
                        else if (!ErrorsViewModel.IsMatchToMinimalEducationLevel(AccountManager.CurrentEnrollee.EnrolleeGUID, SelectedSpeciality))
                        {
                            _errorsViewModel.AddError(nameof(SelectedSpeciality), "Ваше образование не соответствует минимальному образованию на данное направление");
                        }
                    }

                    OnPropertyChanged(nameof(IsApplyApplicationEnabled));
                    OnPropertyChanged(nameof(IsWithdrawEnabled));
                }
            }
        }
        private Financing _selectedFinancing;

        public Financing SelectedFinancing
        {
            get => _selectedFinancing;
            set
            {
                //SelectedSpeciality = null;
                Set(ref _selectedFinancing, value);
                using (AppDbContext db = new())
                {
                    AllSpecialities = new ObservableCollection<Speciality>(db.Specialities.Include(x => x.Financing)
                    .Include(x => x.StudyPeriod)
                    .Include(x => x.SpecialityName)
                    .Where(x => x.Financing == SelectedFinancing)
                    .ToList());
                }
                OnPropertyChanged(nameof(AllSpecialities));
            }
        }

        public int Priority
        {
            get
            {
                if (_enrolleePersonalCabinetVM.Specialities.Count == 0)
                    return 1;

                else
                {
                    using (AppDbContext db = new())
                    {
                        return ++db.Enrollees_Specialities.Where(x => x.EnrolleeGUID == AccountManager.CurrentEnrollee.EnrolleeGUID).OrderBy(x => x.Id).LastOrDefault().Priority;
                    }
                }
            }
        }

        public bool IsApplyApplicationEnabled => SelectedSpeciality != null && !HasErrors;

        public bool IsWithdrawEnabled
        {
            get
            {
                using (AppDbContext db = new())
                {
                    return db.Enrollees_Specialities.Where(x => x.EnrolleeGUID == AccountManager.CurrentEnrollee.EnrolleeGUID && x.Speciality == SelectedSpeciality).Any() && SelectedSpeciality != null;
                }
            }
        }

        #endregion Properties

        #region Commands

        #region Apply
        public ICommand ApplyApplicationCommand { get; set; }

        public void OnApplyApplicationCommandExecute(object param)
        {
            #region Adding a new application
            if (IsApplyApplicationEnabled && !ErrorsViewModel.IsApplicationAlreadyExists(AccountManager.CurrentEnrollee.EnrolleeGUID, SelectedSpeciality))
            {
                using (AppDbContext db = new())
                {
                    db.Enrollees_Specialities.Add(new Enrollee_Specialities() { EnrolleeGUID = AccountManager.CurrentEnrollee.EnrolleeGUID, SpecialityId = SelectedSpeciality.SpecialityId, Priority = this.Priority });
                    db.SaveChanges();
                    MessageBox.Show($"Заявление на направление \"{SelectedSpeciality.SpecialityName.Name}\" успешно подано");
                }
            }

            OnPropertyChanged(nameof(IsApplyApplicationEnabled));
            #endregion
        }

        private bool CanApplyApplicationCommandExecuted(object param) => true;
        #endregion

        #region Withdraw
        public ICommand WithdrawApplicationCommand { get; set; }

        public void OnWithdrawApplicationCommandExecute(object param)
        {
            using (AppDbContext db = new())
            {

                db.Enrollees_Specialities.Remove(db.Enrollees_Specialities.Where(x => x.Speciality == SelectedSpeciality && x.EnrolleeGUID == AccountManager.CurrentEnrollee.EnrolleeGUID).FirstOrDefault());
                db.SaveChanges();

                MessageBox.Show($"Вы успешно отозвали заявление на специальность {SelectedSpeciality.SpecialityName.Name}");
            }
        }

        private bool CanWithdrawApplicationCommandExecuted(object param) => true;
        #endregion


        #endregion Commands

        #region Constructor

        public ApplicationVM()
        {
            //Initialize commands
            ApplyApplicationCommand = new DelegateCommand(OnApplyApplicationCommandExecute, CanApplyApplicationCommandExecuted);
            WithdrawApplicationCommand = new DelegateCommand(OnWithdrawApplicationCommandExecute, CanWithdrawApplicationCommandExecuted);

            //Initialize ErrorsViewModel-class instance
            _errorsViewModel = new ErrorsViewModel();

            _enrolleePersonalCabinetVM = new EnrolleePersonalCabinetVM();

            using (AppDbContext db = new())
            {
                AllFinancings = new ObservableCollection<Financing>(db.Financing.ToList());
                SelectedFinancing = AllFinancings[0];
            }
            

            //Load all specialities and financings from database
            using (AppDbContext db = new())
            {
                AllFinancings = new ObservableCollection<Financing>(db.Financing.ToList());
                SelectedFinancing = AllFinancings[0];
            }
        }

        #endregion Constructor

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errorsViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(IsApplyApplicationEnabled));
        }

        #endregion INotifyDataErrorInfo
    }
}