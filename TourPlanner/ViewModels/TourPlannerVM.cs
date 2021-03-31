using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.ComponentModel;
using TourPlanner.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM:ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; private set; } = new ObservableCollection<Tour>();
        public ICommand AddCommand { get; private set; } = new RelayCommand(() =>
        {
            //Tours.Add(new Tour("", "", "", 0));
            Debug.Print("Test");

        });
        //public ICommand RemoveCommand { get; private set; } = new RelayCommand(() => Tours.RemoveAt(Tours.Count));
        public TourPlannerVM()
        {
        }
    }
}
