using System;
using Xunit;
using Social_Media_Map_Application;
namespace TestProject
{
    public class UnitTest1
    {
        /// <summary>
        /// Checks to see if a carer can be added to the carer list
        /// </summary>
        [Fact] 
        public void AddCarer()
        {
            
            Carer carer = new Carer("name", CareLevel.Friend, "9 - 9", 2, 2, new byte[] { 1 });
            CarerManagerSingleton.Instance.GetAllCarers().Add("name",carer);
            
            Assert.True(CarerManagerSingleton.Instance.GetCarer("name").carerName.Equals("name"), "Inncorrect name");

        }

        /// <summary>
        /// Checks to see if a carer can be deleted from the carer list
        /// </summary>
        [Fact]
        public void DeleteCarer()
        {
            Carer carer = new Carer("name", CareLevel.Friend, "9 - 9", 2, 2, new byte[] { 1 });
            CarerManagerSingleton.Instance.GetAllCarers().Add("name", carer);

            CarerManagerSingleton.Instance.DeleteCarer("name");

            Assert.True(!CarerManagerSingleton.Instance.GetAllCarers().ContainsKey("name"), "Key not removed");

        }

        /// <summary>
        /// Checks to see if a social media can be added to the program
        /// </summary>
        [Fact]
        public void AddSocial()
        {
            SocialMediaManagerSingleton.Instance.PostNewMessage("Facebook");

            Assert.True(!SocialMediaManagerSingleton.Instance.allSocialMedia["Facebook"].isAuthorized, "Cant add new social");

        }
        /// <summary>
        /// Check to see if a social media can be read
        /// </summary>
        [Fact]
        public void ReadSocial()
        {
            SocialMediaManagerSingleton.Instance.ReadAllMessages("Facebook");

            Assert.True(!SocialMediaManagerSingleton.Instance.allSocialMedia["Facebook"].isAuthorized, "Cant add new social");

        }


    }
}
