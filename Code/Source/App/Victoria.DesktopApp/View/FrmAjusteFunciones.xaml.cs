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
        public FrmAjusteFunciones(commonFDP.MetodologiaAjuste metodologia, commonFDP.Segment.Segmentacion segmentacion, List<commonFDP.Evento> eventos, int flagIntervalos, commonFDP.Origen proyecto)
        {
            InitializeComponent();
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.eventos = eventos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
            this.FrmAjusteFunciones_Load();
        }

        public FrmAjusteFunciones(commonFDP.MetodologiaAjuste metodologia, commonFDP.Segment.Segmentacion segmentacion, List<Double> intervalos, int flagIntervalos, commonFDP.Origen proyecto)
        {
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.intervalos = intervalos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
            this.FrmAjusteFunciones_Load();

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
            // try
            //{
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
            /* }
             catch
             {
                 createAlertPopUp("Error al calcular los intervalos");
             } */


        }

        private void CalcularYOrdenarFunciones()
        {
            // try
            //{
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

            /* }
             catch
             {
                 createAlertPopUp("Error al calcular y ordenar las funciones");
             }*/
        }

        private void OrdenarFuncionesEnVista()
        {
            try
            {
                List<Button> buttons = new List<Button>();
                foreach (var item in pnlButtonsGrid.Children.OfType<Button>())
                {
                    buttons.Add((Button)item);
                }


                int indice = 0;
                int childrens = pnlButtonsGrid.Children.Count;
                for (int i = 0; i < childrens; i++)
                {
                    Button btn = pnlButtonsGrid.Children[indice] as Button;
                    if (btn != null)
                    {
                        //Remove children
                        pnlButtonsGrid.Children.RemoveAt(indice);
                    }
                    else
                    {
                        indice++;
                    }
                }

                foreach (var item in lResultadosOrdenados)
                {
                    foreach (var button in buttons)
                    {
                        var auxName = button.Name.Replace("btnFuncion", "");
                        if (String.Compare(item.Key.ToString().Replace("_", ""), auxName, true) == 0)
                            pnlButtonsGrid.Children.Add(button);
                    }
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

            //CambiarLblGraficoFuncion(nombreFuncion);
            //CambiarRepresentacionFuncionEInversa(funcion);
            //GraficarLineaFDP(funcion.FDP);
            //GraficarLineaInversa(funcion.FDP);
            resultadoSeleccionado = funcion;
            //lbxGenerados.Items.Clear();
            if (nombreFuncion.Contains("Poisson") || nombreFuncion.Contains("Binomial"))
                lblFuncionInversa.Content = "Función Acumulada";
            else
                lblFuncionInversa.Content = "Función Inversa";
        }

    }


}


