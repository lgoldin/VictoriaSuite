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
using System.Windows.Controls.DataVisualization;


namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for FrmAjusteFunciones.xaml
    /// </summary>
    public partial class FrmAjusteFunciones : Window
    {
        private commonFDP.Origen proyecto = new commonFDP.Origen();
        private commonFDP.MetodologiaAjuste metodologia;
        private commonFDP.Segment.Segmentacion segmentacion;
        private bool timerActivo = false;
        private List<commonFDP.Evento> eventos = null;
        private Dictionary<string, double> eventosSimplificados = null;
        private double[] eventosParaAjuste = null;
        private commonFDP.ResultadoAjuste resultadoSeleccionado = null;
        private commonFDP.ResultadoAjuste resultadoFuncionWeibull0_5 = null;
        private commonFDP.ResultadoAjuste resultadoFuncionBinomial = null;
        private commonFDP.ResultadoAjuste resultadoFuncionExponencial = null;
        private commonFDP.ResultadoAjuste resultadoFuncionLogistica = null;
        private commonFDP.ResultadoAjuste resultadoFuncionLogNormal = null;
        private commonFDP.ResultadoAjuste resultadoFuncionLogLogistica = null;
        private commonFDP.ResultadoAjuste resultadoFuncionNormal = null;
        private commonFDP.ResultadoAjuste resultadoFuncionWeibull1_5 = null;
        private commonFDP.ResultadoAjuste resultadoFuncionWeibull3 = null;
        private commonFDP.ResultadoAjuste resultadoFuncionPoisson = null;
        private commonFDP.ResultadoAjuste resultadoFuncionUniforme = null;
        private commonFDP.ResultadoAjuste resultadoFuncionWeibull5 = null;
        private Dictionary<commonFDP.FuncionDensidad, commonFDP.ResultadoAjuste> lResultadosOrdenados = new Dictionary<commonFDP.FuncionDensidad, commonFDP.ResultadoAjuste>();
        private int flagIntervalos = 0;
        private List<Double> intervalos;
        private System.Windows.Controls.DataVisualization.Charting.Chart chrtInversa;
        private System.Windows.Controls.DataVisualization.Charting.Chart chrtFuncion;
        private AnalisisPrevio analisisPrevio;
        public FrmAjusteFunciones(commonFDP.MetodologiaAjuste metodologia, commonFDP.Segment.Segmentacion segmentacion, List<commonFDP.Evento> eventos, int flagIntervalos, commonFDP.Origen proyecto, AnalisisPrevio aPrevio)
        {
            InitializeComponent();
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.eventos = eventos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
            this.FrmAjusteFunciones_Load();
            this.analisisPrevio = aPrevio;
        }

        public FrmAjusteFunciones(commonFDP.MetodologiaAjuste metodologia, commonFDP.Segment.Segmentacion segmentacion, List<Double> intervalos, int flagIntervalos, commonFDP.Origen proyecto, AnalisisPrevio aPrevio)
        {
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.intervalos = intervalos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
            this.FrmAjusteFunciones_Load();
            this.analisisPrevio = aPrevio;

        }



        private void FrmAjusteFunciones_Load()
        {
            CalcularEventosSimplificados();
            CalcularYOrdenarFunciones();
            OrdenarFuncionesEnVista();
            // SetupGraficoFuncion();
        }

        private void CalcularEventosSimplificados()
        {
             try
            {
                if (flagIntervalos == 0)
                {
                    if (metodologia == commonFDP.MetodologiaAjuste.DT_CONSTANTE)
                    {
                        List<double> lista = commonFDP.FdPUtils.AgruparSegmentacion(segmentacion, eventos);
                        eventosParaAjuste = lista.ToArray();
                        eventosSimplificados = commonFDP.FdPUtils.AgruparSegmentacionProbabilidad(lista);
                    }
                    else if (metodologia == commonFDP.MetodologiaAjuste.EVENTO_A_EVENTO)
                    {
                        eventosSimplificados = commonFDP.FdPUtils.AgruparIntervalos(eventos);
                        eventosParaAjuste = commonFDP.FdPUtils.CalcularIntervalos(eventos).ToArray();
                    }
                }
                else
                {
                    double cant = intervalos.Count;
                    eventosSimplificados = intervalos.GroupBy(x => x).ToDictionary(x => x.Key.ToString(), x => x.Count() / (cant > 1 ? cant - 1 : cant));
                    eventosParaAjuste = intervalos.ToArray();
                }
            }
             catch
             {
                 createAlertPopUp("Error al calcular los intervalos");
             } 


        }

        private void CalcularYOrdenarFunciones()
        {
             try
            {
            double[] arrEventos = eventosParaAjuste.ToArray();


            resultadoFuncionWeibull0_5 = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.WEIBULL05, arrEventos).Resultado;
            if (resultadoFuncionWeibull0_5 != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.WEIBULL05, resultadoFuncionWeibull0_5);
            resultadoFuncionBinomial = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.BINOMIAL, arrEventos).Resultado;
            if (resultadoFuncionBinomial != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.BINOMIAL, resultadoFuncionBinomial);
            resultadoFuncionExponencial = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.EXPONENCIAL, arrEventos).Resultado;
            if (resultadoFuncionExponencial != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.EXPONENCIAL, resultadoFuncionExponencial);
            resultadoFuncionLogistica = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.LOGISTICA, arrEventos).Resultado;
            if (resultadoFuncionLogistica != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.LOGISTICA, resultadoFuncionLogistica);
            resultadoFuncionLogNormal = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.LOG_NORMAL, arrEventos).Resultado;
            if (resultadoFuncionLogNormal != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.LOG_NORMAL, resultadoFuncionLogNormal);
            resultadoFuncionLogLogistica = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.LOG_LOGISTICA, arrEventos).Resultado;
            if (resultadoFuncionLogLogistica != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.LOG_LOGISTICA, resultadoFuncionLogLogistica);
            resultadoFuncionNormal = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.NORMAL, arrEventos).Resultado;
            if (resultadoFuncionNormal != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.NORMAL, resultadoFuncionNormal);
            resultadoFuncionWeibull1_5 = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.WEIBULL15, arrEventos).Resultado;
            if (resultadoFuncionWeibull1_5 != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.WEIBULL15, resultadoFuncionWeibull1_5);
            resultadoFuncionWeibull3 = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.WEIBULL3, arrEventos).Resultado;
            if (resultadoFuncionWeibull3 != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.WEIBULL3, resultadoFuncionWeibull3);
            resultadoFuncionPoisson = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.POISSON, arrEventos).Resultado;
            if (resultadoFuncionPoisson != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.POISSON, resultadoFuncionPoisson);
            resultadoFuncionUniforme = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.UNIFORME, arrEventos).Resultado;
            if (resultadoFuncionUniforme != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.UNIFORME, resultadoFuncionUniforme);
            resultadoFuncionWeibull5 = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.WEIBULL5, arrEventos).Resultado;
            if (resultadoFuncionWeibull5 != null)
                lResultadosOrdenados.Add(commonFDP.FuncionDensidad.WEIBULL5, resultadoFuncionWeibull5);
            lResultadosOrdenados = lResultadosOrdenados.OrderBy(x => x.Value.FDP.CalcularDesvio(eventosSimplificados)).ToDictionary(x => x.Key, y => y.Value);

             }
             catch
             {
                 createAlertPopUp("Error al calcular y ordenar las funciones");
             }
        }

        private void OrdenarFuncionesEnVista()
        {
            try
            {
                List<Button> buttons = new List<Button>();
                List<Grid> grids = pnlButtonsGrid.Children.OfType<Grid>().ToList();


                foreach (Grid item in grids)
                {

                    foreach (Button butt in item.Children.OfType<Button>())
                    {
                        buttons.Add(butt);
                    }
                    item.Children.Clear();
                }


                int indice = 0;
                foreach (var item in lResultadosOrdenados)
                {
                    foreach (var button in buttons)
                    {

                        var auxName = button.Name.Replace("btnFuncion", "");
                        if (String.Compare(item.Key.ToString().Replace("_", ""), auxName, true) == 0)
                            grids[indice].Children.Add(button);

                    }
                    indice++;

                }
            }
            catch
            {
                createAlertPopUp("Error al ordenar las funciones");
            }
        }

        private void BtnShowResults_onClick(object sender, RoutedEventArgs e)
        {

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
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void btnExport_OnClick(object sender, RoutedEventArgs e)
        {

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

        private void CambiarLblGraficoFuncion(string nombreFuncion)
        {
            lblTituloFuncion.Content = "Función " + nombreFuncion;
        }

        private void CambiarRepresentacionFuncionEInversa(commonFDP.ResultadoAjuste fdp)
        {
            lblFuncion.Content = fdp.Funcion;
            lblFuncionInversa.Content = fdp.Inversa;
        }



        private void btnFuncionWeibull0_5_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Weibull 0.5", resultadoFuncionWeibull0_5);

        private void btnFuncionBinomial_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Binomial", resultadoFuncionBinomial);

        private void btnFuncionExponencial_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Exponencial", resultadoFuncionExponencial);

        private void btnFuncionLogistica_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Logistica", resultadoFuncionLogistica);

        private void btnFuncionLognormal_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Log-Normal", resultadoFuncionLogNormal);

        private void btnFuncionLogLogistica_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Log-Logistica", resultadoFuncionLogLogistica);

        private void btnFuncionNormal_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Normal", resultadoFuncionNormal);

        private void btnFuncionWeibull1_5_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Weibull 1.5", resultadoFuncionWeibull1_5);

        private void btnFuncionWeibull3_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Weibull 3", resultadoFuncionWeibull3);

        private void btnFuncionPoisson_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Poisson", resultadoFuncionPoisson);

        private void btnFuncionUniforme_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Uniforme", resultadoFuncionUniforme);

        private void btnFuncionWeibull5_Click(object sender, EventArgs e) => SetupPantallaSegunFDP(sender, "Weibull", resultadoFuncionWeibull5);

        private void SetupPantallaSegunFDP(object boton, string nombreFuncion, commonFDP.ResultadoAjuste funcion)
        {

            CambiarLblGraficoFuncion(nombreFuncion);
            CambiarRepresentacionFuncionEInversa(funcion);
            //GraficarLineaFDP(funcion.FDP);
            //GraficarLineaInversa(funcion.FDP);
            resultadoSeleccionado = funcion;
            //lbxGenerados.Items.Clear();
            if (nombreFuncion.Contains("Poisson") || nombreFuncion.Contains("Binomial"))
                lblTituloFuncionInversa.Content = "Función Acumulada";
            else
                lblTituloFuncionInversa.Content = "Función Inversa";
        }

        private void btnSelectFDP_OnClick(object sender, RoutedEventArgs e)
        {
            analisisPrevio.addFDPToList(resultadoSeleccionado);
        }

        /* private void SetupGraficoFuncion()
         {
             try
             {
                 this.chrtFuncion.Series.Clear();
                 this.chrtFuncion.Palette = ChartColorPalette.None;
                 this.chrtFuncion.ChartAreas.First().AxisY2.Enabled = AxisEnabled.False;
                 Series series = this.chrtFuncion.Series.Add("Eventos");
                 series.XValueType = ChartValueType.Double;
                 series.XAxisType = AxisType.Primary;
                 series.YAxisType = AxisType.Secondary;
                 series.ChartType = SeriesChartType.Column;
                 series.Color = Color.Red;
                 series.BorderColor = Color.Black;
                 series.IsVisibleInLegend = false;
                 foreach (var item in eventosSimplificados.OrderBy(x => Convert.ToDouble(x.Key)))
                 {
                     series.Points.AddXY(Convert.ToDouble(item.Key), item.Value);
                 }
             }
             catch
             {
                 mostrarMensaje("Error al graficar los eventos", Color.FromArgb(255, 89, 89));
             }
         } 

         private void GraficarLineaFDP(commonFDP.FuncionDensidadProbabilidad fdp)
         {
             try
             {
                 Series series = this.chrtFuncion.Series.FindByName("FDP");
                 if (series == null)
                 {
                     series = this.chrtFuncion.Series.Add("FDP");
                     series.ChartType = SeriesChartType.Line;
                     series.BorderWidth = 5;
                     series.IsVisibleInLegend = false;
                 }
                 else
                     series.Points.Clear();
                 Dictionary<double, double> lGenerados = fdp.ObtenerDensidad(eventosSimplificados.ToDictionary(x => Convert.ToDouble(x.Key), x => x.Value));
                 foreach (var item in lGenerados)
                 {
                     series.Points.AddXY(item.Key, item.Value);
                 }
             }
             catch (Exception e)
             {
                 //mostrarMensaje("Error al graficar la función: " + e.Message, Color.FromArgb(255, 89, 89));
             }
         }*/

    }


}


