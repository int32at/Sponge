using System;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sponge.Models;

namespace Sponge.Tests
{
    [TestClass]
    public class SPManagerTests
    {
        [TestMethod]
        public void CreateListInWeb()
        {
            using (var mgr = new SPManager("http://dev"))
            {
                var listName = "MyList";

                //check if list exists, if it does, delete it
                if (mgr.Lists.Exists(listName))
                    mgr.Lists.Delete(listName);

                //recreate list and get the reference to the SPList object
                var list = mgr.Lists.Create(listName, "A Sample List", SPListTemplateType.GenericList);

                var exists = mgr.Lists.Exists(listName);
                Assert.AreEqual(true, exists);
            }
        }

        [TestMethod]
        public void CreateWeb()
        {
            using (var mgr = new SPManager("http://dev"))
            {
                mgr.Webs.Create("My Awesome Web", "", "awesomeweb", "STS#0");
            }
        }

        [TestMethod]
        public void CreateWebAndCreateList()
        {
            //open the manager to spsite http://dev
            using (var mgr = new SPManager("http://dev"))
            {
                string subWebUrl = "awesomeweb";

                //make sure that the sub web does not exist
                if(mgr.Webs.Exists(subWebUrl))
                    mgr.Webs.Delete(subWebUrl);

                //create sub web http://dev/awesomeweb
                using (var awesome = mgr.Webs.Create("My Awesome Web", "", "awesomeweb", "STS#0"))
                {
                    //change the SPManager reference to the sub web
                    mgr.Change(awesome);

                    //create list in sub web
                    var list = mgr.Lists.Create("MyList", "", SPListTemplateType.GenericList);

                    //make changes to the list
                    list.OnQuickLaunch = true;
                    list.Update();
                }
            }
        }
    }
}
