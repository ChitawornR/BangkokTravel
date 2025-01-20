using BangkokTravel.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace BangkokTravel.ViewModels;

public partial class FavoritePlaceViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<FavoritePlace> favoriteItems;

    public FavoritePlaceViewModel()
    {
        // โหลดข้อมูลทุกครั้งที่สร้าง ViewModel
        Task.Run(() => LoadFavoritePlacesAsync()).Wait();
    }

    private async Task LoadFavoritePlacesAsync()
    {
        try
        {
            // ระบุเส้นทางไฟล์ JSON
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "favoriteplace.json");

            // ตรวจสอบและโหลดไฟล์ JSON
            if (File.Exists(filePath))
            {
                string jsonContent = await File.ReadAllTextAsync(filePath);
                var items = JsonSerializer.Deserialize<ObservableCollection<FavoritePlace>>(jsonContent);

                // อัปเดต FavoriteItems
                FavoriteItems = items ?? new ObservableCollection<FavoritePlace>();
            }
            else
            {
                FavoriteItems = new ObservableCollection<FavoritePlace>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON: {ex.Message}");
        }
    }

    public async Task RefreshFavoriteItemsAsync()
    {
        await LoadFavoritePlacesAsync();
    }
}

