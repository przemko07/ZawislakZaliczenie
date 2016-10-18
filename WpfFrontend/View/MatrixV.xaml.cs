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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfFrontend.ViewModel;

namespace WpfFrontend.View
{
    /// <summary>
    /// Interaction logic for MatrixV.xaml
    /// </summary>
    public partial class MatrixV
    : UserControl
    , INotifyPropertyChanged
    {
        public MatrixVM Matrix
        {
            get { return (MatrixVM)GetValue(MatrixProperty); }
            set { SetValue(MatrixProperty, value); }
        }
        public static readonly DependencyProperty MatrixProperty =
            DependencyProperty.Register("Matrix", typeof(MatrixVM), typeof(MatrixV), new PropertyMetadata(null, MatrixChangedCallback));
        private static void MatrixChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            MatrixV view = d as MatrixV;
            if (view == null) return;

            view.OnPropertyChanged(nameof(view.Matrix));
        }

        public bool Editable
        {
            get { return (bool)GetValue(EditableProperty); }
            set { SetValue(EditableProperty, value); }
        }
        public static readonly DependencyProperty EditableProperty =
            DependencyProperty.Register("Editable", typeof(bool), typeof(MatrixV), new PropertyMetadata(true, EditableChangedCallback));
        private static void EditableChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            MatrixV view = d as MatrixV;
            if (view == null) return;

            view.OnPropertyChanged(nameof(view.Editable));
        }

        public bool CopyByDiagonal
        {
            get { return (bool)GetValue(CopyByDiagonalProperty); }
            set { SetValue(CopyByDiagonalProperty, value); }
        }
        public static readonly DependencyProperty CopyByDiagonalProperty =
            DependencyProperty.Register("CopyByDiagonal", typeof(bool), typeof(MatrixV), new PropertyMetadata(true, CopyByDiagonalChangedCallback));
        private static void CopyByDiagonalChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            MatrixV view = d as MatrixV;
            if (view == null) return;

            view.OnPropertyChanged(nameof(view.CopyByDiagonal));
            if (view.Matrix != null) view.Matrix.CopyByDiagonal = view.CopyByDiagonal;
        }

        
        public MatrixV()
        {
            InitializeComponent();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
