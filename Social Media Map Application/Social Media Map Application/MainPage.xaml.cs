using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mapsui;
using Tweetinvi;
using Tweetinvi.Auth;
using Xamarin.Essentials;
using System.IO;
using System.Diagnostics;
using Xamarin.Auth;
namespace Social_Media_Map_Application
{
    public partial class MainPage : ContentPage
    {
        Mapsui.Map map = new Mapsui.Map
        {
            CRS = "EPSG:3857",
        };

        public MainPage()
        {
            InitializeComponent();
            var tileLayer = Mapsui.Utilities.OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);


            mapView.Map = map;
            mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Forms.Position(-31.9523, 115.8613), false);

            Mapsui.UI.Forms.Position p = new Mapsui.UI.Forms.Position(-31.9523, 115.8613);
            mapView.Navigator.NavigateTo(p.ToMapsui(),2);
            
            var hasCarers = CarerManagerSingleton.Instance.LoadCarersFromJSON();

            if (hasCarers)
            {
                foreach (var carer in CarerManagerSingleton.Instance.GetAllCarers())
                {
                    AddPin(carer.Key, carer.Value.latitude, carer.Value.longitude, carer.Value.image);
                }
            }
        }

        async void Alert_Clicked(object sender, System.EventArgs e)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Send message to what social media:", "Cancel", null, "Twitter");
            SocialMediaManagerSingleton.Instance.PostNewMessage(action);
        }


        async void Read_Clicked(object sender, System.EventArgs e)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Read messages from what social media:", "Cancel", null, "Twitter");
            SocialMediaManagerSingleton.Instance.ReadAllMessages(action);
        }

        private void OnMapClicked(object sender, Mapsui.UI.Forms.MapClickedEventArgs e)
        {
            mapView.MyLocationLayer.UpdateMyLocation(e.Point, false);
            mapView.Refresh();
        }


        async void AddCarer_Clicked(object sender, System.EventArgs e)
        {
            var name = await CarerManagerSingleton.Instance.AddNewCarer(mapView.MyLocationLayer.MyLocation.Latitude, mapView.MyLocationLayer.MyLocation.Longitude);

            if (name != null)
            {
                AddPin(name, CarerManagerSingleton.Instance.GetCarer(name).latitude, CarerManagerSingleton.Instance.GetCarer(name).longitude, CarerManagerSingleton.Instance.GetCarer(name).image.ToArray());
                await DisplayAlert("Success", "Carer Added!", "OK");
            }
        }

        async void DeleteCarer_Clicked(object sender, System.EventArgs e)
        {

            string name = await Application.Current.MainPage.DisplayPromptAsync("Name of Carer", "Please enter the name of the carer you wish to delete below");

            if (name != null)
            {
                mapView.Pins.Clear();
                mapView.HideCallouts();

                CarerManagerSingleton.Instance.DeleteCarer(name);

                foreach (var carer in CarerManagerSingleton.Instance.GetAllCarers())
                {
                    AddPin(carer.Key, carer.Value.latitude, carer.Value.longitude, carer.Value.image);
                }

                mapView.RefreshData();
                await DisplayAlert("Success", "Carer Deleted!", "OK");
            }
        }


        void AddPin(string name, double latitude, double longitude, byte[] image)
        {
            var pin = new Mapsui.UI.Forms.Pin(mapView)
            {
                Label = name.ToString(),
                Type = Mapsui.UI.Forms.PinType.Icon,
                Position = new Mapsui.UI.Forms.Position(latitude, longitude),
                Icon = image,
                Scale = 0.24f
            };

            pin.Callout.CalloutClicked += async (s, e) =>
            {
                var marker = e.Callout.Pin;

                var carer = CarerManagerSingleton.Instance.GetCarer(marker.Label);

                await DisplayAlert("Carer Info", "Name: " + carer.carerName + "\n" + "Carer type: " + carer.careLevel + "\n" + "Availabilty: " + carer.carerTime, "OK");
                pin.ShowCallout();
            };

            mapView.Pins.Add(pin);
            pin.ShowCallout();

        }

    }
}
