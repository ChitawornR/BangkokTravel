namespace BangkokTravel.Pages;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
    }

    async private void navToHomePageBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }
}