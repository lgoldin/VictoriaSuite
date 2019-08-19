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
using System.Collections.ObjectModel;
using Victoria.Shared.AnalisisPrevio;
using DiagramDesigner;
using System.Xml.Linq;
using System.Reflection;
using System.Xml;
using System.Windows.Markup;
using System.IO;
using Microsoft.Win32;
using Victoria.DesktopApp.DiagramDesigner;
using System.Collections;
using Victoria.Shared;


namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for AddAnalisisPrevioPopUp.xaml
    /// </summary>
    public partial class AddAnalisisPrevioPopUp : Window
    {
        private static object syncLock = new object();

        private Window1 VentanaDiagramador { get; set; }

        public AnalisisPrevio AnalisisPrevio { get; set; }

        public ObservableCollection<string> Conditions { get; set; }

        private string LastGridObject { get; set; }
        private bool NewGridObject { get; set; }



        private const string AGREGAR_CONDICION = "[Agregar Condición]";
        private const string AGREGAR_ENCADENADOR = "[Agregar Encadenador]";
        private const string AGREGAR_VARIABLE_CONTROL = "[Agregar Variable de Control]";
        private const string AGREGAR_VARIABLE_ESTADO = "[Agregar Variable de Estado]";
        private const string AGREGAR_VARIABLE_RESULTADO = "[Agregar Variable de Resultado]";

        private const string AGREGAR_EVENTO = "[Agregar Evento]";

        public AddAnalisisPrevioPopUp(Window1 diagramador)
        {
            this.InitializeComponent();
            this.VentanaDiagramador = diagramador;
            this.comboBox.SelectedItem = EaE;
            this.comboBox_EventosEaE.SelectedItem = TEI;
            this.InicializarMetodologia();
            this.InitializeCollections();
            this.eventos.Visibility = Visibility.Visible;
        }

        public void InitializeCollections()
        {
            this.inicializarORecargarDatos();
            this.inicializarORecargarVariablesControl();
            this.inicializarORecargarVariablesEstado();
            this.inicializarORecargarVariablesResultado();
            this.inicializarCondiciones();

            this.dataGridEventosIndependientes.ItemsSource = this.AnalisisPrevio.EventosEaE;
            this.dataGridEventos.ItemsSource = this.AnalisisPrevio.EventosEaE;
            this.propios.ItemsSource = this.AnalisisPrevio.Propios;
            this.comprometidosAnteriores.ItemsSource = this.AnalisisPrevio.ComprometidosAnterior;
            this.comprometidosFuturos.ItemsSource = this.AnalisisPrevio.ComprometidosFuturos;
            this.tefs.ItemsSource = this.AnalisisPrevio.Tefs;

            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.Datos, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.VariablesDeControl, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.VariablesEstado, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.VariablesResultado, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.EventosEaE, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.Propios, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.ComprometidosFuturos, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.ComprometidosAnterior, syncLock);
            BindingOperations.EnableCollectionSynchronization(AnalisisPrevio.Tefs, syncLock);
            BindingOperations.EnableCollectionSynchronization(Conditions, syncLock);
        }

        private void inicializarCondiciones()
        {
            this.Conditions = new ObservableCollection<string>();
            if (this.AnalisisPrevio.TipoDeEjercicio == AnalisisPrevio.Tipo.EaE)
            {
                if (this.AnalisisPrevio.TipoDeEaE == AnalisisPrevio.TipoEvento.Independiente)
                {
                    this.Conditions.Add("NS == 1");
                    this.Conditions.Add("NS > 0");
                }
                else
                {
                    this.Conditions.Add("TMP - T <= (PORC * PM) / 100)");
                }
            }
        }

        private void inicializarORecargarVariablesResultado()
        {
            var itemsSource = new ArrayList();
            foreach(VariableAP variableResultado in this.AnalisisPrevio.VariablesResultado)
            {
                itemsSource.Add(variableResultado.nombre);
            }
            itemsSource.Add(AGREGAR_VARIABLE_RESULTADO);
            this.variablesResultado.ItemsSource = itemsSource;
        }

        private void inicializarORecargarVariablesEstado()
        {
            var itemsSource = new ArrayList();
            foreach (VariableAP variableEstado in this.AnalisisPrevio.VariablesEstado)
            {
                itemsSource.Add(variableEstado.nombre);
            }
            itemsSource.Add(AGREGAR_VARIABLE_ESTADO);
            this.variablesEstado.ItemsSource = itemsSource;
        }

        private void inicializarORecargarVariablesControl()
        {
            var itemsSource = this.AnalisisPrevio.VariablesDeControl.ToList();
            itemsSource.Add(AGREGAR_VARIABLE_CONTROL);
            this.variablesControl.ItemsSource = itemsSource;
            this.reloadTables();
        }

        private void reloadTables()
        {
            dataGridEventosIndependientes.Items.Refresh();
            dataGridEventos.Items.Refresh();
        }

        private void inicializarORecargarDatos()
        {
            var itemsSource = this.AnalisisPrevio.Datos.ToList();
            itemsSource.Add(AGREGAR_ENCADENADOR);
            this.datos.ItemsSource = itemsSource;
        }

        #region Window Elements Bindings

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InicializarMetodologia();
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_Dato(object sender, RoutedEventArgs e)
        {
            this.agregarDato();

        }

        private void agregarDato()
        {
            this.Agregar(AnalisisPrevio.Datos);
            dataGridEventosIndependientes.Items.Refresh();
            this.inicializarORecargarDatos();
        }

        private void MenuItem_Click_Control(object sender, RoutedEventArgs e)
        {
            this.agregarVariableControl();
        }

        private void agregarVariableControl()
        {
            this.Agregar(AnalisisPrevio.VariablesDeControl);
            this.inicializarORecargarVariablesControl();
        }

        private void MenuItem_Click_Estado(object sender, RoutedEventArgs e)
        {
            this.agregarVariableEstado();            
        }

        private void agregarVariableEstado()
        {
            this.AgregarVariableVectorAP(AnalisisPrevio.VariablesEstado, VariableType.State);
            this.inicializarORecargarVariablesEstado();
        }

        private void MenuItem_Click_Resultado(object sender, RoutedEventArgs e)
        {
            this.agregarVariableResultado();
        }

        private void agregarVariableResultado()
        {
            this.AgregarVariableVectorAP(AnalisisPrevio.VariablesResultado, VariableType.Result);
            this.inicializarORecargarVariablesResultado();
        }

        private void MenuItem_Click_EliminarDato(object sender, RoutedEventArgs e)
        {
            this.eliminarDato();

        }

        private void eliminarDato()
        {
            try
            {
                this.Eliminar(AnalisisPrevio.Datos, true, "los Datos");
                this.inicializarORecargarDatos();
                this.dataGridEventosIndependientes.Items.Refresh();

            }
            catch (Exception ex)
            {
                new AlertPopUp(ex.Message).Show();
            }
        }

        private void MenuItem_Click_EliminarVarControl(object sender, RoutedEventArgs e)
        {
            this.eliminarVariableControl();
        }

        private void eliminarVariableControl()
        {
            this.Eliminar(AnalisisPrevio.VariablesDeControl, false, "las Variables de Control");
            this.inicializarORecargarVariablesControl();
        }

        private void MenuItem_Click_EliminarVarEstado(object sender, RoutedEventArgs e)
        {
            this.eliminarVariableEstado();
        }

        private void eliminarVariableEstado()
        {
            this.Eliminar(AnalisisPrevio.VariablesEstado, false, "las Variables de Estado");
            this.inicializarORecargarVariablesEstado();
        }

        private void MenuItem_Click_EliminarVarResultado(object sender, RoutedEventArgs e)
        {
            this.eliminarVariableResultado();
        }

        private void eliminarVariableResultado()
        {
            this.Eliminar(AnalisisPrevio.VariablesResultado, false, "las Variables de Resultado");
            this.inicializarORecargarVariablesResultado();
        }

        private void btnDeleteEvento_OnClick(object sender, RoutedEventArgs e)
        {
            EliminarEvento();
        }

        private void MenuItem_Click_Propio(object sender, RoutedEventArgs e)
        {
            Agregar(AnalisisPrevio.Propios);
        }

        private void MenuItem_Click_ComprometidoAnterior(object sender, RoutedEventArgs e)
        {
            Agregar(AnalisisPrevio.ComprometidosAnterior);
        }

        private void MenuItem_Click_ComprometidoFuturo(object sender, RoutedEventArgs e)
        {
            Agregar(AnalisisPrevio.ComprometidosFuturos);
        }

        private void MenuItem_Click_Tef(object sender, RoutedEventArgs e)
        {
            Agregar(AnalisisPrevio.Tefs);
        }

        private void MenuItem_Click_EliminarPropio(object sender, RoutedEventArgs e)
        {
            Eliminar(AnalisisPrevio.Propios, false, "los Eventos");
        }

        private void MenuItem_Click_EliminarComprometidoAnterior(object sender, RoutedEventArgs e)
        {
            Eliminar(AnalisisPrevio.ComprometidosAnterior, false, "los Eventos");
        }

        private void MenuItem_Click_EliminarComprometidoFuturo(object sender, RoutedEventArgs e)
        {
            Eliminar(AnalisisPrevio.ComprometidosFuturos, false, "los Eventos");
        }

        private void MenuItem_Click_EliminarTef(object sender, RoutedEventArgs e)
        {
            Eliminar(AnalisisPrevio.Tefs, false, "los Eventos");
        }

        private void MenuItem_Click_Condicion(object sender, RoutedEventArgs e)
        {
            AgregarCondicion();
        }

        private void btnGenerarDiagrama_OnClick(object sender, RoutedEventArgs e)
        {
            GenerarDiagrama();
        }
    
        private void btnGenerarFDP_OnClick(object sender, RoutedEventArgs e)
        {
            GenerarFDP();
        }


        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
            
        }

        private void MenuItem_Click_Manual_Usuario(object sender, RoutedEventArgs e)
        {
            DarPDFAlUsuario();
        }

        #endregion

        private void InicializarMetodologia()
        {
            var tipo = this.TipoSeleccionado(comboBox.SelectedItem);
            var tipoEaE = this.TipoEaESeleccionado(comboBox_EventosEaE.SelectedItem);
            
            if (this.AnalisisPrevio != null)
            {
                if(this.AnalisisPrevio.listFDP!= null)
                {
                    ObservableCollection<commonFDP.ResultadoAjuste> listaFdp = this.AnalisisPrevio.listFDP; 
                    this.AnalisisPrevio = new AnalisisPrevio(tipo, tipoEaE);
                    this.AnalisisPrevio.listFDP = listaFdp;
                }
                else
                {
                    this.AnalisisPrevio = new AnalisisPrevio(tipo, tipoEaE);
                }
                
            }

            else
            {
                this.AnalisisPrevio = new AnalisisPrevio(tipo, tipoEaE);
            }
               
            this.eventos.Visibility = tipo.Equals(AnalisisPrevio.Tipo.EaE) ? Visibility.Visible : Visibility.Hidden;
            this.eventosDeltaT.Visibility = tipo.Equals(AnalisisPrevio.Tipo.DeltaT) ? Visibility.Visible : Visibility.Hidden;
            this.nuevoEventoDeltaT.IsEnabled = tipo.Equals(AnalisisPrevio.Tipo.DeltaT);
            this.eliminarEventoDeltaT.IsEnabled = tipo.Equals(AnalisisPrevio.Tipo.DeltaT);
            this.nuevaCondicion.IsEnabled = tipo.Equals(AnalisisPrevio.Tipo.EaE);

            this.InitializeCollections();
        }

        private void Agregar(ObservableCollection<string> collection)
        {
            var popUp = new AddSimpleVariablePopUp();
            popUp.ShowDialog();

            if (popUp.Result == UI.SharedWPF.DialogResult.Accept)
            {
                string variable = popUp.nombreBox.Text;

                if (variable.ToUpper().Equals("TF") || variable.ToUpper().Equals("T") || variable.ToUpper().Equals("HV") || variable.ToUpper().Equals("R") || variable.ToUpper().Equals("I"))
                {
                    new AlertPopUp("Error. No puede agregarse una variable con ese nombre, se encuentra reservado.").Show();
                    return;
                }

                if (this.comboBox.SelectedItem == EaE)
                {
                    variable = variable.ToUpper();
                }

                if (!collection.Any(item => item == variable))
                {
                    collection.Add(variable);
                    new InformationPopUp("Variable creada con éxito.").ShowDialog();
                    return;
                }
                else
                {
                    new AlertPopUp("Error. Ya existe una variable con el nombre especificado.").Show();
                    return;
                }
            }
        }

        private void AgregarVariableVectorAP(ObservableCollection<VariableAP> collection, VariableType type)
        {
            var popUp = new AddVectorVariablePopUp();
            popUp.ShowDialog();

            if (popUp.Result == UI.SharedWPF.DialogResult.Accept)
            {
                string variable = popUp.nombreBox.Text.ToUpper();
                double index = ((bool)popUp.vector.IsChecked) ? 1 : 0;

                if (variable.ToUpper().Equals("TF") || variable.ToUpper().Equals("T") || variable.ToUpper().Equals("HV") || variable.ToUpper().Equals("R") || variable.ToUpper().Equals("I") || variable.ToUpper().Equals("N"))
                {
                    new AlertPopUp("No puede agregarse una variable con ese nombre, se encuentra reservado.").Show();
                    return;
                }

                if (!collection.Any(item => item.nombre == variable))
                {
                    collection.Add(new VariableAP() { nombre = variable, valor = 0, vector = (bool)popUp.vector.IsChecked, i = index, type = type });
                    new InformationPopUp("Variable creada con éxito.").ShowDialog();
                    return;
                }
                else
                {
                    new AlertPopUp("Error. Ya existe una variable con el nombre especificado.").Show();
                    return;
                }
            }
        }

        private void EliminarEvento()
        {
            if (this.AnalisisPrevio.TipoDeEaE == AnalisisPrevio.TipoEvento.Independiente)
            {
                this.eliminarEventoIndependiente();
            } 
            else
            {
                this.eliminarEventoTEventos();
            }
        }

        private void eliminarEventoTEventos()
        {
            var selected = dataGridEventos.SelectedItem as EventoAP;

            if (selected != null)
            {
                this.AnalisisPrevio.EventosEaE.Remove(selected);

                foreach (EventoAP evento in AnalisisPrevio.EventosEaE)
                {
                    this.eliminarEventoEFNC(evento, selected.Nombre);
                    this.eliminarEventoEFC(evento, selected.Nombre);
                }

                this.dataGridEventos.Items.Refresh();
                this.dataGridEventos.SelectedItem = null;

                new InformationPopUp("Evento eliminado con éxito.").ShowDialog();
            }
        }

        private void eliminarEventoIndependiente()
        {
            var selected = dataGridEventosIndependientes.SelectedItem as EventoAP;

            if (selected != null)
            {
                this.AnalisisPrevio.EventosEaE.Remove(selected);
                foreach (var evento in this.AnalisisPrevio.EventosEaE.Where(evento => evento.EventosCondicionados.Contains(selected.Nombre)))
                {
                    var index = evento.EventosCondicionados.IndexOf(selected.Nombre);
                    evento.EventosCondicionados.RemoveAt(index);
                    evento.Condiciones.Clear();
                }
                this.dataGridEventosIndependientes.Items.Refresh();
                this.dataGridEventosIndependientes.SelectedItem = null;

                new InformationPopUp("Evento eliminado con éxito.").ShowDialog();
            }
        }

        private void Eliminar<T>(ObservableCollection<T> collection, bool isDatos, string title)
        {
            var popUp = new DeleteSimpleVariablePopUp(title);
            popUp.listBox.ItemsSource = collection.ToList();
            popUp.ShowDialog();

            var toRemove = new List<T>();

            if (popUp.Result == UI.SharedWPF.DialogResult.Accept)
            {
                foreach (T selected in popUp.listBox.SelectedItems)
                {
                    if (!isDatos || (isDatos && !AnalisisPrevio.EventosEaE.Any(item => item.Encadenador == selected as string)))
                    {
                        toRemove.Add(selected);
                    }
                    else
                    {
                        throw new Exception("El dato '" + selected + "' no puede ser eliminado porque está asignado como Encadenador en la Tabla de Eventos.");
                    }
                }

                foreach (T removed in toRemove)
                {
                    collection.Remove(removed);
                }

                new InformationPopUp("Variable eliminada con éxito.").ShowDialog();
            }
        }

        private void AgregarCondicion()
        {
            var popUp = new AddNewConditionPopUp();
            popUp.ShowDialog();

            if (popUp.Result == UI.SharedWPF.DialogResult.Accept)
            {
                string condicion = popUp.nombreBox.Text;

                if (!this.Conditions.Contains(condicion))
                {
                    this.Conditions.Add(condicion);
                    new InformationPopUp("Condición creada con éxito.").ShowDialog();
                    return;
                }
                else
                {
                    new AlertPopUp("Error. Ya existe dicha condición.").Show();
                    return;
                }
            }
        }

        private void GenerarDiagrama()
        {
            ExpressionResolver.listFdpPreviusAnalisis = AnalisisPrevio.listFDP;
            AutomaticDiagramGenerator diagramGenerator = new AutomaticDiagramGenerator(AnalisisPrevio);
            diagramGenerator.generateDiagram(VentanaDiagramador);
        }

        private void GenerarFDP()
        {
            AddFDPPopUp FDPGenerator = new AddFDPPopUp();
            FDPGenerator.FDPGenerator(this.AnalisisPrevio);

            FDPGenerator.Show();
        }

        private void DarPDFAlUsuario()
        {
            var parentFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var sourcePath = System.IO.Path.Combine(parentFolder, @"Manual de usuario\Manual de usuario Victoria.pdf");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Manual de usuario Victoria.pdf";
            saveFileDialog.Filter = "Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(sourcePath, saveFileDialog.FileName, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private AnalisisPrevio.Tipo TipoSeleccionado(object selectedItem)
        {
            return selectedItem == EaE ? AnalisisPrevio.Tipo.EaE : AnalisisPrevio.Tipo.DeltaT;
        }

        private AnalisisPrevio.TipoEvento TipoEaESeleccionado(object selectedItem)
        {
            return selectedItem == TEI ? AnalisisPrevio.TipoEvento.Independiente : AnalisisPrevio.TipoEvento.ConDependencia;
        }

        private void comboBox_EventosEaE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.dataGridEventosIndependientes.Visibility = (comboBox_EventosEaE.SelectedItem == TEI) ? Visibility.Visible : Visibility.Hidden;
            this.dataGridEventos.Visibility = (comboBox_EventosEaE.SelectedItem == TEventos) ? Visibility.Visible : Visibility.Hidden;
            this.InicializarMetodologia();
        }

        private void TextBoxEFNCInd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxEFCInd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxCondInd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxEncInd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxEvento_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxEnc_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxTEF_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBoxEventoInd_LostFocus(object sender, RoutedEventArgs e)
        {
            string actualEventName = (sender as TextBox).Text;
            if (this.NewGridObject)
            {
                this.addNewEvent(actualEventName);
            }
            else
            {
                this.editEvent(actualEventName);
            }
        }

        private void editEvent(string actualEventName)
        {

            if (actualEventName != this.LastGridObject)
            {
                try
                {
                    this.validateEventName(actualEventName);
                    this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == this.LastGridObject).Nombre = actualEventName;
                }
                catch (ValidationException ex)
                {
                    new AlertPopUp(ex.Message).Show();
                }
                finally
                {
                    dataGridEventosIndependientes.Items.Refresh();
                }
            }
        }

        private void addNewEvent(string eventName)
        {
            const string TEF_TITTLE = "Variable TEF";

            if (!String.IsNullOrEmpty(eventName))
            {
                try
                {
                    this.validateEventName(eventName);
                    var popUp = new AddSimpleVariablePopUp(TEF_TITTLE);
                    popUp.ShowDialog();

                    if (popUp.Result == UI.SharedWPF.DialogResult.Accept)
                    {
                        string tefVariable = popUp.nombreBox.Text;
                        validateTEFName(tefVariable);
                        this.AnalisisPrevio.EventosEaE.Add(new EventoAP { Nombre = eventName, TEF = tefVariable.ToUpper() });
                    }

                }
                catch (ValidationException ex)
                {
                    new AlertPopUp(ex.Message).Show();
                }
                finally
                {
                    dataGridEventosIndependientes.Items.Refresh();
                }
            }

        }

        private void validateEventName(string actualEventName)
        {
            if (this.AnalisisPrevio.EventosEaE.Any(item => item.Nombre == actualEventName))
            {
                throw new ValidationException("Error. Ya existe un evento con el nombre especificado.");
            }

            if (actualEventName == null || actualEventName.Equals(""))
            {
                throw new ValidationException("No se puede crear el Evento. El campo Nombre es obligatorio.");
            }

        }

        private void TEICell_GotFocus(object sender, RoutedEventArgs e)
        {
            this.LastGridObject = (sender as TextBox).Text;
            this.NewGridObject = this.LastGridObject == String.Empty;
            e.Handled = true;
        }

        private void ComboBoxEFNCInd_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;

            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedEvent != null)
            {
                if (selectedEvent.EventosNoCondicionados.Count > 0)
                {
                    selectedEvent.EventosNoCondicionados.RemoveAt(0);
                }
                selectedEvent.EventosNoCondicionados.Add(selectedItem);

            }
            dataGridEventosIndependientes.Items.Refresh();
        }

        private void ComboBoxEFNCInd_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string efncEventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;
                if (!String.IsNullOrEmpty(efncEventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == efncEventName);
                    if (eventAP.EventosNoCondicionados.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(eventAP.EventosNoCondicionados.First())) cmb.SelectedIndex = 0;
                    }
                }
            }
        }

        private void ComboBoxEFCInd_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;
            if (selectedEvent == null)
            {
                new AlertPopUp("Primero debe crear el nombre del nuevo Evento").Show();
                cmb.SelectedItem = null;
                return;
            }
            
            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedEvent != null && selectedEvent.EventosCondicionados.Count > 0)
            {
                selectedEvent.EventosCondicionados.RemoveAt(0);   
            }
            selectedEvent.EventosCondicionados.Add(selectedItem);
            dataGridEventosIndependientes.Items.Refresh();
        }

        private void ComboBoxEFCInd_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string eventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;

                cmb.Items.Clear();
                var itemsSource = this.AnalisisPrevio.EventosEaE.Select(item => item.Nombre).ToList();
                itemsSource.Add(String.Empty);
                cmb.ItemsSource = itemsSource;
                if (!String.IsNullOrEmpty(eventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == eventName);
                    if (eventAP.EventosCondicionados.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(eventAP.EventosCondicionados.First())) cmb.SelectedItem = eventAP.EventosCondicionados.First();
                    }

                }
            }

        }

        private void ComboBoxCondInd_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string eventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;

                cmb.Items.Clear();
                var itemsSource = this.Conditions.ToList();
                itemsSource.Add(String.Empty);
                itemsSource.Add(AGREGAR_CONDICION);
                cmb.ItemsSource = itemsSource;
                if (!String.IsNullOrEmpty(eventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == eventName);
                    if (eventAP.Condiciones.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(eventAP.Condiciones.First())) cmb.SelectedItem = eventAP.Condiciones.First();
                    }
                }
            }
        }

        private void ComboBoxCondInd_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;
            if (selectedEvent == null)
            {
                new AlertPopUp("Primero debe crear el nombre del nuevo Evento").Show();
                cmb.SelectedItem = null;
                return;
            }            

            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedItem == AGREGAR_CONDICION)
            {
                this.AgregarCondicion();                
            }
            else if (selectedEvent != null && selectedEvent.Condiciones.Count > 0)
            {
                selectedEvent.Condiciones.RemoveAt(0);                
            }
            selectedEvent.Condiciones.Add(selectedItem);
            dataGridEventosIndependientes.Items.Refresh();
        }

        private void ComboBoxEncInd_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string eventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;

                cmb.Items.Clear();
                var itemsSource = this.AnalisisPrevio.Datos.ToList();
                itemsSource.AddRange(this.AnalisisPrevio.VariablesDeControl.ToList());
                itemsSource.Add(AGREGAR_ENCADENADOR);
                cmb.ItemsSource = itemsSource;
                if (!String.IsNullOrEmpty(eventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == eventName);
                    if (!String.IsNullOrEmpty(eventAP.Encadenador)) cmb.SelectedItem = eventAP.Encadenador;
                }
            }
        }

        private void ComboBoxEncInd_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;
            if (selectedEvent == null)
            {
                new AlertPopUp("Primero debe crear el nombre del nuevo Evento").Show();
                cmb.SelectedItem = null;
                return;
            }

            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedItem == AGREGAR_ENCADENADOR)
            {
                this.agregarDato();                
            }
            else if(selectedEvent != null)
            {
                selectedEvent.Encadenador = selectedItem; 
            }
            dataGridEventosIndependientes.Items.Refresh();
        }

        private void TextBoxTEFInd_LostFocus(object sender, RoutedEventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);

            string actualTEFName = (sender as TextBox).Text;

            if (selectedEvent != null && actualTEFName != selectedEvent.TEF)
            {
                try
                {
                    this.validateTEFName(actualTEFName);
                    selectedEvent.TEF = actualTEFName;
                }
                catch (ValidationException ex)
                {
                    new AlertPopUp(ex.Message).Show();
                }
                finally
                {
                    dataGridEventosIndependientes.Items.Refresh();
                }
            }
        }

        private void validateTEFName(string tefVariable)
        {
            if (this.AnalisisPrevio.EventosEaE.Any(item => item.TEF == tefVariable.ToUpper()))
            {
                throw new ValidationException("Error. Ya existe un evento con el campo TEF especificado.");
            }

            if (tefVariable.ToUpper().Equals("TF") || tefVariable.ToUpper().Equals("T") || tefVariable.ToUpper().Equals("HV") || tefVariable.ToUpper().Equals("R"))
            {
                throw new ValidationException("Error. No puede agregarse un evento con ese nombre, se encuentra reservado.");
            }

            if (tefVariable == null || tefVariable.Equals(""))
            {
                throw new ValidationException("No se puede crear el Evento. El campo TEF es obligatorio.");
            }

        }

        private void datosTabControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (selectedItem == AGREGAR_ENCADENADOR)
            {
                this.agregarDato();
            }
        }

        private void datosTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (e.Key == Key.Delete && selectedItem != AGREGAR_ENCADENADOR)
            {
                this.eliminarDatosSeleccionado(selectedItem);
            }

            if (e.Key == Key.Enter && selectedItem == AGREGAR_ENCADENADOR)
            {
                this.agregarDato();
            }
        }

        private void eliminarDatosSeleccionado(string selectedItem)
        {
            try
            {
                this.EliminarVariableEspecifica(AnalisisPrevio.Datos, true, selectedItem);
                this.inicializarORecargarDatos();
            }
            catch (Exception ex)
            {
                new AlertPopUp(ex.Message).Show();
            }
            
        }

        private void EliminarVariableEspecifica<T>(ObservableCollection<T> collection, bool isDatos, T element)
        {
            if (isDatos && AnalisisPrevio.EventosEaE.Any(item => item.Encadenador == element as string))
                throw new Exception("El dato '" + element + "' no puede ser eliminado porque está asignado como Encadenador en la Tabla de Eventos.");

            collection.Remove(element);
            
            new InformationPopUp("Variable eliminada con éxito.").ShowDialog();
            this.dataGridEventosIndependientes.Items.Refresh();
        }

        private void variablesControlTabControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (selectedItem == AGREGAR_VARIABLE_CONTROL)
            {
                this.agregarVariableControl();
            }
        }

        private void variablesControlTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;

            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (e.Key == Key.Delete && selectedItem != AGREGAR_VARIABLE_CONTROL)
            {
                this.eliminarVariableControlSeleccionada(selectedItem);
            }

            if (e.Key == Key.Enter && selectedItem == AGREGAR_VARIABLE_CONTROL)
            {
                this.agregarVariableControl();
            }
        }

        private void eliminarVariableControlSeleccionada(string selectedItem)
        {
            try
            {
                this.EliminarVariableEspecifica(AnalisisPrevio.VariablesDeControl, false, selectedItem);
                this.inicializarORecargarVariablesControl();
            }
            catch (Exception ex)
            {
                new AlertPopUp(ex.Message).Show();
            }
        }

        private void ListViewEFNC_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var loadedEvent = ((listView).Items.GetItemAt(0)) as ListViewItem;

            if (loadedEvent != null && loadedEvent.HasContent)
            {
                EventoAP eventAP = this.dataGridEventos.ItemsSource.OfType<EventoAP>().First(item => item.Nombre == loadedEvent.Content);

                var itemsSource = eventAP.EventosNoCondicionados.ToList();
                itemsSource.Add(AGREGAR_EVENTO);

                listView.Items.Clear();
                listView.ItemsSource = itemsSource;
            }
        }

        private void variablesEstadoTabControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (selectedItem == AGREGAR_VARIABLE_ESTADO)
            {
                this.agregarVariableEstado();
            }
        }

        private void variablesEstadoTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (e.Key == Key.Delete && selectedItem != AGREGAR_VARIABLE_ESTADO)
            {
                this.eliminarVariableEstadoSeleccionada(selectedItem);
            }

            if (e.Key == Key.Enter && selectedItem == AGREGAR_VARIABLE_ESTADO)
            {
                this.agregarVariableEstado();
            }
        }

        private void variablesResultadoTabControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (selectedItem == AGREGAR_VARIABLE_RESULTADO)
            {
                this.agregarVariableResultado();
            }
        }

        private void variablesResultadoTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedItem = (sender as ListBox).SelectedItem as String;
            selectedItem = !String.IsNullOrEmpty(selectedItem) ? selectedItem : String.Empty;

            if (e.Key == Key.Delete && selectedItem != AGREGAR_VARIABLE_RESULTADO)
            {
                this.eliminarVariableResultadoSeleccionada(selectedItem);
            }

            if (e.Key == Key.Enter && selectedItem == AGREGAR_VARIABLE_RESULTADO)
            {
                this.agregarVariableResultado();
            }
        }

        private void eliminarVariableEstadoSeleccionada(string selectedItem)
        {
            VariableAP toRemove = AnalisisPrevio.VariablesEstado.First(variable => variable.nombre == selectedItem);
            AnalisisPrevio.VariablesEstado.Remove(toRemove);

            new InformationPopUp("Variable eliminada con éxito.").ShowDialog();
            this.inicializarORecargarVariablesEstado();
        }

        private void eliminarVariableResultadoSeleccionada(string selectedItem)
        {
            VariableAP toRemove = AnalisisPrevio.VariablesResultado.First(variable => variable.nombre == selectedItem);
            AnalisisPrevio.VariablesResultado.Remove(toRemove);

            new InformationPopUp("Variable eliminada con éxito.").ShowDialog();
            this.inicializarORecargarVariablesResultado();
        }

        private void ListViewEFC_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var loadedEvent = ((listView).Items.GetItemAt(0)) as ListViewItem;

            if (loadedEvent != null && loadedEvent.HasContent)
            {
                EventoAP eventAP = this.dataGridEventos.ItemsSource.OfType<EventoAP>().First(item => item.Nombre == loadedEvent.Content);

                var itemsSource = eventAP.EventosCondicionados.ToList();
                itemsSource.Add(AGREGAR_EVENTO);

                listView.Items.Clear();
                listView.ItemsSource = itemsSource;
            }
        }

        private void ComboBoxEnc_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventos.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;
            if (selectedEvent == null)
            {
                new AlertPopUp("Primero debe crear el nombre del nuevo Evento").Show();
                cmb.SelectedItem = null;
                return;
            }

            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedItem == AGREGAR_ENCADENADOR)
            {
                this.agregarDato();
            }
            else if (selectedEvent != null)
            {
                selectedEvent.Encadenador = selectedItem;
            }
            dataGridEventos.Items.Refresh();
        }

        private void ComboBoxEnc_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string eventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;

                cmb.Items.Clear();
                var itemsSource = this.AnalisisPrevio.Datos.ToList();
                itemsSource.AddRange(this.AnalisisPrevio.VariablesDeControl.ToList());
                itemsSource.Add(AGREGAR_ENCADENADOR);
                cmb.ItemsSource = itemsSource;
                if (!String.IsNullOrEmpty(eventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == eventName);
                    if (!String.IsNullOrEmpty(eventAP.Encadenador)) cmb.SelectedItem = eventAP.Encadenador;
                }
            }
        }

        private void ListViewEFNC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedEvent = this.dataGridEventos.SelectedItem as EventoAP;
            if (selectedEvent != null)
            {
                var selectedItem = (sender as ListView).SelectedValue;
                if (selectedItem != null && selectedItem.GetType() == typeof(string))
                {
                    var selectedText = ((sender as ListView).SelectedItem).ToString();
                    if (selectedText == AGREGAR_EVENTO)
                    {
                        this.agregarEventoEFNC(selectedEvent);
                    }
                    else
                    {
                        this.editarEventoEFNC(selectedEvent, selectedText);
                    }
                }

            }

        }

        private void editarEventoEFNC(EventoAP selectedEvent, string selectedItem)
        {
            var editEventPopUp = new EditEventPopUp(selectedItem);
            editEventPopUp.ShowDialog();
            if (editEventPopUp.Result == UI.SharedWPF.DialogResult.Accept)
            {
                var editedItem = editEventPopUp.txtEvent.Text;
                this.editEventTEventos(selectedItem, editedItem);
            }
            else if (editEventPopUp.Result == UI.SharedWPF.DialogResult.Other)
            {
                this.eliminarEventoEFNC(selectedEvent, selectedItem);
                new InformationPopUp("Evento Eliminado correctamente").ShowDialog();
            }
        }

        private void agregarEventoEFNC(EventoAP selectedEvent)
        {
            var eventList = this.AnalisisPrevio.EventosEaE.Select(evento => evento.Nombre)
                .Where(evento => !selectedEvent.EventosNoCondicionados.Contains(evento));
 
            var addEventWindow = new AddEventPopUp(eventList);
            addEventWindow.ShowDialog();

            if (addEventWindow.Result == UI.SharedWPF.DialogResult.Accept)
            {
                selectedEvent.EventosNoCondicionados.Add(addEventWindow.cmbEvent.SelectedItem.ToString());
                this.dataGridEventos.Items.Refresh();
            }
        }

        private void ListViewEFNC_KeyDown(object sender, KeyEventArgs e)
        {
            var selectedEvent = this.dataGridEventos.SelectedItem as EventoAP;
            var listView = sender as ListView;
            if (listView != null && listView.SelectedItem != null)
            {
                var selectedItem = (listView.SelectedItem).ToString();
            
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    if (selectedItem == AGREGAR_EVENTO)
                    {
                        this.agregarEventoEFNC(selectedEvent);
                    }
                    else
                    {
                        this.editarEventoEFNC(selectedEvent, selectedItem);
                    }
                }

                if (e.Key == Key.Delete && selectedItem != AGREGAR_EVENTO)
                {
                    e.Handled = true;
                    this.eliminarEventoEFNC(selectedEvent, selectedItem);
                    new InformationPopUp("Evento Eliminado correctamente").ShowDialog();
                }
            }
        }

        private void eliminarEventoEFNC(EventoAP selectedEvent, string selectedItem)
        {
            selectedEvent.EventosNoCondicionados.Remove(selectedItem);
            this.dataGridEventos.Items.Refresh();
        }

        private void TextBoxEvento_LostFocus(object sender, RoutedEventArgs e)
        {
            var newName = (sender as TextBox).Text;
            if (this.NewGridObject)
            {
                this.addNewEvent(newName);
                this.dataGridEventos.Items.Refresh();
            }
            else
            {
                var eventName = (this.dataGridEventos.SelectedItem as EventoAP).Nombre;

                if (newName != eventName)
                {
                    this.editEventTEventos(eventName, newName);
                }
            }
        }


        private void editEventTEventos(string eventName, string editedName)
        {
            try
            {
                this.validateEventName(editedName);

                foreach (EventoAP evento in this.AnalisisPrevio.EventosEaE)
                {
                    if (evento.Nombre == eventName)
                    {
                        evento.Nombre = editedName;
                    }

                    if (evento.EventosNoCondicionados.Contains(eventName))
                    {
                        evento.EventosNoCondicionados[evento.EventosNoCondicionados.IndexOf(eventName)] = editedName;
                    }

                    if (evento.EventosCondicionados.Contains(eventName))
                    {
                        evento.EventosCondicionados[evento.EventosCondicionados.IndexOf(eventName)] = editedName;
                    }
                }
            }

            catch (ValidationException ex)
            {
                new AlertPopUp(ex.Message).Show();
            }

            finally
            {
                this.dataGridEventos.Items.Refresh();
            }
        }

        private void ListViewEFC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedEvent = this.dataGridEventos.SelectedItem as EventoAP;
            if (selectedEvent != null)
            {
                var selectedItem = (sender as ListView).SelectedValue;
                if (selectedItem != null && selectedItem.GetType() == typeof(string))
                {
                    var selectedText = ((sender as ListView).SelectedItem).ToString();
                    if (selectedText == AGREGAR_EVENTO)
                    {
                        this.agregarEventoEFC(selectedEvent);
                    }
                    else
                    {
                        this.editarEventoEFC(selectedEvent, selectedText);
                    }
                }
            }
  
        }

        private void editarEventoEFC(EventoAP selectedEvent, string selectedItem)
        {
            var editEventPopUp = new EditEventPopUp(selectedItem);
            editEventPopUp.ShowDialog();
            if (editEventPopUp.Result == UI.SharedWPF.DialogResult.Accept)
            {
                var editedItem = editEventPopUp.txtEvent.Text;
                this.editEventTEventos(selectedItem, editedItem);
            }
            else if (editEventPopUp.Result == UI.SharedWPF.DialogResult.Other)
            {
                this.eliminarEventoEFC(selectedEvent, selectedItem);
                new InformationPopUp("Evento Eliminado correctamente").ShowDialog();
            }

        }

        private void agregarEventoEFC(EventoAP selectedEvent)
        {
            var eventList = this.AnalisisPrevio.EventosEaE.Select(evento => evento.Nombre)
             .Where(evento => !selectedEvent.EventosCondicionados.Contains(evento));

            var addEventWindow = new AddEventPopUp(eventList);
            addEventWindow.ShowDialog();

            if (addEventWindow.Result == UI.SharedWPF.DialogResult.Accept)
            {
                var selectConditionWindow = new SelectCondicionPopUp(this.Conditions);
                selectConditionWindow.ShowDialog();

                if (selectConditionWindow.Result == UI.SharedWPF.DialogResult.Accept)
                {
                    selectedEvent.EventosCondicionados.Add(addEventWindow.cmbEvent.SelectedItem.ToString());
                    selectedEvent.Condiciones.Add(selectConditionWindow.cmbCondition.SelectedItem.ToString());
                    this.dataGridEventos.Items.Refresh();
                }
            }
        }

        private void ListViewEFC_KeyDown(object sender, KeyEventArgs e)
        {
            var selectedEvent = this.dataGridEventos.SelectedItem as EventoAP;
            var listView = sender as ListView;
            if (listView != null && listView.SelectedItem != null)
            {
                var selectedItem = (listView.SelectedItem).ToString();

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    if (selectedItem == AGREGAR_EVENTO)
                    {
                        this.agregarEventoEFC(selectedEvent);
                    }
                    else
                    {
                        this.editarEventoEFC(selectedEvent, selectedItem);
                    }
                }

                if (e.Key == Key.Delete && selectedItem != AGREGAR_EVENTO)
                {
                    e.Handled = true;
                    this.eliminarEventoEFC(selectedEvent, selectedItem);
                    new InformationPopUp("Evento Eliminado correctamente").ShowDialog();
                }
            }
        }

        private void eliminarEventoEFC(EventoAP selectedEvent, string selectedItem)
        {
            var conditionIndex = selectedEvent.EventosCondicionados.IndexOf(selectedItem);
            if (conditionIndex >= 0)
            {
                selectedEvent.EventosCondicionados.Remove(selectedItem);
                selectedEvent.Condiciones.RemoveAt(conditionIndex);
                this.dataGridEventos.Items.Refresh();
            }
        }

        private void ListViewCondiciones_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = sender as ListView;
            var loadedEvent = ((listView).Items.GetItemAt(0)) as ListViewItem;

            if (loadedEvent != null && loadedEvent.HasContent)
            {
                EventoAP eventAP = this.dataGridEventos.ItemsSource.OfType<EventoAP>().First(item => item.Nombre == loadedEvent.Content);

                var itemsSource = eventAP.Condiciones.ToList();
                itemsSource.Add(AGREGAR_CONDICION);

                listView.Items.Clear();
                listView.ItemsSource = itemsSource;
            }
        }

        private void ListViewCondiciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedEvent = this.dataGridEventos.SelectedItem as EventoAP;
            if (selectedEvent != null)
            {
                var selectedItem = (sender as ListView).SelectedValue;
                if (selectedItem != null && selectedItem.GetType() == typeof(string))
                {
                    var selectedText = ((sender as ListView).SelectedItem).ToString();
                    if (selectedText == AGREGAR_CONDICION)
                    {
                        this.AgregarCondicion();
                    }
                    else
                    {
                        this.editarCondicion(selectedEvent, selectedText);
                    }
                }
            }

        }

        private void editarCondicion(EventoAP selectedEvent, string selectedItem)
        {
            var selectConditionWindow = new SelectCondicionPopUp(this.Conditions);
            selectConditionWindow.ShowDialog();

            if (selectConditionWindow.Result == UI.SharedWPF.DialogResult.Accept)
            {
                selectedEvent.Condiciones[selectedEvent.Condiciones.IndexOf(selectedItem)] = selectConditionWindow.cmbCondition.SelectedItem.ToString();
                this.dataGridEventos.Items.Refresh();
            }
        }

        private void ListViewCondiciones_KeyDown(object sender, KeyEventArgs e)
        {
            var selectedEvent = this.dataGridEventos.SelectedItem as EventoAP;
            var listView = sender as ListView;
            if (listView != null && listView.SelectedItem != null)
            {
                var selectedItem = (listView.SelectedItem).ToString();

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    if (selectedItem == AGREGAR_CONDICION)
                    {
                        this.AgregarCondicion();
                    }
                    else
                    {
                        this.editarCondicion(selectedEvent, selectedItem);
                    }
                }
            }
        }

        private void TextBoxTEF_LostFocus(object sender, RoutedEventArgs e)
        {
            var selectedEvent = (this.dataGridEventos.SelectedItem as EventoAP);

            string actualTEFName = (sender as TextBox).Text;

            if (selectedEvent != null && actualTEFName != selectedEvent.TEF)
            {
                try
                {
                    this.validateTEFName(actualTEFName);
                    selectedEvent.TEF = actualTEFName;
                }
                catch (ValidationException ex)
                {
                    new AlertPopUp(ex.Message).Show();
                }
                finally
                {
                    dataGridEventos.Items.Refresh();
                }
            }
        }

        private void ComboBoxDimInd_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;
            if (selectedEvent == null)
            {
                new AlertPopUp("Primero debe crear el nombre del nuevo Evento").Show();
                cmb.SelectedItem = null;
                return;
            }

            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedEvent != null)
            {
                if (!selectedEvent.Vector)
                {
                    if (cmb.SelectedItem != null) new AlertPopUp("No es posible agregar dimensión si el elemento no es un Vector").Show();
                    cmb.SelectedItem = null;
                    return;
                }
                else
                {
                    selectedEvent.Dimension = selectedItem;
                }

            }
            dataGridEventosIndependientes.CommitEdit(DataGridEditingUnit.Row, true);
            dataGridEventosIndependientes.Items.Refresh();
        }

        private void ComboBoxDimInd_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string eventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;

                cmb.Items.Clear();
                var itemsSource = this.AnalisisPrevio.VariablesDeControl.ToList();
                cmb.ItemsSource = itemsSource;
                if (!String.IsNullOrEmpty(eventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == eventName);
                    if (!String.IsNullOrEmpty(eventAP.Dimension)) cmb.SelectedItem = eventAP.Dimension;
                }
            }
        }

        private void ComboBoxDim_DropDownClosed(object sender, EventArgs e)
        {
            var selectedEvent = (this.dataGridEventos.SelectedItem as EventoAP);
            ComboBox cmb = sender as ComboBox;
            if (selectedEvent == null)
            {
                new AlertPopUp("Primero debe crear el nombre del nuevo Evento").Show();
                cmb.SelectedItem = null;
                return;
            }

            string selectedItem = cmb.SelectionBoxItem.ToString();
            if (selectedEvent != null)
            {
                if (!selectedEvent.Vector)
                {
                    if(cmb.SelectedItem != null) new AlertPopUp("No es posible agregar dimensión si el elemento no es un Vector").Show();
                    cmb.SelectedItem = null;
                    return;
                }
                else 
                {
                    selectedEvent.Dimension = selectedItem;
                }
                
            }
            dataGridEventos.Items.Refresh();
        }

        private void ComboBoxDim_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ComboBoxItem cmbFirstItem = cmb.Items.GetItemAt(0) as ComboBoxItem;
            if (cmbFirstItem != null)
            {
                string eventName = cmbFirstItem.HasContent ? cmbFirstItem.Content.ToString() : String.Empty;

                cmb.Items.Clear();
                var itemsSource = this.AnalisisPrevio.VariablesDeControl.ToList();
                cmb.ItemsSource = itemsSource;
                if (!String.IsNullOrEmpty(eventName))
                {
                    EventoAP eventAP = this.AnalisisPrevio.EventosEaE.First(evento => evento.Nombre == eventName);
                    if (!String.IsNullOrEmpty(eventAP.Dimension)) cmb.SelectedItem = eventAP.Dimension;
                }
            }
        }

        private void CheckBoxVectorInd_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = (this.dataGridEventosIndependientes.SelectedItem as EventoAP);
            CheckBox check = sender as CheckBox;

            if (check.IsChecked.Value)
            {
                if (AnalisisPrevio.VariablesDeControl.Count == 0)
                {
                    check.IsChecked = false;
                    new AlertPopUp("Sólo pueden haber vectores si hay al menos una variable de control asociada").Show();
                    return;
                }
                else 
                {
                    selectedEvent.Vector = true;
                    selectedEvent.Dimension = AnalisisPrevio.VariablesDeControl.First();
                }
                
            }
            else
            {
                selectedEvent.Vector = false;
                selectedEvent.Dimension = null;
            }

            dataGridEventosIndependientes.CommitEdit(DataGridEditingUnit.Row, true);
            dataGridEventosIndependientes.Items.Refresh(); 
        }

        private void CheckBoxVector_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = (this.dataGridEventos.SelectedItem as EventoAP);
            CheckBox check = sender as CheckBox;

            if (check.IsChecked.Value)
            {
                if (AnalisisPrevio.VariablesDeControl.Count == 0)
                {
                    check.IsChecked = false;
                    new AlertPopUp("Sólo pueden haber vectores si hay al menos una variable de control asociada").Show();
                    return;
                }
                else
                {
                    selectedEvent.Vector = true;
                    selectedEvent.Dimension = AnalisisPrevio.VariablesDeControl.First();
                }

            }
            else
            {
                selectedEvent.Vector = false;
                selectedEvent.Dimension = null;
            }

            dataGridEventos.CommitEdit(DataGridEditingUnit.Row, true);
            dataGridEventos.Items.Refresh(); 
        }


    }
}
