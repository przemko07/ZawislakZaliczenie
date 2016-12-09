using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfFrontend.ViewModel;
using WpfFrontend.Controls;
using WpfFrontend.View;

namespace WpfFrontend
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                _Main(args);
            }
            catch (Exception e)
            {
                try
                {
                    ErrorWindow errorW = new ErrorWindow(e);
                    errorW.ShowDialog();
                }
                catch (Exception e2)
                {
                    Trace.WriteLine("Double exception thrown, while showing error");
                    Trace.WriteLine(e);
                    Trace.WriteLine(e2);
                }
            }
        }

        private static void _Main(string[] args)
        {
            Application app = new Application();

            MainWindowVM context = new MainWindowVM();

            MainWindow mainWindow = new MainWindow(context);

            app.Run(mainWindow);
        }
    }
}
