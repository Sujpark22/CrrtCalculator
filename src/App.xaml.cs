using Microsoft.Maui.ApplicationModel;

namespace CRRT_Calculator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.UserAppTheme = AppTheme.Light;
            MainPage = new AppShell();
            
        }
    }
}
//hello
