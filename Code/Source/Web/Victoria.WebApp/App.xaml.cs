﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Victoria.Shared;
using Victoria.UI.Shared;
using Victoria.ViewModelSL;

namespace Victoria.WebApp
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            SmartDispatcher.Initialize(Deployment.Current.Dispatcher);
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var array = Convert.FromBase64String(e.InitParams["simulacion"]);
            var simulacion = Encoding.UTF8.GetString(array, 0, array.Count());

            var newStage = new StageViewModel(XMLParser.GetSimulation(simulacion)) { Name = "Simulación Web" };

            var mainPage = new MainPage();
            mainPage.DataContext = newStage;
            this.RootVisual = mainPage;
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        private string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Simulacion>
  <Diagrama Name=""Inicializar"">
    <flowchart>
      <block id=""Inicializar00000"" cap-pos=""inside"" type=""nodo_titulo_inicializador"" caption=""Inicializar"" left=""600"" top=""50"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00001"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00001"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""Tf = 1000000"" left=""599"" top=""125"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00002"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00002"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""HV = 99999999"" left=""600"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00003"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00003"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""NS = 0.0"" left=""600"" top=""275"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00004"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00004"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""PPS = 0.0"" left=""600"" top=""350"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00005"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00005"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""PTO = 0.0"" left=""600"" top=""425"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00006"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00006"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""IA = 0.0"" left=""600"" top=""500"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00007"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00007"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""TA = 0.0"" left=""600"" top=""575"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Inicializar00008"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Inicializar00008"" cap-pos=""inside"" type=""nodo_fin"" caption="""" left=""600"" top=""650"" width=""100"" height=""60"" lock=""false"" zindex=""1001"" />
    </flowchart>
  </Diagrama>
  <Diagrama Name=""Principal"">
    <flowchart>
      <block id=""Principal00000"" cap-pos=""inside"" type=""nodo_inicializador"" caption=""Inicializar"" left=""600"" top=""50"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00001"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00001"" cap-pos=""inside"" type=""nodo_condicion"" caption=""TPLL &lt;= TPS"" left=""600"" top=""125"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00091"" output=""3"" input=""2"" label="""" type=""none-arrow"" />
        <connection ref=""Principal00093"" output=""1"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00091"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""SPS = SPS + (TPLL - T) * NS"" left=""1125"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00002"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00002"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""T = TPLL"" left=""1125"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00004"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
	  <block id=""Principal00093"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""SPS = SPS + (TPS - T) * NS"" left=""1125"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00003"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00003"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""T = TPS"" left=""75"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00011"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00004"" cap-pos=""inside"" type=""nodo_diagrama"" caption=""IA"" left=""839"" top=""264"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00005"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00005"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""TPLL = T + IA"" left=""1125"" top=""350"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00095"" output=""4"" input=""2"" />
      </block>
	   <block id=""Principal00095"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""CLL = CLL + 1"" left=""1125"" top=""350"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00006"" output=""4"" input=""2"" />
      </block>
	  <block id=""Principal00006"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""NS = NS + 1"" left=""1125"" top=""350"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00007"" output=""4"" input=""2"" />
      </block>
      <block id=""Principal00007"" cap-pos=""inside"" type=""nodo_condicion"" caption=""NS == 1"" left=""1125"" top=""499"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00008"" output=""3"" input=""2"" label="""" type=""none-arrow"" />
        <connection ref=""Principal00010"" output=""1"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00008"" cap-pos=""inside"" type=""nodo_diagrama"" caption=""TA"" left=""1575"" top=""575"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00009"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00009"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""TPS = T + TA"" left=""1575"" top=""650"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00092"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
	  <block id=""Principal00092"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""STO = STO + (T - ITO)"" left=""1575"" top=""650"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00010"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00010"" cap-pos=""inside"" type=""nodo_condicion_cierre"" caption="""" left=""1125"" top=""725"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00016"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00011"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""NS = NS - 1"" left=""75"" top=""275"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00012"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00012"" cap-pos=""inside"" type=""nodo_condicion"" caption=""NS &gt; 0"" left=""75"" top=""350"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00013"" output=""3"" input=""2"" label="""" type=""none-arrow"" />
        <connection ref=""Principal00094"" output=""1"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00013"" cap-pos=""inside"" type=""nodo_diagrama"" caption=""TA"" left=""450"" top=""425"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00014"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00014"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""TPS = T + TA"" left=""450"" top=""500"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00015"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
	  <block id=""Principal00094"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""ITO = T"" left=""75"" top=""575"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00090"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
	  <block id=""Principal00090"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""TPS = HV"" left=""75"" top=""575"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00015"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00015"" cap-pos=""inside"" type=""nodo_condicion_cierre"" caption="""" left=""75"" top=""575"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00016"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00016"" cap-pos=""inside"" type=""nodo_condicion_cierre"" caption="""" left=""600"" top=""800"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00017"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00017"" cap-pos=""inside"" type=""nodo_condicion"" caption=""T &lt; Tf"" left=""600"" top=""875"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00018"" output=""3"" input=""2"" label="""" type=""none-arrow"" />
        <connection ref=""Principal00019"" output=""1"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00018"" cap-pos=""inside"" type=""nodo_referencia"" caption=""A"" left=""900"" top=""950"" width=""100"" height=""60"" lock=""false"" zindex=""1001"" />
      <block id=""rPrincipal00018"" cap-pos=""inside"" type=""nodo_referencia"" caption=""A"" left=""525"" top=""50"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00001"" output=""3"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00019"" cap-pos=""inside"" type=""nodo_resultado"" caption=""PPS:  PTO:  "" left=""300"" top=""950"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""Principal00020"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""Principal00020"" cap-pos=""inside"" type=""nodo_fin"" caption="""" left=""300"" top=""1025"" width=""100"" height=""60"" lock=""false"" zindex=""1001"" />
    </flowchart>
  </Diagrama>
  <Diagrama Name=""IA"">
    <flowchart>
      <block id=""IA00000"" cap-pos=""inside"" type=""nodo_titulo_diagrama"" caption=""IA"" left=""600"" top=""50"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""IA00001"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""IA00001"" cap-pos=""inside"" type=""nodo_random"" caption="""" left=""600"" top=""125"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""IA00002"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""IA00002"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""IA = R * 10"" left=""600"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""IA00003"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""IA00003"" cap-pos=""inside"" type=""nodo_fin"" caption="""" left=""600"" top=""275"" width=""100"" height=""60"" lock=""false"" zindex=""1001"" />
    </flowchart>
  </Diagrama>
  <Diagrama Name=""TA"">
    <flowchart>
      <block id=""TA00000"" cap-pos=""inside"" type=""nodo_titulo_diagrama"" caption=""TA"" left=""600"" top=""50"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""TA00001"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""TA00001"" cap-pos=""inside"" type=""nodo_random"" caption="""" left=""600"" top=""125"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""TA00002"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""TA00002"" cap-pos=""inside"" type=""nodo_sentencia"" caption=""TA = R * 10"" left=""600"" top=""200"" width=""100"" height=""60"" lock=""false"" zindex=""1001"">
        <connection ref=""TA00003"" output=""4"" input=""2"" label="""" type=""none-arrow"" />
      </block>
      <block id=""TA00003"" cap-pos=""inside"" type=""nodo_fin"" caption="""" left=""600"" top=""275"" width=""100"" height=""60"" lock=""false"" zindex=""1001"" />
    </flowchart>
  </Diagrama>
  <variables>{ ""variables"": [{""nombre"":""CLL"",""valor"":""0""},{""nombre"":""ITO"",""valor"":""0""},{""nombre"":""STO"",""valor"":""0""},{""nombre"":""SPS"",""valor"":""0""},{""nombre"":""Tf"",""valor"":""100000.0""},{""nombre"":""HV"",""valor"":""100001.0""},{""nombre"":""NS"",""valor"":""0.0""},{""nombre"":""PPS"",""valor"":""0.0""},{""nombre"":""PTO"",""valor"":""0.0""},{""nombre"":""IA"",""valor"":""0.0""},{""nombre"":""TA"",""valor"":""0.0""},{""nombre"":""IA"",""valor"":""0.0""},{""nombre"":""TA"",""valor"":""0.0""},{""nombre"":""TPLL"",""valor"":""0.0""},{""nombre"":""TPS"",""valor"":""0.0""}]}</variables>
</Simulacion>";
    }
}
