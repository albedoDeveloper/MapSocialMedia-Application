using System;
using System.Collections.Generic;
using System.Text;

namespace Social_Media_Map_Application
{
    public enum CareLevel
    {
        Relative,
        Friend,
        Professional

    };

    public class Carer
    {
        public CareLevel careLevel { get; set; }
        public string carerName { get; set; }

        public string carerTime { get; set; }

        public double latitude { get; set; }
        
        public double longitude { get; set; }

        public byte[] image{ get; set; }
       
        public Carer(string carerName, CareLevel careLevel, string carerTime, double latitude, double longitude, byte[] image)
        {
            this.carerName = carerName;
            this.careLevel = careLevel;
            this.latitude = latitude;
            this.longitude = longitude;
            this.carerTime = carerTime;
            this.image = image;
        }
    }
}
