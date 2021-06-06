using System.Windows;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TourPlanner.Views
{
    /// <summary>
    /// Interaktionslogik für TourPlannerWindow.xaml
    /// </summary>
    public partial class TourPlannerWindow : Window
    {
        public TourPlannerWindow()
        {
            InitializeComponent();
        }
    }
}
