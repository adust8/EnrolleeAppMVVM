using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnrolleeAppMVVM.ViewModel
{
    public class ErrorsViewModel : INotifyDataErrorInfo
    {
        #region INotiftyDataErrorInfo

        public readonly Dictionary<string, List<string>> _propertyErrors;

        public void ClearErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }

        }
        public bool HasErrors => _propertyErrors.Any();


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public IEnumerable GetErrors(string propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }

            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        }

        #endregion INotiftyDataErrorInfo

        #region Custom validations

        #region Password validation

        /// <summary>
        /// List of the most popular passwords
        /// </summary>
        public static List<string> MostPopularPasswordsList = new List<string>
        {
            "12345",
            "123456",
            "1234567",
            "12345678",
            "123456789",
            "password",
            "root123",
            "asdfg",
            "hackme",
            "zxcvb",
            "qwerty",
            "qwerty123",
            "1q2w3e",
            "111111",
            "1234567890",
            "abc123",
            "1234",
            "password1",
            "iloveyou",
            "1q2w3e4r",
            "000000",
            "zaq12wsx",
            "dragon",
            "sunshine",
            "princess",
            "letmein",
            "654321" ,
            "monkey",
            "27653",
            "1qaz2wsx",
            "123321",
            "qwertyuiop",
            "superman",
            "asdfghjkl"
        };

        /// <summary>
        /// Validation for password(checking for presence of numbers,upper chars and punctuation chars)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");

            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && password.Any(x => char.IsPunctuation(x));
        }

        #endregion

        /// <summary>
        /// Checking for the existence of such login
        /// </summary>
        /// <param name="login">Login</param>
        /// <returns></returns>
        public static bool IsLoginAlreadyExist(string login)
        {
            using (AppDbContext db = new())
            {
                return db.Users.Any(p => p.Login == login);
            }
        }

        /// <summary>
        /// Checking for the existence of application on selected speciality
        /// </summary>
        /// <param name="currentEnrolleeGUID">Current enrollee's Guid</param>
        /// <param name="SelectedSpeciality">Selected speciality</param>
        /// <returns></returns>
        public static bool IsApplicationAlreadyExists(Guid currentEnrolleeGUID, Speciality SelectedSpeciality)
        {
            using (AppDbContext db = new())
            {
                if (SelectedSpeciality != null)
                    return db.Enrollees_Specialities.Where(x => x.EnrolleeGUID == currentEnrolleeGUID && x.SpecialityId == SelectedSpeciality.SpecialityId).Any();
                else return true;

            }
        }

        /// <summary>
        /// Checking for speciality minimal education level
        /// </summary>
        /// <param name="currentEnrolleeGUID">Current enrollee's Guid</param>
        /// <param name="SelectedSpeciality">Selected speciality</param>
        /// <returns></returns>
        public static bool IsMatchToMinimalEducationLevel(Guid currentEnrolleeGUID, Speciality SelectedSpeciality)
        {
            using (AppDbContext db = new())
            {
                if (SelectedSpeciality.MinimumEducationLevel <= db.Enrollees.Where(x => x.EnrolleeGUID == currentEnrolleeGUID).Select(x => x.EducationId).FirstOrDefault())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method that return true if there is a limit for changing password otherwise false
        /// </summary>
        /// <param name="currentEnrolleeGUID"></param>
        /// <returns></returns>
        public static bool ChangingPasswordLimit()
        {
            using (AppDbContext db = new())
            {
                if(AccountManager.CurrentUser.LastPasswordChangeDate != null)
                {
                    if (AccountManager.CurrentUser.LastPasswordChangeDate.Value.AddDays(1) > DateTime.Today) return true;
                }

                return false;
            }
        }

        #endregion Custom validations

        #region Constructor

        public ErrorsViewModel()
        {
            _propertyErrors = new Dictionary<string, List<string>>();
        }

        #endregion Constructor
    }
}