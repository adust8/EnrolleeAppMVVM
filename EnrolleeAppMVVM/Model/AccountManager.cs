using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Services;
using EnrolleeAppMVVM.ViewModel;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EnrolleeAppMVVM.Model
{
    public class AccountManager
    {
        #region Sign up method

        public static void SignUpAccount(Guid userGuid, Guid enrolleeGuid, string login, string password, string name, string surname, string patronymic, DateTime birthDate,string email)
        {
            using (AppDbContext db = new())
            {
                db.Users.Add(new User()
                {
                    UserGUID = userGuid,
                    Login = login,
                    Password = AccountManager.GetHashCode(password),
                    RoleId = 1,
                    LastPasswordChangeDate = DateTime.Today
                }
                );

                db.Enrollees.Add(new Enrollee()
                {
                    EnrolleeGUID = enrolleeGuid,
                    FirstName = name,
                    SecondName = surname,
                    Patronymic = patronymic,
                    BirthDate = birthDate,
                    StatusId = 4,
                    UserGUID = userGuid,
                    Email = email,
                    GPA = 0                   
                });

                db.SaveChanges();
            }
        }

        #endregion Sign up method


        #region Clear Guids method
        /// <summary>
        /// This method clear enrollee's current guids 
        /// </summary>
        public static void ClearCurrentGuids()
        {
            
        }
        #endregion


        //public static Guid CurrentUserGUID { get; private set; }
        //public static Guid CurrentEnrolleeGUID { get; private set; }
        public static User CurrentUser { get; set; }
        public static Enrollee CurrentEnrollee { get; set; }

        #region Hash-generator

        public static string GetHashCode(string Password)
        {
            StringBuilder sb = new();
            using (SHA1Managed sha1 = new())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(Password));

                foreach (var item in hash)
                {
                    sb.Append(item);
                }

                sb.Append(Password[Password.Length - 1]);
                sb.Append(Password.Length);
            }

            return sb.ToString();
        }

        #endregion Hash-generator
    }
}