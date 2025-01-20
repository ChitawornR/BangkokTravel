using BangkokTravel.Models;
using BangkokTravel.ViewModels;
using System.Text.Json;

namespace BangkokTravel.Pages;

public partial class FavoritePage : ContentPage
{
    public FavoritePage()
    {
        InitializeComponent();

        // โหลด ViewModel และรีเฟรชข้อมูล
        var viewModel = new FavoritePlaceViewModel();
        BindingContext = viewModel;
        Task.Run(() => viewModel.RefreshFavoriteItemsAsync()).Wait();
    }


    async private void backBtnFromFavoritePage_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindableObject && bindableObject.BindingContext is FavoritePlace selectedItem)
        {
            var detailPage = new DetailPage
            {
                BindingContext = selectedItem
            };

            await Navigation.PushAsync(detailPage);
        }

    }

    async private void OnDeleteItemInvoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is FavoritePlace placeToDelete)
        {
            bool confirmDelete = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {placeToDelete.name}?", "Yes", "No");
            if (confirmDelete)
            {
                try
                {
                    // ระบุเส้นทางไฟล์ JSON
                    string filePath = Path.Combine(FileSystem.AppDataDirectory, "favoriteplace.json");

                    // โหลดข้อมูลจากไฟล์ JSON
                    string jsonContent = File.ReadAllText(filePath);
                    var favoritePlaces = JsonSerializer.Deserialize<List<FavoritePlace>>(jsonContent) ?? new List<FavoritePlace>();

                    // ลบข้อมูลที่เลือก
                    var itemToRemove = favoritePlaces.FirstOrDefault(p => p.name == placeToDelete.name);
                    if (itemToRemove != null)
                    {
                        favoritePlaces.Remove(itemToRemove);
                    }

                    // บันทึกข้อมูลกลับไปยังไฟล์ JSON
                    string updatedJson = JsonSerializer.Serialize(favoritePlaces, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    File.WriteAllText(filePath, updatedJson);

                    // อัปเดต ViewModel
                    if (BindingContext is FavoritePlaceViewModel viewModel)
                    {
                        viewModel.FavoriteItems.Remove(placeToDelete);
                    }

                    await DisplayAlert("Deleted", $"{placeToDelete.name} has been deleted.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }
    }

}