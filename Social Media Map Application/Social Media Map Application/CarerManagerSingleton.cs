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
    public class CarerManagerSingleton
    {

        private static readonly CarerManagerSingleton instance = new CarerManagerSingleton();


        private CarerManagerSingleton()
        {

        }


        public static CarerManagerSingleton Instance
        {
            get
            {
                return instance;
            }
        }
        private Dictionary<string,Carer> listOfCarers = new Dictionary<string, Carer>();

        /// <summary>
        /// Add a new carer at the location
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<string> AddNewCarer(double latitude, double longitude)
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Name of Carer", "Please enter the name of the carer below");

            if (name != null)
            {
                string action = await Application.Current.MainPage.DisplayActionSheet("Type of Carer", "Cancel", null, "Relative", "Friend", "Professional");
                string time = await Application.Current.MainPage.DisplayPromptAsync("Time available", "Please enter the times the carer is available below");
                var i = await Application.Current.MainPage.DisplayAlert("Image", "Select an image to represent the carer.", "Yes","No");

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
                    MemoryStream ma;
                    if (i)
                    {
                        var icon = photo.OpenReadAsync();
                      
                        MemoryStream ImageStream = new MemoryStream();


                        using (MemoryStream ms = new MemoryStream())
                        {
                            icon.Result.CopyTo(ms);
                            ma = ms;
                        }
                        AddCarerToDictionary(name, action, time, latitude, longitude, ma.ToArray());
                    }

                    else
                    {
                        AddCarerToDictionary(name, action, time, latitude, longitude, new byte[1]);
                    }


                }

                else
                    name = null;
            }

            return name;
        }

        /// <summary>
        /// Add that new carer to a the list of caters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="levelOfCare"></param>
        /// <param name="carerTime"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="image"></param>
        public void AddCarerToDictionary(string name, string levelOfCare, string carerTime, double latitude, double longitude, byte[] image)
        {
            CareLevel careLevel = CareLevel.Professional;

            if (levelOfCare.Equals("Relative"))
                careLevel = CareLevel.Relative;

            else if (levelOfCare.Equals("Friend"))
                careLevel = CareLevel.Friend;

            listOfCarers.Add(name, new Carer(name, careLevel, carerTime, latitude, longitude, image));
            SaveCarersToJSON();
        }

        /// <summary>
        /// Get a particular carer
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Carer GetCarer(string key)
        {
            return listOfCarers[key];
        }


        /// <summary>
        /// Get all carers
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Carer> GetAllCarers()
        {
            return listOfCarers;
        }

        /// <summary>
        /// Delete a carer from the list
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCarer (string name)
        {
            if(listOfCarers.ContainsKey(name))
                listOfCarers.Remove(name);

            SaveCarersToJSON();
        }

        /// <summary>
        /// Save the list of carers
        /// </summary>
        public void SaveCarersToJSON()
        {
            string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            using (StreamWriter file = File.CreateText(documentsPath + "/SavedCarers.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, listOfCarers);
            }

        }

        /// <summary>
        /// Load the list of carers
        /// </summary>
        /// <returns></returns>
        public bool LoadCarersFromJSON()
        {
            //SaveCarersToJSON();
            string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            try
            {
                using (StreamReader file = File.OpenText(documentsPath + "/SavedCarers.json"))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    listOfCarers = (Dictionary<string, Carer>)serializer.Deserialize(file, typeof(Dictionary<string, Carer>));
                }
               
                return true;
            }
           
            catch(FileNotFoundException)
            {
                return false;

            }
        }

    }
}
