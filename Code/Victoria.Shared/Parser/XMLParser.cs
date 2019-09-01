using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Victoria.Shared.Parser;
using Victoria.Shared.AnalisisPrevio;



namespace Victoria.Shared
{
    public static class XMLParser
    {
        
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        public static Simulation GetSimulation(string xmlString)
        {
            try
            {

                //logger.Info("Inicio Obtener Simulacion");
                var doc = XElement.Parse(xmlString);

                Dictionary<string,bool> nodosBreakPoint = NodosBreakPoint(doc.Descendants("DesignerItem"));                

                var diagramasPreProcesados = doc.Descendants("Diagrama").Where(diag => diag.Attribute("Name").Value != "ModeloAnalisisSensibilidad").Select(diag => new PreParsedDiagram
                {
                    name = diag.Attribute("Name").Value,
                    diagram = new Diagram
                    {
                        Name = diag.Attribute("Name").Value
                    },
                    nodos = parseNodes(diag.Descendants("flowchart").Descendants("block"), nodosBreakPoint)
                });

                var variables = parseVariables(doc.Descendants("variables").First());

                List<Diagram> diagramas = posprocesarDiagramas(diagramasPreProcesados);

                foreach (var diagram in diagramas)
                {
                    foreach (var node in diagram.Nodes)
                    {
                        if (node is NodeDiagram)
                        {
                            ((NodeDiagram)node).Diagram = diagramas.First(X => X.Name == ((NodeDiagram)node).DiagramName);
                        }
                    }
                }

                foreach (var variable in variables)
                {
                    variable.Value.InitialValue = variable.Value.ActualValue;
                    variable.Value.ActualValue = 0;
                }

                List<Stage> stages;
                if (doc.Descendants("Stages").Any())
                {
                    stages = parseStages(doc.Descendants("Stages").First());
                }
                else
                {
                    stages = new List<Stage>();
                }

                //logger.Info("Fin Obtener Simulacion");
                return new Simulation(diagramas, variables, stages);
            }
            catch (Exception e)
            {
                logger.Error("Error al parsear la simulacion: "+e.Message);
                throw new ParsingException("Error al parsear la simulacion", e);
            }

        }

        static Dictionary<string,bool> NodosBreakPoint(IEnumerable<XElement> designerItems)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();

            foreach (XElement e in designerItems)
            {
                var nodo_id = e.Elements().Where(n => n.Name.LocalName.Equals("ID")).Select(n => n.Value).First();
                var nodo_has_bp = e.Elements().Where(n => n.Name.LocalName.Equals("Content")).Select(n => n.Value.Contains("BreakPoint")).First();

                dict.Add(nodo_id, nodo_has_bp);
            }

            return dict;
        }

        static Dictionary<string, PreParsedNode> parseNodes(IEnumerable<XElement> XmlNodes,Dictionary<string,bool> nodosBreakPoint)
        {
            return XmlNodes.Select(node => parseBlock(node, nodosBreakPoint)).ToDictionary(node => node.name);
        }

        static PreParsedNode parseBlock(XElement node, Dictionary<string, bool> nodosBreakPoint)
        {
            bool hasBreakPoint;

            nodosBreakPoint.TryGetValue(node.Attribute("id").Value.ToString(),out hasBreakPoint);

            switch (node.Attribute("type").Value)
            {
                case "nodo_titulo_inicializador":
                    return parseNodoInicializador(node);
                case "nodo_sentencia":
                    return parseNodoSentencia(node,hasBreakPoint);
                case "nodo_iterador":
                    return parseNodoIterador(node);
                case "nodo_fin":
                    return parseNodoFin(node);
                case "nodo_condicion":
                    return parseNodoCondicion(node,hasBreakPoint);
                case "nodo_inicializador":
                    return parseNodoDiagrama(node, true, false);
                case "nodo_diagrama":
                    return parseNodoDiagrama(node, false, hasBreakPoint);
                case "nodo_condicion_cierre":
                    return parseNodoCondicionCierre(node);
                case "nodo_referencia":
                    return parseNodoReferencia(node);
                case "nodo_resultado":
                    return parseNodoResultado(node);
                case "nodo_titulo_diagrama":
                    return parseNodoTituloDiagrama(node);
                case "nodo_random":
                    return parseNodoRandom(node);
                default:
                    throw new XMLFormatError("tipo de nodo desconocido");
            }
        }

