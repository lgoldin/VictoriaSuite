using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Victoria.DesktopApp.DiagramDesigner.Nodes
{
    abstract class Node
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        public Guid guid { get; set; }
        public DesignerItem designerItem { get; set; }

        private const double CENTER_POSITION_LEFT = 0;

        public Node() 
        {
            logger.Info("Inicio set propiedades Nodo");
            this.guid = Guid.NewGuid();
            this.designerItem = new DesignerItem(this.guid);
            this.designerItem.ParentID = new Guid("00000000-0000-0000-0000-000000000000");
            this.designerItem.IsGroup = false;
            logger.Info("Fin set propiedades Nodo");
        }

        public void setItemText(string text)
        {
            logger.Info("Inicio set Item Nodo");
            if (this.designerItem.Content is Grid)
            {
                Grid hijo = (Grid)this.designerItem.Content;
                foreach (UIElement item in hijo.Children)
                {
                    item.SetValue(UIElement.VisibilityProperty, Visibility.Visible);
                }

                this.designerItem.Content = hijo;
                (this.designerItem.Content as Grid).Children.OfType<TextBox>().First().Text = text;
            }

            logger.Info("Fin set Item Nodo");
        }

        abstract public double getLeftReference();
    }
}
