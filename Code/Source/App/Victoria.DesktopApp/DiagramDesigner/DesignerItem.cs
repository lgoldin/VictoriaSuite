﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using DiagramDesigner.Controls;
using Victoria.DesktopApp.DiagramDesigner.Nodes;
using Victoria.Shared;
using Node = Victoria.DesktopApp.DiagramDesigner.Nodes.Node;
using Path = System.Windows.Shapes.Path;

namespace DiagramDesigner
{
    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    
    public class DesignerItem : ContentControl, ISelectable, IGroupable
    {
        #region ID
        private Guid id;
        public Guid ID
        {
            get { return id; }
        }
        #endregion

        #region ParentID
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(DesignerItem));
        #endregion

        #region IsGroup
        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }
        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(DesignerItem));
        #endregion

        #region IsSelected Property

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DesignerItem),
                                       new FrameworkPropertyMetadata(false));

        #endregion

        #region DragThumbTemplate Property

        // can be used to replace the default template for the DragThumb
        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
        }

        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        #endregion

        #region ConnectorDecoratorTemplate Property

        // can be used to replace the default template for the ConnectorDecorator
        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        #endregion

        #region IsDragConnectionOver

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectorDecorator is triggered
        // to be visible, see template
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }
        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                                         typeof(bool),
                                         typeof(DesignerItem),
                                         new FrameworkPropertyMetadata(false));
        #endregion

        #region hasBreakpoint

        public bool hasBreakpoint
        {
            get { return (bool)GetValue(hasBreakpointProperty); }
            set { SetValue(hasBreakpointProperty, value); }
        }
        public static readonly DependencyProperty hasBreakpointProperty =
            DependencyProperty.Register("hasBreakpoint",
                                         typeof(bool),
                                         typeof(DesignerItem),
                                         new FrameworkPropertyMetadata(false));

        #endregion

        #region originalColor Property

        private Brush originalColor;

        #endregion

        private List<String> lstNodesToBreakpoint = new List<string> { "nodo_sentencia", "nodo_condicion" };
        private static List<DesignerItem> nodesWithBreakPoints = new List<DesignerItem>();        

        static DesignerItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }

        public DesignerItem(Guid id)
        {
            this.id = id;
            this.Loaded += new RoutedEventHandler(DesignerItem_Loaded);
            this.MouseDoubleClick += DesignerItem_MouseDoubleClick;
        }

        public DesignerItem()
            : this(Guid.NewGuid())
        {
            this.MouseDoubleClick += DesignerItem_MouseDoubleClick;
        }

        public DesignerItem(Guid id, string tag, string uid)
        {
            this.id = id;
            this.Tag = tag;
            this.Uid = uid;
            this.MouseDoubleClick += DesignerItem_MouseDoubleClick;
        }


        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
            
            // update selection
            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                    if (this.IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                else if (!this.IsSelected)
                {
                    designer.SelectionService.SelectItem(this);
                }
                Focus();
            }

            e.Handled = false;
        }

        void DesignerItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            if ( (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None )
            {
                
                try
                {
                    Grid grid = (Grid)this.Content;
                    Path shape = (Path)grid.Children[0];

                    if (lstNodesToBreakpoint.Contains(shape.ToolTip.ToString()))
                    {
                        
                        //Cambio color del borde a rojo para indicar breakpoint
                        if (!this.hasBreakpoint)
                        {
                            this.originalColor = shape.Stroke;
                            changeColor(this, Brushes.Red);
                            nodesWithBreakPoints.Add(this);
                        }
                        else
                        {
                            changeColor(this, this.originalColor);
                            nodesWithBreakPoints.Remove(this);
                        }

                        grid.Tag = ToogleBreakPoint();
                        
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("No puede agregarse un breakpoint a este nodo");
                }

            }

        }

        private static void changeColor(DesignerItem node, Brush color)
        {
            Grid grid = (Grid)node.Content;
            Path shape = (Path)grid.Children[0];
            shape.Stroke = color;
        }

        public static Boolean ifAnyNodeHasBreakpoint() {
            return nodesWithBreakPoints.Count > 0;
        }

        public static void setDebugColor(DesignerItem executing_node,DesignerItem previous_node) {
                       
            changeColor(executing_node, Brushes.Blue);

            if (previous_node != null)
            {
                if (nodesWithBreakPoints.Contains(previous_node))
                {
                    changeColor(previous_node, Brushes.Red);
                }
                else
                {
                    changeColor(previous_node, Brushes.DarkOrange);
                }
                
            }
                
        }        

        String ToogleBreakPoint()
        {
            this.hasBreakpoint = !this.hasBreakpoint;            
            return this.hasBreakpoint ? "BreakPoint" : String.Empty;
        }

        void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                
                if (contentPresenter != null)
                {
                    UIElement contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                    
                    if (contentVisual != null)
                    {
                        DragThumb thumb = this.Template.FindName("PART_DragThumb", this) as DragThumb;
                        if (thumb != null)
                        {
                            ControlTemplate template =
                                DesignerItem.GetDragThumbTemplate(contentVisual) as ControlTemplate;
                            if (template != null)
                                thumb.Template = template;
                            
                            //Para que sea responsive
                            this.Height = (double)contentVisual.GetAnimationBaseValue(HeightProperty);
                            this.Width = (double)contentVisual.GetAnimationBaseValue(WidthProperty);

                            /*Traigo la TAG del elemento hijo y la convierto en string*/
                            var tag = (contentVisual.GetAnimationBaseValue(TagProperty) ?? "" ).ToString();
                                 if (tag == "DIAG")
                                     {
                                       this.Tag = "DIAG"; //Le pongo la TAG al item del diagram designer
                                     }
                                 else if (tag == "COND")  //Si es un nodo_condicion le cambio la altura
                                     {
                                      this.Height = 60;
                                     }
                                 else if (tag == "REFR") 
                                     {
                                  this.Tag = "REFR";
                                    }
                            /*Traigo el Uid del elemento hijo y la convierto en string*/
                            var uid = (contentVisual.GetAnimationBaseValue(UidProperty) ?? "").ToString();
                                 if (uid == "Principal")
                                    {
                                     this.Uid = "Principal"; //Le pongo la uid al item del diagram designer
                                    }


                        }
                    }
                }
            }
        }
    }
}
