using BangkokTravel.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace BangkokTravel.ViewModels;

public partial class AllPlaceViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<AllPlace> allPlaceItems;

    [ObservableProperty]
    private ObservableCollection<AllPlace> filteredPlaceItems;

    [ObservableProperty]
    private ObservableCollection<AllPlace> searchedPlaceItems;


    public AllPlaceViewModel()
    {
        AllPlaceItems = new ObservableCollection<AllPlace>();
        FilteredPlaceItems = new ObservableCollection<AllPlace>();
        SearchedPlaceItems = new ObservableCollection<AllPlace>();
        LoadAllPlaceAsync();
    }

    public async Task LoadAllPlaceAsync()
    {
        try
        {
            // ระบุเส้นทางไฟล์ JSON
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "allBangkokInfo.json");

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
                    using var stream = FileSystem.OpenAppPackageFileAsync("allBangkokInfo.json").Result;
                    using var reader = new StreamReader(stream);
                    string json = reader.ReadToEnd();
                    File.WriteAllText(filePath, json);
                }

                return File.ReadAllText(filePath);
            });

            var items = await Task.Run(() =>
            {
                return JsonSerializer.Deserialize<ObservableCollection<AllPlace>>(jsonContent);
            });

            AllPlaceItems = items ?? new ObservableCollection<AllPlace>();
            FilteredPlaceItems = new ObservableCollection<AllPlace>(AllPlaceItems);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON: {ex.Message}");
        }
    }

    // Function for filter category
    public async Task FilterItemsAsync(string category)
    {
        Console.WriteLine($"Filtering by category: {category}");

        if (string.IsNullOrEmpty(category) || category.Equals("All Place", StringComparison.OrdinalIgnoreCase))
        {
            FilteredPlaceItems = new ObservableCollection<AllPlace>(AllPlaceItems);
        }
        else
        {
            var filteredItems = await Task.Run(() =>
            {
                return AllPlaceItems.Where(item => item.category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            });

            if (filteredItems.Count == 0)
            {
                Console.WriteLine($"No items found for category: {category}");
            }

            FilteredPlaceItems = new ObservableCollection<AllPlace>(filteredItems);
        }

        Console.WriteLine($"Filtered items count: {FilteredPlaceItems.Count}");
    }

    // Function for search
    public async Task SearchItemsAsync(string query)
    {
        Console.WriteLine($"Searching by query: {query}");

        if (string.IsNullOrEmpty(query))
        {
            SearchedPlaceItems = new ObservableCollection<AllPlace>(AllPlaceItems);
        }
        else
        {
            var searchedItems = await Task.Run(() =>
            {
                return AllPlaceItems.Where(item => item.name.ToLower().Contains(query.ToLower())).ToList();
            });

            SearchedPlaceItems = new ObservableCollection<AllPlace>(searchedItems);
        }

        Console.WriteLine($"Searched items count: {SearchedPlaceItems.Count}");
    }



}
