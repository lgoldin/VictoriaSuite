using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using Victoria.DesktopApp;
using Victoria.Shared;
using Victoria.DesktopApp.Behavior;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Victoria.Shared.AnalisisPrevio;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Victoria.DesktopApp.View;
using System.Printing;
using System.Reflection;
using Victoria.DesktopApp.DiagramDesigner;
using Victoria.DesktopApp.Helpers;

namespace DiagramDesigner
{
    public partial class DesignerCanvas
    {
        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        public static RoutedCommand DistributeVertical = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();

        public DataGrid dataGridVariables { get; internal set; }
        public DataGridComboBoxColumn dimensiones { get; internal set; }
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));


        public DesignerCanvas()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, Erase_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Simulate_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Group, Group_Executed, Group_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Ungroup, Ungroup_Executed, Ungroup_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringForward, BringForward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringToFront, BringToFront_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendBackward, SendBackward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendToBack, SendToBack_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignTop, AlignTop_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignBottom, AlignBottom_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignLeft, AlignLeft_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignRight, AlignRight_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeHorizontal, DistributeHorizontal_Executed, Distribute_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeVertical, DistributeVertical_Executed, Distribute_Enabled));
            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SelectAll, SelectAll_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.PrintPreview, Imprimir_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, Help_Executed));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            this.AllowDrop = true;
            Clipboard.Clear();
        }

        private void Imprimir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)this.Parent;
            scroll.ScrollToTop();
            ImprimirDiagrama();
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DarPDFAlUsuario();
        }

        #region New Command

        private void Erase_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BorrarDiagrama();
        }

        #endregion

        #region Open Command

        public void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AbrirDiagrama();
        }

        #endregion

        #region Save Command

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GuardarDiagrama();
        }

        #endregion

        #region Print Command

        private void Simulate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ValidarYLanzarSimulador();
        }

        #endregion

        #region Copy Command

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopiarSeleccionDiagrama();
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Paste Command

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PegarSeleccionDiagrama();
        }

        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);
        }

        #endregion

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BorrarSeleccionDiagrama();
        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Cut Command

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopiarSeleccionDiagrama();
            BorrarSeleccionDiagrama();
        }

        private void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Group Command

        private void Group_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AgruparSeleccionDiagrama();
        }

        private void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentID == Guid.Empty
                         select item).Count();
            e.CanExecute = count > 1;
        }

        #endregion

        #region Ungroup Command

        private void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesagruparSeleccionDiagrama();
        }

        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentID != Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 0;
        }

        #endregion

        #region BringForward Command

        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) descending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
            e.CanExecute = true;
        }

        #endregion

        #region BringToFront Command

        private void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();

            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, childrenSorted.Count - selectionSorted.Count + j++);
                }
                else
                {
                    Canvas.SetZIndex(item, i++);
                }
            }
        }

        #endregion

        #region SendBackward Command

        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) ascending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Max(i, currentIndex - 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region SendToBack Command

        private void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();
            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, j++);

                }
                else
                {
                    Canvas.SetZIndex(item, selectionSorted.Count + i++);
                }
            }
        }

        #endregion

        #region AlignTop Command

        private void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = top - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        private void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region AlignVerticalCenters Command

        private void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignBottom Command

        private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignLeft Command

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = left - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignHorizontalCenters Command

        private void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double center = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = center - (Canvas.GetLeft(item) + item.Width / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region AlignRight Command

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double right = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = right - (Canvas.GetLeft(item) + item.Width);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
        }

        #endregion

        #region DistributeHorizontal Command

        private void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemLeft = Canvas.GetLeft(item)
                                orderby itemLeft
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Double.MaxValue;
                double right = Double.MinValue;
                double sumWidth = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    left = Math.Min(left, Canvas.GetLeft(item));
                    right = Math.Max(right, Canvas.GetLeft(item) + item.Width);
                    sumWidth += item.Width;
                }

                double distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                double offset = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                    offset = offset + item.Width + distance;
                }
            }
        }

        private void Distribute_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region DistributeVertical Command

        private void DistributeVertical_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemTop = Canvas.GetTop(item)
                                orderby itemTop
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Double.MaxValue;
                double bottom = Double.MinValue;
                double sumHeight = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    top = Math.Min(top, Canvas.GetTop(item));
                    bottom = Math.Max(bottom, Canvas.GetTop(item) + item.Height);
                    sumHeight += item.Height;
                }

                double distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                double offset = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                    offset = offset + item.Height + distance;
                }
            }
        }

        #endregion

        #region SelectAll Command

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.SelectAll();
        }

        #endregion

        #region SaveAs Command

        #endregion

        #region Helper Methods

        private XElement SerializarDesignerItems(IEnumerable<DesignerItem> designerItems)
        {
            List<Connector> connectors2 = new List<Connector>();
            List<Connection> connectors3 = new List<Connection>();


            foreach (DesignerItem item in designerItems)
            {
                Control cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;
                GetConnectors(cd, connectors2);
            }

            XElement serializedItems = new XElement("Diagrama",
                                       new XElement("Flowchart",
                                       from item in designerItems
                                       let contentXaml = XamlWriter.Save(((DesignerItem)item).Content)
                                       select new XElement("DesignerItem",
                                                  new XElement("Left", Canvas.GetLeft(item)),
                                                  new XElement("Top", Canvas.GetTop(item)),
                                                  new XElement("Width", item.Width),
                                                  new XElement("Height", item.Height),
                                                  new XElement("ID", item.ID),
                                                  new XElement("Tag", item.Tag),
                                                  new XElement("Uid", item.Uid),
                                                  new XElement("zIndex", Canvas.GetZIndex(item)),
                                                  new XElement("IsGroup", item.IsGroup),
                                                  new XElement("ParentID", item.ParentID),
                                                  new XElement("Content", contentXaml),
                                                  new XElement("Connection", connectors2)

                                              )
                                   ));
            return serializedItems;
        }

        private XElement SerializarConnections(IEnumerable<Connection> connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceOrientation", connection.Source.Orientation),
                                      new XElement("SinkOrientation", connection.Sink.Orientation),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", Canvas.GetZIndex(connection))
                                     )
                                  );

            return serializedConnections;
        }

        public static DesignerItem DeserializarDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            DesignerItem item = new DesignerItem(id);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
            Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
            item.Content = content;
            return item;
        }

        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby Canvas.GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                Canvas.SetZIndex(ordered[i], i);
            }
        }

        private static Rect GetBoundingRectangle(IEnumerable<DesignerItem> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItem item in items)
            {
                x1 = Math.Min(Canvas.GetLeft(item), x1);
                y1 = Math.Min(Canvas.GetTop(item), y1);

                x2 = Math.Max(Canvas.GetLeft(item) + item.Width, x2);
                y2 = Math.Max(Canvas.GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        private void GetConnectors(DependencyObject parent, List<Connector> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                    GetConnectors(child, connectors);
            }
        }

        public Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();

            Control connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }

        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        private void ImprimirDiagrama()
        {
            logger.Info("Inicio Imprimir Diagrama");
            SelectionService.ClearSelection();
            PrintDialog printDialog = new PrintDialog();
            if (true == printDialog.ShowDialog()) printDialog.PrintVisual(this, "Diagrama");
            logger.Info("Fin Imprimir Diagrama");
        }

        private void DarPDFAlUsuario()
        {

            logger.Info("Inicio dar PDF Al Usuario");
            var parentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var sourcePath = Path.Combine(parentFolder, @"Manual de usuario\Manual de usuario Victoria.pdf");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Manual de usuario Victoria.pdf";
            saveFileDialog.Filter = "Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(sourcePath, saveFileDialog.FileName, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    logger.Error("Error dar PDF Al Usuario "+ex.Message);
                }
            }

            logger.Info("Fin dar PDF Al Usuario");
        }

        public void AbrirDiagrama()
        {
            try
            {

                logger.Info("Inicio Abrir Diagrama");
                XElement root = LoadSerializedDataFromFile();

                if (root == null)
                    return;

                this.Children.Clear();
                this.SelectionService.ClearSelection();

                string content = root.Element("variables").Value;
                JObject jobject = JObject.Parse(content);
                List<VariableAP> variables = JsonConvert.DeserializeObject<List<VariableAP>>(jobject.GetValue("variables").ToString());
                dataGridVariables.ItemsSource = variables;
                dimensiones.ItemsSource = variables.Where(x => x.type == VariableType.Control);
                IEnumerable<XElement> itemsXML = root.Elements("Diagrama").Elements("Flowchart").Elements("DesignerItem");
                foreach (XElement itemXML in itemsXML)
                {
                    Guid id = new Guid(itemXML.Element("ID").Value);
                    DesignerItem item = DeserializarDesignerItem(itemXML, id, 0, 0);
                    this.Children.Add(item);
                    SetConnectorDecoratorTemplate(item);
                }

                this.InvalidateVisual();

                IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
                foreach (XElement connectionXML in connectionsXML)
                {
                    Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                    Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                    String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                    String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                    Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                    Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

                    Connection connection = new Connection(sourceConnector, sinkConnector);
                    Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                    this.Children.Add(connection);
                }

                logger.Info("Fin Abrir Diagrama");
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al abrir el diagrama.");
                viewException.ShowDialog();
                logger.Error("Se produjo un error al abrir el diagrama: "+ex.Message);
                return;
            }
        }

        private void BorrarDiagrama()
        {

            logger.Info("Inicio Borrar Diagrama");
            this.Children.Clear();
            this.SelectionService.ClearSelection();

            logger.Info("Fin Guardar Diagrama");
            /*var viewDeleteDiagram = new DeleteDiagramPopUp();
            viewDeleteDiagram.ShowDialog();


            switch (viewDeleteDiagram.Result)
            {
                case Victoria.UI.SharedWPF.DialogResult.Accept:
                    this.Children.Clear();
                    this.SelectionService.ClearSelection();
                    break;
                default:
                    return;
            }*/
        }

        private void GuardarDiagrama()
        {

            logger.Info("Inicio Guardar Diagrama");
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            XElement designerItemsXML = SerializarDesignerItems(designerItems);
            XElement connectionsXML = SerializarConnections(connections);

            XElement root = new XElement("Simulacion");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);
            var variables = JsonConvert.DeserializeObject<List<VariableAP>>(collectionJson());
            HelperVIC helperVic = new HelperVIC();
            root.Add(helperVic.generarTagDeVariables(variables));

            GuardarArchivoDialog(root);

            logger.Info("Fin Guardar Diagrama");
        }

        void GuardarArchivoDialog(XElement xElement)
        {
            logger.Info("Inicio Guardar Archivo");
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    logger.Error("Error al Guardar Archivo: "+ ex.Message);
                }
            }
            logger.Info("Fin Guardar Archivo");
        }

        private XElement LoadSerializedDataFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                return XElement.Load(openFile.FileName);
            }

            return null;
        }

        private void ValidarYLanzarSimulador()
        {
            try
            {
                logger.Info("Inicio Validar y Lanzar Simulador");
                ValidarDiagrama();
                var root = this.GenerarVicXmlDelDiagrama();
                var mainWindow = new MainWindow(root.ToString(), true);
                mainWindow.Show();
                logger.Info("Fin Validar y Lanzar Simulador");

            }
            catch (DiagramValidationException ex)
            {
                var viewException = new AlertPopUp(ex.Message);
                viewException.ShowDialog();
                logger.Error("Validar y Lanzar Simulador: "+ex.Message);
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Error de parseo. Revisa tu diagrama.");
                viewException.ShowDialog();
                logger.Error("Error de parseo.Revisa tu diagrama. " + ex.Message);

            }
        }

        private void ValidarDiagrama()
        {

            logger.Info("Inicio Validar Diagrama");
            var errorList = ValidateUseOfCorrectVariables().Concat(ValidateFinDiagrama());
            if (errorList.Any())
            {
                throw new DiagramValidationException(String.Join("\n", errorList.ToArray()));
            }
            logger.Info("Fin Validar Diagrama");
        }

        private HashSet<string> ValidateUseOfCorrectVariables()
        {

            logger.Info("Inicio Validar Uso Correcto de Variables");
            var variables = JsonConvert.DeserializeObject<List<VariableAP>>(collectionJson());

            foreach (var variable in variables)
            {
                variable.nombre = variable.nombre.Split('(')[0];
            }

            var variableNames = variables.Select(x => x.nombre).ToList();

            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            var errorList = new HashSet<string>();
            var referencesList = new List<string>();

            foreach (var item in designerItems)
            {
                if ((item.GetAnimationBaseValue(TagProperty) ?? "").ToString() == "")
                {
                    var tipo = "";
                    var contenido = (Grid)item.Content;
                    var listaChildren = contenido.Children;
                    var pretipo = listaChildren.OfType<System.Windows.Shapes.Path>().FirstOrDefault();
                    if (pretipo != null)
                    {
                        tipo = ((UIElement)pretipo).GetAnimationBaseValue(ToolTipProperty).ToString();
                    }
                    var textBox = listaChildren.OfType<TextBox>().FirstOrDefault();

                    if (tipo.ToString() == "nodo_sentencia" || tipo.ToString() == "nodo_condicion")
                    {
                        ValidateUseOfCorrectVariablesInTextBox(errorList, textBox.Text, variableNames);
                        ValidateUseOfCorrectCharacters(errorList, textBox.Text);
                        if (tipo.ToString() == "nodo_condicion")
                        {
                            ValidateUseOfValidConditionOperators(errorList, textBox.Text);
                        }
                    }
                }

                if (((System.Windows.Controls.Grid)item.Content).Tag != null && ((System.Windows.Controls.Grid)item.Content).Tag.ToString() == "REFR")
                {

                    var tipo = "";
                    var contenido = (Grid)item.Content;
                    var listaChildren = contenido.Children;
                    var pretipo = listaChildren.OfType<System.Windows.Shapes.Ellipse>().FirstOrDefault();
                    if (pretipo != null)
                    {
                        tipo = ((UIElement)pretipo).GetAnimationBaseValue(ToolTipProperty).ToString();
                    }
                    var textBox = listaChildren.OfType<TextBox>().FirstOrDefault();

                    if (tipo.ToString() == "nodo_referencia")
                    {
                        referencesList.Add(textBox.Text);
                    }
                }
            }

            ValidateReferences(errorList, referencesList);


            logger.Info("Fin Validar Uso Correcto de Variables");
            return errorList;
        }

        private void ValidateReferences(HashSet<string> errorLIst, List<string> referencesList)
        {
            var masDeDosReferencias = referencesList.GroupBy(x => x)
                        .Where(group => group.Count() > 2)
                        .Select(group => group.Key);

            var soloUnaReferencia = referencesList.GroupBy(x => x)
                        .Where(group => group.Count() == 1)
                        .Select(group => group.Key);

            if (masDeDosReferencias.Any())
            {
                foreach (var refe in masDeDosReferencias)
                {
                    errorLIst.Add("-Se esta repitiendo el uso de la referencia " + '"' + refe + '"' + ". Por favor, utilice otro nombre.");
                }
            }

            if (soloUnaReferencia.Any())
            {
                foreach (var refe in soloUnaReferencia)
                {
                    errorLIst.Add("-No se esta cerrando el uso de la referencia " + '"' + refe + '"' + ".");
                }
            }
        }

        private HashSet<string> ValidateFinDiagrama()
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();
            var cantidadNodosFin = 0;
            var cantidadDiagramas = 0;
            var errorList = new HashSet<string>();

            foreach (var item in designerItems)
            {

                if ((item.GetAnimationBaseValue(TagProperty) ?? "").ToString() == "DIAG")
                {
                    cantidadDiagramas++;
                }
                if ((item.GetAnimationBaseValue(TagProperty) ?? "").ToString() == "")
                {

                    var tipo = "";
                    var contenido = (Grid)item.Content;
                    var listaChildren = contenido.Children;
                    var pretipo = listaChildren.OfType<System.Windows.Shapes.Ellipse>().FirstOrDefault();
                    if (pretipo != null)
                    {
                        tipo = ((UIElement)pretipo).GetAnimationBaseValue(ToolTipProperty).ToString();
                    }
                    var textBox = listaChildren.OfType<TextBox>().FirstOrDefault();

                    if (tipo.ToString() == "nodo_fin")
                    {
                        cantidadNodosFin++;
                    }
                }
            }
            if (cantidadDiagramas != cantidadNodosFin)
            {
                errorList.Add("-Tenes " + cantidadDiagramas + " diagrama/s y " + cantidadNodosFin + " nodo/s de cierre. Deben coincidir.");
            }
            return errorList;
        }
        
        private void ValidateUseOfCorrectVariablesInTextBox(HashSet<string> errorLIst, string textBoxText, List<string> variableNames)
        {

            logger.Info("Inicio Validar Caracteres");
            var regex = "[a-zA-Z0-9]+";
            var matches = Regex.Matches(textBoxText, regex);

            foreach (var match in matches.Cast<Match>().Select(match => match.Value.ToUpper()).ToList())
            {
                int n;
                bool isNumeric = int.TryParse(match, out n);

                if (!isNumeric && !variableNames.Contains(match) && match != "T" && match != "R" && 
                    match != "FACTORIAL" && match != "LN" && match != "E" && match != "LOG" && match != "NOT" 
                    && match != "PI" && match != "RANDOM" && match != "SUMATORIA" && match != "INT")
                {
                    errorLIst.Add("-Estas utilizando una variable " + '"' + match + '"' + " no declarada.");
                }
            }

            logger.Info("Fin Validar Caracteres");
        }

        private void ValidateUseOfCorrectCharacters(HashSet<string> errorLIst, string textBoxText)
        {

            logger.Info("Inicio Validar Caracteres");
            var regex = @"(?![a-zA-Z0-9\!\&\|\ \<\>\%\^\+\=\-\*\/\(\)\,]+).";
            var matches = Regex.Matches(textBoxText, regex);

            foreach (var match in matches.Cast<Match>().Select(match => match.Value).ToList())
            {
                errorLIst.Add("-Estas utilizando un caracter desconocido " + '"' + match + '"' + ".");
            }
            logger.Info("Fin Validar Caracteres");
        }

        private void ValidateUseOfValidConditionOperators(HashSet<string> errorLIst, string textBoxText)
        {
            logger.Info("Inicio Validar Condiciones y Operadores");
            var regex = @"[^a-zA-Z0-9\ ]+";
            var operatorsUsed = Regex.Matches(textBoxText, regex);

            foreach (var op in operatorsUsed.Cast<Match>().Select(op => op.Value).ToList())
            {
                if (op != "==" && op != "<=" && op != ">=" && op != "!=" && op != "<" && op != ">" && op != "&&" && op != "||" && op != "(" && op != ")" && op != "-" && op != "+" && op != "*" && op != "/" && op != "%")
                {
                    errorLIst.Add("-Estas utilizando un operador incorrecto " + '"' + op + '"' + " en una condición.");
                }
                if (op == "=")
                {
                    errorLIst.Add("Para hacer comparaciones utilizá " + '"' + "==" + '"' + ".");
                }
            }

            logger.Info("Fin Validar Condiciones y Operadores");
        }

        private XElement GenerarVicXmlDelDiagrama()
        {
            var variables = JsonConvert.DeserializeObject<List<VariableAP>>(collectionJson());

            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            HelperVIC helperVIC = new HelperVIC();
            List<XElement> listaDesignerItemsXML = helperVIC.SerializeVic(designerItems, connections, variables);

            XElement root = new XElement("Simulacion");
            XElement modelo = serializarModelo(designerItems, connections);
            root.Add(modelo);
            listaDesignerItemsXML.ForEach(root.Add);

            return root;
        }

        private XElement serializarModelo(IEnumerable<DesignerItem> designerItems, IEnumerable<Connection> connections)
        {
            logger.Info("Inicio Sereliazar Modelo");
            XElement modelo = new XElement("Modelo");
            XElement designerItemsXML = SerializarDesignerItems(designerItems);
            XName name = "Name";
            designerItemsXML.SetAttributeValue(name, "ModeloAnalisisSensibilidad");
            XElement connectionsXML = SerializarConnections(connections);
            connectionsXML.SetAttributeValue(name, "ModeloAnalisisSensibilidad");
            modelo.Add(designerItemsXML);
            modelo.Add(connectionsXML);
            logger.Info("Fin Serializar Modelo");
            return modelo;

        }

        private void CopiarSeleccionDiagrama()
        {
            logger.Info("Inicio Copiar Seleccion Diagrama");
            IEnumerable<DesignerItem> selectedDesignerItems =
                this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            List<Connection> selectedConnections =
                this.SelectionService.CurrentSelection.OfType<Connection>().ToList();

            foreach (Connection connection in this.Children.OfType<Connection>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    DesignerItem sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    DesignerItem sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
                logger.Info("Fin Borrar Seleccion Diagrama");
            }

            XElement designerItemsXML = SerializarDesignerItems(selectedDesignerItems);
            XElement connectionsXML = SerializarConnections(selectedConnections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private void PegarSeleccionDiagrama()
        {
            logger.Info("Inicio Pegar Seleccion Diagrama");
            XElement root = LoadSerializedDataFromClipBoard();

            if (root == null)
                return;

            // create DesignerItems
            Dictionary<Guid, Guid> mappingOldToNewIDs = new Dictionary<Guid, Guid>();
            List<ISelectable> newItems = new List<ISelectable>();
            IEnumerable<XElement> itemsXML = root.Elements("Diagrama").Elements("Flowchart").Elements("DesignerItem");

            double offsetX = Double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
            double offsetY = Double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);

            foreach (XElement itemXML in itemsXML)
            {
                Guid oldID = new Guid(itemXML.Element("ID").Value);
                Guid newID = Guid.NewGuid();
                mappingOldToNewIDs.Add(oldID, newID);
                DesignerItem item = DeserializarDesignerItem(itemXML, newID, offsetX, offsetY);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
                newItems.Add(item);
            }

            // update group hierarchy
            SelectionService.ClearSelection();
            foreach (DesignerItem el in newItems)
            {
                if (el.ParentID != Guid.Empty)
                    el.ParentID = mappingOldToNewIDs[el.ParentID];
            }


            foreach (DesignerItem item in newItems)
            {
                if (item.ParentID == Guid.Empty)
                {
                    SelectionService.AddToSelection(item);
                }
            }

            // create Connections
            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid oldSourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid oldSinkID = new Guid(connectionXML.Element("SinkID").Value);

                if (mappingOldToNewIDs.ContainsKey(oldSourceID) && mappingOldToNewIDs.ContainsKey(oldSinkID))
                {
                    Guid newSourceID = mappingOldToNewIDs[oldSourceID];
                    Guid newSinkID = mappingOldToNewIDs[oldSinkID];

                    String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                    String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                    Connector sourceConnector = GetConnector(newSourceID, sourceConnectorName);
                    Connector sinkConnector = GetConnector(newSinkID, sinkConnectorName);

                    Connection connection = new Connection(sourceConnector, sinkConnector);
                    Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                    this.Children.Add(connection);

                    SelectionService.AddToSelection(connection);
                }
            }

            DesignerCanvas.BringToFront.Execute(null, this);

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
            logger.Info("Fin Pegar Seleccion Diagrama");
        }

        private void BorrarSeleccionDiagrama()
        {
            logger.Info("Inicio Borrar Seleccion Diagrama");
            foreach (Connection connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                this.Children.Remove(connection);
            }

            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                Control cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                List<Connector> connectors = new List<Connector>();
                GetConnectors(cd, connectors);

                foreach (Connector connector in connectors)
                {
                    foreach (Connection con in connector.Connections)
                    {
                        this.Children.Remove(con);
                    }
                }
                this.Children.Remove(item);
            }

            SelectionService.ClearSelection();
            UpdateZIndex();
            logger.Info("Fin Borrar Seleccion Diagrama");
        }

        private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;
                if (String.IsNullOrEmpty(clipboardData)) return null;

                try
                {
                    return XElement.Load(new StringReader(clipboardData));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        private void AgruparSeleccionDiagrama()
        {
            logger.Info("Inicio Agrupar Seleccion Diagrama");
            var items = from item in this.SelectionService.CurrentSelection.OfType<DesignerItem>()
                        where item.ParentID == Guid.Empty
                        select item;

            Rect rect = GetBoundingRectangle(items);

            DesignerItem groupItem = new DesignerItem();
            groupItem.IsGroup = true;
            groupItem.Width = rect.Width;
            groupItem.Height = rect.Height;
            Canvas.SetLeft(groupItem, rect.Left);
            Canvas.SetTop(groupItem, rect.Top);
            Canvas groupCanvas = new Canvas();
            groupItem.Content = groupCanvas;
            Canvas.SetZIndex(groupItem, this.Children.Count);
            this.Children.Add(groupItem);

            foreach (DesignerItem item in items)
                item.ParentID = groupItem.ID;

            this.SelectionService.SelectItem(groupItem);
            logger.Info("Fin Agrupar Seleccion Diagrama");
        }

        private void DesagruparSeleccionDiagrama()
        {
            logger.Info("Inicio Desagrupar Seleccion Diagrama");
            var groups = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          where item.IsGroup && item.ParentID == Guid.Empty
                          select item).ToArray();

            foreach (DesignerItem groupRoot in groups)
            {
                var children = from child in SelectionService.CurrentSelection.OfType<DesignerItem>()
                               where child.ParentID == groupRoot.ID
                               select child;

                foreach (DesignerItem child in children)
                    child.ParentID = Guid.Empty;

                this.SelectionService.RemoveFromSelection(groupRoot);
                this.Children.Remove(groupRoot);
                UpdateZIndex();
            }
            logger.Info("Fin Desagrupar Seleccion Diagrama");

        }

        public string collectionJson()
        {
            var variables = new List<VariableAP>();

            foreach (VariableAP d in dataGridVariables.ItemsSource)
            {
                if (!string.IsNullOrWhiteSpace(d.nombre))
                {
                    variables.Add(d);
                }
            }

            foreach (VariableAP v in variables.FindAll(item => item.vector))
            {
                if (string.IsNullOrWhiteSpace(v.dimension))
                {
                    throw new DiagramValidationException(string.Format("El vector {0} no tiene dimensión asociada. Todos los vectores necesitan dimensión.", v.nombre));
                }

                VariableAP variableDimension = variables.Find(x => x.nombre == v.dimension);

                if (variableDimension.valor == 0)
                {
                    throw new DiagramValidationException(string.Format("Falta asignarle un valor a la variable {0}", variableDimension.nombre));
                }

                string[] words = v.nombre.Split('(');
                v.nombre = words.GetValue(0) + "(" + variableDimension.valor.ToString() + ")";
            }

            return JsonConvert.SerializeObject(variables);
        }

        #endregion
    }
}
