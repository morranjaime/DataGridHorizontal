using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataGridHorizontal
{
    /// <summary>
    /// Lógica de interacción para DataGridCustom.xaml
    /// </summary>
    public partial class DataGridCustom : DataGrid
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public DataGridCustom()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Private Properties

        /// <summary>
        ///  Número de columnas a mostrar
        /// </summary>
        private int _numberColumns;

        /// <summary>
        /// Letra de comienzo para el binding
        /// </summary>
        private string _letter = "A";

        /// <summary>
        /// Objeto dinamico
        /// </summary>
        private ExpandoObject _obj = null;

        /// <summary>
        /// Lista para el ItemSource
        /// </summary>
        private ObservableCollection<IDictionary<string, object>> _listItemSource = new ObservableCollection<IDictionary<string, object>>();

        /// <summary>
        /// DatagridTextColum
        /// </summary>
        private DataGridTextColumn column = null;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Dependencia de Bindeo
        /// </summary>
        public static readonly DependencyProperty LoadItemsProperty = DependencyProperty.Register("LoadItems", typeof(IEnumerable), typeof(DataGridCustom), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));

        /// <summary>
        /// Evento OnItemsSourceChanged
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridCustom dataGridCustom = d as DataGridCustom;
            dataGridCustom.loadDataGrid();
        }

        /// <summary>
        /// Colección de objetos del datagrid.
        /// </summary>
        public IEnumerable LoadItems
        {
            get
            {
                return (IEnumerable)GetValue(LoadItemsProperty);
            }
            set
            {
                SetValue(LoadItemsProperty, value);
            }
        }

        /// <summary>
        /// Número de columnas a mostrar
        /// </summary>
        public int NumberColumns
        {
            get
            {
                return _numberColumns;
            }

            set
            {
                _numberColumns = value;
            }
        }

        #endregion Public Properties

        #region Private Method

        /// <summary>
        /// Metodo que carga el datagrid con las columnas predefinidas en NumColumns
        /// </summary>
        private void loadDataGrid()
        {
            List<object> listAux = LoadItems.OfType<object>().ToList();

            var s = LoadItems.OfType<object>().ToList().ElementAt(107);

            // Creamos la columnas con su binding
            for (int i = 0; i < _numberColumns; i++)
            {
                actIndText(ref _letter);

                ///Generamos el DatagridColumn con las propiedades
                column = new DataGridTextColumn();
                Binding binding = new Binding(_letter);
                binding.Mode = BindingMode.TwoWay;
                column.Binding = binding;
                column.IsReadOnly = true;

                dg.Columns.Add(column);
            }

            //Generamos el objeto con sus propiedades
            while (listAux.Count > 0)
            {
                int _numColumnsAux = _numberColumns;
                if (listAux.Count < _numberColumns)
                {
                    _numColumnsAux = listAux.Count;
                }

                ConvertExpandObject(listAux.Take(_numColumnsAux));
                listAux.RemoveRange(0, _numColumnsAux);

                _listItemSource.Add(_obj);
            }
            dg.ItemsSource = _listItemSource;
        }

        /// <summary>
        /// Metodo para convertir a ExpandObject
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private IDictionary<string, object> ConvertExpandObject(IEnumerable values)
        {
            _letter = "A";
            _obj = new ExpandoObject();

            foreach (string item in values)
            {
                actIndText(ref _letter);
                addProperty(_obj, _letter, item);
            }

            return _obj;
        }

        /// <summary>
        /// Evento que aumenta un caracter en uno
        /// </summary>
        /// <param name="ind"></param>
        public void actIndText(ref string ind)
        {
            string b = ind;
            if (b.Length.Equals(1))
            {
                if (b[0].Equals('Z'))
                {
                    b = "AA";
                }
                else
                {
                    char aux = b[0];
                    b = ((char)(Convert.ToUInt16(aux) + 1)).ToString();
                }
            }
            else
            {
                if (b[1].Equals('Z'))
                {
                    char aux = b[0];
                    b = ((char)(Convert.ToUInt16(aux) + 1)).ToString();
                    b += "A";
                }
                else
                {
                    string b0 = b[0].ToString();
                    char aux = b[1];
                    string b1 = ((char)(Convert.ToUInt16(aux) + 1)).ToString();
                    b = b0 + b1;
                }
            }
            ind = b;
        }

        /// <summary>
        /// Metodo para añadir una propiedad al objecto
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public void addProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
            {
                expandoDict[propertyName] = propertyValue;
            }
            else
            {
                expandoDict.Add(propertyName, propertyValue);
            }
        }

        #endregion Private Method
    }
}