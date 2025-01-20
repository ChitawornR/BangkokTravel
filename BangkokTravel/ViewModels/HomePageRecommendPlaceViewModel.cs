using BangkokTravel.Models;
using BangkokTravel.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;



namespace BangkokTravel.ViewModels;

public partial class HomePageRecommendPlaceViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<RecommendPlace> recommendItems;

    

    public HomePageRecommendPlaceViewModel()
    {
        LoadRecommendPlaceAsync();
    }


    private async Task LoadRecommendPlaceAsync()
    {
        try
        {
            // ระบุเส้นทางไฟล์ JSON
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "recommend.json");

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
                    using var stream = FileSystem.OpenAppPackageFileAsync("recommend.json").Result;
                    using var reader = new StreamReader(stream);
                    string json = reader.ReadToEnd();
                    File.WriteAllText(filePath, json); // คัดลอกไฟล์ JSON
                }

                return File.ReadAllText(filePath); // โหลด JSON
            });

            // Deserialize ใน Background Thread
            var items = await Task.Run(() =>
            {
                return JsonSerializer.Deserialize<ObservableCollection<RecommendPlace>>(jsonContent);
            });

            RecommendItems = items ?? new ObservableCollection<RecommendPlace>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON: {ex.Message}");
        }
    }
}
