using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mapsui;


namespace Social_Media_Map_Application
{
    public partial class MainPage : ContentPage
    {
        Map map = new Map
        {
            CRS = "EPSG:3857",
            
        };

        //to be removed
        int testCounter = 0;

        public MainPage()
        {
            InitializeComponent();
            var tileLayer = Mapsui.Utilities.OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);
            //map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(map) { TextAlignment = Mapsui.Widgets.Alignment.Center, HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left, VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Bottom });
           // map.Widgets.Add(new Mapsui.Widgets.Zoom.ZoomInOutWidget());
            mapView.Map = map;


           // mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Forms.Position(115.8613,31.9523));
        }

        async void AddEvent_Clicked(object sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("Tweet", "Please enter your tweet below");

            if (result != null)
            {
                testCounter++;
                var pin = new Mapsui.UI.Forms.Pin(mapView)
                {
                    Label = testCounter.ToString(),
                    Address = e.ToString(),
                    Type = Mapsui.UI.Forms.PinType.Pin,
                    Position = new Mapsui.UI.Forms.Position(mapView.MyLocationLayer.MyLocation.Latitude, mapView.MyLocationLayer.MyLocation.Longitude)
                };

                mapView.Pins.Add(pin);
            }

        }

        private void OnMapClicked(object sender, Mapsui.UI.Forms.MapClickedEventArgs e)
        {
            mapView.MyLocationLayer.UpdateMyLocation(e.Point, false);
            mapView.Refresh();
        }

        async private void OnPinClicked(object sender, Mapsui.UI.Forms.MapClickedEventArgs e)
        {
            await DisplayAlert("Tweet", "Here is the tweet", "OK");
        }
    }
}
