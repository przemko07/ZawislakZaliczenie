using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfFrontend.Extensions;

namespace WpfFrontend.Controls
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        private Exception _exception = null;
        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get
            {
                if (_exception != null) return _exception.ToString();
                if (_errorMessage != null) return _errorMessage;

                return "Unknown error";
            }
        }

        public ActionCommand CloseCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.Close();
                    });
                });
            }
        }


        public ErrorWindow(Exception exception)
        {
            this._exception = exception;

            InitializeComponent();
        }

        public ErrorWindow(string errorMessage)
        {
            _errorMessage = errorMessage;

            InitializeComponent();
        }
    }
}
