﻿using System;
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
        private commonFDP.DistributionResult resultadoSeleccionado = null;
        private commonFDP.DistributionResult resultadoFuncionWeibull0_5 = null;
        private commonFDP.DistributionResult resultadoFuncionBinomial = null;
        private commonFDP.DistributionResult resultadoFuncionExponencial = null;
        private commonFDP.DistributionResult resultadoFuncionLogistica = null;
        private commonFDP.DistributionResult resultadoFuncionLogNormal = null;
        private commonFDP.DistributionResult resultadoFuncionLogLogistica = null;
        private commonFDP.DistributionResult resultadoFuncionNormal = null;
        private commonFDP.DistributionResult resultadoFuncionWeibull1_5 = null;
        private commonFDP.DistributionResult resultadoFuncionWeibull3 = null;
        private commonFDP.DistributionResult resultadoFuncionPoisson = null;
        private commonFDP.DistributionResult resultadoFuncionUniforme = null;
        private commonFDP.DistributionResult resultadoFuncionWeibull5 = null;
        private Dictionary<commonFDP.FuncionDensidad, commonFDP.DistributionResult> lResultadosOrdenados = new Dictionary<commonFDP.FuncionDensidad, commonFDP.DistributionResult>();
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
        }

        public FrmAjusteFunciones(commonFDP.MetodologiaAjuste metodologia, commonFDP.Segment.Segmentacion segmentacion, List<Double> intervalos, int flagIntervalos, commonFDP.Origen proyecto)
        {
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.intervalos = intervalos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
        }



        private void FrmAjusteFunciones_Load(object sender, EventArgs e)
        {
            CalcularEventosSimplificados();
            //CalcularYOrdenarFunciones();
           // OrdenarFuncionesEnVista();
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

                /*
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
                resultadoFuncionPoisson = commonFDP..FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.POISSON, arrEventos).Resultado;
                if (resultadoFuncionPoisson != null)
                    lResultadosOrdenados.Add(commonFDP.FuncionDensidad.POISSON, resultadoFuncionPoisson);
                resultadoFuncionUniforme = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.UNIFORME, arrEventos).Resultado;
                if (resultadoFuncionUniforme != null)
                    lResultadosOrdenados.Add(commonFDP.FuncionDensidad.UNIFORME, resultadoFuncionUniforme);
                resultadoFuncionWeibull5 = commonFDP.FactoryFuncionDensidad.Instancia(commonFDP.FuncionDensidad.WEIBULL5, arrEventos).Resultado;
                if (resultadoFuncionWeibull5 != null)
                    lResultadosOrdenados.Add(commonFDP.FuncionDensidad.WEIBULL5, resultadoFuncionWeibull5);
                lResultadosOrdenados = lResultadosOrdenados.OrderBy(x => x.Value.FDP.CalcularDesvio(eventosSimplificados)).ToDictionary(x => x.Key, y => y.Value);
                */
            }
            catch
            {
                createAlertPopUp("Error al calcular y ordenar las funciones");
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

    }


}


