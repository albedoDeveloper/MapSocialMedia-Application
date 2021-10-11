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

    class Carer
    {
        public CareLevel careLevel { get; set; }
        public string carerName { get; set; }

        public string carerTime { get; set; }
        
        public Carer(string carerName, CareLevel careLevel, string carerTime)
        {
            this.carerName = carerName;
            this.careLevel = careLevel;
            this.carerTime = carerTime;
        }
    }
}
