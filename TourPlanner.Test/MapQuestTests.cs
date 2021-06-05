using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer.MapQuest;

namespace TourPlanner.Test
{
    class MapQuestTests
    {
        [Test]
        public async Task Test_Get_Route_with_bad_Parameters()
        {
            Route route = await MapQuest.GetRoute(null, "");
            Assert.IsNull(route.sessionID);
        }
        [Test]
        public async Task Test_download_image()
        {
            Route route = await MapQuest.GetRoute("Wien", "Tirol");
            string id = await MapQuest.SaveImage(route);
            string p = "F:\\Studium\\SWE\\TourPlanner\\TourPlanner.Test\\bin\\Debug\\net5.0-windows\\Images\\" + id + ".jpg";
            Assert.IsTrue(File.Exists(p));
        }
        [Test]
        public async Task Test_Save_Image_with_bad_Parameters()
        {
            Assert.IsNull(await MapQuest.SaveImage(new Route()));
        }

    }
}
