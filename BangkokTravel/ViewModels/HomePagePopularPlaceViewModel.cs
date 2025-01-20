using BangkokTravel.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json;


namespace BangkokTravel.ViewModels;

public partial class HomePagePopularPlaceViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PopularPlace> popularItems;

    public HomePagePopularPlaceViewModel()
    {
        LoadPopularPlaceAsync();
    }

    private async Task LoadPopularPlaceAsync()
    {
        try
        {
            // ระบุเส้นทางไฟล์ JSON
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "popular.json");

            // ใช้ code นี้ถ้าแก้ Json
            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);  // ลบไฟล์เก่า
            //}

            // ใช้ Background Thread ในการโหลดข้อมูล
            var jsonContent = await Task.Run(() =>
            {
                if (!File.Exists(filePath))
                {
                    using var stream = FileSystem.OpenAppPackageFileAsync("popular.json").Result;
                    using var reader = new StreamReader(stream);
                    string json = reader.ReadToEnd();
                    File.WriteAllText(filePath, json); // คัดลอกไฟล์ JSON
                }

                return File.ReadAllText(filePath); // โหลด JSON
            });

            // Deserialize ใน Background Thread
            var items = await Task.Run(() =>
            {
                return JsonSerializer.Deserialize<ObservableCollection<PopularPlace>>(jsonContent);
            });

            PopularItems = items ?? new ObservableCollection<PopularPlace>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON: {ex.Message}");
        }
    }



}
