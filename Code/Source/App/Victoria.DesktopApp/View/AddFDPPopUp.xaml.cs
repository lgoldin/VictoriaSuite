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
using Victoria.Shared.AnalisisPrevio;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using System.Data;




namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>

    // esto va a ir en la libreria common fdp
    public class Evento : IEquatable<Evento>
    {
        public int Id { get; set; }
        public DateTime fecha { get; set; }
        public bool activo { get; set; }

        public int idOrigen { get; set; }
        public Origen origen { get; set; }
        public double vIntervalo { get; set; }


        public bool Equals(Evento other)
        {
            return this.Id == other.Id;
        }
    }

    public class Origen
    {
        public int Id { get; set; }
        public DateTime fechaCreacion { get; set; }

        public string nombreOrigen { get; set; }

        public bool activo { get; set; }

        public List<Evento> eventos { get; set; }
       
    }

    // hasta aca va a ir a la common fdp 

    public partial class AddFDPPopUp : Window
    {
        private List<Evento> eventos = new List<Evento>();
        public List<Evento> interv = new List<Evento>();
        public int idEvento = 0;
        private AnalisisPrevio analisisPrevio;
        public string dateFormat = "yyyy-MM-dd";
        public string hourFormat = "HH:mm:ss";

        public void FDPGenerator(AnalisisPrevio analisisPrevio)
        {
            this.analisisPrevio = analisisPrevio;
        }

        public AddFDPPopUp()
        {
            InitializeComponent();          

        }



        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)comboBox.SelectedItem;
            string value = typeItem.Content.ToString();
            if (value == "Archivo Excel")
            {
                Archivo.Visibility = Visibility.Visible;
                rutaFile.IsReadOnly = true;
               
            }
        }

        private void comboBoxIntervalo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private String getFileName()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xls)|*.xlsx|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                return openFile.FileName;
                
               
            }

            return null;
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCalcularFDP_OnClick(object sender, RoutedEventArgs e)
        {   
            Archivo.Visibility = Visibility.Visible; 
        }

        private void Btnserch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rutaFile.Text = getFileName();
            }
            catch
            {
                createAlertPopUp("No se pudo obtener el archivo, intente nuevamente con un formato valido");
            }

        }

        private void createAlertPopUp(string s)
        {
            GenerarAlerta(s);
        }

        private void GenerarAlerta(string s)
        { 
            AlertPopUp Alert = new AlertPopUp(s);

            Alert.Show();
        }

        private void RutaFile_TextChanged(object sender, TextChangedEventArgs e)
        {
           pnlPosicion_datos.Visibility = Visibility.Visible; 
            
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Btnimport_Click(object sender, RoutedEventArgs e)
        {
           // try
           // {
                
                dgvDatosFdp.ItemsSource = eventos;
                
                using (var archivo = new XLWorkbook(rutaFile.Text ))
                {
                    
                    int numeroFila = Convert.ToInt32(txtFila.Text);
                    int columna = Convert.ToInt32(txtCol.Text);
                //Origen nuevoOrigen = new Origen();

                try
                {
                    var hoja = archivo.Worksheet(Convert.ToInt32(txtHoja.Text));
                    idEvento++;
                    while (!hoja.Cell(numeroFila, columna).IsEmpty())
                    {
                            DateTime auxFecha = hoja.Cell(numeroFila, columna).GetDateTime();
                            eventos.Add(new Evento() { fecha = auxFecha, Id= idEvento });//, origen = nuevoOrigen, activo = true });
                            numeroFila++;
                            idEvento++;
                    }

                
                    dgvDatosFdp.Columns[0].Width = 235;
                    try
                    {
                        dgvDatosFdp.Columns[1].ClipboardContentBinding.StringFormat = "dd'/'MM'/'yyyy HH:mm:ss";
                    }
                    catch
                    {

                    }
                    dgvDatosFdp.Columns[0].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[2].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[1].Visibility = Visibility.Visible;
                    dgvDatosFdp.Columns[1].Header = "Eventos";
                    dgvDatosFdp.Columns[3].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[4].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[5].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Visibility = Visibility.Visible;
                    rbFecha.IsChecked = true;
                    pnlButtonsGrid.Visibility = Visibility.Visible;
                    dgvDatosFdp.Items.Refresh();
                }

                catch
                {
                    dgvDatosFdp.Visibility = Visibility.Hidden;
                    createAlertPopUp("El excel importado no tiene el formato correcto o no se definieron correctamente los parametros de lectura. Por favor seleccione otro archivo o verifique los parametros ingresados y vuelva a intentarlo");
                    rutaFile.Text = "";
                    pnlPosicion_datos.Visibility = Visibility.Hidden;
                    pnlButtonsGrid.Visibility = Visibility.Hidden;


                }

            }
               
                
                
            //}
            /*/catch
            {
                createAlertPopUp("No se pudo obtener los datos");
            } /*/
        }

        private void DgvDatosFdp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteSelection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnModifyRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddRegister_onClick(object sender, RoutedEventArgs e)
        {
            var tipoAccion = (int)commonFDP.commonFDP.TipoAccionProcesamiento.AGREGAR_REGISTRO;
            modificarLayout(tipoAccion);
            botonSeleccionado(addRegisterGrid);
        }

        private void RbIntervalos_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbIntervalos.IsChecked.Value)
      
                {
                    var tipoAccion =  (int)commonFDP.commonFDP.TipoAccionProcesamiento.FILTRAR; 
             
                    modificarLayout(tipoAccion);
                    
                    rbDtConstante.Visibility = Visibility.Hidden;
                    pnlSegmentacion.Visibility = Visibility.Hidden;
                    
                    rbEventoAEvento.IsChecked = true;
                    if (rbDtConstante.IsChecked.Value)
                    {
                        rbEventoAEvento.IsChecked = true;
                        pnlSegmentacion.Visibility = Visibility.Visible;
                    }

                    if (eventos != null && eventos.Count > 0)
                    {
                        //calculo intervalos
                        if (interv != null && interv.Count > 0)
                        {
                            interv.Clear();
                        }
                        
                        var eventosOrdenados = eventos.OrderBy(x => x.fecha);
                        List<double> lista = new List<double>();

                        lista = eventosOrdenados.Select(x => x.fecha.TimeOfDay)
                        .Zip(eventosOrdenados.Select(x => x.fecha)
                        .Skip(1), (x, y) => y - x)
                        .Select(x => Math.Abs(x.TimeOfDay.TotalSeconds))
                        .ToList();
                        List<double> intervalos = lista; //para limpir los filtros y volver al original
                        List<double> intervalosParciales = lista; //para los filtros consecutivos
                        
                   

                        foreach (var intervalo in  lista)
                        {
                            interv.Add(new Evento() { vIntervalo = intervalo });
                        }

                        dgvDatosFdp.ItemsSource = null;
                        dgvDatosFdp.ItemsSource = interv;
                        dgvDatosFdp.Items.Refresh();

                        dgvDatosFdp.Columns[0].Visibility = Visibility.Hidden;
                        dgvDatosFdp.Columns[2].Visibility = Visibility.Hidden;
                        dgvDatosFdp.Columns[1].Visibility = Visibility.Hidden;
                        dgvDatosFdp.Columns[3].Visibility = Visibility.Hidden;
                        dgvDatosFdp.Columns[4].Visibility = Visibility.Hidden;
                        dgvDatosFdp.Columns[5].Visibility = Visibility.Visible;
                        dgvDatosFdp.Visibility = Visibility.Visible;
                        dgvDatosFdp.Columns[5].Header = "Intervalos";


                        //dgvDatosFdp.Columns[0].Visible = true;
                        // dgvDatosFdp.Columns[0].Width = 235;

                        //deshabilito funciones
                       
                        btnAddRegister.IsEnabled = false;
                        addRegisterGrid.Background = Brushes.LightGray ;
                        btnModifyRegister.IsEnabled = false;
                        modifyRegisterGrid.Background = Brushes.LightGray;
                        btnDeleteRegisters.IsEnabled = false;
                        deleteRegistersGrid.Background = Brushes.LightGray;
                        //btnDeleteRegisters. = Color.FromArgb(134, 0, 3);
                        btnSelectAll.IsEnabled = false;
                        selectAllGrid.Background = Brushes.LightGray;
                        CheckListBoxFiltros.IsEnabled = false;
   
                        
                        // chlFiltros.Enabled = false;

                        modificarLayout((int)commonFDP.commonFDP.TipoAccionProcesamiento.FILTRAR);

                    }
                    //  ();

                }
                else
                {
;                    cargarFiltros();
                }
            }
            catch
            {
                createAlertPopUp("Error al calcular los intervalos");
            }
        }

        private void cargarEventos()
        {
            try
            {

                dgvDatosFdp.ItemsSource = eventos;
                dgvDatosFdp.Columns[0].Width = 235;
                dgvDatosFdp.Columns[1].ClipboardContentBinding.StringFormat = "dd'/'MM'/'yyyy HH:mm:ss";
                dgvDatosFdp.Columns[0].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[2].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[1].Visibility = Visibility.Visible;
                dgvDatosFdp.Columns[1].Header = "Eventos";
                dgvDatosFdp.Columns[3].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[4].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[5].Visibility = Visibility.Hidden;
                dgvDatosFdp.Visibility = Visibility.Visible;

                //if (eventos != null && tipoAccion == TipoAccionProcesamiento.FILTRAR)
                   // filtrar();
            }
            catch
            {
                createAlertPopUp("Error al cargar los eventos");
            }
        }

        private void rbFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFecha.IsChecked.Value)
            {
                //quitarFiltrosIntervalos();
                
                rbDtConstante.Visibility = Visibility.Visible;
                cargarEventos();
                cargarFiltros();
                btnAddRegister.IsEnabled = true;
                addRegisterGrid.Background = Brushes.Gray;
                btnModifyRegister.IsEnabled = true;
                modifyRegisterGrid.Background = Brushes.Gray;
                btnDeleteRegisters.IsEnabled = true;
                deleteRegistersGrid.Background = Brushes.Gray;

                //btnDeleteRegisters. = Color.FromArgb(134, 0, 3);
                btnSelectAll.IsEnabled = true;
                selectAllGrid.Background = Brushes.Gray;
                CheckListBoxFiltros.IsEnabled = true;
                //modificarLayout(tipoAccion);
                //botonSeleccionado(btnFiltrar);
                rbDtConstante.Visibility = Visibility.Visible;
                
                //actualizarEstadisticas();
            }
        }

        private void rbDtConstante_Checked(object sender, RoutedEventArgs e)
        {
            if (rbDtConstante.IsChecked.Value)
                pnlSegmentacion.Visibility= Visibility.Visible;
            else
                pnlSegmentacion.Visibility = Visibility.Hidden;
        }

        private void modificarLayout(int tipoAccion)
         {
             hacerVisible(tipoAccion);
             switch (tipoAccion)
             {
                 case 0:
                     lblTituloAccion.Content = "Agregar Registro";

                     dtp1.FormatString = dateFormat;

                     dtp2.FormatString = hourFormat;
                     pnlMoficable.Visibility = Visibility.Visible;
                     rbAgregarPorFechaYHora.IsChecked = true;
                     rbAgregarPorFechaYHora.Visibility = Visibility.Visible;
                     rbAgregarPorIntervalo.IsChecked = false;
                     rbAgregarPorIntervalo.Visibility = Visibility.Visible;
                     nudAgregarPorIntervalo.Visibility = Visibility.Hidden;
                     cbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                     //cambiarFiltrosVistaFecha(0);
                     break;
                 case 1:
                    lblTituloAccion.Content = "Modificar Registro";
                     lbldtp1.Content = "Fecha";

                     lbldtp2.Content = "Hora";

                    dtp2.FormatString = hourFormat;
                    rbAgregarPorFechaYHora.Visibility = Visibility.Hidden;
                    rbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    nudAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    cbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    // cambiarFiltrosVistaFecha(0);
                     break;
                 case 4:
                     lblTituloAccion.Content = "Filtrar";

                     dtp1.FormatString = dateFormat;
                     if (rbFecha.IsChecked.Value)
                     {     /*
                         lblAccion1.Text = "Fecha";
                         txtIntervalo.Visible = false;
                         txtIntervalo2.Visible = false;
                         cambiarFiltrosVistaFecha(0); */
                     }
                     else
                     {     
                         //lblAccion1.Text = "Intervalo";
                         dtp1.Visibility = Visibility.Hidden;
                        // txtIntervalo.Visible = true; 
                     }
                     lbldtp2.Content = "Hora";
                     dtp2.FormatString = hourFormat;
                     rbAgregarPorFechaYHora.Visibility = Visibility.Hidden;
                     rbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    nudAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    cbAgregarPorIntervalo.Visibility = Visibility.Hidden;


                    break;
                 default:
                     break;
             }
         }


        private void hacerVisible(int tipoAccion)
        {
            switch (tipoAccion)
            {
                case (int)commonFDP.commonFDP.TipoAccionProcesamiento.AGREGAR_REGISTRO:
                case (int)commonFDP.commonFDP.TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    pnlMoficable.Visibility = Visibility.Visible;
                    /*foreach (Control control in pnlMoficable.Controls)
                    {
                        control.Visible = true;
                    }
                    lblTipoFiltro.Visible = false;*/
                    comboBoxFilters.Visibility = Visibility.Hidden;
                    btnLimpiar.Visibility = Visibility.Hidden;
                    /*txtIntervalo.Visible = false;
                    txtIntervalo2.Visible = false;*/
                    break;
                case (int)commonFDP.commonFDP.TipoAccionProcesamiento.BORRAR_SELECCIONADOS:
                case (int)commonFDP.commonFDP.TipoAccionProcesamiento.SELECCIONAR_TODOS:
                    pnlMoficable.Visibility = Visibility.Hidden;
                    break;
                case (int)commonFDP.commonFDP.TipoAccionProcesamiento.FILTRAR:
                    pnlMoficable.Visibility = Visibility.Visible;
                    
                    if (rbFecha.IsChecked.Value)
                    {
                        //lblTipoFiltro.Visible = true;
                        
                        btnLimpiar.Visibility = Visibility.Visible;

                        cargarFiltros();
                    }
                    else
                    {
                        comboBoxFilters.ItemsSource = null;
                        comboBoxFilters.Items.Clear();
                        List<commonFDP.ComboItem> filtros = new List<commonFDP.ComboItem> { new commonFDP.ComboItem(0, "Intervalo menor a"), new commonFDP.ComboItem(1, "Intervalo mayor a"), new commonFDP.ComboItem(2, "Intervalo entre") };
                        comboBoxFilters.ItemsSource = filtros;
                        comboBoxFilters.DisplayMemberPath ="Display";

                        lblTituloAccion.Content = "Filtrar";
                       // lblAccion1.Text = "Intervalo";
                        dtp1.Visibility = Visibility.Hidden;
                       // lblAccion2.Visible = false;
                        dtp2.Visibility = Visibility.Hidden;
                        //txtIntervalo.Visible = true;
                    }
                    comboBoxFilters.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void botonSeleccionado(Grid boton)
        {

            // btnBorrarSeleccionados.BackColor = Color.FromArgb(187, 0, 4);
            IEnumerable<Grid> grids = pnlButtonsGrid.Children.OfType<Grid>(); 

             foreach (Grid grid in grids)
             {
                 if (grid != deleteRegistersGrid)
                 {
                    
                     grid.Background = Brushes.Gray;
                     
                 }
             }

             if (boton != deleteRegistersGrid)
             {
                 boton.Background = Brushes.LightGray;
             }
        }

        private void BtnModifyRegister_onClick(object sender, RoutedEventArgs e)
        {
            var tipoAccion = (int)commonFDP.commonFDP.TipoAccionProcesamiento.MODIFICAR_REGISTRO;
            modificarLayout(tipoAccion);
            botonSeleccionado(modifyRegisterGrid);
        }

        private void BtnDeleteRegisters_onClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var tipoAccion = (int)commonFDP.commonFDP.TipoAccionProcesamiento.BORRAR_SELECCIONADOS;
                modificarLayout(tipoAccion);
                botonSeleccionado(deleteRegistersGrid);
                System.Collections.IList itemsToDelete = dgvDatosFdp.SelectedItems;
                foreach (var itemToDelete in itemsToDelete)
                {
                      
                    Evento eventToDelete = (Evento)itemToDelete;
                    eventos.Remove(eventToDelete);
                }
                dgvDatosFdp.Items.Refresh();
            }
            catch
            {

            }
            botonSeleccionado(deleteRegistersGrid);
        }

        private void BtnSelectAll_onClick(object sender, RoutedEventArgs e)
        {
            dgvDatosFdp.SelectAll();
            var tipoAccion = (int)commonFDP.commonFDP.TipoAccionProcesamiento.SELECCIONAR_TODOS;
            modificarLayout(tipoAccion);
            botonSeleccionado(selectAllGrid);

        }

        private void BtnFilterRegisters_onClick(object sender, RoutedEventArgs e)
        {
            if (eventos.Count() > 0)
            {
                var tipoAccion = (int)commonFDP.commonFDP.TipoAccionProcesamiento.FILTRAR;
                modificarLayout(tipoAccion);
                botonSeleccionado(filterRegisterGrid);
            }
            else
            {
                createAlertPopUp("No hay ningún evento cargado");
            }
        }

        private void cargarFiltros()
        {
            comboBoxFilters.ItemsSource = null;
            comboBoxFilters.Items.Clear();
            List<commonFDP.ComboItem> filtros = new List<commonFDP.ComboItem> { new commonFDP.ComboItem(0, "Fecha menor a"), new commonFDP.ComboItem(1, "Fecha mayor a"), new commonFDP.ComboItem(2, "Fecha entre"), new commonFDP.ComboItem(3, "Hora menor a"), new commonFDP.ComboItem(4, "Hora mayor a"), new commonFDP.ComboItem(5, "Hora entre") };
            comboBoxFilters.DisplayMemberPath = "Display";
            comboBoxFilters.SelectedValuePath = "Value";
            comboBoxFilters.ItemsSource = filtros;
        }

    }
}
