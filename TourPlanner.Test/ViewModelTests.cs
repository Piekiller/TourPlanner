using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    class ViewModelTests
    {
        [Test]
        public void Test_Add_TourLog_Valid()
        {
            TourPlannerVM tourPlannerVm = new();
            tourPlannerVm.SelectedTour = new();
            tourPlannerVm.SaveNewTourLog(new());
            Assert.AreEqual(tourPlannerVm.SelectedTour.Logs.Count, 1);
        }
        [Test]
        public void Test_Add_TourLog_Invalid_TourLog_Null()
        {
            TourPlannerVM tourPlannerVm = new();
            tourPlannerVm.SelectedTour = new();
            tourPlannerVm.SaveNewTourLog(null);
            Assert.AreEqual(tourPlannerVm.SelectedTour.Logs.Count, 0);
        }
        [Test]
        public void Test_Add_TourLog_Invalid_Tour_Null()
        {
            TourPlannerVM tourPlannerVm = new();
            tourPlannerVm.SelectedTour = null;
            Assert.DoesNotThrow(()=>tourPlannerVm.SaveNewTourLog(new()));
        }
        [Test]
        public void Test_Add_Tour_Valid()
        {
            TourPlannerVM tourPlannerVm = new();
            tourPlannerVm.SaveNewTour(new());
            Assert.AreEqual(tourPlannerVm.Tours.Count, 1);
        }
        [Test]
        public void Test_Add_Tour_Invalid_Null()
        {
            TourPlannerVM tourPlannerVm = new();
            tourPlannerVm.SaveNewTour(null);
            Assert.AreEqual(tourPlannerVm.Tours.Count, 0);
        }
    }
}
