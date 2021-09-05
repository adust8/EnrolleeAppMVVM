using EnrolleeAppMVVM.View;
using EnrolleeAppMVVM.ViewModel;
using System.Windows;

namespace EnrolleeAppMVVM.Services
{
    public class WindowService : IWindowService
    {
        public static void OpenNewWindow(object vm)
        {
            var win = new Window();

            switch (vm)
            {
                case LoginUserControlViewModel:
                    win = new SignInWindow();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is SignUpWindow || window is MainWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                    break;

                case SignUpWindowViewModel:
                    win = new SignUpWindow();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is SignInWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                    break;

                case MainWindowViewModel:
                    win = new MainWindow();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is SignInWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                    break;
                case EnrolleePersonalCabinetVM:
                    win = new SignInWindow();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is MainWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                    break;
            }

            win.Show();
        }
    }
}