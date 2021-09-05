using EnrolleeAppMVVM.Services.Commands;
using EnrolleeAppMVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EnrolleeAppMVVM.ViewModel
{

    internal class TitleButtonsPanelVM : BaseViewModel
    {
        #region Close window command

        public ICommand CloseWindowCommand { get; }

        public void OnCloseWindowCommandExecute(object param)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if(window is SignInWindow || window is SignUpWindow || window is MainWindow)
                {
                    window.Close();
                    Application.Current.Shutdown();
                }
            }


        }

        public bool CanCloseWindowCommandExecuted(object param) => true;
        #endregion

        #region Minimize window command
        public ICommand MinimizeWindowCommand { get; }

        public void OnMinimizeWindowCommandExecute(object param)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is SignInWindow || window is SignUpWindow || window is MainWindow)
                {
                    window.WindowState = WindowState.Minimized;
                }
            }
        }

        public bool CanMinimizeWindowCommandExecuted(object param) => true;
        #endregion

        #region Maximize window command
        public ICommand MaximizeWindowCommand { get; }
        

        public void OnMaximizeWindowCommandExecute(object param)
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (window is SignInWindow || window is SignUpWindow || window is MainWindow)
                {
                    if (window.WindowState == WindowState.Maximized)
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    else window.WindowState = WindowState.Maximized;

                }
            }
        }

        public bool CanMaximizeWindowCommandExecuted(object param) => true;
        #endregion

        #region Constructor
        public TitleButtonsPanelVM()
        {
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommandExecute, CanCloseWindowCommandExecuted);
            MinimizeWindowCommand = new DelegateCommand(OnMinimizeWindowCommandExecute, CanMinimizeWindowCommandExecuted);
            MaximizeWindowCommand = new DelegateCommand(OnMaximizeWindowCommandExecute, CanMaximizeWindowCommandExecuted);
        }
        #endregion
    }
}