        static Dictionary<string, Variable> parseVariables(XElement node)
        {
            //logger.Info("Inicio parse Variables");
            JObject vars = JObject.Parse(node.Value);
            var result = new Dictionary<string, Variable>();
            foreach (var variable in vars["variables"])
            {
                if (!result.ContainsKey((string)variable["nombre"]))
                {
                    var nombre = (string)variable["nombre"];
                    if ((bool)variable["vector"])
                    {
                        var indexFirstParenthesis = nombre.LastIndexOf('(');
                        var indexLastParenthesis = nombre.LastIndexOf(')');
                        var nombreItem = nombre.Remove(indexFirstParenthesis, nombre.Length - indexFirstParenthesis);
                        var lenght = Convert.ToInt32(nombre.Substring(indexFirstParenthesis + 1, indexLastParenthesis - indexFirstParenthesis - 1));

                        var variableList = new List<Variable>();
                        for (int i = 1; i <= lenght; i++)
                        {
                            var variableItem = new Variable
                            {
                                Name = new StringBuilder(nombreItem).Append('(').Append(i).Append(')').ToString(),
                                InitialValue = (double)variable["valor"],
                                ActualValue = (double)variable["valor"],
                                Type = (VariableType)(int)variable["type"]
                            };

                            variableList.Add(variableItem);
                        }
                        //es vector
                        result.Add(nombre, new VariableArray()
                        {
                            Name = nombre,
                            Variables = variableList,
                            Dimension = (string)variable["dimension"],
                            Type = (VariableType)(int)variable["type"]
                        });

                    }
                    else
                    {
                        result.Add(nombre, new Variable
                        {
                            Name = nombre,
                            InitialValue = (double)variable["valor"],
                            ActualValue = (double)variable["valor"],
                            Type = (VariableType)(int)variable["type"]
                        });
                    }
                }
            }

            //logger.Info("Fin parse Variables");
            return result;
        }

        static IList<string> getNextNodes(XElement node)
        {
            return node.Descendants("connection").Select(con => con.Attribute("ref").Value).ToList();
        }

