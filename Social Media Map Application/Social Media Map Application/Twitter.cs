using System;
using System.Collections.Generic;
using System.Text;

namespace Social_Media_Map_Application
{
    class Twitter : SocialMediaType
    {
        public Twitter()
        {


        }
        

        public override async bool AuthorizeSocialMedia()
        {

            var appCredentials = new TwitterClient("cBpRVUldNZjRZe3TL2ZAEwwZ5", "B83oc4cAX68u037ZLzwDKlPQBb5bpj9vFtEd7sgyqoRZ5VDmnV");
            var authenticationRequest = await appCredentials.Auth.RequestAuthenticationUrlAsync();

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
}
