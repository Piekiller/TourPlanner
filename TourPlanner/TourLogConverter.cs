using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TourPlanner.Models;

namespace TourPlanner
{
    public class TourLogConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts a List of TourLogs and a Tour to the TourLogs of the Tour
        /// </summary>
        /// <param name="values">values are List of Tourlogs and then the SelectedTour</param>
        /// <param name="targetType">TargetType of the Function</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values[0] is null || values[1] is null)
                return null;
            ObservableCollection<TourLog> tourLogs = (ObservableCollection<TourLog>)values[0];
            Tour SelectedTour = (Tour)values[1];
            return tourLogs.Where(tl => tl.Tour.Id == SelectedTour.Id);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
