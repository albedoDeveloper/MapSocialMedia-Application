using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin;
using System.IO;
using System.Threading.Tasks;

namespace Social_Media_Map_Application
{
    class Twitter : SocialMediaType
    {
        private Tweetinvi.TwitterClient userClient;
        

        public Twitter()
        {


        }
        /// <summary>
        /// Authroize the users twitter account
        /// </summary>
        public override async void AuthorizeSocialMedia()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var appCredentials = new TwitterClient("cBpRVUldNZjRZe3TL2ZAEwwZ5", "B83oc4cAX68u037ZLzwDKlPQBb5bpj9vFtEd7sgyqoRZ5VDmnV");
            var authenticationRequest = await appCredentials.Auth.RequestAuthenticationUrlAsync();

            await App.Current.MainPage.DisplayAlert("Authorization", "Please authorize the app by inputting the pin given to by Twitter, then retry the button again", "OK");
            
            try
            {
                await Browser.OpenAsync(authenticationRequest.AuthorizationURL, BrowserLaunchMode.SystemPreferred);

                string pin = await Application.Current.MainPage.DisplayPromptAsync("Authentication", "Please enter the given pin");

                if (pin != null)
                {
                    var userCredentials = await appCredentials.Auth.RequestCredentialsFromVerifierCodeAsync(pin, authenticationRequest);

                    if (userCredentials != null)
                    {
                        userClient = new TwitterClient(userCredentials);
                        var user = await userClient.Users.GetAuthenticatedUserAsync();
                        isAuthorized = true;
                    }
                }
            }

            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to login into twitter, please try again", "OK");
            }

        }

        /// <summary>
        /// Read all messages that the user has tweeted
        /// </summary>
        public override async void ReadAllMessages()
        {
            var user = await userClient.Users.GetAuthenticatedUserAsync();
            var url = "https://twitter.com/" + user.ToString();
            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }

        /// <summary>
        /// Tweet a message
        /// </summary>
        public override async void PostMessage()
        {

            string result = await App.Current.MainPage.DisplayPromptAsync("Tweet", "Please enter your tweet below");

            if (result != null)
            {
                bool answer = await App.Current.MainPage.DisplayAlert("Tweet", "Would you like to also upload an image?", "Yes", "No");

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
}
