using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlanner.BusinessLayer;
//[assembly: log4net.Config.XmlConfigurator(Watch = true,ConfigFile="TourPlanner.BusinessLayer.dll.config")]

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            ConfigLoader.LoadConfig();//Could be solved with using a static ctor, but seems to be bad practice
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("App.config"));Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date{ABSOLUTE} [%logger] %level -%message%newline%exception";
            patternLayout.ActivateOptions();

            FileAppender roller = new FileAppender();
            roller.AppendToFile = true;
            roller.File = @"Logs\EventLog.txt";
            roller.Layout = patternLayout;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
            InitializeComponent();
        }
    }
}
