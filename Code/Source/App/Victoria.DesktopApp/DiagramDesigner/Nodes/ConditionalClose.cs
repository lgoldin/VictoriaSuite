using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Victoria.DesktopApp.DiagramDesigner.Nodes
{
    class ConditionalClose: Node
    {
        private const string CONTENT = "<Grid ToolTip=\"Cierre condición\" ToolTipService.IsEnabled=\"False\" ToolTipService.InitialShowDelay=\"0\" ToolTipService.BetweenShowDelay=\"0\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:s=\"clr-namespace:System;assembly=mscorlib\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dd=\"clr-namespace:DiagramDesigner;assembly=Victoria\"><Path ToolTip=\"nodo_condicion_cierre\"><Path.Style><Style TargetType=\"Path\"><Style.BasedOn><Style TargetType=\"Path\"><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Shape.Fill\"><Setter.Value><LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"0,1\"><LinearGradientBrush.GradientStops><GradientStop Color=\"#FFFAFBE9\" Offset=\"0\" /><GradientStop Color=\"#FFFFA500\" Offset=\"1\" /></LinearGradientBrush.GradientStops></LinearGradientBrush></Setter.Value></Setter><Setter Property=\"Shape.Stroke\"><Setter.Value><SolidColorBrush>#FFD69436</SolidColorBrush></Setter.Value></Setter><Setter Property=\"Shape.StrokeThickness\"><Setter.Value><s:Double>1</s:Double></Setter.Value></Setter><Setter Property=\"Shape.StrokeLineJoin\"><Setter.Value><x:Static Member=\"PenLineJoin.Round\" /></Setter.Value></Setter><Setter Property=\"Shape.Stretch\"><Setter.Value><x:Static Member=\"Stretch.Fill\" /></Setter.Value></Setter><Setter Property=\"UIElement.IsHitTestVisible\"><Setter.Value><s:Boolean>False</s:Boolean></Setter.Value></Setter><Setter Property=\"UIElement.SnapsToDevicePixels\"><Setter.Value><s:Boolean>True</s:Boolean></Setter.Value></Setter></Style></Style.BasedOn><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Path.Data\"><Setter.Value><StreamGeometry>M0,0L60,0 60,40 0,40z</StreamGeometry></Setter.Value></Setter><Setter Property=\"FrameworkElement.Height\"><Setter.Value><s:Double>10</s:Double></Setter.Value></Setter></Style></Path.Style><dd:DesignerItem.DragThumbTemplate><ControlTemplate><Path><Path.Style><Style TargetType=\"Path\"><Style.BasedOn><Style TargetType=\"Path\"><Style.BasedOn><Style TargetType=\"Path\"><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Shape.Fill\"><Setter.Value><LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"0,1\"><LinearGradientBrush.GradientStops><GradientStop Color=\"#FFFAFBE9\" Offset=\"0\" /><GradientStop Color=\"#FFFFA500\" Offset=\"1\" /></LinearGradientBrush.GradientStops></LinearGradientBrush></Setter.Value></Setter><Setter Property=\"Shape.Stroke\"><Setter.Value><SolidColorBrush>#FFD69436</SolidColorBrush></Setter.Value></Setter><Setter Property=\"Shape.StrokeThickness\"><Setter.Value><s:Double>1</s:Double></Setter.Value></Setter><Setter Property=\"Shape.StrokeLineJoin\"><Setter.Value><x:Static Member=\"PenLineJoin.Round\" /></Setter.Value></Setter><Setter Property=\"Shape.Stretch\"><Setter.Value><x:Static Member=\"Stretch.Fill\" /></Setter.Value></Setter><Setter Property=\"UIElement.IsHitTestVisible\"><Setter.Value><s:Boolean>False</s:Boolean></Setter.Value></Setter><Setter Property=\"UIElement.SnapsToDevicePixels\"><Setter.Value><s:Boolean>True</s:Boolean></Setter.Value></Setter></Style></Style.BasedOn><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"Path.Data\"><Setter.Value><StreamGeometry>M0,0L60,0 60,40 0,40z</StreamGeometry></Setter.Value></Setter><Setter Property=\"FrameworkElement.Height\"><Setter.Value><s:Double>10</s:Double></Setter.Value></Setter></Style></Style.BasedOn><Style.Resources><ResourceDictionary /></Style.Resources><Setter Property=\"UIElement.IsHitTestVisible\"><Setter.Value><s:Boolean>True</s:Boolean></Setter.Value></Setter><Setter Property=\"Shape.Fill\"><Setter.Value><SolidColorBrush>#00FFFFFF</SolidColorBrush></Setter.Value></Setter><Setter Property=\"Shape.Stroke\"><Setter.Value><SolidColorBrush>#00FFFFFF</SolidColorBrush></Setter.Value></Setter><Setter Property=\"FrameworkElement.Height\"><Setter.Value><s:Double>10</s:Double></Setter.Value></Setter></Style></Path.Style></Path></ControlTemplate></dd:DesignerItem.DragThumbTemplate></Path><TextBox BorderBrush=\"#00FFFFFF\" BorderThickness=\"0,0,0,0\" Background=\"#00FFFFFF\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" Visibility=\"Hidden\" IsHitTestVisible=\"False\" xml:space=\"preserve\" /></Grid>";
        private const double LEFT_TO_INIT_PRINCIPAL = 44.5;
        public ConditionalClose()
        {
            this.designerItem.Content = XamlReader.Parse(CONTENT);
        }

        public ConditionalClose(string itemText)
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
