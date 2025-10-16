namespace MauiGeoJsonMapping.Models
{
    public class Feature
    {
        public string type { get; set; } = string.Empty;
        public Properties properties { get; set; } = new Properties();
        public Geometry geometry { get; set; } = new Geometry();
    }

    public class Geometry
    {
        public string type { get; set; } = string.Empty;
        public List<List<List<double>>> coordinates { get; set; } = new List<List<List<double>>>();
    }

    public class Properties
    {
    }

    public class GeoFence
    {
        public string type { get; set; } = string.Empty;
        public List<Feature> features { get; set; } = new List<Feature>();
    }
}