        private static PreParsedNode parseNodoIterador(XElement node)
        {

            //logger.Info("Inicio parse nodo Iterador");
            var ns = new NodeIterator();
            var code = node.Attribute("caption").Value.Trim();
            code = code.Replace(" ", string.Empty);
            ns.Name = node.Attribute("id").Value;
            var parametros = code.Split(';');
            ns.ValorInicial = Convert.ToInt32(parametros[0]);
            ns.ValorFinal = Convert.ToInt32(parametros[1]);
            ns.Incremento = Convert.ToInt32(parametros[2]);
            ns.VariableName = (parametros.Count() > 3) ? parametros[3] : string.Empty;

            //logger.Info("Fin parse nodo Iterador");
            return new PreParsedNodeIterator()
            {
                name = ns.Name,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoSentencia(XElement node, bool hasBreakPoint)
        {
            //logger.Info("Inicio Parse Nodo Sentencia");
            var ns = new NodeSentence();
            ns.Code = node.Attribute("caption").Value;
            ns.Name = node.Attribute("id").Value;
            ns.HasBreakPoint = hasBreakPoint;
            //logger.Info("Fin Parse Nodo Sentencia");

            return new PreParsedNode
            {
                name = ns.Name,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoInicializador(XElement node)
        {
            //logger.Info("Inicio Parse Nodo Inicializador");
            var ns = new Node
            {
                Name = node.Attribute("id").Value,
            };

            //logger.Info("Fin Parse Nodo Inicializador");
            return new PreParsedNode
            {
                name = node.Attribute("id").Value,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoFin(XElement node)
        {
            //logger.Info("Inicio parse Nodo Fin");
            var ns = new Node
            {
                Name = node.Attribute("id").Value,
            };
            //logger.Info("Fin parse Nodo Fin");
            return new PreParsedNode
            {
                name = node.Attribute("id").Value,
                node = ns,
                next = null
            };
        }

        static PreParsedNode parseNodoDiagrama(XElement node, bool isInitializer, bool hasBreakPoint)
        {
            //logger.Info("Inicio Parse Nodo Diagrama");
            var ns = new NodeDiagram
            {
                Name = node.Attribute("id").Value,
                DiagramName = node.Attribute("caption").Value,
                IsInitializer = isInitializer,
                HasBreakPoint = hasBreakPoint

            };
            //logger.Info("Fin Parse Nodo Diagrama");
            return new PreparsedNodeDiagram
            {
                name = node.Attribute("id").Value,
                node = ns,
                next = getNextNodes(node)

            };
        }

        static PreParsedNode parseNodoCondicion(XElement node, bool hasBreakPoint)
        {
            //logger.Info("Inicio Parse Nodo Condicion");
            var ns = new NodeCondition
            {
                Name = node.Attribute("id").Value,
                Code = node.Attribute("caption").Value,
                HasBreakPoint = hasBreakPoint
            };
            //logger.Info("Fin Parse Nodo Condicion");
            return new PreParsedNodeCondition
            {
                name = ns.Name,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoCondicionCierre(XElement node)
        {
            //logger.Info("Inicio Parse Nodo Condicion Cierre");
            var ns = new Node
            {
                Name = node.Attribute("id").Value,
            };
            //logger.Info("Fin Parse Nodo Condicion Cierre");
            return new PreparsedNodeEndCondition
            {
                name = node.Attribute("id").Value,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoReferencia(XElement node)
        {
            //logger.Info("Inicio Parse Nodo Referencia");
            var ns = new NodeReferencia();
            ns.Code = node.Attribute("caption").Value;
            ns.Name = node.Attribute("id").Value;

            //logger.Info("Inicio Parse Nodo Referencia");
            return new PreParsedNodeReferencia
            {
                name = node.Attribute("id").Value,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoResultado(XElement node)
        {
            //logger.Info("Inicio Parse Nodo Resultado");
            var ns = new NodeResult
            {
                Name = node.Attribute("id").Value,
                Variables = node.Attribute("caption").Value.Split(':')
            };

            //logger.Info("Fin Parse Nodo Referencia");
            return new PreParsedNode
            {
                name = ns.Name,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoTituloDiagrama(XElement node)
        {
            //logger.Info("Inicio Parse Nodo Titulo Diagrama");
            var ns = new Node
            {
                Name = node.Attribute("id").Value,
            };

            //logger.Info("Fin Parse Nodo Titulo Diagrama");
            return new PreParsedNode
            {
                name = node.Attribute("id").Value,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static PreParsedNode parseNodoRandom(XElement node)
        {
            //logger.Info("Inicio Parse Nodo Random");
            var ns = new NodeRandom
            {
                Name = node.Attribute("id").Value,
                Code = node.Attribute("caption").Value
            };
            //logger.Info("Fin Parse Nodo Random");
            return new PreParsedNode
            {
                name = ns.Name,
                node = ns,
                next = getNextNodes(node)
            };
        }

        static List<Diagram> posprocesarDiagramas(IEnumerable<PreParsedDiagram> diagramasPreProcesados)
        {
            //logger.Info("Inicio posprocesar Diagramas");
            var result = new List<Diagram>();
            foreach (PreParsedDiagram preD in diagramasPreProcesados)
            {
                preD.diagram.Nodes = procesarNodos(preD.nodos, diagramasPreProcesados);
                result.Add(preD.diagram);
            }

            //logger.Info("Fin posprocesar Diagramas");
            return result;
        }

        static ObservableCollection<Node> procesarNodos(Dictionary<string, PreParsedNode> nodos, IEnumerable<PreParsedDiagram> diagramasPreProcesados)
        {
            //logger.Info("Inicio procesar Nodos");
            var result = new List<Node>();
            foreach (var nodoPreProcesado in nodos.Values)
            {
                nodoPreProcesado.posprocesar(nodos, result);
            }
            //logger.Info("Fin procesar Nodos");
            return new ObservableCollection<Node>(result);
        }

        static List<Stage> parseStages(XElement stages)
        {   
            return stages.Descendants("Stage").Select(st => new Stage
            {
                Name = st.Attribute("Name").Value,
                Variables = st.Elements("Variable").Select(v => new Variable
                {
                    Name = v.Attribute("Name").Value,
                    InitialValue = Convert.ToDouble(v.Attribute("Value").Value)
                }).ToList(),
                Charts = st.Descendants("Chart").Select(c => new Chart
                {
                    Name = c.Attribute("Name").Value,
                    DependentVariables = c.Descendants("Variable").Select(v => v.Attribute("Name").Value).ToList()
                }).ToList()
            }).ToList();
        }
    }
}
