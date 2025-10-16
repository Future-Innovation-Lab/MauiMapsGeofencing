# MAUI GeoJson Mapping Application

**📚 Educational Project**: This is a .NET MAUI learning application demonstrating GeoJson mapping functionality with custom polygons and pins on interactive maps.

## Features

- **Interactive Map**: Displays a map with custom pins showing points of interest
- **GeoJson Polygon**: Renders a polygon fence from embedded GeoJson data
- **Custom Map Control**: Uses a custom map control for enhanced functionality
- **Cross-Platform**: Supports Android, iOS, Windows, and macOS
- **MVVM Architecture**: Clean separation of concerns with ViewModel-driven UI bindings
- **Geofencing Logic**: Demonstrates point-in-polygon calculations and distance measurements

## Project Structure

```
MauiGeoJsonMapping/
├── Controls/           # Custom map control
├── Geostore/          # Embedded GeoJson data
├── Handlers/          # Platform-specific map handlers
├── Models/            # Data models for POI and GeoFence
├── Services/          # Business logic services
├── ViewModels/        # MVVM ViewModels
├── Views/             # XAML pages
├── Resources/         # App resources (icons, fonts, styles)
└── Platforms/         # Platform-specific code
```

## Architecture & Implementation

This project demonstrates core .NET MAUI patterns:

### 1. MVVM Pattern
- **MapGeoJsonViewModel**: Contains all business logic, state management, and commands
- **MapGeoJsonPage**: Minimal code-behind; binds entirely to ViewModel
- **Property Bindings**: Two-way binding for user inputs and map state
- **Commands**: `PlotUserPointCommand` handles user interactions

### 2. Custom Map Control
- Extended `Microsoft.Maui.Controls.Maps.Map` with bindable properties
- `MapElementsSource`: Binds collection of map elements (polygons, circles)
- `Region`: Bindable property for map centering and zooming
- Automatic collection change tracking via `INotifyCollectionChanged`

### 3. Geofencing Logic
- NetTopologySuite for geometric operations (point-in-polygon, distance calculations)
- Visual feedback: Green circle for points inside geofence, red for outside
- Distance calculation to polygon boundary
- GeoJSON data loaded from embedded resources

## Setup Instructions

1. **Clone the repository**
2. **Install .NET 9 SDK**
3. **Add Font Files**:
   - Download OpenSans-Regular.ttf and OpenSans-Semibold.ttf
   - Place them in `Resources/Fonts/` directory
4. **Android Setup**:
   - Add a custom pin icon to `Platforms/Android/Resources/drawable/pin.png`
   - Update Android SDK and build tools
5. **Build and Run**:
   ```
   dotnet build
   dotnet run --project MauiGeoJsonMapping.csproj -f net9.0-android
   ```

## Configuration

### Map API Keys

#### Android - Google Maps API Key

1. **Get a Google Maps API Key**:
   - Go to [Google Cloud Console](https://console.cloud.google.com/)
   - Create a new project or select an existing one
   - Enable the **Maps SDK for Android** API
   - Create a new API key (restrict it to Android apps for security)
   - Note your API key

2. **Add the API Key to AndroidManifest.xml**:
   - Open `Platforms/Android/AndroidManifest.xml`
   - Find the line with `INSERT_YOUR_API_KEY_HERE`
   - Replace it with your actual Google Maps API key:
     ```xml
     <meta-data android:name="com.google.android.geo.API_KEY" android:value="YOUR_API_KEY_HERE" />
     ```
   - Save the file

3. **Example**:
   ```xml
   <meta-data android:name="com.google.android.geo.API_KEY" android:value="AIzaSyD1234567890abcdefghijklmnopqrstu" />
   ```

⚠️ **Important**: Never commit your real API key to version control. Use environment variables or a local configuration file not tracked by git.

#### iOS - Maps Configuration
Configure Maps in `Platforms/iOS/Info.plist` if needed for iOS builds.

### GeoJson Data
The polygon data is stored in `Geostore/geofence.json` as an embedded resource. You can modify this file to change the polygon coordinates.

## Technical Details

- **.NET Version**: 9.0
- **MAUI Version**: 9.0.10
- **Map Package**: Microsoft.Maui.Controls.Maps 9.0.10
- **JSON Parsing**: Newtonsoft.Json 13.0.3

## Known Issues

- Custom pin icons require manual setup of resource files
- Map API keys need to be configured for production use
- Font files are referenced but not included in the repository

## Contributing

This is an educational project. Contributions for learning purposes are welcome:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test on multiple platforms
5. Submit a pull request with a description of what you learned or improved

## License & Usage

This project is provided for **educational purposes only**. Use it to learn:
- .NET MAUI development patterns
- MVVM architecture in real applications
- Geospatial data visualization
- Map API integration on mobile platforms