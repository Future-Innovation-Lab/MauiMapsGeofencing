# MAUI GeoJson Mapping Application

This is a .NET MAUI application migrated from a Xamarin.Forms project. The app demonstrates GeoJson mapping functionality with custom polygons and pins.

## Features

- **Interactive Map**: Displays a map with custom pins showing points of interest
- **GeoJson Polygon**: Renders a polygon fence from embedded GeoJson data
- **Custom Map Control**: Uses a custom map control for enhanced functionality
- **Cross-Platform**: Supports Android, iOS, Windows, and macOS

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

## Migration from Xamarin.Forms

This project was migrated from Xamarin.Forms with the following key changes:

### 1. References Updated
- `Xamarin.Forms.Maps` → `Microsoft.Maui.Controls.Maps`
- `Xamarin.Forms` → `Microsoft.Maui.Controls`
- Position → Location

### 2. Custom Renderers → Handlers
- Xamarin custom renderers converted to MAUI handlers
- Android renderer logic moved to `CustomMapHandler`

### 3. Assembly Loading
- Updated embedded resource loading for MAUI
- Proper null checking and error handling

### 4. MVVM Pattern
- Maintained the same MVVM architecture
- Updated property change notifications
- Navigation service integration

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

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test on multiple platforms
5. Submit a pull request