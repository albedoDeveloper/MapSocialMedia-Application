using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace Social_Media_Map_Application
{
    public class SocialMediaManagerSingleton
    {
        private static readonly SocialMediaManagerSingleton instance = new SocialMediaManagerSingleton();


        private SocialMediaManagerSingleton()
        {

        }


        public static SocialMediaManagerSingleton Instance
        {
            get
            {
                return instance;
            }
        }


        public Dictionary<string,SocialMediaType> allSocialMedia = new Dictionary<string,SocialMediaType>();
        
        /// <summary>
        /// Add a new social media
        /// </summary>
        /// <param name="type"></param>
        public void AddNewSocialMedia(string type)
        {
            if(type == "Twitter")
            {
                allSocialMedia.Add("Twitter", new Twitter());
                allSocialMedia["Twitter"].AuthorizeSocialMedia();
            }

            else if (type == "Facebook")
            {
                allSocialMedia.Add("Facebook", new Facebook()); // Not implemented, purely here for unit testing
            }
        }

        /// <summary>
        /// Post the message to the users selected
        /// </summary>
        /// <param name="type"></param>
        public void PostNewMessage(string type)
        {
            if(allSocialMedia.ContainsKey(type))
            {
                if (!allSocialMedia[type].isAuthorized)
                    allSocialMedia.Remove(type);
            }

            if (!allSocialMedia.ContainsKey(type))
                AddNewSocialMedia(type);
            else
                allSocialMedia[type].PostMessage();
        }

        /// <summary>
        /// Read all the messages the user has posted
        /// </summary>
        /// <param name="type"></param>
        public void ReadAllMessages(string type)
        {
            if (allSocialMedia.ContainsKey(type))
            {
                if (!allSocialMedia[type].isAuthorized)
                    allSocialMedia.Remove(type);
            }

            if (!allSocialMedia.ContainsKey(type))
                AddNewSocialMedia(type);
            else
                allSocialMedia[type].ReadAllMessages();
        }
    }
}
