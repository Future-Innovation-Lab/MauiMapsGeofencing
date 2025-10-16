using Microsoft.Maui.Controls.Maps;
using MauiGeoJsonMapping.Models;

namespace MauiGeoJsonMapping.Services
{
    public class PositioningService
    {
        private List<ItemOfInterest> _itemsOfInterest = new List<ItemOfInterest>();

        public PositioningService()
        {
            GenerateMockData();
        }

        private void GenerateMockData()
        {
            _itemsOfInterest = new List<ItemOfInterest>();

            var itemOfInterest = new ItemOfInterest
            {
                Name = "Place 1",
                Position = new Location(-33.933329, 18.6333308)
            };
            _itemsOfInterest.Add(itemOfInterest);

            itemOfInterest = new ItemOfInterest
            {
                Name = "Place 2",
                Position = new Location(-33.931, 18.6331)
            };
            _itemsOfInterest.Add(itemOfInterest);

            itemOfInterest = new ItemOfInterest
            {
                Name = "Place 3",
                Position = new Location(-33.932322, 18.632)
            };
            _itemsOfInterest.Add(itemOfInterest);

            itemOfInterest = new ItemOfInterest
            {
                Name = "Place 4",
                Position = new Location(-33.934, 18.6300)
            };
            _itemsOfInterest.Add(itemOfInterest);
        }

        public List<ItemOfInterest> GetPointsOfInterest()
        {
            return _itemsOfInterest;
        }

        public Location GetCurrentPosition()
        {
            return new Location(-33.933329, 18.6300);
        }
    }
}