using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services.Commands;
using System.Linq;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel { get; set; }

        public string FullName
        {
            get
            {
                using (AppDbContext db = new())
                {
                    Enrollee enrollee = db.Enrollees.Where(x => x.EnrolleeGUID == AccountManager.CurrentEnrollee.EnrolleeGUID).FirstOrDefault();
                    return enrollee.SecondName + ' ' + enrollee.FirstName + ' ' + enrollee.Patronymic;
                }
            }
        }

        #region Commands

        #region Personal cabinet

        public ICommand ChangeViewToPersonalCabinetCommand { get; set; }
        public void OnChangeViewToPersonalCabinetExecute(object param)
        {
            CurrentViewModel = new EnrolleePersonalCabinetVM();
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public bool CanChangeViewToPersonalCabinetExecuted(object param) => true;

        #endregion

        #region Application

        public ICommand ChangeViewToApplicationCommand { get; set; }
        public void ChangeViewToApplicationExecute(object param)
        {
            CurrentViewModel = new ApplicationVM();
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public bool CanChangeViewToApplicationExecuted(object param) => true;


        #endregion

        #region Settings

        public ICommand ChangeViewToSettingsCommand { get; set; }

        public void OnChangeViewToSettingsCommandExecute(object param)
        {
            CurrentViewModel = new SettingsVM();
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private bool CanChangeViewToSettingsCommandExecuted(object param) => true;
        #endregion

        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            ChangeViewToPersonalCabinetCommand = new DelegateCommand(OnChangeViewToPersonalCabinetExecute, CanChangeViewToPersonalCabinetExecuted);
            ChangeViewToApplicationCommand = new DelegateCommand(ChangeViewToApplicationExecute, CanChangeViewToApplicationExecuted);
            ChangeViewToSettingsCommand = new DelegateCommand(OnChangeViewToSettingsCommandExecute, CanChangeViewToSettingsCommandExecuted);
        }
        #endregion

    }
}
