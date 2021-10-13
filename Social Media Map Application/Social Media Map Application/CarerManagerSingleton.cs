using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft;
using Xamarin.Forms;

namespace Social_Media_Map_Application
{
    class CarerManagerSingleton
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


        public void AddNewCarer(string name, string levelOfCare, string carerTime, byte[] image)
        {
            CareLevel careLevel = CareLevel.Professional;

            if (levelOfCare.Equals("Relative"))
                careLevel = CareLevel.Relative;

            else if (levelOfCare.Equals("Friend"))
                careLevel = CareLevel.Friend;

            listOfCarers.Add(name, new Carer(name, careLevel, carerTime, image));
            SaveCarersToJSON();
        }

        public Carer GetCarer(string key)
        {
            return listOfCarers[key];
        }

        public Dictionary<string, Carer> GetAllCarers()
        {
            return listOfCarers;
        }

        public void SaveCarersToJSON()
        {
            string documentsPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            using (StreamWriter file = File.CreateText(documentsPath + "/SavedCarers.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, listOfCarers);
            }

        }

        public bool LoadCarersFromJSON()
        {
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
