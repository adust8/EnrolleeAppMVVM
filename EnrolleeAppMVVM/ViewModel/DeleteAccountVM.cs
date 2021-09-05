using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using EnrolleeAppMVVM.Model;
using EnrolleeAppMVVM.Services;
using EnrolleeAppMVVM.Services.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{
    internal class DeleteAccountVM:BaseViewModel
    {
        private string _deleteString;

        public string DeleteString
        {
            get => _deleteString;
            set => Set(ref _deleteString, value);
        }

        public ICommand DeleteAccountCommand { get; set; }

        public void OnDeleteAccountCommandExecute(object param)
        {
            using (AppDbContext db = new())
            {
                User user = db.Users.Where(x => x.UserGUID.Equals(AccountManager.CurrentUser.UserGUID)).FirstOrDefault();
                Enrollee enrollee = db.Enrollees.Where(x => x.UserGUID.Equals(user.UserGUID)).FirstOrDefault();
                if (DeleteString.StartsWith("Delete") && DeleteString[7..] == user.Login)
                {
                    MessageBox.Show("Вы удалили свой аккаунт");
                    db.Users.Remove(user);
                    db.Enrollees.Remove(enrollee);
                    db.SaveChanges();
                    WindowService.OpenNewWindow(new LoginUserControlViewModel());
                }          
            }
        }

        private bool CanDeleteAccountCommandExecuted(object param) => true;

        public DeleteAccountVM()
        {
            DeleteAccountCommand = new DelegateCommand(OnDeleteAccountCommandExecute, CanDeleteAccountCommandExecuted);
        }
    }
}
