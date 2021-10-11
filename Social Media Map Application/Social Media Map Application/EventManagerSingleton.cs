using System;
using System.Collections.Generic;
using System.Text;

namespace Social_Media_Map_Application
{
    class EventManagerSingleton
    {

        private static readonly EventManagerSingleton instance = new EventManagerSingleton();


        private EventManagerSingleton()
        {

        }


        public static EventManagerSingleton Instance
        {
            get
            {
                return instance;
            }
        }
        private Dictionary<string,Carer> listOfCarers = new Dictionary<string, Carer>();


        public void AddNewCarer(string name, string levelOfCare, string carerTime)
        {
            CareLevel careLevel = CareLevel.Professional;

            if (levelOfCare.Equals("Relative"))
                careLevel = CareLevel.Relative;

            else if (levelOfCare.Equals("Friend"))
                careLevel = CareLevel.Friend;

            listOfCarers.Add(name, new Carer(name, careLevel, carerTime));
            
        }

        public Carer GetCarer(string key)
        {
            return listOfCarers[key];
        }
    }
}
