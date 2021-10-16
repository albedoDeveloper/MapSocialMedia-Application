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
    public abstract class SocialMediaType
    {

        public bool isAuthorized { get; set; } = false;
        
        abstract public void AuthorizeSocialMedia();

        abstract public void PostMessage();

        abstract public void ReadAllMessages();

        protected async Task<bool> DisplayAlert(string one, string two)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            await Application.Current.MainPage.DisplayAlert(one, two, "Cancel");
            return tcs.Task.Result;
        }

    }
}
