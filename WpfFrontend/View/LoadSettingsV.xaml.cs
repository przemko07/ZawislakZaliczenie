using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfFrontend.Extensions;
using WpfFrontend.Model;

namespace WpfFrontend.View
{
    /// <summary>
    /// Interaction logic for LoadSettingsV.xaml
    /// </summary>
    public partial class LoadSettingsV
    : Window
    , INotifyPropertyChanged
    {
        public LoadSettingsV()
        {
            InitializeComponent();
            DataContext = this;
        }

        private bool _LoadMatricies = true;
        public bool LoadMatricies
        {
            get { return _LoadMatricies; }
            set
            {
                _LoadMatricies = value;
                OnPropertyChanged(nameof(LoadMatricies));
            }
        }

        private bool _LoadPopSize = false;
        public bool LoadPopSize
        {
            get { return _LoadPopSize; }
            set
            {
                _LoadPopSize = value;
                OnPropertyChanged(nameof(LoadPopSize));
            }
        }

        private bool _LoadIndividuals = false;
        public bool LoadIndividuals
        {
            get { return _LoadIndividuals; }
            set
            {
                _LoadIndividuals = value;
                OnPropertyChanged(nameof(LoadIndividuals));
            }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public Engine engine;

        private void LoadSettingsFile()
        {
            SettingsXml s = SettingsXml.Load(Path);
            if (s.F1.Empty) throw new Exception("F1 matrix empty");
            if (s.F2.Empty) throw new Exception("F2 matrix empty");
            if (s.F1.Cols != s.F1.Rows) throw new Exception("F1 matrix must be a square matrix");
            if (s.F2.Cols != s.F2.Rows) throw new Exception("F2 matrix must be a square matrix");
            if (s.F1.Cols != s.F2.Cols) throw new Exception("F1 must be the same size as F2");

            uint oldCols = engine.Matrix1.Cols;

            if (LoadMatricies)
            {
                engine.Matrix1 = s.F1;
                engine.Matrix2 = s.F2;
                engine.NodesCount = s.F1.Cols;
                engine.Mutation = s.Mutation;
            }

            if (LoadPopSize)
            {
                engine.IndividualsLength = s.PopSize;
                engine.ReCreateEvolutionary();
            }
            else if (LoadIndividuals)
            {
                engine.Evolutionary.Individuals = s.Individuals;
            }
            else
            {
                if (oldCols != s.F1.Cols)
                {
                    throw new Exception("Cannot leave old population, matricies size are different");
                }

                var copy = engine.Evolutionary.Individuals.ToArray();
                engine.ReCreateEvolutionary();
                engine.Evolutionary.Individuals = copy;
            }
        }

        public ActionCommand Browse
        {
            get
            {
                return new ActionCommand(() =>
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Graphs settings (*.xml)|*.xml|All Files (*.*)|*.*";
                    if (ofd.ShowDialog() == true && ofd.FileName != string.Empty)
                    {
                        Path = ofd.FileName;
                    }
                });
            }
        }

        public ActionCommand CancelCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    Close();
                });
            }
        }

        public Action OnLoadCommand = null;
        public ActionCommand LoadCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    try
                    {
                        LoadSettingsFile();
                        OnLoadCommand?.Invoke();
                        Close();
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.ToString());
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
