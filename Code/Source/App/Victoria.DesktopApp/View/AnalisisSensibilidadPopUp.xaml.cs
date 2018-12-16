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
using System.Text.RegularExpressions;
using WinForms = System.Windows.Forms;
using Victoria.ViewModelWPF;
using Victoria.Shared;
using Victoria.DesktopApp.Helpers;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for AnalisisSensibilidadPopUp.xaml
    /// </summary>
    public partial class AnalisisSensibilidadPopUp : Window
    {
        private static object _syncLock = new object();
        private List<ModelWPF.Variable> variablesDeControl;
        private string simulationFile;

        public struct ItemAnalisisSensibilidad
        {
            public GroupBox groupBox;
            public Button button;

            public ItemAnalisisSensibilidad(GroupBox gb, Button trash)
            {
                this.groupBox = gb;
                this.button = trash;
            }
        }

        private List<ItemAnalisisSensibilidad> gbList { get; set; }

        private int numeroEscenarios { get; set; }

        private double factorAlineacionGb {get; set;}
        
        private double posicionInicialGb {get; set;}

        private double posicionInicialBtnNuevoEscenario { get; set; }

        private double posicionInicialBtnSimular { get; set; }

        private double posicionInicialTrash { get; set; }

        private bool primerAgregado { get; set; }

        private int gbAgregados { get; set; }

        public AnalisisSensibilidadPopUp(string simulationFile)
        {
            InitializeComponent();

            this.simulationFile = simulationFile;
            Simulation simulation = XMLParser.GetSimulation(this.simulationFile);

            List<ModelWPF.Variable> variablesDeControl = new List<ModelWPF.Variable>();
            foreach (var variable in simulation.GetVariables())
            {
                if (variable.Type == VariableType.Control)
                {
                    variablesDeControl.Add(new ModelWPF.Variable(variable));
                }
            }
            this.variablesDeControl = variablesDeControl;

            CargarVariablesGroupBox(Wpanel_Escenario1);
            CargarVariablesGroupBox(Wpanel_Escenario2);

            BtnSimular.IsEnabled = false;

            GbEscenario2.Margin = new Thickness(52, GbEscenario1.Margin.Top + GbEscenario1.ActualHeight + 40, 99, 0);
            numeroEscenarios = 2;

            primerAgregado = true;

            gbList = new List<ItemAnalisisSensibilidad>();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonNuevoEscenario_Click(object sender, RoutedEventArgs e)
        {
            var nombresGroupBox = gbList.Select(x => x.groupBox.Header.ToString()).ToList();

            var nameEscenarioPopUp = new AddNameEscenarioPopUp(nombresGroupBox, GbEscenario1.Header.ToString(), GbEscenario2.Header.ToString());
            nameEscenarioPopUp.ShowDialog();    

            string nombreEscenario;
            switch (nameEscenarioPopUp.Result)
            {
                case UI.SharedWPF.DialogResult.Accept:
                    nombreEscenario = nameEscenarioPopUp.nombreEscenario;
                    break;

                default:
                    return;
            }

            if (primerAgregado)
            {
                InicializarVariablesDePosicion();
                primerAgregado = false;
            };

            numeroEscenarios += 1;

            gbAgregados = numeroEscenarios - 3;

            //Creacion del groupBox.
            GroupBox gb = CrearGroupBox(gbAgregados, nombreEscenario);

            //Creacion del boton que contiene el trash.
            Button btnTrash = CrearTrash(gbAgregados, nombreEscenario);

            var item = new ItemAnalisisSensibilidad(gb, btnTrash);
            gbList.Add(item);

            BtnNuevoEscenario.Margin = new Thickness(213, (GbEscenario1.ActualHeight + 40) * gbList.Count + posicionInicialGb > posicionInicialBtnNuevoEscenario ? (GbEscenario1.ActualHeight + 40) * gbList.Count + posicionInicialGb : posicionInicialBtnNuevoEscenario, 0, 0);
            BtnSimular.Margin = new Thickness(399, (GbEscenario1.ActualHeight + 40) * gbList.Count + posicionInicialGb > posicionInicialBtnSimular ? (GbEscenario1.ActualHeight + 40) * gbList.Count + posicionInicialGb : posicionInicialBtnSimular, 135, 125);

            //Agrego elementos a la vista.
            this.eventosDeltaT.Children.Add(gb);
            this.eventosDeltaT.Children.Add(btnTrash);

            ScrollBar.ScrollToEnd();
            BtnNuevoEscenario.Focusable = false;
        }

        //ASDF Reemplazar gbAgregados.
        private GroupBox CrearGroupBox(int gbAgregados, string nombreEscenario)
        {
            var nombreEscenarioSinEspacios = nombreEscenario.Replace(" ", string.Empty);
            GroupBox gb = new GroupBox();
            gb.Name = "GbEscenario_" + nombreEscenarioSinEspacios;
            gb.Header = nombreEscenario;
            gb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            gb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            gb.Margin = new Thickness(52, numeroEscenarios > 3 ? posicionInicialGb + factorAlineacionGb * gbAgregados : posicionInicialGb, 99, 0);
            gb.Width = 602;

            WrapPanel wPanel = new WrapPanel();

            int i = 0;
            foreach (var variable in variablesDeControl)
            //for (int i = 1; i <= variablesDeControl.Count(); i++)
            {
                Label lbl = new Label
                {
                    Name = "Lbl_" + variable.Name,
                    Content = variable.Name + ":",
                };

                TextBox txtbox = new TextBox
                {
                    Name = "TxtBox_" + variable.Name,
                    Height = 23,
                    Width = 120
                };

                txtbox.KeyDown += txtbox_KeyDown;
                txtbox.TextChanged += txtbox_TextChanged;

                if ((i % 2) != 0)
                {
                    lbl.Margin = new Thickness(20, 0, 0, 0);
                } 
                else
                {
                    lbl.Margin = new Thickness(50, 0, 0, 0);
                }

                wPanel.Children.Add(lbl);
                wPanel.Children.Add(txtbox);
            }

            gb.Content = wPanel;

            return gb;
        }

        //Verifica que los caracteres ingresados en los textbox sean numéricos admitiendo decimales
        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtbox = sender as TextBox;

            if (((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal))
            {
                if (txtbox.Text.Contains('.') && e.Key == Key.Decimal || txtbox.Text.Count() == 0  && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            } 
            else
            {
                e.Handled = true;
            }
        }

        private Button CrearTrash(int gbAgregados, string nombreEscenario)
        {
            var nombreEscenarioSinEspacios = nombreEscenario.Replace(" ", string.Empty);
            Button trashBtn = new Button();
            trashBtn.Name = "BtnTrash_" + nombreEscenarioSinEspacios;
            trashBtn.BorderBrush = Brushes.Transparent;
            trashBtn.Background = Brushes.Transparent;
            trashBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            trashBtn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            trashBtn.Width = 41;
            trashBtn.Height = 40;
            trashBtn.Margin = new Thickness(669, numeroEscenarios > 3 ? posicionInicialTrash + factorAlineacionGb * gbAgregados : posicionInicialTrash, 0, 0);
            trashBtn.Click += new RoutedEventHandler(BtnTrash_Click);
            
            trashBtn.Content = new Image
            {
                Source = new BitmapImage(new Uri("/Victoria.UI.SharedWPF;Component/resources/trashG.png", UriKind.RelativeOrAbsolute)),
                VerticalAlignment = VerticalAlignment.Center
            };

            return trashBtn;
        }

        private void InicializarVariablesDePosicion()
        {
            factorAlineacionGb = GbEscenario1.ActualHeight + 40;

            //La posicionInicialGb esta compuesta por la altura de los gb fijos + las distancias entre gbs + la distancia con el tercer gb
            posicionInicialGb = (GbEscenario1.ActualHeight + 40) * 2 + 40;

            posicionInicialBtnNuevoEscenario = BtnNuevoEscenario.Margin.Top;

            posicionInicialBtnSimular = BtnSimular.Margin.Top;

            //Para el Trash respetamos el Top de los gb
            posicionInicialTrash = posicionInicialGb;
        }

        private void BtnTrash_Click(object sender, RoutedEventArgs e)
        {
            Button trash = sender as Button;

            var item = gbList.Single(x => x.button.Name == trash.Name);
            var index = gbList.FindIndex(x => x.button.Name == trash.Name);

            for (int j = index + 1; j < gbList.Count; j++)
            {
                gbList[j].groupBox.Margin = new Thickness(52, numeroEscenarios > 3 ? gbList[j].groupBox.Margin.Top - factorAlineacionGb : posicionInicialGb, 99, 0);
                gbList[j].button.Margin = new Thickness(669, numeroEscenarios > 3 ? gbList[j].button.Margin.Top - factorAlineacionGb : posicionInicialTrash, 0, 0);
            }

            //Elimino groupbox y trash de la pantalla
            eventosDeltaT.Children.Remove(item.groupBox);
            eventosDeltaT.Children.Remove(item.button);

            //Actualizo lista de groupBox
            gbList.Remove(item);

            numeroEscenarios--;
            gbAgregados--;

            //Cambio la ubicación del boton simular y del boton nuevo escenario
            BtnNuevoEscenario.Margin = new Thickness(213, ScrollBar.VerticalOffset > GbEscenario1.ActualHeight && gbList.Count != 0 ? BtnNuevoEscenario.Margin.Top - factorAlineacionGb : posicionInicialBtnNuevoEscenario, 0, 0);
            BtnSimular.Margin = new Thickness(399, ScrollBar.VerticalOffset > GbEscenario1.ActualHeight && gbList.Count != 0 ? BtnSimular.Margin.Top - factorAlineacionGb : posicionInicialBtnSimular, 135, 125);
        }

        private void ButtonSimular_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();

            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory; 

            if (folderDialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                var fechaHoy = DateTime.Now;
                var nombreCarpeta = "analisisSensibilidad-" + fechaHoy.Year.ToString() + fechaHoy.Month.ToString() + fechaHoy.Day.ToString() + fechaHoy.Hour.ToString() + fechaHoy.Minute.ToString();
                var simulationPath = folderDialog.SelectedPath + "\\" + nombreCarpeta;

                Directory.CreateDirectory(simulationPath);
                var filePath = simulationPath + "\\simulacion.vic";
                // Genero un nuevo .vic y lo guardo para cada uno de los stages que se cargaron
                List<string> vicPaths = this.generateSensibilityStagesVic(simulationPath, this.simulationFile);

                var simulacionPopUp = new AnalisisSensibilidadSimulacionPopUp(simulationPath, vicPaths, this);
                simulacionPopUp.ShowDialog();
            }

            BtnSimular.Focusable = false;
        }

        private List<string> generateSensibilityStagesVic(string simulationPath, string simulationFile)
        {
            List<string> vicPaths = new List<string>();
            Simulation simulation = XMLParser.GetSimulation(simulationFile);
            List<Variable> originalVariables = simulation.GetVariables();
            
            List<GroupBox> groupBoxes = this.GetAllGroupBoxes();
            foreach (var groupBox in groupBoxes)
            {
                List<TextBox> txtBoxList = new List<TextBox>();
                txtBoxList.AddRange(((WrapPanel)groupBox.Content).Children.OfType<TextBox>());

                // ACTUALIZO VALORES DE VARIABLES DE CONTROL
                List<Variable> variables = new List<Variable>();
                foreach (var variable in originalVariables)
                {
                    if (variable.Type == VariableType.Control)
                    {
                        TextBox txtBox = txtBoxList.Find(element => element.Name.Equals("TxtBox_" + variable.Name));
                        if (txtBox != null)
                        {
                            double value = Convert.ToDouble(txtBox.Text);
                            variable.InitialValue = value;
                        }
                    }
                    variables.Add(variable);
                }
                // AHORA ACTUALIZO TAMAÑO DE LAS VARIABLES VECTORES
                foreach (var variable in variables)
                {
                    if (variable is VariableArray)
                    {
                        var variableArray = (VariableArray)variable;
                        int actualLength = variableArray.Variables.Count();
                        double newLength = variables.Find(aux => aux.Name == variableArray.Dimension).InitialValue;
                        if (actualLength < newLength)
                        {
                            int index = actualLength;
                            Variable exampleVariable = variableArray.Variables.First();
                            string nombre = exampleVariable.Name;
                            var indexFirstParenthesis = nombre.LastIndexOf('(');
                            var indexLastParenthesis = nombre.LastIndexOf(')');
                            var nombreItem = nombre.Remove(indexFirstParenthesis, nombre.Length - indexFirstParenthesis);

                            for (var i = 0; i < newLength - actualLength; i++)
                            {
                                index++;
                                Variable newVariable = new Variable();
                                newVariable.Name = new StringBuilder(nombreItem).Append('(').Append(index).Append(')').ToString();
                                newVariable.Type = exampleVariable.Type;
                                newVariable.InitialValue = exampleVariable.InitialValue;
                                newVariable.ActualValue = exampleVariable.ActualValue;
                                variableArray.Variables.Add(newVariable);
                            }
                        }
                        else if (actualLength > newLength)
                        {
                            variableArray.Variables.RemoveRange(Convert.ToInt32(newLength), Convert.ToInt32(actualLength - newLength));
                        }
                        //Actualizo el nombre de la variable vector
                        variableArray.Name = variableArray.Variables.Last().Name;
                    }
                }
                // seteo las nuevas variables a la simulacion para su correcta ejecucion
                simulation.SetVariables(variables);
                var stage = new StageViewModel(simulation) { Name = groupBox.Header.ToString() };
                IList<StageViewModelBase> stages = new ObservableCollection<StageViewModelBase>();
                stages.Add(stage);
                //Genero el archivo .vic con la nueva simulacion
                HelperVIC helperVIC = new HelperVIC();
                XDocument vicSimulation = helperVIC.CreateVic(simulationFile, variables, stages);
                // Guardo el .vic generado del escenario
                var vicPath = simulationPath + "\\" + groupBox.Header.ToString() + ".vic";
                vicSimulation.Save(vicPath);
                vicPaths.Add(vicPath);
            }
            return vicPaths;
        }

        private IList<StageViewModelBase> generateSensibilityStages()
        {
            IList<StageViewModelBase> stages = new ObservableCollection<StageViewModelBase>();
            
            List<GroupBox> groupBoxes = this.GetAllGroupBoxes();
            foreach (var groupBox in groupBoxes)
            {
                List<TextBox> txtBoxList = new List<TextBox>();
                txtBoxList.AddRange(((WrapPanel)groupBox.Content).Children.OfType<TextBox>());

                // ACTUALIZO VALORES DE VARIABLES DE CONTROL
                Simulation newSimulation = XMLParser.GetSimulation(this.simulationFile);
                List<Variable> originalVariables = newSimulation.GetVariables();
                List<Variable> variables = new List<Variable>();
                foreach (var variable in originalVariables)
                {
                    if (variable.Type == VariableType.Control) {
                        TextBox txtBox = txtBoxList.Find(element => element.Name.Equals("TxtBox_" + variable.Name));
                        if (txtBox != null)
                        {
                            double value = Convert.ToDouble(txtBox.Text);
                            variable.InitialValue = value;
                        }
                    }
                    variables.Add(variable);
                }
                // AHORA ACTUALIZO TAMAÑO DE LAS VARIABLES VECTORES
                foreach (var variable in variables)
                {
                    if (variable is VariableArray)
                    {
                        var variableArray = (VariableArray)variable;
                        int actualLength = variableArray.Variables.Count();
                        double newLength = variables.Find(aux => aux.Name == variableArray.Dimension).InitialValue;
                        if (actualLength < newLength)
                        {
                            int index = actualLength;
                            Variable exampleVariable = variableArray.Variables.First();
                            string nombre = exampleVariable.Name;
                            var indexFirstParenthesis = nombre.LastIndexOf('(');
                            var indexLastParenthesis = nombre.LastIndexOf(')');
                            var nombreItem = nombre.Remove(indexFirstParenthesis, nombre.Length - indexFirstParenthesis);

                            for (var i = 0; i < newLength - actualLength; i++)
                            {
                                index++;
                                Variable newVariable = new Variable();
                                newVariable.Name = new StringBuilder(nombreItem).Append('(').Append(index).Append(')').ToString();
                                newVariable.Type = exampleVariable.Type;
                                newVariable.InitialValue = exampleVariable.InitialValue;
                                newVariable.ActualValue = exampleVariable.ActualValue;
                                variableArray.Variables.Add(newVariable);
                            }
                        }
                        else if (actualLength > newLength)
                        {
                            variableArray.Variables.RemoveRange(Convert.ToInt32(newLength), Convert.ToInt32(actualLength - newLength));
                        }
                        //Actualizo el nombre de la variable vector
                        variableArray.Name = variableArray.Variables.Last().Name;
                    }
                }
                // seteo las nuevas variables a la simulacion para su correcta ejecucion
                newSimulation.SetVariables(variables);
                var stage = new StageViewModel(newSimulation) { Name = groupBox.Header.ToString() };
                stages.Add(stage);
            }

            return stages;
        }

        private List<GroupBox> GetAllGroupBoxes()
        {
            List<GroupBox> list = new List<GroupBox>();
            list.Add(GbEscenario1);
            list.Add(GbEscenario2);
            foreach (var item in gbList)
            {
                list.Add(item.groupBox);
            }
            return list;
        }

        private void CargarVariablesGroupBox(WrapPanel wpanel)
        {
            var i = 0;

            foreach (var variable in variablesDeControl)
            {
                Label lbl = new Label
                {
                    Name = "Lbl_" + variable.Name,
                    Content = variable.Name + ":",
                };

                TextBox txtbox = new TextBox
                {
                    Name = "TxtBox_" + variable.Name,
                    Text = wpanel == Wpanel_Escenario1 ? variable.InitialValue.ToString() : string.Empty,
                    Height = 23,
                    Width = 120
                };

                txtbox.KeyDown +=txtbox_KeyDown;
                txtbox.TextChanged += txtbox_TextChanged;

                if ((i % 2) != 0)
                {
                    lbl.Margin = new Thickness(20, 0, 0, 0);
                }
                else
                {
                    lbl.Margin = new Thickness(50, 0, 0, 0);
                }

                wpanel.Children.Add(lbl);
                wpanel.Children.Add(txtbox);

                i++;
            }
        }

        void txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerificarVariablesControlCompletas();
        }

        //Este metodo ajusta la posicion del segundo groupbox en base al tamaño 
        //ocupado del primero luego de cargadas las variables
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GbEscenario2.Margin = new Thickness(52, GbEscenario1.Margin.Top + GbEscenario1.ActualHeight + 40, 99, 0);

            //Asigno posición de boton simular y nuevoEscenario según tamaño de
            //los dos groupbox predeterminados.
            BtnNuevoEscenario.Margin = new Thickness(213, (GbEscenario1.ActualHeight + 40) * 2 + 40 > BtnNuevoEscenario.Margin.Top ? (GbEscenario1.ActualHeight + 40) * 2 + 40 : BtnNuevoEscenario.Margin.Top, 0, 0);
            BtnSimular.Margin = new Thickness(399, (GbEscenario1.ActualHeight + 40) * 2 + 40 > BtnSimular.Margin.Top ? (GbEscenario1.ActualHeight + 40) * 2 + 40 : BtnSimular.Margin.Top, 135, 125);
        }

        private void VerificarVariablesControlCompletas()
        {
            //iterate through the child controls of "grid"
            foreach (Object child in ((WrapPanel)GbEscenario1.Content).Children)
            {
                var elemento = child;
            }

            var txtBoxListGb1 = ((WrapPanel)GbEscenario1.Content).Children.OfType<TextBox>();
            var txtBoxListGb2 = ((WrapPanel)GbEscenario2.Content).Children.OfType<TextBox>();

            List<TextBox> txtBoxListGbList = new List<TextBox>();

            foreach (var elementos in gbList.Select(x => x.groupBox).Select(y => ((WrapPanel)y.Content).Children))
            {
                txtBoxListGbList.AddRange(elementos.OfType<TextBox>());
            }

            var allTxtBox = txtBoxListGbList.Union(txtBoxListGb1).Union(txtBoxListGb2);

            BtnSimular.IsEnabled = allTxtBox.All(x => !string.IsNullOrWhiteSpace(x.Text));
        }
    }
}
