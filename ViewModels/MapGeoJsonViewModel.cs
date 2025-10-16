using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;
using MauiGeoJsonMapping.Models;
using MauiGeoJsonMapping.Services;
using System.Text;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Microsoft.Maui.Controls;
using System.Linq;
using System.Globalization;

namespace MauiGeoJsonMapping.ViewModels
{
    public class MapGeoJsonViewModel : BaseViewModel
    {
        private ObservableCollection<ItemOfInterest> _locations;
        private PositioningService _positioningService;
        private GeoJsonFencingService _geoJsonFencingService;

        private List<MauiLocation> _mapGeoFence;

        public List<MauiLocation> MapGeoFence
        {
            get { return _mapGeoFence; }
            set { _mapGeoFence = value; }
        }

    private MapSpan _mapCenter = MapSpan.FromCenterAndRadius(new MauiLocation(0, 0), Distance.FromKilometers(1));
    private string _latitudeText = "-33.9320";
    private string _longitudeText = "18.6320";
    private string _resultText = string.Empty;

    private ObservableCollection<MapElement> _mapElements = new();
    private int _userPointCount = 0;

    private NetTopologySuite.Geometries.Polygon? _ntsPolygon;

        public MapSpan MapCenter
        {
            get { return _mapCenter; }
            set
            {
                _mapCenter = value;
                OnPropertyChanged();
            }
        }

        public string LatitudeText
        {
            get => _latitudeText;
            set { _latitudeText = value; OnPropertyChanged(); }
        }

        public string LongitudeText
        {
            get => _longitudeText;
            set { _longitudeText = value; OnPropertyChanged(); }
        }

        public string ResultText
        {
            get => _resultText;
            set { _resultText = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MapElement> MapElements
        {
            get => _mapElements;
            set { _mapElements = value; OnPropertyChanged(); }
        }

        public Command PlotUserPointCommand { get; }

        public ObservableCollection<ItemOfInterest> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }

        public MapGeoJsonViewModel(INavigation? navigation) : base(navigation)
        {
            _positioningService = new PositioningService();
            _geoJsonFencingService = new GeoJsonFencingService();
            _locations = new ObservableCollection<ItemOfInterest>();
            _mapGeoFence = new List<MauiLocation>();

            try
            {
                CreateMapPins();
                CenterOnMapPosition();
                FetchGeoFence();
                BuildFence();

                PlotUserPointCommand = new Command(async () => await PlotUserPointAsync());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing MapGeoJsonViewModel: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw;
            }
        }

        private void CreateMapPins()
        {
            var points = _positioningService.GetPointsOfInterest();
            Locations = new ObservableCollection<ItemOfInterest>(points);
        }

        private void CenterOnMapPosition()
        {
            var pos = _positioningService.GetCurrentPosition();
            MapCenter = MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(0.5));
        }

        private void FetchGeoFence()
        {
            var geoJsonFence = _geoJsonFencingService.GetFencePosition();
            if (geoJsonFence.features.Count > 0)
            {
                var polypoints = geoJsonFence.features[0].geometry.coordinates[0];

                _mapGeoFence = new List<MauiLocation>();

                foreach (var polyPoint in polypoints)
                {
                    if (polyPoint.Count >= 2)
                    {
                        var position = new MauiLocation(polyPoint[1], polyPoint[0]);
                        _mapGeoFence.Add(position);
                    }
                }
            }
        }

        private void BuildFence()
        {
            if (MapGeoFence != null && MapGeoFence.Count > 0)
            {
                var polygon = new Microsoft.Maui.Controls.Maps.Polygon
                {
                    FillColor = Colors.Red.WithAlpha(0.3f),
                    StrokeColor = Colors.Black,
                    StrokeWidth = 2
                };

                var coordinates = new Coordinate[MapGeoFence.Count];
                for (int i = 0; i < MapGeoFence.Count; i++)
                {
                    var location = MapGeoFence[i];
                    polygon.Geopath.Add(location);
                    coordinates[i] = new Coordinate(location.Longitude, location.Latitude);
                }

                var geometryFactory = new NetTopologySuite.Geometries.GeometryFactory();
                _ntsPolygon = geometryFactory.CreatePolygon(coordinates);

                MapElements.Add(polygon);
            }
        }

        private async Task PlotUserPointAsync()
        {
            try
            {
                // Validate and parse input
                if (string.IsNullOrWhiteSpace(LatitudeText) || string.IsNullOrWhiteSpace(LongitudeText))
                {
                    ResultText = "Please enter both latitude and longitude";
                    await ShowAlertAsync("Input Required", "Please enter both latitude and longitude values");
                    return;
                }

                if (!double.TryParse(LatitudeText, NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude) ||
                    !double.TryParse(LongitudeText, NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude))
                {
                    ResultText = "Invalid coordinates. Please enter valid numbers";
                    await ShowAlertAsync("Invalid Input", "Please enter valid numeric values for latitude and longitude");
                    return;
                }

                if (latitude < -90 || latitude > 90)
                {
                    ResultText = "Latitude must be between -90 and 90";
                    await ShowAlertAsync("Invalid Latitude", "Latitude must be between -90 and 90");
                    return;
                }

                if (longitude < -180 || longitude > 180)
                {
                    ResultText = "Longitude must be between -180 and 180";
                    await ShowAlertAsync("Invalid Longitude", "Longitude must be between -180 and 180");
                    return;
                }

                var userLocation = new MauiLocation(latitude, longitude);
                var pinLabel = $"User Point {++_userPointCount}";

                // Add to Locations so the map shows a pin via ItemTemplate binding
                Locations.Add(new ItemOfInterest
                {
                    InterestId = Guid.NewGuid().ToString(),
                    Name = pinLabel,
                    Description = $"{latitude:F6}, {longitude:F6}",
                    Position = userLocation
                });

                var sb = new StringBuilder();
                sb.AppendLine($"=== Plotted: {pinLabel} ===");
                sb.AppendLine($"Coordinates: {latitude:F6}, {longitude:F6}");

                if (_ntsPolygon != null)
                {
                    var geometryFactory = new NetTopologySuite.Geometries.GeometryFactory();
                    var point = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
                    bool isInside = _ntsPolygon.Contains(point);

                    sb.AppendLine($"Inside Geofence: {(isInside ? "YES \u2713" : "NO \u2717")}");

                    double distance = _ntsPolygon.Distance(point);
                    double distanceMeters = distance * 111000;
                    sb.AppendLine($"Distance to boundary: {distanceMeters:F2} meters");

                    var circle = new Microsoft.Maui.Controls.Maps.Circle
                    {
                        Center = userLocation,
                        Radius = new Distance(50),
                        StrokeColor = isInside ? Colors.Green : Colors.Red,
                        StrokeWidth = 3,
                        FillColor = (isInside ? Colors.Green : Colors.Red).WithAlpha(0.3f)
                    };
                    MapElements.Add(circle);
                }
                else
                {
                    sb.AppendLine("Geofence not loaded");
                }

                MapCenter = MapSpan.FromCenterAndRadius(userLocation, Distance.FromMeters(500));

                ResultText = sb.ToString();
                await ShowAlertAsync("Point Plotted", sb.ToString());
            }
            catch (Exception ex)
            {
                ResultText = $"Error: {ex.Message}";
                await ShowAlertAsync("Error", $"An error occurred: {ex.Message}");
            }
        }

        private Task ShowAlertAsync(string title, string message)
        {
            var page = Application.Current?.Windows?.FirstOrDefault()?.Page;
            if (page != null)
                return page.DisplayAlert(title, message, "OK");
            return Task.CompletedTask;
        }
    }
}