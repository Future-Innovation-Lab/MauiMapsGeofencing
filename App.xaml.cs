using MauiGeoJsonMapping.Views;

namespace MauiGeoJsonMapping;

public partial class App : Application
{
	public App()
	{
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error initializing App: {ex.Message}");
			System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
			// Continue without resources if initialization fails
		}
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new NavigationPage(new MapGeoJsonPage()));
	}
}