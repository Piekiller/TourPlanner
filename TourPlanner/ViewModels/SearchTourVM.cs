using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer.TourFactory;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class SearchTourVM:ViewModelBase
    {
        public RelayCommand SearchCommand { get; private set; }
        public string SearchTerm { get; set; } = "Search";
        public bool CaseSensitve { get; set; } = false;
        public IEnumerable<Tour> Results { get; set; }
        public SearchTourVM(List<Tour> Tours)
        {
            SearchCommand = new(Search);
            Results = Tours;
        }
        private void Search()
        {
            //Results = TourFactory.GetInstance().Search(SearchTerm, CaseSensitve);
            SearchTerm = "Search";
        }
    }
}
