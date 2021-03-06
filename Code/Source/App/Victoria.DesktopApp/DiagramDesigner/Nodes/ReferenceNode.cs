﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Victoria.DesktopApp.DiagramDesigner.Nodes
{
    class ReferenceNode : Node
    {
        private const string CONTENT = "<Grid Tag=\"REFR\" MinWidth=\"40\" MinHeight=\"40\" ToolTip=\"Referencia\" ToolTipService.IsEnabled=\"False\" ToolTipService.InitialShowDelay=\"0\" ToolTipService.BetweenShowDelay=\"0\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Ellipse ToolTip=\"nodo_referencia\" IsHitTestVisible=\"False\"><Ellipse.Fill><RadialGradientBrush Center=\"0.2,0.2\" RadiusX=\"0.8\" RadiusY=\"0.8\" GradientOrigin=\"0.2,0.2\"><RadialGradientBrush.GradientStops><GradientStop Color=\"#FFFFFFFF\" Offset=\"0\" /><GradientStop Color=\"#FF0000FF\" Offset=\"0.9\" /></RadialGradientBrush.GradientStops></RadialGradientBrush></Ellipse.Fill></Ellipse><TextBox BorderThickness=\"0,0,0,0\" Background=\"#00FFFFFF\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\" Visibility=\"Hidden\">Ref</TextBox></Grid>";
        private const double LEFT_TO_INIT_PRINCIPAL = 55;

        public ReferenceNode()
        {
            //logger.Info("Nodo Referencia");
            this.designerItem.Content = XamlReader.Parse(CONTENT);
        }

        public ReferenceNode(string itemText)
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
