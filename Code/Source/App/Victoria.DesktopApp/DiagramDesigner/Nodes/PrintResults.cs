using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Victoria.DesktopApp.DiagramDesigner.Nodes
{
    class PrintResults: Node
    {
        private const string CONTENT = "<Grid ToolTip=\"Imprimir resultados\" ToolTipService.IsEnabled=\"False\" ToolTipService.InitialShowDelay=\"0\" ToolTipService.BetweenShowDelay=\"0\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:s=\"clr-namespace:System;assembly=mscorlib\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dd=\"clr-namespace:DiagramDesigner;assembly=Victoria\" xmlns:ddc=\"clr-namespace:DiagramDesigner.Controls;assembly=Victoria\"><Path ToolTip=\"nodo_resultado\"><Path.Style><Style TargetType=\"Path\"><Style.BasedOn><Style TargetType=\"Path\"><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Shape.Fill\"><Setter.Value><LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"0,1\"><LinearGradientBrush.GradientStops><GradientStop Color=\"#FFFAFBE9\" Offset=\"0\" /><GradientStop Color=\"#FFFFA500\" Offset=\"1\" /></LinearGradientBrush.GradientStops></LinearGradientBrush></Setter.Value></Setter><Setter Property=\"Shape.Stroke\"><Setter.Value><SolidColorBrush>#FFD69436</SolidColorBrush></Setter.Value></Setter><Setter Property=\"Shape.StrokeThickness\"><Setter.Value><s:Double>1</s:Double></Setter.Value></Setter><Setter Property=\"Shape.StrokeLineJoin\"><Setter.Value><x:Static Member=\"PenLineJoin.Round\" /></Setter.Value></Setter><Setter Property=\"Shape.Stretch\"><Setter.Value><x:Static Member=\"Stretch.Fill\" /></Setter.Value></Setter><Setter Property=\"UIElement.IsHitTestVisible\"><Setter.Value><s:Boolean>False</s:Boolean></Setter.Value></Setter><Setter Property=\"UIElement.SnapsToDevicePixels\"><Setter.Value><s:Boolean>True</s:Boolean></Setter.Value></Setter></Style></Style.BasedOn><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Path.Data\"><Setter.Value><StreamGeometry>M0,0L60,0 60,40C30,30,30,50,0,40z</StreamGeometry></Setter.Value></Setter></Style></Path.Style><dd:DesignerItem.DragThumbTemplate><ControlTemplate><Path><Path.Style><Style TargetType=\"Path\"><Style.BasedOn><Style TargetType=\"Path\"><Style.BasedOn><Style TargetType=\"Path\"><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Shape.Fill\"><Setter.Value><LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"0,1\"><LinearGradientBrush.GradientStops><GradientStop Color=\"#FFFAFBE9\" Offset=\"0\" /><GradientStop Color=\"#FFFFA500\" Offset=\"1\" /></LinearGradientBrush.GradientStops></LinearGradientBrush></Setter.Value></Setter><Setter Property=\"Shape.Stroke\"><Setter.Value><SolidColorBrush>#FFD69436</SolidColorBrush></Setter.Value></Setter><Setter Property=\"Shape.StrokeThickness\"><Setter.Value><s:Double>1</s:Double></Setter.Value></Setter><Setter Property=\"Shape.StrokeLineJoin\"><Setter.Value><x:Static Member=\"PenLineJoin.Round\" /></Setter.Value></Setter><Setter Property=\"Shape.Stretch\"><Setter.Value><x:Static Member=\"Stretch.Fill\" /></Setter.Value></Setter><Setter Property=\"UIElement.IsHitTestVisible\"><Setter.Value><s:Boolean>False</s:Boolean></Setter.Value></Setter><Setter Property=\"UIElement.SnapsToDevicePixels\"><Setter.Value><s:Boolean>True</s:Boolean></Setter.Value></Setter></Style></Style.BasedOn><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Path.Data\"><Setter.Value><StreamGeometry>M0,0L60,0 60,40C30,30,30,50,0,40z</StreamGeometry></Setter.Value></Setter></Style></Style.BasedOn><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"UIElement.IsHitTestVisible\"><Setter.Value><s:Boolean>True</s:Boolean></Setter.Value></Setter><Setter Property=\"Shape.Fill\"><Setter.Value><SolidColorBrush>#00FFFFFF</SolidColorBrush></Setter.Value></Setter><Setter Property=\"Shape.Stroke\"><Setter.Value><SolidColorBrush>#00FFFFFF</SolidColorBrush></Setter.Value></Setter></Style></Path.Style></Path></ControlTemplate></dd:DesignerItem.DragThumbTemplate><dd:DesignerItem.ConnectorDecoratorTemplate><ControlTemplate><ddc:RelativePositionPanel Margin=\"-4,-4,-4,-4\"><dd:Connector Orientation=\"Top\" Position=\"0,0\" Name=\"Top\" ddc:RelativePositionPanel.RelativePosition=\"0.5,0\" /><dd:Connector Orientation=\"Left\" Position=\"0,0\" Name=\"Left\" ddc:RelativePositionPanel.RelativePosition=\"0,0.5\" /><dd:Connector Orientation=\"Right\" Position=\"0,0\" Name=\"Right\" ddc:RelativePositionPanel.RelativePosition=\"1,0.5\" /><dd:Connector Orientation=\"Bottom\" Position=\"0,0\" Name=\"Bottom\" ddc:RelativePositionPanel.RelativePosition=\"0.5,0.93\" /></ddc:RelativePositionPanel></ControlTemplate></dd:DesignerItem.ConnectorDecoratorTemplate></Path><TextBox BorderThickness=\"0,0,0,0\" Background=\"#00FFFFFF\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" Visibility=\"Hidden\">Resultados</TextBox></Grid>";
        private const double LEFT_TO_INIT_PRINCIPAL = 44.5;

        public PrintResults()
        {
            //logger.Info("Inicio Imprimir Resultados");
            this.designerItem.Content = XamlReader.Parse(CONTENT);

            //logger.Info("Fin Imprimir Resultados");
        }

        public PrintResults(string itemText)
            : this()
        {
            this.setItemText(itemText);
        }

        public override double getLeftReference()
        {
            return LEFT_TO_INIT_PRINCIPAL;
        }
    }
}
