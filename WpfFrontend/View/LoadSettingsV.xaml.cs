using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public void LoadSettingsFile()
        {
            SettingsXml s = null;
            try
            {
                s = SettingsXml.Load(Path);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error during loading xml->{e}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (LoadMatricies)
            {
                engine.Matrix1 = s.F1;
                engine.Matrix2 = s.F2;
                engine.NodesCount = s.F1.Cols;
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
                if (engine.Matrix1.Cols != s.F1.Cols)
                {
                    throw new Exception("Cannot leave old population, NodesCount changed.");
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
                    LoadSettingsFile();
                    OnLoadCommand?.Invoke();
                    Close();
                });
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
