using BangkokTravel.Models;
using BangkokTravel.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BangkokTravel.Pages;

public partial class HomePage : ContentPage
{
    private AllPlaceViewModel _allPlaceViewModel;

    public HomePage()
    {
        InitializeComponent();
        _allPlaceViewModel = new AllPlaceViewModel();
        var popularPlaceViewModel = new HomePagePopularPlaceViewModel();
        var recommendPlaceViewModel = new HomePageRecommendPlaceViewModel();

        Console.WriteLine($"Popular items count: {popularPlaceViewModel.PopularItems?.Count ?? 0}");
        Console.WriteLine($"Recommend items count: {recommendPlaceViewModel.RecommendItems?.Count ?? 0}");

        this.BindingContext = new
        {
            PopularPlace = popularPlaceViewModel,
            RecommendPlace = recommendPlaceViewModel,
            AllPlaces = _allPlaceViewModel
        };
    }

    async private void PopularTapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindableObject && bindableObject.BindingContext is PopularPlace selectedItem)
        {
            var detailPage = new DetailPage
            {
                BindingContext = selectedItem
            };

            await Navigation.PushAsync(detailPage);
        }

    }
    async private void RecommendTapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindableObject && bindableObject.BindingContext is RecommendPlace selectedItem)
        {
            var detailPage = new DetailPage
            {
                BindingContext = selectedItem
            };

            await Navigation.PushAsync(detailPage);
        }

    }

    async private void navFavoritePageBtn_Clicked(object sender, EventArgs e)
    {
        await navFavoritePageBtn.ScaleTo(1.2, 100, Easing.CubicInOut);
        await navFavoritePageBtn.ScaleTo(1.0, 100, Easing.CubicInOut);
        await Navigation.PushAsync(new FavoritePage());
    }

    async private void historicalBtn_Clicked(object sender, EventArgs e)
    {
        await historicalBtn.ScaleTo(1.1, 100, Easing.CubicInOut);
        await historicalBtn.ScaleTo(1.0, 100, Easing.CubicInOut);
        await Navigation.PushAsync(new CategoryPage("Historical"));
    }

    async private void natureAndParksBtn_Clicked(object sender, EventArgs e)
    {
        await natureAndParks.ScaleTo(1.1, 100, Easing.CubicInOut);
        await natureAndParks.ScaleTo(1.0, 100, Easing.CubicInOut);
        await Navigation.PushAsync(new CategoryPage("Nature and Parks"));
    }

    async private void shoppingBtn_Clicked(object sender, EventArgs e)
    {
        await shoppingBtn.ScaleTo(1.1, 100, Easing.CubicInOut);
        await shoppingBtn.ScaleTo(1.0, 100, Easing.CubicInOut);
        await Navigation.PushAsync(new CategoryPage("Shopping"));
    }

    async private void lifestyleAndNightlifeBtn_Clicked(object sender, EventArgs e)
    {
        await lifestyleAndNightlifeBtn.ScaleTo(1.1, 100, Easing.CubicInOut);
        await lifestyleAndNightlifeBtn.ScaleTo(1.0, 100, Easing.CubicInOut);
        await Navigation.PushAsync(new CategoryPage("Lifestyle and Nightlife"));
    }

    async private void allPlaceBtn_Clicked(object sender, EventArgs e)
    {
        await allPlaceBtn.ScaleTo(1.1, 100, Easing.CubicInOut);
        await allPlaceBtn.ScaleTo(1.0, 100, Easing.CubicInOut);
        await Navigation.PushAsync(new CategoryPage());
    }

    private async void homePageSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var query = e.NewTextValue;  // ค่าคำค้นหาจาก TextChanged
        await _allPlaceViewModel.SearchItemsAsync(query);  // เรียกใช้ฟังก์ชันค้นหาจาก ViewModel
        searchResultListView.IsVisible = !string.IsNullOrEmpty(query);
    }

    async private void SearchResultListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is AllPlace selectedItem)
        {
            var detailPage = new DetailPage
            {
                BindingContext = selectedItem
            };

            await Navigation.PushAsync(detailPage);

            // ยกเลิกการเลือก (เพื่อไม่ให้รายการคงเลือกอยู่)
            ((ListView)sender).SelectedItem = null;
        }
    }

   
}
