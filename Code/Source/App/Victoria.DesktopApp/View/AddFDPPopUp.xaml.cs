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
using System.IO;
using System.Collections.ObjectModel;




namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>

    // esto va a ir en la libreria common fdp


    // hasta aca va a ir a la common fdp 

    public partial class AddFDPPopUp : Window
    {
        private List<commonFDP.Evento> eventos = new List<commonFDP.Evento>();
        public List<commonFDP.Evento> interv = new List<commonFDP.Evento>();
        public int idEvento = 0;
        public List<String> origenes =  new List<string>();
        public AnalisisPrevio analisisPrevio { get; set; }
        public string dateFormat = "yyyy-MM-dd";
        public string hourFormat = "HH:mm:ss";
        public commonFDP.TipoAccionProcesamiento tipoAccion;
        private List<commonFDP.Filtro> filtros = new List<commonFDP.Filtro>();
        private List<commonFDP.Filtro> filtrosSeleccionados = new List<commonFDP.Filtro>();
        public  ObservableCollection<commonFDP.Filtro> auxfiltros = new ObservableCollection<commonFDP.Filtro>();
        private readonly commonFDP.INuevoFiltro filtrador = new commonFDP.FiltroImpl();
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        List<double> intervalosParciales;

        public void FDPGenerator(AnalisisPrevio aPrevio)
        {
            try
            {
                comboBox.ItemsSource = origenes;
                origenes.Add("Archivo Excel");
                origenes.Add("Archivo txt");
                this.analisisPrevio = aPrevio;
                dgvDatosFdp.ItemsSource = null;
                comboBox.SelectedItem = comboBox.Items[0];
                rbFecha.IsChecked = true;
                if (analisisPrevio.TipoDeEjercicio == AnalisisPrevio.Tipo.EaE)
                {
                    rbDtConstante.IsEnabled = false;
                    rbDtConstante.Visibility = Visibility.Hidden;
                    rbEventoAEvento.IsChecked = true;
                }
                else
                {
                    rbEventoAEvento.IsEnabled = false;
                    rbEventoAEvento.Visibility = Visibility.Hidden;
                    rbIntervalos.IsEnabled = false;
                    rbDtConstante.IsChecked = true;
                    rbDia.IsChecked = true;
                }
            }catch(Exception ex)
            {
                //logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                createAlertPopUp(String.Format("Ha ocurrido un error: {0} - {1}",ex.Source,ex.Message));                
            }

        }

        public AddFDPPopUp()
        {
            try
            {
                InitializeComponent();
                pnlButtonsGrid.Visibility = Visibility.Visible;
                pnlMetodologia.Visibility = Visibility.Visible;
            }catch(Exception ex)
            {
                //logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                createAlertPopUp(String.Format("Ha ocurrido un error: {0} - {1}", ex.Source, ex.Message));                
            }

}



        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBoxItem typeItem = (ComboBoxItem)comboBox.SelectedItem;
            try { 
                string value = comboBox.SelectedValue.ToString();
                if (value == "Archivo Excel" || value == "Archivo txt")
                {
                    rutaFile.Text = "";
                    pnlPosicion_datos.Visibility = Visibility.Hidden;
                    Archivo.Visibility = Visibility.Visible;
                    rutaFile.IsReadOnly = true;
               
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                createAlertPopUp(String.Format("Ha ocurrido un error: {0} - {1}", ex.Source, ex.Message));
            }
        }

        private void comboBoxIntervalo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }catch(Exception ex)
            {
                //logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                createAlertPopUp(String.Format("Ha ocurrido un error: {0} - {1}", ex.Source, ex.Message));                
            }
}

        private String getFileName(string tipo)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (tipo == "Archivo Excel")
            {
                openFile.Filter = "Designer Files (*.xls)|*.xlsx|All Files (*.*)|*.*";
            }
            else
            {
                openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            }

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
            try
            {
                if (eventos.Count() >= 15)
                {
                    try
                    {
                        commonFDP.MetodologiaAjuste metodologia = commonFDP.MetodologiaAjuste.EVENTO_A_EVENTO;
                        commonFDP.Segment.Segmentacion segmentacion = commonFDP.Segment.Segmentacion.SEGUNDO;
                        int flagIntervalos = 0;

                        if (rbFecha.IsChecked.Value)
                        {
                            metodologia = rbEventoAEvento.IsChecked.Value ? commonFDP.MetodologiaAjuste.EVENTO_A_EVENTO : commonFDP.MetodologiaAjuste.DT_CONSTANTE;
                            segmentacion = rbDia.IsChecked.Value ? commonFDP.Segment.Segmentacion.DIA : (rbHora.IsChecked.Value ? commonFDP.Segment.Segmentacion.HORA : (rbMinuto.IsChecked.Value ? commonFDP.Segment.Segmentacion.MINUTO : commonFDP.Segment.Segmentacion.SEGUNDO));
                            FrmAjusteFunciones frm = new FrmAjusteFunciones(metodologia, segmentacion, eventos, flagIntervalos, null, this.analisisPrevio);
                            this.Visibility = Visibility.Hidden;
                            frm.ShowDialog();
                            this.Visibility = Visibility.Visible;
                        }
                        else if (rbIntervalos.IsChecked.Value)
                        {
                            metodologia = commonFDP.MetodologiaAjuste.EVENTO_A_EVENTO;
                            flagIntervalos = 1;
                            FrmAjusteFunciones frm = new FrmAjusteFunciones(metodologia, segmentacion, intervalosParciales, flagIntervalos, null, this.analisisPrevio);
                            this.Visibility = Visibility.Hidden;
                            frm.ShowDialog();
                            this.Visibility = Visibility.Visible;
                        }



                    }
                    catch
                    {
                        createAlertPopUp("Error al calcular funciones");
                    }
                    this.Close();
                }
                else
                {
                    createAlertPopUp("Debe haber al menos 15 eventos para poder calcular la FDP");
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                createAlertPopUp(String.Format("Ha ocurrido un error: {0} - {1}", ex.Source, ex.Message));
            }

        } 
       
        


        private void Btnserch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rutaFile.Text = getFileName(comboBox.SelectedItem.ToString());
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
           switch(comboBox.SelectedValue.ToString())
            {
                case ("Archivo Excel"):
                    label_posicion_datos.Content = "Indique la ubicación de los datos: ";
                    label_hoja.Content = "Hoja";
                    label_hoja.Visibility = Visibility.Visible;
                    label_columna.Visibility = Visibility.Visible;
                    label_fila.Visibility = Visibility.Visible;
                    txtHoja.Visibility = Visibility.Visible;
                    txtFila.Visibility = Visibility.Visible;
                    txtCol.Visibility = Visibility.Visible;
                    txtDelimitador.Visibility = Visibility.Hidden;


                    pnlPosicion_datos.Visibility = Visibility.Visible;

                  break;

                case ("Archivo txt"):
                    label_hoja.Content = "";
                    label_posicion_datos.Content = "Indique el delemitador a utilizar : ";
                    pnlPosicion_datos.Visibility = Visibility.Visible;
                    label_columna.Visibility = Visibility.Hidden;
                    label_fila.Visibility = Visibility.Hidden;
                    label_hoja.Visibility = Visibility.Hidden;
                    txtCol.Visibility = Visibility.Hidden;
                    txtFila.Visibility = Visibility.Hidden;
                    txtHoja.Visibility = Visibility.Hidden;
                    txtDelimitador.Visibility = Visibility.Visible;




                  break;
            }
           
        
            
            
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
            if (comboBox.SelectedItem.ToString() =="Archivo Excel") 
            {
               

                using (var archivo = new XLWorkbook(rutaFile.Text))
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
                            eventos.Add(new commonFDP.Evento() { fecha = auxFecha, Id = idEvento });//, origen = nuevoOrigen, activo = true });
                            numeroFila++;
                            idEvento++;
                        }

                    }

                    catch
                    {
                        
                        createAlertPopUp("El excel importado no tiene el formato correcto o no se definieron correctamente los parametros de lectura. Por favor seleccione otro archivo o verifique los parametros ingresados y vuelva a intentarlo");
                        rutaFile.Text = "";
                        pnlPosicion_datos.Visibility = Visibility.Hidden;
                       
                    }

                }
            }
            else 
            {

                try
                {
                    StreamReader objReader = new StreamReader(rutaFile.Text);
                    List<string> eventosLeidos = new List<string>();
                    string todoElArchivo = objReader.ReadToEnd();
                    objReader.Close();
                    foreach (var item in todoElArchivo.Split(Convert.ToChar(txtDelimitador.Text)))
                    {
                        DateTime aux = DateTime.ParseExact(item.Replace("\r\n", "").Replace("\n", "").Replace("\r", ""), "dd/MM/yyyy HH:mm:ss", null);
                                   
                        eventos.Add(new commonFDP.Evento() { fecha = aux, Id = idEvento });//, origen = nuevoOrigen, activo = true });

                    }

                }
                catch (Exception ex)
                {
                    createAlertPopUp("No se pudo obtener los datos, valide que el formato del archivo y de las fechas ingresadas sean corerctos");
                    rutaFile.Text = "";
                    pnlPosicion_datos.Visibility = Visibility.Hidden;

                } 

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
            pnlModificable.Visibility = Visibility.Hidden;
            pnlMetodologia.Visibility = Visibility.Visible;

            if (analisisPrevio.TipoDeEjercicio == AnalisisPrevio.Tipo.EaE)
            {
                rbDtConstante.Visibility = Visibility.Hidden;
                rbEventoAEvento.IsChecked = true;
            }
            else
            {
                rbEventoAEvento.Visibility = Visibility.Hidden;
                rbDtConstante.IsChecked = true;
            }

        }


        public List<string> leerDelimitadorCaracter(string pathArchivo, string caracter)
        {
            StreamReader objReader = new StreamReader(pathArchivo);
            List<string> eventosLeidos = new List<string>();
            string todoElArchivo = objReader.ReadToEnd();
            objReader.Close();
            foreach (var item in todoElArchivo.Split(Convert.ToChar(caracter)))
            {
                DateTime aux = DateTime.Parse(item);
                eventosLeidos.Add(aux.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            return eventosLeidos;
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
            tipoAccion = commonFDP.TipoAccionProcesamiento.AGREGAR_REGISTRO;
            modificarLayout(tipoAccion);
            //botonSeleccionado(addRegisterGrid);
        }

        private void RbIntervalos_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvDatosFdp.Items.Count <= 1)
                {
                    rbFecha.IsChecked = true;
                    createAlertPopUp("No se pueden calcular los intervalos con menos de 2 eventos");

                }
                else
                {
                    if (rbIntervalos.IsChecked.Value)

                    {
                        var tipoAccion = commonFDP.TipoAccionProcesamiento.FILTRAR;

                        modificarLayout(tipoAccion);

                        //rbDtConstante.Visibility = Visibility.Hidden;
                        pnlSegmentacion.Visibility = Visibility.Hidden;

                       // rbEventoAEvento.IsChecked = true;
                        if (rbDtConstante.IsChecked.Value)
                        {
                            //rbEventoAEvento.IsChecked = true;
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



                            foreach (var intervalo in lista)
                            {
                                interv.Add(new commonFDP.Evento() { vIntervalo = intervalo });
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
                            addRegisterGrid.Background = Brushes.LightGray;
                            btnModifyRegister.IsEnabled = false;
                            modifyRegisterGrid.Background = Brushes.LightGray;
                            btnDeleteRegisters.IsEnabled = false;
                            deleteRegistersGrid.Background = Brushes.LightGray;
                            //btnDeleteRegisters. = Color.FromArgb(134, 0, 3);
                            btnSelectAll.IsEnabled = false;
                            selectAllGrid.Background = Brushes.LightGray;
                            CheckListBoxFiltros.IsEnabled = false;


                            //chlFiltros.Enabled = false;

                            //modificarLayout(commonFDP.commonFDP.TipoAccionProcesamiento.FILTRAR);
                            pnlModificable.Visibility = Visibility.Hidden;

                        }

                        //  ();

                    }
                    else
                    {
                         cargarFiltros();
                    }
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
                dgvDatosFdp.Items.Refresh();

                if (eventos != null && tipoAccion == commonFDP.TipoAccionProcesamiento.FILTRAR)
                 filtrar();
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
                
                //rbDtConstante.Visibility = Visibility.Visible;
                if (eventos.Count() > 0)
                {
                    cargarEventos();
                }
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
                modificarLayout(tipoAccion);
                //botonSeleccionado(btnFiltrar);
                lbldtp1.Visibility = Visibility.Hidden;
                lbldtp2.Visibility = Visibility.Hidden;
                txtInterv1.Visibility = Visibility.Hidden;
                txtInterv2.Visibility = Visibility.Hidden;
                //rbDtConstante.Visibility = Visibility.Visible;
                pnlModificable.Visibility = Visibility.Hidden;
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

        private void modificarLayout(commonFDP.TipoAccionProcesamiento tipoAccion)
         {
             hacerVisible(tipoAccion);
             switch (tipoAccion)
             {
                 case commonFDP.TipoAccionProcesamiento.AGREGAR_REGISTRO:
                     lblTituloAccion.Content = "Agregar Registro";

                     dtp1.FormatString = dateFormat;

                     dtp2.FormatString = hourFormat;
                     pnlModificable.Visibility = Visibility.Visible;
                     rbAgregarPorFechaYHora.IsChecked = true;
                     rbAgregarPorFechaYHora.Visibility = Visibility.Visible;
                     rbAgregarPorIntervalo.IsChecked = false;
                     rbAgregarPorIntervalo.Visibility = Visibility.Visible;
                     nudAgregarPorIntervalo.Visibility = Visibility.Hidden;
                     cbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                     //cambiarFiltrosVistaFecha(0);
                     break;
                 case commonFDP.TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    lblTituloAccion.Content = "Modificar Registro";
                     lbldtp1.Content = "Fecha";

                     lbldtp2.Content = "Hora";

                    dtp1.FormatString = dateFormat;
                    dtp2.FormatString = hourFormat;
                    rbAgregarPorFechaYHora.Visibility = Visibility.Hidden;
                    rbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    nudAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    cbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                    // cambiarFiltrosVistaFecha(0);
                     break;
                 case commonFDP.TipoAccionProcesamiento.FILTRAR:
                     lblTituloAccion.Content = "Filtrar";

                     dtp1.FormatString = dateFormat;
                     if (rbFecha.IsChecked.Value)
                     {    
                         lbldtp1.Content = "Fecha";
                         txtInterv1.Visibility = Visibility.Hidden;
                         txtInterv2.Visibility = Visibility.Hidden;
                         //cambiarFiltrosVistaFecha(0); 
                     }
                     else
                     {     
                         lbldtp1.Content = "Intervalo";
                         dtp1.Visibility = Visibility.Visible;
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


        private void hacerVisible(commonFDP.TipoAccionProcesamiento tipoAccion)
        {
            switch (tipoAccion)
            {
                case commonFDP.TipoAccionProcesamiento.AGREGAR_REGISTRO:
                case commonFDP.TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    pnlModificable.Visibility = Visibility.Visible;
                    /*foreach (Control control in pnlModificable.Controls)
                    {
                        control.Visible = true;
                    }
                    lblTipoFiltro.Visible = false;*/
                    comboBoxFilters.Visibility = Visibility.Hidden;
                    btnClean.Visibility = Visibility.Hidden;
                    lbldtp1.Visibility = Visibility.Visible;
                    lbldtp2.Visibility = Visibility.Visible;
                    dtp1.Visibility = Visibility.Visible;
                    dtp2.Visibility = Visibility.Visible;
                    /*txtIntervalo.Visible = false;
                    txtIntervalo2.Visible = false;*/
                    break;
                case commonFDP.TipoAccionProcesamiento.BORRAR_SELECCIONADOS:
                case commonFDP.TipoAccionProcesamiento.SELECCIONAR_TODOS:
                    pnlModificable.Visibility = Visibility.Hidden;
                    break;
                case commonFDP.TipoAccionProcesamiento.FILTRAR:
                    pnlModificable.Visibility = Visibility.Visible;
                    dtp1.Visibility = Visibility.Hidden;
                    dtp2.Visibility = Visibility.Hidden;
                    lbldtp1.Visibility = Visibility.Hidden;
                    lbldtp2.Visibility = Visibility.Hidden;
                    if (rbFecha.IsChecked.Value)
                    {
                        //lblTipoFiltro.Visible = true;

                        btnClean.Visibility = Visibility.Visible;

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
            tipoAccion = commonFDP.TipoAccionProcesamiento.MODIFICAR_REGISTRO;
            modificarLayout(tipoAccion);
            //botonSeleccionado(modifyRegisterGrid);
        }

        private void BtnDeleteRegisters_onClick(object sender, RoutedEventArgs e)
        {
            try
            {
                tipoAccion = commonFDP.TipoAccionProcesamiento.BORRAR_SELECCIONADOS;
                modificarLayout(tipoAccion);
                //botonSeleccionado(deleteRegistersGrid);                

                System.Collections.IList itemsToDelete = dgvDatosFdp.SelectedItems;

                foreach (var itemToDelete in itemsToDelete)
                {

                    commonFDP.Evento eventToDelete = (commonFDP.Evento)itemToDelete;
                    eventos.Remove(eventToDelete);                    

                    logger.Info(String.Format("Se ha eliminado el evento '{0}'.",eventToDelete.fecha.ToString()));
                }

                dgvDatosFdp.Items.Refresh();

                filtrar();
                
            }
            catch(Exception ex)
            {
                createAlertPopUp(String.Format("Ha ocurrido un error: {0} - {1}", ex.Source, ex.Message));
            }
            //botonSeleccionado(deleteRegistersGrid);
        }

        private void BtnSelectAll_onClick(object sender, RoutedEventArgs e)
        {
            dgvDatosFdp.SelectAll();
            var tipoAccion = commonFDP.TipoAccionProcesamiento.SELECCIONAR_TODOS;
            modificarLayout(tipoAccion);
            //botonSeleccionado(selectAllGrid);

        }

        private void BtnFilterRegisters_onClick(object sender, RoutedEventArgs e)
        {
            if (eventos.Count() > 0)
            {
                tipoAccion = commonFDP.TipoAccionProcesamiento.FILTRAR;
                modificarLayout(tipoAccion);
                //botonSeleccionado(filterRegisterGrid);
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

        private void rbIntervalPNLM_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (rbAgregarPorIntervalo.IsChecked.Value)
            {
                nudAgregarPorIntervalo.Visibility = Visibility.Visible;
                cbAgregarPorIntervalo.Visibility = Visibility.Visible;
                dtp1.Visibility = Visibility.Hidden;
                dtp2.Visibility = Visibility.Hidden;
                lbldtp1.Visibility = Visibility.Hidden;
                lbldtp2.Visibility = Visibility.Hidden;
            }
        }




        private void rbFechaPNLM_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (rbAgregarPorFechaYHora.IsChecked.Value)
            {
                nudAgregarPorIntervalo.Visibility = Visibility.Hidden;
                cbAgregarPorIntervalo.Visibility = Visibility.Hidden;
                dtp1.Visibility = Visibility.Visible;
                dtp2.Visibility = Visibility.Visible;
                lbldtp1.Visibility = Visibility.Visible;
                lbldtp2.Visibility = Visibility.Visible;
            }
        }


        public void nuevoPorIntervalo(int cantidad, commonFDP.Segment.Segmentacion segm, int idEvento)
        {
            DateTime fechaAAgregar = DateTime.Now;
             
            DateTime maxFecha;
            if (eventos.Count > 0)
                maxFecha =eventos.Max(x => x.fecha);
            else
                maxFecha = DateTime.Now;

            switch (segm)
            {
                case commonFDP.Segment.Segmentacion.DIA:
                    fechaAAgregar = maxFecha.AddDays(cantidad);
                    break;
                case commonFDP.Segment.Segmentacion.HORA:
                    fechaAAgregar = maxFecha.AddHours(cantidad);
                    break;
                case commonFDP.Segment.Segmentacion.MINUTO:
                    fechaAAgregar = maxFecha.AddMinutes(cantidad);
                    break;
                case commonFDP.Segment.Segmentacion.SEGUNDO:
                    fechaAAgregar = maxFecha.AddSeconds(cantidad);
                    break;
                default:
                    break;
            }
            eventos.Add( new commonFDP.Evento { fecha = fechaAAgregar,Id= idEvento, activo = true });
            
        }
        private void BtnAcept_Click(object sender, RoutedEventArgs e)
        {
            DateTime fecha;
            commonFDP.Segment.Segmentacion segmentacion = commonFDP.Segment.Segmentacion.SEGUNDO;            

            switch (tipoAccion)
            {
                case commonFDP.TipoAccionProcesamiento.AGREGAR_REGISTRO:
                    if (rbAgregarPorFechaYHora.IsChecked.Value)
                    {
                        DateTime fechaMSelected = (DateTime)dtp1.Value;
                        DateTime horaMSelected = (DateTime)dtp2.Value;
                        fecha = new DateTime(fechaMSelected.Year, fechaMSelected.Month, fechaMSelected.Day, horaMSelected.Hour, horaMSelected.Minute, horaMSelected.Second);
                        idEvento++;
                        eventos.Add(new commonFDP.Evento() { fecha = fecha, Id = idEvento });

                        logger.Info(String.Format("Se Agrega Registro por Fecha y Hora: {0} {1}.",fechaMSelected.ToShortDateString(), horaMSelected.ToShortTimeString()));
                    }
                    else if (rbAgregarPorIntervalo.IsChecked.Value)
                    {
                        ComboBoxItem SegItem = (ComboBoxItem)cbAgregarPorIntervalo.SelectedItem;
                        string segment = SegItem.Content.ToString();
                        switch (segment)
                        {
                            case "Segundos":
                                segmentacion = commonFDP.Segment.Segmentacion.SEGUNDO;
                                break;
                            case "Minutos":
                                segmentacion = commonFDP.Segment.Segmentacion.MINUTO;
                                break;
                            case "Horas":
                                segmentacion = commonFDP.Segment.Segmentacion.HORA;
                                break;
                            case "Dias":
                                segmentacion = commonFDP.Segment.Segmentacion.DIA;
                                break;
                            default:
                                break;
                        }
                        idEvento++;
                        nuevoPorIntervalo(Convert.ToInt32(nudAgregarPorIntervalo.Value), segmentacion, idEvento );

                        logger.Info(String.Format("Se Agrega Registro por Intervalo {0}.",segmentacion));
                    }
                    //mostrarMensaje("Registro agregado correctamente", Color.FromArgb(128, 255, 128));
                    cargarEventos();
                   // actualizarEstadisticas();

                    break;
                case commonFDP.TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    DateTime fechaSelected = (DateTime)dtp1.Value;
                    DateTime horaSelected = (DateTime)dtp2.Value;
                    fecha = new DateTime(fechaSelected.Year, fechaSelected.Month, fechaSelected.Day, horaSelected.Hour, horaSelected.Minute, horaSelected.Second);
                    var mensaje = "";

                    if (dgvDatosFdp.SelectedItems.Count == 1)

                    {
                        commonFDP.Evento eventToModify = (commonFDP.Evento)dgvDatosFdp.SelectedItem;
                        commonFDP.Evento obj = eventos.FirstOrDefault(x => x.Id == eventToModify.Id);
                        
                        if (obj != null)
                        {
                            DateTime fecha_modificada = obj.fecha;
                            obj.fecha = fecha;

                            logger.Info(String.Format("Se ha Modificado la fecha [{0}] por la fecha [{1}].",fecha_modificada.ToString(),fecha.ToString()));
                        }
                        cargarEventos();
                        break;
                        //actualizarEstadisticas();
                    }
                    else
                        
                        if (dgvDatosFdp.SelectedItems.Count == 0)
                        {
                            mensaje = "Seleccione un registro a modificar y vuelva a intentarlo";
                        }
                        else
                        {
                            mensaje = "Seleccione solo un registro a modificar";
                        }
                        createAlertPopUp(mensaje);
                    break;
                case commonFDP.TipoAccionProcesamiento.FILTRAR:
                    agregarFiltro();                    
                    //mostrarMensaje("Filtro aplicado correctamente", Color.FromArgb(128, 255, 128));
                    //actualizarEstadisticas();
                    break;                
                default:
                    break;
            }

            filtrar();
        }
        
        private void agregarFiltro()
        {
            int selectedValue = Convert.ToInt32(comboBoxFilters.SelectedValue);
            commonFDP.Filtro auxFiltro = null;
            DateTime fecha = DateTime.Now;
            DateTime fecha2 = DateTime.Now;
            fecha = (DateTime)dtp1.Value;
            fecha2 = (DateTime)dtp2.Value;
            double intervalo = -1;
            double intervalo2 = -1;
            if (rbFecha.IsChecked.Value)
            {
                switch (selectedValue)
                {
                    case 0:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.FECHA_MENOR, fecha);
                        break;
                    case 1:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.FECHA_MAYOR, fecha);
                        break;
                    case 2:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.FECHA_ENTRE, fecha, fecha2);
                        break;
                    case 3:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.HORA_MENOR, fecha);
                        break;
                    case 4:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.HORA_MAYOR, fecha);
                        break;
                    case 5:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.HORA_ENTRE, fecha, fecha2);
                        break;
                    default:
                        auxFiltro = null;
                        break;
                }
            }
            else if (rbIntervalos.IsChecked.Value)
            {
                intervalo = Convert.ToDouble(txtInterv1.Text);
                intervalo2 = Convert.ToDouble(txtInterv2.Text);

                switch (selectedValue)
                {
                    case 0:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.INTERVALO_MENOR, intervalo);
                        break;
                    case 1:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.INTERVALO_MAYOR, intervalo);
                        break;
                    case 2:
                        auxFiltro = new commonFDP.Filtro(commonFDP.TipoFiltro.INTERVALO_ENTRE, intervalo, intervalo2);
                        break;
                    default:
                        break;
                }
            }
            auxFiltro.IsChecked = true;

            bool exists = false;

            for (int i = 0; i < this.auxfiltros.Count; i++)
            {

                if (auxfiltros[i].Name == auxFiltro.Name)
                {
;                   exists = true;

                }
            }

            if (!exists)
            {
                filtros.Add(auxFiltro);
                logger.Info(String.Format("Se ha agregado el filtro '{0}'.", auxFiltro.Name));
            }
            else
            {
                createAlertPopUp("Error al crear un nuevo filtro : El filtro que se intenta crear ya existe");
            }
            setupFiltrosCheckboxList();
        }
        
        private void filtrar()
        {
            if (rbFecha.IsChecked.Value)
            {
                List<commonFDP.Evento> filtrado = filtrador.FiltrarFechas(0, filtros,eventos);
                if (filtrado != null)
                {                 
                    dgvDatosFdp.ItemsSource = null;
                    dgvDatosFdp.ItemsSource = filtrado;
                    dgvDatosFdp.Items.Refresh();
                    dgvDatosFdp.Columns[1].ClipboardContentBinding.StringFormat = "dd'/'MM'/'yyyy HH:mm:ss";
                    dgvDatosFdp.Columns[0].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[2].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[1].Visibility = Visibility.Visible;
                    dgvDatosFdp.Columns[1].Header = "Eventos";
                    dgvDatosFdp.Columns[3].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[4].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Columns[5].Visibility = Visibility.Hidden;
                    dgvDatosFdp.Visibility = Visibility.Visible;

                }
            }
            else if (rbIntervalos.IsChecked.Value)
            {
                List<double> filtrado = filtrador.FiltrarIntervalos(this.intervalosParciales, comboBoxFilters.SelectedIndex, Convert.ToInt32(txtInterv1.Text), Convert.ToInt32(txtInterv2.Text));
                intervalosParciales = filtrado; //para filtros acumulativos

                //leno dataGridView con los intervalos
                
                List<double> listaInterv = new List<double>();
             
                foreach (var item in filtrado)
                {
                    listaInterv.Add(item);
                }

                dgvDatosFdp.ItemsSource = null;
                dgvDatosFdp.ItemsSource = listaInterv;
                dgvDatosFdp.Items.Refresh();

                dgvDatosFdp.Columns[0].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[2].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[1].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[3].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[4].Visibility = Visibility.Hidden;
                dgvDatosFdp.Columns[5].Visibility = Visibility.Visible;
                dgvDatosFdp.Visibility = Visibility.Visible;
                dgvDatosFdp.Columns[5].Header = "Intervalos";


            }
        }
        
        private void borrarFiltroSeleccionado()
        {

            System.Collections.IList filtros_seleccionados = this.filtros.Where(x => x.IsChecked).ToList<commonFDP.Filtro>();
            
            foreach (commonFDP.Filtro f in filtros_seleccionados)
            {
                if (CheckListBoxFiltros.SelectedItems.Contains(f))
                {
                    this.filtros.Remove(f);
                    CheckListBoxFiltros.Items.Remove(f);
                }
            }
        }

        private void setupFiltrosCheckboxList()
        {
            CheckListBoxFiltros.DisplayMemberPath = "Name";
            CheckListBoxFiltros.ValueMemberPath = "IsChecked";
           
            for (int i = 0; i < this.filtros.Count; i++)
            {
                
                if (!CheckListBoxFiltros.Items.Contains(filtros[i]))
                {
                    CheckListBoxFiltros.Items.Add(filtros[i]);
                    //commonFDP.Filtro obj = (commonFDP.Filtro)CheckListBoxFiltros.Items[i];
                    auxfiltros.Add(filtros[i]);
                    
                }
            }
            CheckListBoxFiltros.SelectedItemsOverride = auxfiltros;
        }


        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            this.tipoAccion = commonFDP.TipoAccionProcesamiento.BORRAR_SELECCIONADOS;
            borrarFiltroSeleccionado();
            filtrar();
            this.tipoAccion = commonFDP.TipoAccionProcesamiento.FILTRAR;
            logger.Info(String.Format("Se han eliminado los filtros seleccionados."));
        }


        private void comboBoxFilters_SelectionChanged(object sender, EventArgs e)
        {
            if (comboBoxFilters.SelectedValue != null)
            {
                if (rbFecha.IsChecked.Value)
                    cambiarFiltrosVistaFecha(Convert.ToInt32(comboBoxFilters.SelectedValue));
                else if (rbIntervalos.IsChecked.Value)
                    cambiarFiltrosVistaIntervalos(Convert.ToInt32(comboBoxFilters.SelectedValue));
            }
        }

        private void cambiarFiltrosVistaFecha(int valorSeleccionado)
        {
            if (tipoAccion == commonFDP.TipoAccionProcesamiento.FILTRAR)
            {
                txtInterv1.Visibility = Visibility.Hidden;
                txtInterv2.Visibility = Visibility.Hidden;
                if (valorSeleccionado > 2)
                {
                    dtp1.FormatString = hourFormat;
                    DateTime fechaInicial = (DateTime)dtp1.Value;
                    fechaInicial.AddHours(fechaInicial.Hour * -1);
                    fechaInicial.AddMinutes(fechaInicial.Minute * -1);
                    fechaInicial.AddSeconds(fechaInicial.Second * -1);
                    dtp1.Value = fechaInicial;
                    dtp2.FormatString = hourFormat;
                    dtp2.Value = dtp1.Value;
                }
                else
                {
                    dtp1.FormatString = dateFormat;
                    dtp2.FormatString = hourFormat;
                    dtp2.Value = dtp1.Value;

                }
                switch (valorSeleccionado)
                {
                    case 0:
                        btnAcept.Visibility = Visibility.Visible;
                        lblTituloAccion.Visibility = Visibility.Visible;
                        lblTituloAccion.Content = "Filtrar";
                        lbldtp1.Visibility = Visibility.Visible;
                        lbldtp1.Content = "Fecha";
                        dtp1.Visibility = Visibility.Visible;
                        lbldtp2.Visibility = Visibility.Hidden;
                        dtp2.Visibility = Visibility.Hidden;
                        btnClean.Visibility = Visibility.Visible;
                        break;
                    case 1:
                        btnAcept.Visibility = Visibility.Visible;
                        lblTituloAccion.Visibility = Visibility.Visible;
                        lblTituloAccion.Content = "Filtrar";
                        lbldtp1.Content = "Fecha";
                        dtp1.Visibility = Visibility.Visible;
                        lbldtp2.Visibility = Visibility.Hidden;
                        dtp2.Visibility = Visibility.Hidden;
                        btnClean.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        btnAcept.Visibility = Visibility.Visible;
                        lblTituloAccion.Visibility = Visibility.Visible;
                        lblTituloAccion.Content = "Filtrar";
                        lbldtp1.Content = "Fecha desde";
                        dtp1.Visibility = Visibility.Visible;
                        lbldtp2.Visibility = Visibility.Visible;
                        lbldtp2.Content = "Fecha hasta";
                        dtp2.FormatString = dateFormat;
                        dtp2.Visibility = Visibility.Visible;
                        btnClean.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        btnAcept.Visibility = Visibility.Visible;
                        lblTituloAccion.Visibility = Visibility.Visible;
                        lblTituloAccion.Content = "Filtrar";
                        lbldtp1.Content = "Hora";
                        dtp1.Visibility = Visibility.Visible;
                        lbldtp2.Visibility = Visibility.Hidden;
                        dtp2.Visibility = Visibility.Hidden;
                        btnClean.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        btnAcept.Visibility = Visibility.Visible;
                        lblTituloAccion.Visibility = Visibility.Visible;
                        lblTituloAccion.Content = "Filtrar";
                        lbldtp1.Content = "Hora";
                        dtp1.Visibility = Visibility.Visible;
                        lbldtp2.Visibility = Visibility.Hidden;
                        dtp2.Visibility = Visibility.Hidden;
                        btnClean.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        btnAcept.Visibility = Visibility.Visible;
                        lblTituloAccion.Visibility = Visibility.Visible;
                        lblTituloAccion.Content = "Filtrar";
                        lbldtp1.Content = "Hora desde";
                        dtp1.Visibility = Visibility.Visible;
                        lbldtp2.Visibility = Visibility.Visible;
                        lbldtp2.Content = "Hora hasta";
                        dtp2.Visibility = Visibility.Visible;
                        btnClean.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }

        private void cambiarFiltrosVistaIntervalos(int valorSeleccionado)
        {
            btnAcept.Visibility = Visibility.Visible;
            lblTituloAccion.Visibility = Visibility.Visible;
            lblTituloAccion.Content = "Filtrar";
            txtInterv1.Text = "0";
            txtInterv2.Text = "0";

            switch (valorSeleccionado)
            {
                case 0: //intervalo menor a
                case 1: //intervalo mayor a
                    lbldtp1.Visibility = Visibility.Visible;
                    lbldtp1.Content = "Intervalo";
                    txtInterv1.Visibility = Visibility.Visible;
                    txtInterv2.Visibility = Visibility.Hidden;
                    btnClean.Visibility = Visibility.Visible;
                    lbldtp2.Visibility = Visibility.Hidden;
                   
                    break;
                case 2://intervalo entre
                    lbldtp1.Visibility = Visibility.Visible;
                    lbldtp1.Content = "Intervalo desde";
                    txtInterv1.Visibility = Visibility.Visible;
                    lbldtp2.Visibility = Visibility.Visible;
                    lbldtp2.Content = "Intervalo hasta";
                    txtInterv2.Visibility = Visibility.Visible;
                    btnClean.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void CheckListBoxFiltros_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            foreach(commonFDP.Filtro f in filtros)
            {
                if (CheckListBoxFiltros.SelectedItemsOverride.Contains(f))
                {
                    f.IsChecked = true;
                }
                else {
                    f.IsChecked = false;
                }

            }

            filtrar();
        }

        /*
        private void CheckListBoxFiltros_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {

           
            filtrosSeleccionados = null;
            foreach(var item in CheckListBoxFiltros.SelectedValue.ToString())
            {
                filtrosSeleccionados.Add(item);
            }
            
            filtrar();
        }  */
    }
}
