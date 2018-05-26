using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DataGridHorizontal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            ListSymbol.Clear();

            for (int i = 32; i <= 126; i++)
            {
                ListSymbol.Add(Char.ConvertFromUtf32(i).ToString());
            }

            for (int i = 161; i <= 255; i++)
            {
                ListSymbol.Add(Char.ConvertFromUtf32(i).ToString());
            }

            DataContext = this;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Lista de columnas para los simbolos
        /// </summary>
        public ObservableCollection<string> ListSymbol { get; set; } = new ObservableCollection<string>();

        #endregion Public Properties


        #region Private Properties

        /// <summary>
        /// String que almacena el código que se va a insertar en el editor
        /// </summary>
        private string _lineExpression;

        #endregion Private Properties

        #region Events
        private void dg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGridCellInfo cellInfo = dg.CurrentCell;

            if (cellInfo != null)
            {
                DataGridBoundColumn column = cellInfo.Column as DataGridBoundColumn;
                if (column != null)
                {
                    FrameworkElement element = new FrameworkElement() { DataContext = cellInfo.Item };
                    BindingOperations.SetBinding(element, FrameworkElement.TagProperty, column.Binding);
                    string cellValue = element.Tag as string;
                    if (cellValue != null)
                    {
                        _lineExpression = cellValue;
                        MessageBox.Show(_lineExpression);
                    }
                }
            }
            
        }


        #endregion

    }
}
