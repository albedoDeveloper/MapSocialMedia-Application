using System;
using System.Collections.Generic;
using System.Text;

namespace Social_Media_Map_Application
{
    public abstract class SocialMediaType
    {
        virtual public bool AuthorizeSocialMedia()
        {

            return true;

        }
    
    }
}
