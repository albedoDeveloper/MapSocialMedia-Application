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

        Tweetinvi.TwitterClient userClient;
        Map map2;
        public MainPage()
        {
            InitializeComponent();
            var tileLayer = Mapsui.Utilities.OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);
           
            map2 = new Map(mapView);
           
            mapView.Map = map;
            mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Forms.Position(31.9523, 115.8613), false);

            AuthorizeTwitter();

            var hasCarers = CarerManagerSingleton.Instance.LoadCarersFromJSON();

            if (hasCarers)
            {
                foreach (var carer in CarerManagerSingleton.Instance.GetAllCarers())
                {
                    AddPin(carer.Key, carer.Value.image);
                }
            }
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


        async void Read_Clicked(object sender, System.EventArgs e)
        {
            var user = await userClient.Users.GetAuthenticatedUserAsync();
            var url = "https://twitter.com/" + user.ToString();
            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }

        private void OnMapClicked(object sender, Mapsui.UI.Forms.MapClickedEventArgs e)
        {
            mapView.MyLocationLayer.UpdateMyLocation(e.Point, false);
            mapView.Refresh();
        }


        async void AddCarer_Clicked(object sender, System.EventArgs e)
        {
            string name = await DisplayPromptAsync("Name of Carer", "Please enter the name of the carer below");
            string action = await DisplayActionSheet("Type of Carer", "Cancel",null,"Relative", "Friend", "Professional");
            string time = await DisplayPromptAsync("Time available", "Please enter the times the carer is available below");
            await DisplayAlert("Image", "Please select an image to represent the carer.", "OK");

            FileResult photo = null;
            try
            {
                photo = await MediaPicker.PickPhotoAsync();
                
            }

            catch (Exception ex)
            {
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }


            if (name != null && time != null && !action.Equals("Cancel"))
            {
 
                var icon = photo.OpenReadAsync();
                MemoryStream ma;
                MemoryStream ImageStream = new MemoryStream();


                using (MemoryStream ms = new MemoryStream())
                {
                    icon.Result.CopyTo(ms);
                    ma = ms;
                }
                
                CarerManagerSingleton.Instance.AddNewCarer(name, action, time, ma.ToArray());

                AddPin(name, ma.ToArray());
            }

        }

         void AddPin(string name, byte[] image)
        {
            var pin = new Mapsui.UI.Forms.Pin(mapView)
            {
                Label = name.ToString(),
                Type = Mapsui.UI.Forms.PinType.Icon,
                Position = new Mapsui.UI.Forms.Position(mapView.MyLocationLayer.MyLocation.Latitude, mapView.MyLocationLayer.MyLocation.Longitude),
                Icon = image,
                Scale = 0.04f
            };

            pin.Callout.CalloutClicked += async(s, e) =>
            {
                var marker = e.Callout.Pin;

                var carer = CarerManagerSingleton.Instance.GetCarer(marker.Label);

                await DisplayAlert("Carer Info", "Name: " + carer.carerName + "\n" + "Carer type: " + carer.careLevel + "\n" + "Availabilty: " + carer.carerTime, "OK");
                pin.ShowCallout();
            };

            mapView.Pins.Add(pin);
            pin.ShowCallout();

        }

        async void AuthorizeTwitter()
        {
            var appCredentials = new TwitterClient("cBpRVUldNZjRZe3TL2ZAEwwZ5", "B83oc4cAX68u037ZLzwDKlPQBb5bpj9vFtEd7sgyqoRZ5VDmnV");


            var authenticationRequest = await appCredentials.Auth.RequestAuthenticationUrlAsync();

            /*  OAuth2Authenticator auth = new OAuth2Authenticator
              (
              clientId: "User",
              scope: "",
              authorizeUrl: new Uri(authenticationRequest.AuthorizationURL),
              null,
              null,
              true
              );

              auth.Completed += (sender, eventArgs) =>
              {
                  this.Finish();
              }*/



            try
            {
                await Browser.OpenAsync(authenticationRequest.AuthorizationURL, BrowserLaunchMode.SystemPreferred);

                string pin = await DisplayPromptAsync("Authentication", "Please enter the given pin");

                var userCredentials = await appCredentials.Auth.RequestCredentialsFromVerifierCodeAsync(pin, authenticationRequest);

                userClient = new TwitterClient(userCredentials);
                var user = await userClient.Users.GetAuthenticatedUserAsync();
            }

            catch
            {
                await DisplayAlert("Error", "Unable to login into twitter, please try again", "OK");
            }

        }


    }
}
