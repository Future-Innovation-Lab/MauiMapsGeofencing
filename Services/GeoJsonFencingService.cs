using Newtonsoft.Json;
using System.Reflection;
using MauiGeoJsonMapping.Models;

namespace MauiGeoJsonMapping.Services
{
    public class GeoJsonFencingService
    {
        public GeoJsonFencingService()
        {
        }

        public GeoFence GetFencePosition()
        {
            try
            {
                // Load embedded resource using MAUI assembly loading
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GeoFence)).Assembly;
                
                // Debug: List all available resources
                var resourceNames = assembly.GetManifestResourceNames();
                System.Diagnostics.Debug.WriteLine("Available embedded resources:");
                foreach (var name in resourceNames)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {name}");
                }
                
                using Stream? stream = assembly.GetManifestResourceStream("MauiGeoJsonMapping.Geostore.geofence.json");
                
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load geofence.json embedded resource. " +
                        $"Available resources: {string.Join(", ", resourceNames)}");
                }

                using var reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                var geoFence = JsonConvert.DeserializeObject<GeoFence>(text);
                
                return geoFence ?? new GeoFence();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading GeoFence: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}