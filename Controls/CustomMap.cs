using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MauiGeoJsonMapping.Controls
{
    public class CustomMap : Microsoft.Maui.Controls.Maps.Map
    {
        public CustomMap() : base()
        {
            // Initialize collection change tracking if using ObservableCollection
            if (MapElementsSource is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += MapElementsSource_CollectionChanged;
            }
        }

        // Bindable property to allow binding a collection of MapElements (Polygon, Circle, Polyline)
        public static readonly BindableProperty MapElementsSourceProperty = BindableProperty.Create(
            nameof(MapElementsSource),
            typeof(IList<MapElement>),
            typeof(CustomMap),
            defaultValue: null,
            propertyChanged: OnMapElementsSourceChanged);

        public IList<MapElement>? MapElementsSource
        {
            get => (IList<MapElement>?)GetValue(MapElementsSourceProperty);
            set => SetValue(MapElementsSourceProperty, value);
        }

        private static void OnMapElementsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = (CustomMap)bindable;

            if (oldValue is INotifyCollectionChanged oldIncc)
            {
                oldIncc.CollectionChanged -= map.MapElementsSource_CollectionChanged;
            }

            map.MapElements.Clear();

            if (newValue is IList<MapElement> newElements)
            {
                foreach (var element in newElements)
                {
                    if (element != null)
                        map.MapElements.Add(element);
                }

                if (newValue is INotifyCollectionChanged newIncc)
                {
                    newIncc.CollectionChanged += map.MapElementsSource_CollectionChanged;
                }
            }
        }

        private void MapElementsSource_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                MapElements.Clear();
                if (sender is IEnumerable<MapElement> elems)
                {
                    foreach (var el in elems)
                        MapElements.Add(el);
                }
                return;
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is MapElement el)
                        MapElements.Remove(el);
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is MapElement el)
                        MapElements.Add(el);
                }
            }
        }

        // Bindable property to trigger MoveToRegion via binding
        public static readonly BindableProperty RegionProperty = BindableProperty.Create(
            nameof(Region),
            typeof(MapSpan),
            typeof(CustomMap),
            defaultValue: null,
            propertyChanged: OnRegionChanged);

        public MapSpan? Region
        {
            get => (MapSpan?)GetValue(RegionProperty);
            set => SetValue(RegionProperty, value);
        }

        private static void OnRegionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = (CustomMap)bindable;
            if (newValue is MapSpan span)
            {
                map.MoveToRegion(span);
            }
        }
    }
}