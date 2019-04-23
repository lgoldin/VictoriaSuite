using Kairos.Entidades;
using Kairos.FuncionesDensidad;
using Kairos.Modelo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Kairos.Forms
{
    public partial class FrmAjusteFunciones : Form
    {
        private Origen proyecto = new Origen();
        private MetodologiaAjuste metodologia;
        private Segmentacion segmentacion;
        private bool timerActivo = false;
        private List<Evento> eventos = null;
        private Dictionary<string, double> eventosSimplificados = null;
        private double[] eventosParaAjuste = null;
        private ResultadoAjuste resultadoSeleccionado = null;
        private ResultadoAjuste resultadoFuncionWeibull0_5 = null;
        private ResultadoAjuste resultadoFuncionBinomial = null;
        private ResultadoAjuste resultadoFuncionExponencial = null;
        private ResultadoAjuste resultadoFuncionLogistica = null;
        private ResultadoAjuste resultadoFuncionLogNormal = null;
        private ResultadoAjuste resultadoFuncionLogLogistica = null;
        private ResultadoAjuste resultadoFuncionNormal = null;
        private ResultadoAjuste resultadoFuncionWeibull1_5 = null;
        private ResultadoAjuste resultadoFuncionWeibull3 = null;
        private ResultadoAjuste resultadoFuncionPoisson = null;
        private ResultadoAjuste resultadoFuncionUniforme = null;
        private ResultadoAjuste resultadoFuncionWeibull5 = null;
        private Dictionary<FuncionDensidad, ResultadoAjuste> lResultadosOrdenados = new Dictionary<FuncionDensidad, ResultadoAjuste>();
        private int flagIntervalos = 0;
        private List<Double> intervalos;

        public FrmAjusteFunciones(MetodologiaAjuste metodologia, Segmentacion segmentacion, List<Evento> eventos, int flagIntervalos, Origen proyecto)
        {
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.eventos = eventos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
        }

        public FrmAjusteFunciones(MetodologiaAjuste metodologia, Segmentacion segmentacion, List<Double> intervalos, int flagIntervalos, Origen proyecto)
        {
            InitializeComponent();
            this.metodologia = metodologia;
            this.segmentacion = segmentacion;
            this.intervalos = intervalos;
            this.flagIntervalos = flagIntervalos;
            this.proyecto = proyecto;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmAjusteFunciones_Load(object sender, EventArgs e)
        {
            CalcularEventosSimplificados();
            CalcularYOrdenarFunciones();
            OrdenarFuncionesEnVista();
            SetupGraficoFuncion();
        }

        private void CalcularEventosSimplificados()
        {
            try
            {
                if (flagIntervalos == 0)
                {
                    if (metodologia == MetodologiaAjuste.DT_CONSTANTE)
                    {
                        List<double> lista = FdPUtils.AgruparSegmentacion(segmentacion, eventos);
                        eventosParaAjuste = lista.ToArray();
                        eventosSimplificados = FdPUtils.AgruparSegmentacionProbabilidad(lista);
                    }
                    else if (metodologia == MetodologiaAjuste.EVENTO_A_EVENTO)
                    {
                        eventosSimplificados = FdPUtils.AgruparIntervalos(eventos);
                        eventosParaAjuste = FdPUtils.CalcularIntervalos(eventos).ToArray();
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
                mostrarMensaje("Error al calcular los intervalos", Color.FromArgb(255, 89, 89));
            }


        }

        private void CalcularYOrdenarFunciones()
        {
            try
            {
                double[] arrEventos = eventosParaAjuste.ToArray();


                resultadoFuncionWeibull0_5 = FactoryFuncionDensidad.Instancia(FuncionDensidad.WEIBULL05, arrEventos).Resultado;
                if (resultadoFuncionWeibull0_5 != null)
                    lResultadosOrdenados.Add(FuncionDensidad.WEIBULL05, resultadoFuncionWeibull0_5);
                resultadoFuncionBinomial = FactoryFuncionDensidad.Instancia(FuncionDensidad.BINOMIAL, arrEventos).Resultado;
                if (resultadoFuncionBinomial != null)
                    lResultadosOrdenados.Add(FuncionDensidad.BINOMIAL, resultadoFuncionBinomial);
                resultadoFuncionExponencial = FactoryFuncionDensidad.Instancia(FuncionDensidad.EXPONENCIAL, arrEventos).Resultado;
                if (resultadoFuncionExponencial != null)
                    lResultadosOrdenados.Add(FuncionDensidad.EXPONENCIAL, resultadoFuncionExponencial);
                resultadoFuncionLogistica = FactoryFuncionDensidad.Instancia(FuncionDensidad.LOGISTICA, arrEventos).Resultado;
                if (resultadoFuncionLogistica != null)
                    lResultadosOrdenados.Add(FuncionDensidad.LOGISTICA, resultadoFuncionLogistica);
                resultadoFuncionLogNormal = FactoryFuncionDensidad.Instancia(FuncionDensidad.LOG_NORMAL, arrEventos).Resultado;
                if (resultadoFuncionLogNormal != null)
                    lResultadosOrdenados.Add(FuncionDensidad.LOG_NORMAL, resultadoFuncionLogNormal);
                resultadoFuncionLogLogistica = FactoryFuncionDensidad.Instancia(FuncionDensidad.LOG_LOGISTICA, arrEventos).Resultado;
                if (resultadoFuncionLogLogistica != null)
                    lResultadosOrdenados.Add(FuncionDensidad.LOG_LOGISTICA, resultadoFuncionLogLogistica);
                resultadoFuncionNormal = FactoryFuncionDensidad.Instancia(FuncionDensidad.NORMAL, arrEventos).Resultado;
                if (resultadoFuncionNormal != null)
                    lResultadosOrdenados.Add(FuncionDensidad.NORMAL, resultadoFuncionNormal);
                resultadoFuncionWeibull1_5 = FactoryFuncionDensidad.Instancia(FuncionDensidad.WEIBULL15, arrEventos).Resultado;
                if (resultadoFuncionWeibull1_5 != null)
                    lResultadosOrdenados.Add(FuncionDensidad.WEIBULL15, resultadoFuncionWeibull1_5);
                resultadoFuncionWeibull3 = FactoryFuncionDensidad.Instancia(FuncionDensidad.WEIBULL3, arrEventos).Resultado;
                if (resultadoFuncionWeibull3 != null)
                    lResultadosOrdenados.Add(FuncionDensidad.WEIBULL3, resultadoFuncionWeibull3);
                resultadoFuncionPoisson = FactoryFuncionDensidad.Instancia(FuncionDensidad.POISSON, arrEventos).Resultado;
                if (resultadoFuncionPoisson != null)
                    lResultadosOrdenados.Add(FuncionDensidad.POISSON, resultadoFuncionPoisson);
                resultadoFuncionUniforme = FactoryFuncionDensidad.Instancia(FuncionDensidad.UNIFORME, arrEventos).Resultado;
                if (resultadoFuncionUniforme != null)
                    lResultadosOrdenados.Add(FuncionDensidad.UNIFORME, resultadoFuncionUniforme);
                resultadoFuncionWeibull5 = FactoryFuncionDensidad.Instancia(FuncionDensidad.WEIBULL5, arrEventos).Resultado;
                if (resultadoFuncionWeibull5 != null)
                    lResultadosOrdenados.Add(FuncionDensidad.WEIBULL5, resultadoFuncionWeibull5);
                lResultadosOrdenados = lResultadosOrdenados.OrderBy(x => x.Value.FDP.CalcularDesvio(eventosSimplificados)).ToDictionary(x => x.Key, y => y.Value);
            }
            catch
            {
                mostrarMensaje("Error al calcular y ordenar las funciones", Color.FromArgb(255, 89, 89));
            }
        }

        private void OrdenarFuncionesEnVista()
        {
            try
            {
                List<Button> controls = new List<Button>();
                foreach (var item in pnlFunciones.Controls)
                {
                    controls.Add((Button)item);
                }
                pnlFunciones.Controls.Clear();
                foreach (var item in lResultadosOrdenados)
                {
                    foreach (var control in controls)
                    {
                        var auxName = control.Name.Replace("btnFuncion", "");
                        if (String.Compare(item.Key.ToString().Replace("_", ""), auxName, true) == 0)
                            pnlFunciones.Controls.Add(control);
                    }
                }
            }
            catch
            {
                mostrarMensaje("Error al ordenar las funciones", Color.FromArgb(255, 89, 89));
            }
        }

        private void SetupGraficoFuncion()
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

        private void GraficarLineaFDP(FuncionDensidadProbabilidad fdp)
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
            catch(Exception e)
            {
                mostrarMensaje("Error al graficar la función: " + e.Message, Color.FromArgb(255, 89, 89));
            }
        }

        private void GraficarLineaInversa(FuncionDensidadProbabilidad fdp)
        {
            try
            {
                Series series = this.chrtInversa.Series.FindByName("Inversa");
                if (series == null)
                {
                    series = this.chrtInversa.Series.Add("Inversa");
                    series.ChartType = SeriesChartType.Line;
                    series.BorderWidth = 2;
                    series.IsVisibleInLegend = false;
                }
                else
                    series.Points.Clear();
                Dictionary<double, double> lGenerados = fdp.ObtenerDensidadInversa(100);
                foreach (var item in lGenerados)
                {
                    series.Points.AddXY(item.Key, item.Value);
                }
            }
            catch
            {
                mostrarMensaje("Error al graficar la función inversa", Color.FromArgb(255, 89, 89));
            }
        }

        private void BotonSeleccionado(Control boton)
        {
            boton.BackColor = Color.FromArgb(187, 0, 4);
            foreach (Control control in pnlFunciones.Controls)
            {
                if (control != boton)
                {
                    control.BackColor = Color.Black;
                    control.ForeColor = Color.White;
                }
            }
        }

        private void CambiarLblGraficoFuncion(string nombreFuncion)
        {
            lblGraficoFuncion.Text = "Función " + nombreFuncion;
        }

        private void CambiarRepresentacionFuncionEInversa(ResultadoAjuste fdp)
        {
            lblFuncion.Text = fdp.Funcion;
            lblFuncionInversa.Text = fdp.Inversa;
        }

        private void SetupPantallaSegunFDP(object boton, string nombreFuncion, ResultadoAjuste funcion)
        {
            BotonSeleccionado((Control)boton);
            CambiarLblGraficoFuncion(nombreFuncion);
            CambiarRepresentacionFuncionEInversa(funcion);
            GraficarLineaFDP(funcion.FDP);
            GraficarLineaInversa(funcion.FDP);
            resultadoSeleccionado = funcion;
            lbxGenerados.Items.Clear();
            if (nombreFuncion.Contains("Poisson") || nombreFuncion.Contains("Binomial"))
                lblGraficoFuncionInversa.Text = "Función Acumulada";
            else
                lblGraficoFuncionInversa.Text = "Función Inversa";
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

        private void btnGenerarValoresAleatorios_Click(object sender, EventArgs e)
        {
            try
            {
                if (resultadoSeleccionado != null)
                {
                    lbxGenerados.Items.Clear();
                    int cant = Convert.ToInt32(nudCantidadGenerados.Value);
                    int[] arrGenerados = resultadoSeleccionado.FDP.GenerarValores(cant, eventosParaAjuste.ToArray()).Select(x => Convert.ToInt32(x)).ToArray();
                    lbxGenerados.Items.AddRange(arrGenerados.Select(x => (object)x).ToArray());
                }
                else
                    mostrarMensaje("Debe seleccionar una función", Color.FromArgb(255, 255, 0));
            }
            catch
            {
                mostrarMensaje("Error al general valores aleatorios", Color.FromArgb(255, 89, 89));
            }
        }

        private void mostrarMensaje(string mensaje, Color color)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.Visible = true;
            pnlMensaje.Visible = true;
            pnlMensaje.BackColor = color;
            if (this.timerActivo)
                timerMensaje.Stop();

            timerMensaje.Start();
            this.timerActivo = true;
        }

        private void timerMensaje_Tick(object sender, EventArgs e)
        {
            pnlMensaje.Visible = false;
            timerMensaje.Stop();
            this.timerActivo = false;
        }

        private void btnExportarResultados_Click(object sender, EventArgs e)
        {
            if (resultadoSeleccionado != null)
            {
                try
                {
                    var folderBrowserDialog1 = new FolderBrowserDialog();

                    DialogResult result = folderBrowserDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string directorio = folderBrowserDialog1.SelectedPath;
                        //crear carpeta con el nombre del proyecto
                        string nombreProyecto = directorio + "\\" + this.proyecto.nombreOrigen;
                        bool exists = System.IO.Directory.Exists(nombreProyecto);
                        string carpetaFinal = "";
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(nombreProyecto);
                            carpetaFinal = nombreProyecto;
                        }
                        else
                        {
                            int cont = 1;
                            while (System.IO.Directory.Exists(nombreProyecto + " (" + cont + ")"))
                            {
                                cont++;
                            }
                            carpetaFinal = nombreProyecto + " (" + cont + ")";
                            System.IO.Directory.CreateDirectory(carpetaFinal);
                        }
                        //crear excel con los eventos utilizados
                        if (flagIntervalos == 0)
                        {
                            using (ExcelPackage excelEventos = new ExcelPackage())
                            {
                                excelEventos.Workbook.Worksheets.Add("Hoja1");

                                var worksheet = excelEventos.Workbook.Worksheets["Hoja1"];

                                List<string[]> data = new List<string[]>();

                                foreach (Evento evento in eventos)
                                {
                                    data.Add(new string[] { evento.fecha.ToString("yyyy-MM-dd HH:mm:ss") });
                                }
                                worksheet.Cells["A1"].LoadFromArrays(data);

                                FileInfo excelFile = new FileInfo(carpetaFinal + "\\Eventos.xlsx");
                                excelEventos.SaveAs(excelFile);
                            }

                        }
                        //crear excel con los intervalos utilizados y los creados adicionalmente
                        using (ExcelPackage excelIntervalos = new ExcelPackage())
                        {
                            excelIntervalos.Workbook.Worksheets.Add("Hoja1");

                            var worksheet = excelIntervalos.Workbook.Worksheets["Hoja1"];

                            List<string[]> data = new List<string[]>();

                            foreach (int intervalo in eventosParaAjuste)
                            {
                                data.Add(new string[] { intervalo.ToString() });
                            }
                            foreach (var item in lbxGenerados.Items)
                            {
                                data.Add(new string[] { item.ToString() });
                            }

                            worksheet.Cells["A1"].LoadFromArrays(data);

                            FileInfo excelFile = new FileInfo(carpetaFinal + "\\Intervalos.xlsx");
                            excelIntervalos.SaveAs(excelFile);
                        }
                        //crear txt con la fdp
                        string pathFDP = carpetaFinal + "\\FDP.txt";
                        File.Create(pathFDP).Dispose();

                        using (TextWriter tw = new StreamWriter(pathFDP))
                        {
                            tw.WriteLine(lblFuncion.Text);
                        }

                        //exportar gráfico de fdp
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.FileName = carpetaFinal+"\\GraficoFDP.png";
                        System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                        this.chrtFuncion.SaveImage(fs, ChartImageFormat.Png);
                        fs.Close();

                        //exportar gráfico de inversa
                        SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                        saveFileDialog2.FileName = carpetaFinal + "\\GraficoFuncionInversa.png";
                        System.IO.FileStream fs2 = (System.IO.FileStream)saveFileDialog2.OpenFile();
                        this.chrtInversa.SaveImage(fs2, ChartImageFormat.Png);
                        fs2.Close();

                        //crear txt con la inversa
                        string pathInversa = carpetaFinal + "\\FuncionInversa.txt";
                        using (TextWriter tw = new StreamWriter(pathInversa))
                        {
                            tw.WriteLine(lblFuncionInversa.Text);
                        }
                        mostrarMensaje("Se exportaron los resultados exitosamente", Color.FromArgb(128, 255, 128));
                    }
                }
                catch
                {
                    mostrarMensaje("Error al exportar los resultados", Color.FromArgb(255, 89, 89));
                }
            }
            else
                mostrarMensaje("Debe seleccionar una función", Color.FromArgb(255, 255, 0));
        }

    }
}
