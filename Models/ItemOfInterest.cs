using Microsoft.Maui.Controls.Maps;

namespace MauiGeoJsonMapping.Models
{
    public class ItemOfInterest
    {
        public string InterestId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Location Position { get; set; } = new Location();
    }
}