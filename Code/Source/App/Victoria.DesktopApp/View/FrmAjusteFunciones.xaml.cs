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
    }


}


