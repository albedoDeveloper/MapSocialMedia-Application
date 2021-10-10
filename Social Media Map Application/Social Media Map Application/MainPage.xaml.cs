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

        }

        void AddEvent_Clicked(object sender, System.EventArgs e)
        {
            testCounter++;
            var pin = new Mapsui.UI.Forms.Pin(mapView)
            {
                Label = testCounter.ToString(),
                Address = e.ToString(),
                Type = Mapsui.UI.Forms.PinType.Pin,
                Position = new Mapsui.UI.Forms.Position(36.9641949, -122.0177232)
            };

            //mapView.Pins.Add(pin);
        }
    }
}
