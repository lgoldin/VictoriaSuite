using DiagramDesigner;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using Victoria.Shared.AnalisisPrevio;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.Helpers
{
    public class HelperVIC : Canvas
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        public XDocument CreateVic(string simulationFile, List<Shared.Variable> variables, IList<StageViewModelBase> stagesList)
        {
            try
            {
                //logger.Info("Inicio Crear Vic");
                XDocument xmlSimulation = XDocument.Parse(simulationFile);
                // Conservo el modelo y borro todo el resto del vic original
                XElement modelo = xmlSimulation.Descendants("Modelo").First();
                xmlSimulation.Root.RemoveNodes();
                xmlSimulation.Root.Add(modelo);

                // Genero los nuevos elementos para el vic
                IList<DesignerItem> designerItems = deserializarDesignerItems(xmlSimulation);
                IList<Connection> connections = deserializarConnections(xmlSimulation, designerItems);
                List<VariableAP> variablesAP = transformVariables(variables);

                List<XElement> listaDesignerItemsXML = SerializeVic(designerItems, connections, variablesAP);

                // agrego los nuevos elementos al vic
                listaDesignerItemsXML.ForEach(xmlSimulation.Root.Add);

                // genero y agrego el nuevo stage
                XElement newStage = serializarVicNuevoStage(stagesList);
                xmlSimulation.Root.Add(newStage);

                //logger.Info("Fin Crear Vic");        
                return xmlSimulation;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private XElement serializarVicNuevoStage(IList<StageViewModelBase> stagesList)
        {
            //logger.Info("Inicio Vic Serializar Nuevo Escenario");
            var stages = (stagesList.Cast<StageViewModel>()).Select(st =>

                new XElement("Stage",

                    new XAttribute("Name", st.Name),

                    st.Variables.Select(v =>

                        new XElement("Variable",

                            new XAttribute("Name", v.Name),

                            new XAttribute("Value", v.InitialValue)

                            )

                        ),
                        st.Charts.Select(ch => new XElement("Chart",
                            new XAttribute("Name", ch.Name),

                            ch.DependentVariables.Select(v =>

                            new XElement("Variable",

                                new XAttribute("Name", v.Name)

                                ))
                            ))

                    )

                ).ToArray();
            //logger.Info("Fin Vic Serializar Nuevo Escenario");
            return new XElement("Stages", stages);
        }

        private IList<Connection> deserializarConnections(XDocument xmlSimulation, IList<DesignerItem> designerItems)
        {
            //logger.Info("Inicio Deserializar Conexiones");
            IList<Connection> connections = new ObservableCollection<Connection>();
            IEnumerable<XElement> connectionsXML = xmlSimulation.Descendants("Modelo").Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                ConnectorOrientation sourceOrientation = GetConnectorOrientation(connectionXML.Element("SourceOrientation").Value);
                ConnectorOrientation sinkOrientation = GetConnectorOrientation(connectionXML.Element("SinkOrientation").Value);

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName, designerItems, sourceOrientation);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName, designerItems, sinkOrientation);

                Connection connection = new Connection(sourceConnector, sinkConnector);
                connections.Add(connection);
            }
            //logger.Info("Fin Deserializar Conexiones");
            return connections;
        }

        private IList<DesignerItem> deserializarDesignerItems(XDocument xmlSimulation)
        {
            //logger.Info("Inicio Deserializar Diseñador Items");
            IList<DesignerItem> designerItems = new ObservableCollection<DesignerItem>();
            IEnumerable<XElement> itemsXML = xmlSimulation.Descendants("Modelo").Elements("Diagrama").Elements("Flowchart").Elements("DesignerItem");
            foreach (XElement itemXML in itemsXML)
            {
                Guid id = new Guid(itemXML.Element("ID").Value);
                string tag = itemXML.Element("Tag").Value;
                string uid = itemXML.Element("Uid").Value;
                DesignerItem item = DeserializarDesignerItem(itemXML, id, 0, 0, tag, uid);
                designerItems.Add(item);
            }
            //logger.Info("Fin Deserializar Diseñador Items");
            return designerItems;
        }

        public List<XElement> SerializeVic(IEnumerable<DesignerItem> designerItems, IEnumerable<Connection> connections, List<VariableAP> variables) //antes devolvia un XElement ahora devuelve una lista de XElements
        {
            //logger.Info("Inicio Serializar Vic");
            XElement serializedItems = null;
            List<XElement> serializedItemsAccum = new List<XElement>();
            List<Guid> listaAncestros = new List<Guid>();
            List<Guid> listaAuxiliar = new List<Guid>();
            var refrElements = new List<DesignerItem>();

            var tagInit = CreateInitializaionDiagram(variables);
            serializedItemsAccum.Add(tagInit);

            connections = connections.OrderBy(x => x.Source.Orientation); //Para invertir validaciones cambiar por orderByDescending

            foreach (DesignerItem item in designerItems)
            {
                if (item.Tag != null && "DIAG".Equals(item.Tag.ToString()))
                {
                    var nodoshijos = this.TraeNodosHijos(item.ID, connections, listaAncestros);  // RECURSIVIDAD
                    foreach (var nodo in nodoshijos)
                    {
                        listaAuxiliar.Clear();
                        if (nodo.Tag != null && "REFR".Equals(nodo.Tag.ToString()))
                        {
                            var nombre = ((Grid)nodo.Content).Children.OfType<TextBox>().FirstOrDefault().Text;
                            var itemsWithSameName = designerItems.Where(x => ((Grid)x.Content).Children.OfType<TextBox>().FirstOrDefault().Text == nombre);
                            if (itemsWithSameName.Count() == 2)
                            {
                                var itemHuerfano = itemsWithSameName.Where(x => x.ID != nodo.ID).FirstOrDefault();
                                itemHuerfano.Uid = nodo.ID.ToString();
                                itemHuerfano.Tag = item.ID.ToString();
                                refrElements.Add(itemHuerfano);
                            }
                        }
                    }
                    if (item.Tag != null && "DIAG".Equals(item.Tag.ToString()))
                    {
                        var contenido = (Grid)item.Content;
                        var listaChildren = contenido.Children;
                        var tipo = listaChildren.OfType<System.Windows.Shapes.Path>().FirstOrDefault().ToolTip.ToString();
                        var textBox = listaChildren.OfType<TextBox>().FirstOrDefault();
                        var attributeName = textBox.Text;
                        if (item.Uid != null && item.Uid.ToString() != "")
                        {
                            attributeName = item.Uid;
                        }

                        serializedItems = new XElement("Diagrama", new XAttribute("Name", attributeName),
                                          new XElement("flowchart",
                                                 new XElement("block",
                                                 new XAttribute("id", item.ID),
                                                 new XAttribute("caption", ((string)tipo == "nodo_inicializador") ? "Inicializar" : textBox.Text ?? ""),
                                                 new XAttribute("type", tipo),
                                                 new XAttribute("left", Canvas.GetLeft(item)),
                                                 new XAttribute("top", Canvas.GetTop(item)),
                                                 new XAttribute("width", item.Width),
                                                 new XAttribute("height", item.Height),
                                                 new XAttribute("zIndex", Canvas.GetZIndex(item)),
                                                     from connection in connections
                                                     where connection.Source.ParentDesignerItem.ID == item.ID
                                                     select new XElement("connection",
                                                     new XAttribute("ref", connection.Sink.ParentDesignerItem.ID))),
                                                                 from item2 in nodoshijos.Concat(refrElements)
                                                                 let contenido2 = (Grid)item2.Content
                                                                 let listaChildren2 = contenido2.Children
                                                                 let textBox2 = listaChildren2.OfType<TextBox>().FirstOrDefault()
                                                                 let tipo2 = (System.Windows.Shapes.Shape)(listaChildren2.OfType<System.Windows.Shapes.Path>().FirstOrDefault())
                                                                 let tipo3 = (System.Windows.Shapes.Shape)(listaChildren2.OfType<System.Windows.Shapes.Ellipse>().FirstOrDefault())  //Definir acá los tipos de shape que precisemos
                                                                 let tipo4 = (tipo2 == null ? tipo3 : tipo2).ToolTip
                                                                 where (!refrElements.Contains(item2) || item.ID.ToString() == item2.Tag.ToString())
                                                                 select new XElement("block",
                                                                        new XAttribute("id", refrElements.Contains(item2) ? "r" + item2.Uid : string.Empty + item2.ID),
                                                                        new XAttribute("caption", GetCaption((string)tipo4, textBox2.Text, variables)),
                                                                        new XAttribute("type", ((string)tipo4 ?? "")),
                                                                        new XAttribute("left", Canvas.GetLeft(item2)),
                                                                        new XAttribute("top", Canvas.GetTop(item2)),
                                                                        new XAttribute("width", item2.Width),
                                                                        new XAttribute("height", item2.Height),
                                                                        new XAttribute("zIndex", Canvas.GetZIndex(item2)),
                                                                             from connection in connections
                                                                             where (connection.Source.ParentDesignerItem.ID == item2.ID && ((string)tipo4 != "nodo_referencia" || refrElements.Contains(item2)))
                                                                             select new XElement("connection",
                                                                             new XAttribute("ref", connection.Sink.ParentDesignerItem.ID)
                                                                                               )

                                                              )))
                                                              ;
                    }
                    serializedItemsAccum.Add(serializedItems);
                }
            }
            serializedItemsAccum.Add(generarTagDeVariables(variables));
            //logger.Info("Fin Serializar Vic");
            return serializedItemsAccum;
        }

        public static DesignerItem DeserializarDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY, string tag, string uid)
        {
            //logger.Info("Inicio Deserializar Diseñador Item");
            DesignerItem item = new DesignerItem(id, tag, uid);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
            Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
            item.Content = content;
            //logger.Info("Fin Deserializar Diseñador Item");
            return item;
        }

        private ConnectorOrientation GetConnectorOrientation(string source)
        {
            ConnectorOrientation dest = ConnectorOrientation.None;
            if (ConnectorOrientation.Bottom.ToString().Equals(source)) {
                dest = ConnectorOrientation.Bottom;
            } else if (ConnectorOrientation.Left.ToString().Equals(source)) {
                dest = ConnectorOrientation.Left;
            } else if (ConnectorOrientation.Right.ToString().Equals(source)) {
                dest = ConnectorOrientation.Right;
            } else if (ConnectorOrientation.Top.ToString().Equals(source)) {
                dest = ConnectorOrientation.Top;
            } else {
                dest = ConnectorOrientation.None;
            }                
            return dest;
        }

        public Connector GetConnector(Guid itemID, String connectorName, IList<DesignerItem> designerItems, ConnectorOrientation orientation)
        {
            //logger.Info("Inicio Obtener Conector");
            DesignerItem designerItem = (from item in designerItems
                                         where item.ID == itemID
                                         select item).FirstOrDefault();
            Connector result = new Connector();
            result.Name = connectorName;
            result.ParentDesignerItem = designerItem;
            result.Orientation = orientation;
            //logger.Info("Fin Obtener Conector");
            return result;
        }

        private IEnumerable<DesignerItem> TraeNodosHijos(Guid itemId, IEnumerable<Connection> connections, List<Guid> listaAncestros)
        {
            try
            {
                //logger.Info("Inicio Trae Nodos Hijos");
                listaAncestros.Add(itemId);

                var nodosHijos = from connection in connections where (connection.Source.ParentDesignerItem.ID == itemId) select connection.Sink.ParentDesignerItem;
                foreach (DesignerItem nodoHijo in nodosHijos)
                {
                    if (!listaAncestros.Contains(nodoHijo.ID))
                    {
                        var nodosHijos2 = this.TraeNodosHijos(nodoHijo.ID, connections, listaAncestros);
                        nodosHijos = nodosHijos.Union(nodosHijos2.Where(x => (x.Tag == null || x.Tag.ToString() != "DIAG")));
                    }
                }
                //logger.Info("Fin Trae Nodos Hijos");
                return nodosHijos;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private List<VariableAP> transformVariables(List<Shared.Variable> variables)
        {
            try
            {
                //logger.Info("Inicio Transformar Variables");
                List<VariableAP> result = new List<VariableAP>();
                foreach (Shared.Variable variable in variables)
                {
                    result.Add(transformVariable(variable));
                }
                //logger.Info("Fin Transformar Variables");
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private VariableAP transformVariable(Shared.Variable variable)
        {
            try
            {
                //logger.Info("Inicio Transformar Variable");
                VariableAP dest = new VariableAP();
                dest.nombre = variable.Name;
                dest.valor = variable.InitialValue;
                dest.vector = false;
                dest.type = variable.Type;
                dest.dimension = null;
                dest.i = 0;
                if (variable is Victoria.Shared.VariableArray)
                {
                    Victoria.Shared.VariableArray variableArray = (Victoria.Shared.VariableArray)variable;
                    dest.vector = true;
                    dest.dimension = variableArray.Dimension;
                    dest.i = 1;
                }
                //logger.Info("Fin Transformar Variable");
                return dest;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public XElement CreateInitializaionDiagram(List<VariableAP> variables)
        {

            try
            {
                //logger.Info("Inicio Crear Inicializacion Diagrama");
                int i = 1;
                var vectores = (from variable in variables
                                where (variable.vector == true)
                                select variable);
                int cantVec = vectores.Count();
                //logger.Info("Fin Crear Inicializacion Diagrama");
                return new XElement("Diagrama",
                    new XAttribute("Name", "Inicializar"),
                        new XElement("flowchart",
                            new XElement("block",
                                new XAttribute("id", "Inicializar0"),
                                new XAttribute("caption", "Inicializar"),
                                new XAttribute("type", "nodo_titulo_inicializador"),
                                    new XElement("connection",
                                        new XAttribute("ref", "Inicializar1"))),
                    from variable in variables
                    select
                        new XElement("block",
                        new XAttribute("id", "Inicializar" + (i++).ToString()),
                        // TODO: cambiar nombre si es vector
                        //new XAttribute("caption", variable.nombre + "=" + variable.valor),
                        new XAttribute("caption", variable.GetNameForDesigner() + "=" + variable.valor),
                        new XAttribute("type", "nodo_sentencia"),
                        new XElement("connection",
                        new XAttribute("ref", "Inicializar" + (i).ToString()),
                        new XAttribute("left", (variable.vector ? SetItToVector(variable, (i++)) : (i).ToString())))),
                    from variable in variables
                    where variable.vector
                    select
                        new XElement("block",
                        new XAttribute("id", "Inicializar" + variable.i.ToString()),
                        new XAttribute("caption", GetCaption("nodo_iterador", "1;" + variable.dimension + ";1;I", variables)),
                        new XAttribute("type", "nodo_iterador"),
                        new XElement("connection",
                        new XAttribute("ref", "Inicializar" + ((variable.i) - 1).ToString())),
                        new XElement("connection",
                        new XAttribute("ref", "Inicializar" + (++variable.i).ToString()))),
                        new XElement("block",
                            new XAttribute("id", "Inicializar" + (variables.Count() + cantVec + 1).ToString()),
                            new XAttribute("caption", ""),
                            new XAttribute("type", "nodo_fin"))));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }

        }

        public string SetItToVector(VariableAP variable, double i)
        {
            variable.i = i;
            return string.Empty;
        }

        public string GetCaption(string tipo4, string text, List<VariableAP> variables)
        {   
            string caption = text ?? string.Empty;

            if (tipo4 == "nodo_iterador")
            {
                string[] iterator = text.Split(';');
                caption = string.Format("{0};{1};{2};{3}", iterator[0], variables.First(x => x.nombre == iterator[1]).valor, iterator[2], iterator[3]);
            }

            return caption;
        }

        public XElement generarTagDeVariables(List<VariableAP> variables)
        {
            //logger.Info("Inicio Generar Tag de Variables");
            string aux = JsonConvert.SerializeObject(variables);
            //logger.Info("Fin Generar Tag de Variables");
            return new XElement("variables",
               @" { ""variables"":" + aux + @"}");
        }

    }
}
