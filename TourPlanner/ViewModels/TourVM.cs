using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class TourVM:ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Route_Information { get; set; } 
        public double Distance { get; set; }
        public AsyncCommand AddTourCommand { get; private set; } 
        public IDataAccess DataAccess { get; private set; }
        public TourVM()
        {
            AddTourCommand = new AsyncCommand(AddTour);
            DataAccess = new InMemoryDB();
        }
        public async Task AddTour()
        {
            Tour t = new Tour(this.Name, this.Description, this.Route_Information, this.Distance);
            DataAccess.SaveTour(t);
        }
    }
}
