using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mapsui;
using Tweetinvi;
using Xamarin.Essentials;
using System.IO;


namespace Social_Media_Map_Application
{
    public partial class MainPage : ContentPage
    {
        Mapsui.Map map = new Mapsui.Map
        {
            CRS = "EPSG:3857",

        };

        //to be removed
        int testCounter = 0;

        Tweetinvi.TwitterClient userClient;
        public MainPage()
        {
            InitializeComponent();
            var tileLayer = Mapsui.Utilities.OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);

            mapView.Map = map;
            mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Forms.Position(31.9523, 115.8613), false);

            userClient = new TwitterClient("cBpRVUldNZjRZe3TL2ZAEwwZ5", "B83oc4cAX68u037ZLzwDKlPQBb5bpj9vFtEd7sgyqoRZ5VDmnV", "1445049682854506506-uLKCbWcLGflxgQoYxrZSvYPaN6pUqr", "KNHAWhJDpe6p9F9eMgXMZdMKxwJhkQluzLjBG40O3iq9s");
        }



        async void Alert_Clicked(object sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("Tweet", "Please enter your tweet below");

            if (result != null)
            {
                bool answer = await DisplayAlert("Tweet", "Would you like to also upload an image?", "Yes", "No");

                FileResult photo = null;

                if (answer)
                {
                    try
                    {
                        photo = await MediaPicker.PickPhotoAsync();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
                    }
                }

                string action = await DisplayActionSheet("Upload event to:", "Cancel", null, "Twitter");

                if (action == "Twitter")
                {
                    if (photo == null)
                        await userClient.Tweets.PublishTweetAsync(result);

                    else
                    {
                        var tweetinviLogoBinary = File.ReadAllBytes(photo.FullPath);
                        var uploadedImage = await userClient.Upload.UploadTweetImageAsync(tweetinviLogoBinary);
                        var tweetWithImage = await userClient.Tweets.PublishTweetAsync(new Tweetinvi.Parameters.PublishTweetParameters(result)
                        {
                            Medias = { uploadedImage }
                        });
                    }
                }


            }
        }

        private void OnMapClicked(object sender, Mapsui.UI.Forms.MapClickedEventArgs e)
        {
            mapView.MyLocationLayer.UpdateMyLocation(e.Point, false);
            mapView.Refresh();
        }

       /*async private void OnPinClicked(object sender, Mapsui.UI.Forms.MapClickedEventArgs e)
        {


            await DisplayAlert("Carer Info", "here", "OK");


        }*/


        async void AddCarer_Clicked(object sender, System.EventArgs e)
        {
            string name = await DisplayPromptAsync("Name of Carer", "Please enter the name of the carer below");
            string action = await DisplayActionSheet("Type of Carer", "Cancel",null,"Relative", "Friend", "Professional");
            string time = await DisplayPromptAsync("Time available", "Please enter the times the carer is available below");

            if (name != null && time != null && !action.Equals("Cancel"))
            {

                EventManagerSingleton.Instance.AddNewCarer(name, action, time);



                AddPin(name);



            }

        }

         void AddPin(string name)
        {
            var pin = new Mapsui.UI.Forms.Pin(mapView)
            {
                Label = name.ToString(),
                Type = Mapsui.UI.Forms.PinType.Icon,
                Position = new Mapsui.UI.Forms.Position(mapView.MyLocationLayer.MyLocation.Latitude, mapView.MyLocationLayer.MyLocation.Longitude)
            };

            pin.Callout.CalloutClicked += async(s, e) =>
            {
                var marker = e.Callout.Pin;

                var carer = EventManagerSingleton.Instance.GetCarer(marker.Label);

                await DisplayAlert("Carer Info", carer.carerName + "\n" + carer.careLevel + "\n" + carer.carerTime, "OK");

            };

            mapView.Pins.Add(pin);

        }


    }
}
