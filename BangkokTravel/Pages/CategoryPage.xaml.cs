using BangkokTravel.Models;
using BangkokTravel.ViewModels;

namespace BangkokTravel.Pages;

public partial class CategoryPage : ContentPage
{

    private AllPlaceViewModel _allPlaceViewModel;
    public CategoryPage(string selectedCategory = "All Place")
    {
        InitializeComponent();

        // รอโหลดข้อมูลก่อน
        LoadDataAsync(selectedCategory);
    }

    public async Task LoadDataAsync(string selectedCategory = "All Place")
    {
        _allPlaceViewModel = new AllPlaceViewModel();
        await _allPlaceViewModel.LoadAllPlaceAsync();  // รอจนกว่าข้อมูลจะโหลดเสร็จ
        BindingContext = _allPlaceViewModel;

        // เลือกหมวดหมู่ที่กำหนดจากปุ่มหรือ Picker
        pickerCategory.SelectedItem = selectedCategory;
        // ตั้ง title ให้หัวของ Page
        titleCategoryPage.Text = selectedCategory;

        // กรองข้อมูลตามหมวดหมู่ที่เลือก
        _allPlaceViewModel.FilterItemsAsync(selectedCategory);
    }


    private async void OnCategoryChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            var selectedCategory = picker.SelectedItem.ToString();
            var viewModel = BindingContext as AllPlaceViewModel;
            await viewModel?.FilterItemsAsync(selectedCategory);
            // ตั้ง title ให้หัวของ Page
            titleCategoryPage.Text = selectedCategory;
        }
    }

    async private void backBtnFromCategoryPage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindableObject && bindableObject.BindingContext is AllPlace selectedItem)
        {
            var detailPage = new DetailPage
            {
                BindingContext = selectedItem
            };

            await Navigation.PushAsync(detailPage);
        }
    }

    async private void searchResultListViewCategoryPage_ItemTapped(object sender, ItemTappedEventArgs e)
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

    async private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var query = e.NewTextValue;  // ค่าคำค้นหาจาก TextChanged
        await _allPlaceViewModel.SearchItemsAsync(query);  // เรียกใช้ฟังก์ชันค้นหาจาก ViewModel
        searchResultListViewCategoryPage.IsVisible = !string.IsNullOrEmpty(query);
    }
}