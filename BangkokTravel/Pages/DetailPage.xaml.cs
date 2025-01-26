using BangkokTravel.Models;
using BangkokTravel.ViewModels;
using System.Text.Json;

namespace BangkokTravel.Pages;
public partial class DetailPage : ContentPage
{
    private bool isExpanded = false;

    public DetailPage()
    {
        InitializeComponent();
    }

    async private void backBtnFromDetailPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void ReadMoreBtn_Tapped(object sender, TappedEventArgs e)
    {
        // Toggle between TailTruncation and WordWrap
        if (isExpanded)
        {
            descriptionLabel.LineBreakMode = LineBreakMode.TailTruncation;
            descriptionLabel.MaxLines = 7;
            ((Label)((Border)sender).Content).Text = "Read More";
        }
        else
        {
            descriptionLabel.LineBreakMode = LineBreakMode.WordWrap;
            descriptionLabel.MaxLines = int.MaxValue; // Show all lines
            ((Label)((Border)sender).Content).Text = "Read Less";
        }

        // Toggle the flag
        isExpanded = !isExpanded;
    }

    async private void showMapBtn_Clicked(object sender, EventArgs e)
    {
        // ดึงข้อมูล MapUrl จาก BindingContext
        var popularPlace = BindingContext as PopularPlace ;
        var recommendPlace = BindingContext as RecommendPlace ;
        if (popularPlace != null && !string.IsNullOrEmpty(popularPlace.map))
        {
            // เปิด URL ของแผนที่ในเบราว์เซอร์
            await Launcher.OpenAsync(new Uri(popularPlace.map));
        }
        else if(recommendPlace != null && !string.IsNullOrEmpty(recommendPlace.map))
        {
            // เปิด URL ของแผนที่ในเบราว์เซอร์
            await Launcher.OpenAsync(new Uri(recommendPlace.map));
        }
        else
        {
            await DisplayAlert("Error", "Map URL is not available", "OK");
        }

    }

    async private void favoriteBtn_Clicked(object sender, EventArgs e)
    {
        await favoriteBtn.ScaleTo(1.2, 100, Easing.CubicInOut);
        await favoriteBtn.ScaleTo(1.0, 100, Easing.CubicInOut);
        
        var bindingData = BindingContext;

        if (bindingData != null)
        {
            try
            {
                string fileName = Path.Combine(FileSystem.AppDataDirectory, "favoriteplace.json");

                List<object> favoritePlaces;

                // ถ้าไฟล์มีอยู่แล้ว ให้อ่านข้อมูลเดิมก่อน
                if (File.Exists(fileName))
                {
                    string existingJson = File.ReadAllText(fileName);
                    favoritePlaces = JsonSerializer.Deserialize<List<object>>(existingJson) ?? new List<object>();
                }
                else
                {
                    favoritePlaces = new List<object>();
                }

                // เพิ่มข้อมูลใหม่เข้าไปใน list
                favoritePlaces.Add(bindingData);

                // แปลง list เป็น JSON และเขียนกลับไปที่ไฟล์
                string updatedJson = JsonSerializer.Serialize(favoritePlaces, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(fileName, updatedJson);

                // แจ้งให้ FavoritePlaceViewModel โหลดข้อมูลใหม่
                if (Application.Current.MainPage is NavigationPage navPage &&
                    navPage.RootPage is FavoritePage favoritePage &&
                    favoritePage.BindingContext is FavoritePlaceViewModel favoriteViewModel)
                {
                    await favoriteViewModel.RefreshFavoriteItemsAsync();
                }

                await DisplayAlert("Favorited", "Successfully added to favorite place", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "No data to save", "OK");
        }
    }
}
