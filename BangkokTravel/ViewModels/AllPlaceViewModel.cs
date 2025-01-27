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

    [ObservableProperty]
    private bool isLoading;

    private bool _isDataLoaded = false;

    public AllPlaceViewModel()
    {
        AllPlaceItems = new ObservableCollection<AllPlace>();
        FilteredPlaceItems = new ObservableCollection<AllPlace>();
        SearchedPlaceItems = new ObservableCollection<AllPlace>();
        LoadAllPlaceAsync();
    }

    public async Task LoadAllPlaceAsync()
    {
        if (_isDataLoaded) return;
        IsLoading = true;
        try
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "allBangkokInfo.json");
            if (!File.Exists(filePath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("allBangkokInfo.json");
                using var reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                File.WriteAllText(filePath, json);
            }

            string jsonContent = await File.ReadAllTextAsync(filePath);

            // ใช้ List ชั่วคราว
            var items = JsonSerializer.Deserialize<List<AllPlace>>(jsonContent);
            AllPlaceItems = new ObservableCollection<AllPlace>(items);

            FilteredPlaceItems = new ObservableCollection<AllPlace>(items);

            _isDataLoaded = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }



    // Function for filter category
    public async Task FilterItemsAsync(string category)
    {
        try
        {
            IsLoading = true;
            List<AllPlace> filteredItems;

            if (string.IsNullOrEmpty(category) || category.Equals("All Place", StringComparison.OrdinalIgnoreCase))
            {
                filteredItems = AllPlaceItems.ToList();
            }
            else
            {
                filteredItems = await Task.Run(() =>
                    AllPlaceItems.AsParallel() // ใช้ Parallel LINQ
                        .Where(item => item.category.Equals(category, StringComparison.OrdinalIgnoreCase))
                        .ToList()
                );
            }

            FilteredPlaceItems = new ObservableCollection<AllPlace>(filteredItems);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering items: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Function for search
    public async Task SearchItemsAsync(string query)
    {
        try
        {
            IsLoading = true;
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading JSON: {ex.Message}");
        }
        finally { IsLoading = false; }
    }
}