﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TSP.ViewModel;
using TSP.Controls;
using TSP.View;
using Model;
using TSP.Model;
using System.Xml.Serialization;
using System.IO;

namespace TSP
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            MainWindow mainWindow = null;
            try
            {
                Application app = new Application();

                MainWindowVM context = new MainWindowVM();

                mainWindow = new MainWindow(context);

                app.Run(mainWindow);
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
            finally
            {
                mainWindow?.Close();
            }
        }
    }
}
