using Microsoft.Maui.Controls.Maps;
using MauiGeoJsonMapping.ViewModels;

namespace MauiGeoJsonMapping.Views;

public partial class MapGeoJsonPage : ContentPage
{
    private MapGeoJsonViewModel? _viewModel;

    public MapGeoJsonPage()
    {
        try
        {
            InitializeComponent();

            // Pass null for Navigation initially, it will be available later
            _viewModel = new MapGeoJsonViewModel(null);
            BindingContext = _viewModel;
        }
        catch (Exception ex)
        {
            // Log the actual exception to help with debugging
            System.Diagnostics.Debug.WriteLine($"Error in MapGeoJsonPage constructor: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw;
        }
    }
}