using BangkokTravel.Pages;

namespace BangkokTravel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new WelcomePage()));
        }
    }
}