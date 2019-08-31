using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Xml.Linq;
using Victoria.DesktopApp.DiagramDesigner.Nodes;
using Victoria.Shared.AnalisisPrevio;

namespace Victoria.DesktopApp.DiagramDesigner
{
    public class AutomaticDiagramGenerator
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        private const string TOP = "Top";
        private const string BOTTOM = "Bottom";
        private const string LEFT = "Left";
        private const string RIGHT = "Right";
        
        private double topHeightStep = 100;
        private double leftWidthStep = 300;
        private double startTop = 14;
        private double startLeft = 120;

        private DesignerCanvas canvas;
        //private AnalisisPrevio analisisPrevio;
        public AnalisisPrevio analisisPrevio { get; set; }

        private double eventsCount;
        private double branchCount;
        private double ifCount;
        private int ifSteps;
        private int linesCount = 0;

        private Node lastCenterNode;
        private Node firstNode;
        private Dictionary<string, string> vectorEventsIndexes = new Dictionary<string, string>();
        private static AutomaticDiagramGenerator _instance = null;

        //Evito crear mas de una instancia de AutomaticDiagramGenerator
        public static AutomaticDiagramGenerator sharedInstance(){
     
            if ( _instance == null) {
                _instance = new AutomaticDiagramGenerator();
            }

            return _instance;
        }

        public AutomaticDiagramGenerator(){
            if (_instance != null) {
                throw new System.ArgumentException("Usar metodo sharedInstance para acceder a un bojeto AutomaticDiagramGenerator");
            }
        }

        public AutomaticDiagramGenerator(AnalisisPrevio analisisPrevio)
        {
            logger.Info("Inicio Generador Automatico de Diagrama");
            this.analisisPrevio = analisisPrevio;
            logger.Info("Fin Generador Automatico de Diagrama");

        }

        public void generateDiagram(Window1 diagramWindow)
        {

            logger.Info("Inicio Generar Diagrama");
            if (analisisPrevio.TipoDeEjercicio.Equals(AnalisisPrevio.Tipo.EaE))
            {
                generateEaEDiagram(diagramWindow);
            }
            else
            {
                generateDeltaTDiagram(diagramWindow);
            }
        
            logger.Info("Fin Generar Diagrama");

            setupAndShowDiagramWindow(diagramWindow, this.analisisPrevio);
        }

        private void generateDeltaTDiagram(Window1 diagramWindow)
        {
            logger.Info("Inicio Generar Diagrama Delta T");
            initialSetupForDeltaT(diagramWindow);
            generateInitNode();

            // SENTENCIA INICIAL
            Sentence deltaTSentence = new Sentence("T = T + DeltaT");
            setElementIntoCanvas(deltaTSentence, getCenterPositionForCentralBranch(), getHeightAndIncrement());
            connectNodes(lastCenterNode, BOTTOM, deltaTSentence, TOP);
            lastCenterNode = deltaTSentence;

            //EVENTOS TEF
            foreach(string tefEvent in this.analisisPrevio.Tefs)
            {
                Conditional conditionInIf = new Conditional("T == " + tefEvent);
                setElementIntoCanvas(conditionInIf, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, conditionInIf, TOP);
                lastCenterNode = conditionInIf;

                SubdiagramCall eventoComprometido = new SubdiagramCall("EventoComprometido" + tefEvent);
                setElementIntoCanvas(eventoComprometido, getRightPositionForBranch(0), getHeightAndIncrement());
                connectNodes(conditionInIf, RIGHT, eventoComprometido, TOP);

                ConditionalClose conditionalClose = new ConditionalClose();
                setElementIntoCanvas(conditionalClose, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(eventoComprometido, BOTTOM, conditionalClose, TOP);
                connectNodes(conditionInIf, LEFT, conditionalClose, TOP);
                lastCenterNode = conditionalClose;
            }

            //SUBRUTINAS DE EVENTOS PROPIOS
            foreach (string propio in this.analisisPrevio.Propios)
            {
                SubdiagramCall eventoPropio = new SubdiagramCall("EventoPropio" + propio);
                setElementIntoCanvas(eventoPropio, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, eventoPropio, TOP);
                lastCenterNode = eventoPropio;
            }

            //EVENTOS QUE COMPROMETEN DELTAS ANTERIORES
            foreach (string comprometidoAnterior in this.analisisPrevio.ComprometidosAnterior)
            {
                Conditional conditionInIf = new Conditional("");
                setElementIntoCanvas(conditionInIf, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, conditionInIf, TOP);
                lastCenterNode = conditionInIf;

                SubdiagramCall eventoComprometido = new SubdiagramCall("EventoQueCompromete" + comprometidoAnterior);
                setElementIntoCanvas(eventoComprometido, getRightPositionForBranch(0), getHeightAndIncrement());
                connectNodes(conditionInIf, RIGHT, eventoComprometido, TOP);

                ConditionalClose conditionalClose = new ConditionalClose();
                setElementIntoCanvas(conditionalClose, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(eventoComprometido, BOTTOM, conditionalClose, TOP);
                connectNodes(conditionInIf, LEFT, conditionalClose, TOP);
                lastCenterNode = conditionalClose;
            }

            //EVENTOS QUE COMPROMETEN DELTAS FUTUROS
            foreach (string comprometidoFuturo in this.analisisPrevio.ComprometidosFuturos)
            {
                Conditional conditionInIf = new Conditional("");
                setElementIntoCanvas(conditionInIf, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, conditionInIf, TOP);
                lastCenterNode = conditionInIf;

                SubdiagramCall eventoComprometido = new SubdiagramCall("EventoQueCompromete" + comprometidoFuturo);
                setElementIntoCanvas(eventoComprometido, getRightPositionForBranch(0), getHeightAndIncrement());
                connectNodes(conditionInIf, RIGHT, eventoComprometido, TOP);

                ConditionalClose conditionalClose = new ConditionalClose();
                setElementIntoCanvas(conditionalClose, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(eventoComprometido, BOTTOM, conditionalClose, TOP);
                connectNodes(conditionInIf, LEFT, conditionalClose, TOP);
                lastCenterNode = conditionalClose;
            }

            //ACTUALIZAR VARIABLE DE ESTADO
            foreach (VariableAP varEstado in this.analisisPrevio.VariablesEstado)
            {
                Sentence sentence = new Sentence(varEstado + " = " + varEstado + " + Entra" + varEstado + " - Sale" + varEstado);
                setElementIntoCanvas(sentence, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, sentence, TOP);
                lastCenterNode = sentence;
            }

            //CONTROL DE MÍNIMA
            foreach (VariableAP estado in this.analisisPrevio.VariablesEstado)
            {
                Conditional conditionInIf = new Conditional(estado + " < 0");
                setElementIntoCanvas(conditionInIf, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, conditionInIf, TOP);
                lastCenterNode = conditionInIf;

                Sentence sentence = new Sentence(estado + " = 0");
                setElementIntoCanvas(sentence, getRightPositionForBranch(0), getHeightAndIncrement());
                connectNodes(conditionInIf, RIGHT, sentence, TOP);

                ConditionalClose conditionalClose = new ConditionalClose();
                setElementIntoCanvas(conditionalClose, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(sentence, BOTTOM, conditionalClose, TOP);
                connectNodes(conditionInIf, LEFT, conditionalClose, TOP);
                lastCenterNode = conditionalClose;
            }

            //CONTROL DE MAXIMA
            foreach (VariableAP estado in this.analisisPrevio.VariablesEstado)
            {
                Conditional conditionInIf = new Conditional(estado + " > Max" + estado);
                setElementIntoCanvas(conditionInIf, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, conditionInIf, TOP);
                lastCenterNode = conditionInIf;

                Sentence sentence = new Sentence(estado + " = Max" + estado);
                setElementIntoCanvas(sentence, getRightPositionForBranch(0), getHeightAndIncrement());
                connectNodes(conditionInIf, RIGHT, sentence, TOP);

                ConditionalClose conditionalClose = new ConditionalClose();
                setElementIntoCanvas(conditionalClose, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(sentence, BOTTOM, conditionalClose, TOP);
                connectNodes(conditionInIf, LEFT, conditionalClose, TOP);
                lastCenterNode = conditionalClose;
            }
            
            //FIN DE DIAGRAMA
            Conditional simulationFinished = new Conditional("T > TF");
            setElementIntoCanvas(simulationFinished, getCenterPositionForCentralBranch(), getHeightAndIncrement());
            connectNodes(lastCenterNode, BOTTOM, simulationFinished, TOP);
            lastCenterNode = simulationFinished;

            //NODO REFERENCIAS A INICIO
            ReferenceNode referenceToStart = new ReferenceNode("Inicio");
            setElementIntoCanvas(referenceToStart, getCenterPositionForCentralBranch() - leftWidthStep / 3.1, getHeightAndIncrement() - topHeightStep - topHeightStep);
            connectNodes(lastCenterNode, LEFT, referenceToStart, TOP);
            
            ReferenceNode reference = new ReferenceNode("Inicio");
            setElementIntoCanvas(reference, getCenterPositionForCentralBranch() - 100, getHeight(1) - topHeightStep / 2);
            connectNodes(reference, RIGHT, deltaTSentence, TOP);

            SubdiagramCall calculateResults = new SubdiagramCall("CalcularResultados");
            setElementIntoCanvas(calculateResults, getCenterPositionForCentralBranch() + leftWidthStep / 3.1, getHeightAndIncrement());
            connectNodes(lastCenterNode, RIGHT, calculateResults, TOP);
            lastCenterNode = calculateResults;

            PrintResults printResults = new PrintResults(String.Join("; ", analisisPrevio.VariablesResultado.ToList()));
            setElementIntoCanvas(printResults, getCenterPositionForCentralBranch(), getHeightAndIncrement());
            connectNodes(lastCenterNode, BOTTOM, printResults, TOP);
            lastCenterNode = printResults;

            CloseDiagram closeDiagram = new CloseDiagram();
            setElementIntoCanvas(closeDiagram, getCenterPositionForCentralBranch(), getHeightAndIncrement());
            connectNodes(lastCenterNode, BOTTOM, closeDiagram, TOP);
            lastCenterNode = closeDiagram;

            //GENERO LAS SUBRUTINAS VACIAS DE EVENTOS
            double actualPosition = generateDeltaTEventoComprometidoSubdiagrams(getHeight());
            actualPosition = generateDeltaTPropiosSubdiagrams(actualPosition);
            actualPosition = generateDeltaTComprometidosAnteriorSubdiagrams(actualPosition);
            actualPosition = generateDeltaTComprometidosFuturoSubdiagrams(actualPosition);
            actualPosition = generateDeltaTCalcularResultadosSubdiagram(actualPosition);
            logger.Info("Fin Generar Diagrama Delta T");
        }

        private double generateDeltaTCalcularResultadosSubdiagram(double nextTopPosition)
        {
            logger.Info("Inicio Generar Delta T Calcular Resultados Subdiagrama");
            double actualPosition = nextTopPosition;
            actualPosition = nextTopPosition;

            SubdiagramInit init = new SubdiagramInit("CalcularResultados");
            setElementIntoCanvas(init, getLeftPositionForBranch(0), actualPosition);

            lastCenterNode = init;
            actualPosition += topHeightStep;

            CloseDiagram close = new CloseDiagram("Fin");
            setElementIntoCanvas(close, getLeftPositionForBranch(0), actualPosition);
            connectNodes(lastCenterNode, BOTTOM, close, TOP);
            lastCenterNode = close;
            actualPosition += topHeightStep;

            logger.Info("Fin Generar Delta T Calcular Resultados Subdiagrama");
            return actualPosition;
        }

        private double generateDeltaTPropiosSubdiagrams(double nextTopPosition)
        {

            logger.Info("Inicio Generar Delta T Propios Subdiagrama");
            double actualPosition = nextTopPosition;
            int i;
            for (i = 0; i < analisisPrevio.Propios.Count; i++)
            {
                actualPosition = nextTopPosition;

                SubdiagramInit init = new SubdiagramInit("EventoPropio" +  analisisPrevio.Propios.ElementAt(i));
                setElementIntoCanvas(init, getLeftPositionForBranch(i), actualPosition);

                lastCenterNode = init;
                actualPosition += topHeightStep;

                CloseDiagram close = new CloseDiagram("Fin");
                setElementIntoCanvas(close, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, close, TOP);
                lastCenterNode = close;
                actualPosition += topHeightStep;
            }


            logger.Info("Fin Generar Delta T Propios Subdiagrama");
            return actualPosition;
        }

        private double generateDeltaTEventoComprometidoSubdiagrams(double nextTopPosition)
        {
            logger.Info("Inicio Generar Delta T Evento Comprometido Subdiagramas");
            double actualPosition = nextTopPosition;
            int i;
            for (i = 0; i < analisisPrevio.Tefs.Count; i++)
            {
                actualPosition = nextTopPosition;

                SubdiagramInit init = new SubdiagramInit("EventoComprometido" + analisisPrevio.Tefs.ElementAt(i));
                setElementIntoCanvas(init, getLeftPositionForBranch(i), actualPosition);

                lastCenterNode = init;
                actualPosition += topHeightStep;

                CloseDiagram close = new CloseDiagram("Fin");
                setElementIntoCanvas(close, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, close, TOP);
                lastCenterNode = close;
                actualPosition += topHeightStep;
            }


            logger.Info("Fin Generar Delta T Evento Comprometido Subdiagramas");
            return actualPosition;
        }


        private double generateDeltaTComprometidosFuturoSubdiagrams(double nextTopPosition)
        {
            logger.Info("Inicio Generar Delta T Comprometidos Futuros Subdiagramas");
            double actualPosition = nextTopPosition;
            int i;
            for (i = 0; i < analisisPrevio.ComprometidosFuturos.Count; i++)
            {
                actualPosition = nextTopPosition;

                SubdiagramInit init = new SubdiagramInit("EventoQueCompromete" + analisisPrevio.ComprometidosFuturos.ElementAt(i));
                setElementIntoCanvas(init, getLeftPositionForBranch(i), actualPosition);

                lastCenterNode = init;
                actualPosition += topHeightStep;

                CloseDiagram close = new CloseDiagram("Fin");
                setElementIntoCanvas(close, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, close, TOP);
                lastCenterNode = close;
                actualPosition += topHeightStep;
            }

            logger.Info("Fin Generar Delta T Comprometidos Futuros Subdiagramas");
            return actualPosition;
        }

        private double generateDeltaTComprometidosAnteriorSubdiagrams(double nextTopPosition)
        {

            logger.Info("Inicio Generar Delta T Comprometidos Anterior Subdiagramas");
            double actualPosition = nextTopPosition;
            int i;
            for (i = 0; i < analisisPrevio.ComprometidosAnterior.Count; i++)
            {
                actualPosition = nextTopPosition;

                SubdiagramInit init = new SubdiagramInit("EventoQueCompromete" + analisisPrevio.ComprometidosAnterior.ElementAt(i));
                setElementIntoCanvas(init, getLeftPositionForBranch(i), actualPosition);

                lastCenterNode = init;
                actualPosition += topHeightStep;

                CloseDiagram close = new CloseDiagram("Fin");
                setElementIntoCanvas(close, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, close, TOP);
                lastCenterNode = close;
                actualPosition += topHeightStep;
            }

            logger.Info("Fin Generar Delta T Comprometidos Anterior Subdiagramas");
            return actualPosition;
        }


        private XElement loadXMLTemplate(TemplateManager templateManager, AnalisisPrevio analisisPrevio)
        {
            logger.Info("Inicio Carga Template XML");
            var parentFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var absolutePath = System.IO.Path.Combine(parentFolder, @"templates");
            var fileName = templateManager.obtenerTemplate(this.analisisPrevio);
            XElement xml = XElement.Load(System.IO.Path.Combine(absolutePath, fileName));
            logger.Info("Fin Carga Template XML");
            return xml;
        }

        private void generateEaEDiagram(Window1 diagramWindow)
        {
            logger.Info("Inicio Generar Diagrama EaE");
            initialSetupForEaE(diagramWindow);

            generateInitNode();
            var subDiagramCalls = generateVectorSubDiagramCalls();
            var createdBranchesInfo = generateEventBranches();
            double nextTopPosition = getLastTopPosition(createdBranchesInfo);

            ConditionalClose branchesConditionalClose = new ConditionalClose();
            setElementIntoCanvas(branchesConditionalClose, getCenterPositionForCentralBranch(), nextTopPosition);
            lastCenterNode = branchesConditionalClose;
            nextTopPosition += topHeightStep;

            foreach (var branch in createdBranchesInfo)
            {
                connectNodes(branch.Item3, BOTTOM, lastCenterNode, TOP);
            }

            Conditional simulationFinished = new Conditional("T > TF");
            setElementIntoCanvas(simulationFinished, getCenterPositionForCentralBranch(), nextTopPosition);
            connectNodes(lastCenterNode, BOTTOM, simulationFinished, TOP);
            lastCenterNode = simulationFinished;
            nextTopPosition += topHeightStep;

            ReferenceNode referenceToStart = new ReferenceNode("Inicio");
            setElementIntoCanvas(referenceToStart, getCenterPositionForCentralBranch() - leftWidthStep / 3.1, nextTopPosition - topHeightStep - topHeightStep);
            connectNodes(lastCenterNode, LEFT, referenceToStart, TOP);

            SubdiagramCall calculateResults = new SubdiagramCall("CalcularResultados");
            setElementIntoCanvas(calculateResults, getCenterPositionForCentralBranch() + leftWidthStep / 3.1, nextTopPosition);
            connectNodes(lastCenterNode, RIGHT, calculateResults, TOP);
            lastCenterNode = calculateResults;
            nextTopPosition += topHeightStep;

            PrintResults printResults = new PrintResults(String.Join("; ", analisisPrevio.VariablesResultado.ToList()));
            setElementIntoCanvas(printResults, getCenterPositionForCentralBranch(), nextTopPosition);
            connectNodes(lastCenterNode, BOTTOM, printResults, TOP);
            lastCenterNode = printResults;
            nextTopPosition += topHeightStep;

            CloseDiagram closeDiagram = new CloseDiagram();
            setElementIntoCanvas(closeDiagram, getCenterPositionForCentralBranch(), nextTopPosition);
            connectNodes(lastCenterNode, BOTTOM, closeDiagram, TOP);
            lastCenterNode = closeDiagram;
            nextTopPosition += topHeightStep;

            //NODO REFERENCIA A INICIO
            ReferenceNode reference = new ReferenceNode("Inicio");
            setElementIntoCanvas(reference, getCenterPositionForCentralBranch() - 100, getHeight(1) - topHeightStep / 2);
            connectNodes(reference, RIGHT, firstNode, TOP);

            //GENERACION DE RUTINAS DE EVENTOS
            if (analisisPrevio.EventosEaE.Count >= 4)
            {
                nextTopPosition = generateEventSubdiagrams(nextTopPosition);
            }

            //GENERACION DE RUTINAS DE LOS DATOS
            nextTopPosition = generateDataAndResultsSubDiagrams(nextTopPosition);

            //GENERACIÓN DE RUTINAS PARA VARIABLES TEF VECTORES
            if (analisisPrevio.EventosEaE.Any(evento => evento.Vector))
            {
                nextTopPosition = generateVectorSubDiagrams(nextTopPosition);
            }
            logger.Info("Fin Generar Diagrama EaE");
        }

        private double generateVectorSubDiagrams(double nextTopPosition)
        {
            logger.Info("Inicio Generar Vector Subdiagramas");
            double actualPosition = nextTopPosition;
            var vectorEvents = analisisPrevio.EventosEaE.Where(evento => evento.Vector);
            for (int i = 0; i < vectorEvents.Count(); i++)
            {
                string tefVariableName = vectorEvents.ElementAt(i).TEF;
                actualPosition = nextTopPosition;

                SubdiagramInit init = new SubdiagramInit("Menor" + tefVariableName);
                setElementIntoCanvas(init, getLeftPositionForBranch(i), actualPosition);

                lastCenterNode = init;
                actualPosition += topHeightStep;

                Sentence menSentence = new Sentence("MEN = HV");
                setElementIntoCanvas(menSentence, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, menSentence, TOP);
                lastCenterNode = menSentence;
                actualPosition += topHeightStep;

                Sentence iteratorSentence = new Sentence("H = 1");
                setElementIntoCanvas(iteratorSentence, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, iteratorSentence, TOP);
                lastCenterNode = iteratorSentence;
                actualPosition += topHeightStep;

                Conditional conditional = new Conditional(tefVariableName + "(H) < MEN");
                setElementIntoCanvas(conditional, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, conditional, TOP);
                lastCenterNode = conditional;
                actualPosition += topHeightStep;

                Sentence rightSide1 = new Sentence("MEN = " + tefVariableName + "(H)");
                setElementIntoCanvas(rightSide1, getLeftPositionForBranch(i) + leftWidthStep / 3.1, actualPosition);
                connectNodes(lastCenterNode, RIGHT, rightSide1, TOP);
                lastCenterNode = rightSide1;
                actualPosition += topHeightStep;

                Sentence rightSide2 = new Sentence(vectorEventsIndexes[tefVariableName] + " = H");
                setElementIntoCanvas(rightSide2, getLeftPositionForBranch(i) + leftWidthStep / 3.1, actualPosition);
                connectNodes(lastCenterNode, BOTTOM, rightSide2, TOP);
                lastCenterNode = rightSide2;
                actualPosition += topHeightStep;

                ConditionalClose condClose = new ConditionalClose();
                setElementIntoCanvas(condClose, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, condClose, TOP);
                connectNodes(conditional, LEFT, condClose, TOP);
                lastCenterNode = condClose;
                actualPosition += topHeightStep;

                Iterator iterator = new Iterator("1;" + getVariableDimention(tefVariableName) + ";1;H");
                setElementIntoCanvas(iterator, getLeftPositionForBranch(i) + leftWidthStep / 2, actualPosition);
                connectNodes(lastCenterNode, BOTTOM, iterator, TOP);
                connectNodes(iterator, RIGHT, conditional, TOP);
                lastCenterNode = iterator;
                actualPosition += topHeightStep;

                CloseDiagram close = new CloseDiagram("Fin");
                setElementIntoCanvas(close, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, close, TOP);
                lastCenterNode = close;
                actualPosition += topHeightStep;
            }
            logger.Info("Fin Generar Vector Subdiagramas");
            return actualPosition;
        }

        private string getVariableDimention(string tefVariableName)
        {
            return analisisPrevio.EventosEaE.First(evento => evento.TEF == tefVariableName).Dimension;
        }

        private double generateDataAndResultsSubDiagrams(double nextTopPosition)
        {
            logger.Info("Inicio Generar Datos y Resultados Subdiagramas");
            double actualPosition = nextTopPosition;
            int i;
            for (i = 0; i < analisisPrevio.Datos.Count; i++)
            {
                actualPosition = nextTopPosition;

                SubdiagramInit init = new SubdiagramInit(analisisPrevio.Datos.ElementAt(i));
                setElementIntoCanvas(init, getLeftPositionForBranch(i), actualPosition);

                lastCenterNode = init;
                actualPosition += topHeightStep;

                RandomNode random = new RandomNode("R");
                setElementIntoCanvas(random, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, random, TOP);
                lastCenterNode = random;
                actualPosition += topHeightStep;

                //Sentence sentence = new Sentence(analisisPrevio.Datos.ElementAt(i));
                //if (!analisisPrevio.Datos.ElementAt(i).Contains("="))
                //    sentence = new Sentence(analisisPrevio.Datos.ElementAt(i) + " = R");
                commonFDP.ResultadoAjuste associatedInverse = analisisPrevio.listFDP.FirstOrDefault(x => x.DatoAsociado == analisisPrevio.Datos.ElementAt(i));
                string inverse = associatedInverse == null ? "R" : associatedInverse.Inversa.Split('=')[1];
                Sentence sentence = new Sentence(analisisPrevio.Datos.ElementAt(i) + "= " +  inverse);
                //analisisPrevio.Datos[i] = analisisPrevio.Datos[i] + " = R";
                //Sentence sentence = new Sentence(analisisPrevio.Datos.ElementAt(i));
                setElementIntoCanvas(sentence, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, sentence, TOP);
                lastCenterNode = sentence;
                actualPosition += topHeightStep;

                CloseDiagram close = new CloseDiagram("Fin");
                setElementIntoCanvas(close, getLeftPositionForBranch(i), actualPosition);
                connectNodes(lastCenterNode, BOTTOM, close, TOP);
                lastCenterNode = close;
                actualPosition += topHeightStep;
            }
            
            double resultsActualPosition = nextTopPosition;
            
            SubdiagramInit resultsInit = new SubdiagramInit("CalcularResultados");
            setElementIntoCanvas(resultsInit, getLeftPositionForBranch(i), resultsActualPosition);

            lastCenterNode = resultsInit;
            resultsActualPosition += topHeightStep * 2;

            CloseDiagram resultsClose = new CloseDiagram("Fin");
            setElementIntoCanvas(resultsClose, getLeftPositionForBranch(i), resultsActualPosition);
            connectNodes(lastCenterNode, BOTTOM, resultsClose, TOP);
            lastCenterNode = resultsClose;

            logger.Info("Fin Generar Datos y Resultados Subdiagramas");
            return actualPosition;
        }

        private double generateEventSubdiagrams(double topPosition)
        {
            logger.Info("Inicio Generar Evento Subdiagrama");
            double nextTopPosition = topPosition;
            double maxTopPosition = 0;
            for (int i = 0; i < analisisPrevio.EventosEaE.Count; i++) 
            {
                double actualTopPosition = topPosition;

                SubdiagramInit subDiagramInit = new SubdiagramInit(analisisPrevio.EventosEaE.ElementAt(i).Nombre);
                setElementIntoCanvas(subDiagramInit, getLeftPositionForBranch(i), topPosition);

                lastCenterNode = subDiagramInit;
                actualTopPosition += topHeightStep;

                var branchInfo = generateBranch(analisisPrevio.EventosEaE.ElementAt(i), getLeftPositionForBranch(i), actualTopPosition, lastCenterNode, BOTTOM);

                maxTopPosition = (maxTopPosition < branchInfo.Item1) ? branchInfo.Item1 : maxTopPosition;
                actualTopPosition = branchInfo.Item1;

                CloseDiagram subDiagramExit = new CloseDiagram();
                setElementIntoCanvas(subDiagramExit, getLeftPositionForBranch(i), actualTopPosition);
                connectNodes(branchInfo.Item3, BOTTOM, subDiagramExit, TOP);
            }
            logger.Info("Fin Generar Evento Subdiagrama");
            return maxTopPosition + topHeightStep;
        }

        private double getLastTopPosition(List<Tuple<double, Node, Node>> createdBranchesInfo)
        {
            return createdBranchesInfo.Max(tupleElement => tupleElement.Item1);
        }

        private void initialSetupForEaE(Window1 diagramWindow)
        {
            logger.Info("Inicio Setup para EaE");
            this.canvas = diagramWindow.diagrama();
            this.canvas.Children.Clear();

            this.eventsCount = this.analisisPrevio.EventosEaE.Count;
            this.branchCount = Math.Pow(2, (this.eventsCount - 1));
            this.ifCount = branchCount - 1;
            this.ifSteps = this.analisisPrevio.EventosEaE.Count - 1;
            logger.Info("Fin Setup para EaE");
        }
        private void initialSetupForDeltaT(Window1 diagramWindow)
        {
            logger.Info("Inicio Setup para Delta T");
            this.canvas = diagramWindow.diagrama();
            this.canvas.Children.Clear();

            this.eventsCount = 2;
            this.branchCount = Math.Pow(2, (this.eventsCount - 1));
            this.ifCount = branchCount - 1;
            this.ifSteps = this.analisisPrevio.EventosEaE.Count - 1;
            logger.Info("Fin Setup para Delta T");

        }


        private List<Tuple<double, Node, Node>> generateEventBranches()
        {
            logger.Info("Inicio Generar Rama de Evento");
            var branchesInfo = new List<Tuple<double, Node, Node>>();

            int eventsCount = analisisPrevio.EventosEaE.Count();
            var tef = analisisPrevio.EventosEaE.Select(item => item).ToArray();
            if (eventsCount >= 4)
            {
                branchesInfo = generateBranchesWithIfAlgorithm();
            }
            
            if (eventsCount > 0 && eventsCount < 4)
            {
                branchesInfo = generateBranchesManually();
            }

            logger.Info("Fin Generar Rama de Evento");
            return branchesInfo;
        }

        private List<Tuple<double, Node, Node>> generateBranchesManually()
        {
            logger.Info("Inicio Generar Rama Manualmente");
            var tef = analisisPrevio.EventosEaE.Select(item => item).ToArray();
            string tefVar1 = getTEFVariableName(tef[0].TEF);
            var createdBranchesInfo = new List<Tuple<double,Node,Node>>();
            
            if (eventsCount == 1)
            {
                createdBranchesInfo.Add(generateBranch(tef[0], getCenterPositionForCentralBranch(), getHeight(), lastCenterNode, BOTTOM));
            }

            if (eventsCount >= 2)
            {
                string tefVar2 = getTEFVariableName(tef[1].TEF);
                
                Conditional condicional = new Nodes.Conditional(tefVar1 + " <= " + tefVar2);
                setElementIntoCanvas(condicional, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                connectNodes(lastCenterNode, BOTTOM, condicional, TOP);

                firstNode = (firstNode == null) ? condicional : firstNode;

                if (eventsCount == 2)
                {
                    createdBranchesInfo.Add(generateBranch(tef[1], getLeftPositionForBranch(0), getHeight(), condicional, LEFT));
                    createdBranchesInfo.Add(generateBranch(tef[0], getRightPositionForBranch(0), getHeight(), condicional, RIGHT));
                }
                
                if (eventsCount == 3)
                {
                    string tefVar3 = getTEFVariableName(tef[2].TEF);

                    Conditional subConditional = new Nodes.Conditional(tefVar3 + " <= " + tefVar2);
                    setElementIntoCanvas(subConditional,getCenterPositionForBranch(0), getHeight());
                    connectNodes(condicional, LEFT, subConditional, TOP);

                    Conditional subConditional2 = new Nodes.Conditional(tefVar1 + " <= " + tefVar3);
                    setElementIntoCanvas(subConditional2, getCenterPositionForBranch(1), getHeightAndIncrement());
                    connectNodes(condicional, RIGHT, subConditional2, TOP);

                    createdBranchesInfo.Add(generateBranch(tef[1], getLeftPositionForBranch(0), getHeight(), subConditional, LEFT));

                    var branchInfo = generateBranch(tef[2], getCenterPositionForCentralBranch(), getHeight(), subConditional, RIGHT);
                    createdBranchesInfo.Add(branchInfo);

                    createdBranchesInfo.Add(generateBranch(tef[0], getRightPositionForBranch(1), getHeightAndIncrement(), subConditional2, RIGHT));

                    connectNodes(subConditional2, LEFT, branchInfo.Item2, TOP);
                }
            }
            logger.Info("Fin Generar Rama Manualmente");
            return createdBranchesInfo;
        }

        private Tuple<double, Node, Node> generateBranch(EventoAP evento, double leftPosition, double topPosition, Node lastConnectedNode, string lastConnectedNodePosition)
        {
            logger.Info("Inicio Generar Rama");
            double actualTopPosition = topPosition;
            Node lastConnectedSubNode;

            //AVANCE DEL TIEMPO HASTA EL INSTANTE T 
            string tefVar = getTEFVariableName(evento.TEF);
            Sentence sentence = new Sentence("T = " + tefVar);

            setElementIntoCanvas(sentence, leftPosition, actualTopPosition);
            connectNodes(lastConnectedNode, lastConnectedNodePosition, sentence, TOP);
            actualTopPosition += topHeightStep;
            lastConnectedSubNode = sentence;

            firstNode = (firstNode == null) ? sentence : firstNode; 

            //EFNC
            foreach (string efnc in evento.EventosNoCondicionados)
            {
                EventoAP efncEvent = analisisPrevio.EventosEaE.First(item => item.Nombre == efnc);
                if (isDato(efncEvent.Encadenador))
                {
                    SubdiagramCall subDiagramCall = new SubdiagramCall(efncEvent.Encadenador);

                    setElementIntoCanvas(subDiagramCall, leftPosition, actualTopPosition);
                    connectNodes(lastConnectedSubNode, BOTTOM, subDiagramCall, TOP);
                    actualTopPosition += topHeightStep;
                    lastConnectedSubNode = subDiagramCall;
                }

                string newTefVariable = getTEFVariableName(efncEvent.TEF);
                Sentence newTefValue = new Sentence(newTefVariable + " = T + " + efncEvent.Encadenador);

                setElementIntoCanvas(newTefValue, leftPosition, actualTopPosition);
                connectNodes(lastConnectedSubNode, BOTTOM, newTefValue, TOP);
                actualTopPosition += topHeightStep;
                lastConnectedSubNode = newTefValue;
            }

            //ARREPENTIMIENTO
            if (evento.Arrepentimiento)
            {
                SubdiagramCall arrepentimiento = new SubdiagramCall("Arrepentimiento");

                setElementIntoCanvas(arrepentimiento, leftPosition, actualTopPosition);
                connectNodes(lastConnectedSubNode, BOTTOM, arrepentimiento, TOP);
                actualTopPosition += topHeightStep;
                lastConnectedSubNode = arrepentimiento;    
            }

            //ACTUALIZACIÓN DE VECTOR DE ESTADO
            Sentence stateVEctor = new Sentence("Actualiz. Vector Estado");

            setElementIntoCanvas(stateVEctor, leftPosition, actualTopPosition);
            connectNodes(lastConnectedSubNode, BOTTOM, stateVEctor, TOP);
            actualTopPosition += topHeightStep;
            lastConnectedSubNode = stateVEctor;

            //EFC
            if (evento.Condiciones.Count >= 1)
            {
                var branchInfo = Tuple.Create<double, Node>(actualTopPosition, lastConnectedSubNode);
                if (eventHasDistinctConditions(evento.Condiciones))
                {
                    
                    Node conditionFirstNode = lastConnectedSubNode;
                    foreach (string efc in evento.EventosCondicionados)
                    {
                       branchInfo = generateConditionBranchesWithDistinctConditions(evento, efc, leftPosition, actualTopPosition, conditionFirstNode);
                        actualTopPosition = branchInfo.Item1;
                        conditionFirstNode = branchInfo.Item2;
                    }
                }
                else 
                {
                    branchInfo = generateConditionBranches(evento, leftPosition, actualTopPosition, lastConnectedSubNode);
                }

                actualTopPosition = branchInfo.Item1 + topHeightStep;
                lastConnectedSubNode = branchInfo.Item2;
            }

            logger.Info("Fin Generar Rama");
            return Tuple.Create(actualTopPosition, (Node)sentence, (Node)lastConnectedSubNode);
        }

        private Tuple<double, Node> generateConditionBranchesWithDistinctConditions(EventoAP evento, string efc, double leftPosition, double topPosition, Node lastConnectedSubNode)
        {
            logger.Info("Inicio Generar Ramas con Distintas Condiciones");
            double actualTopPosition = topPosition;

            Conditional conditionInIf = new Conditional(evento.Condiciones.ElementAt(evento.EventosCondicionados.IndexOf(efc)));
            setElementIntoCanvas(conditionInIf, leftPosition, topPosition);
            connectNodes(lastConnectedSubNode, BOTTOM, conditionInIf, TOP);
            actualTopPosition += topHeightStep;
            lastConnectedSubNode = conditionInIf;
            
            EventoAP efcEvent = analisisPrevio.EventosEaE.First(item => item.Nombre == efc);
            if (isDato(efcEvent.Encadenador))
            {
                SubdiagramCall subDiagramCall = new SubdiagramCall(efcEvent.Encadenador);

                setElementIntoCanvas(subDiagramCall, leftPosition + leftWidthStep / 3.1, actualTopPosition);
                connectNodes(lastConnectedSubNode, RIGHT, subDiagramCall, TOP);
                actualTopPosition += topHeightStep;
                lastConnectedSubNode = subDiagramCall;
            }

            string newTefVariable = getTEFVariableName(efcEvent.TEF);
            Sentence newTefValue = new Sentence(newTefVariable + " = T + " + efcEvent.Encadenador);

            setElementIntoCanvas(newTefValue, leftPosition + leftWidthStep / 3.1, actualTopPosition);
            connectNodes(lastConnectedSubNode, BOTTOM, newTefValue, TOP);
            actualTopPosition += topHeightStep;
            lastConnectedSubNode = newTefValue;
            
            ConditionalClose closeCondition = new ConditionalClose();
            setElementIntoCanvas(closeCondition, leftPosition, actualTopPosition);
            connectNodes(conditionInIf, LEFT, closeCondition, TOP);
            connectNodes(lastConnectedSubNode, BOTTOM, closeCondition, TOP);
            actualTopPosition += topHeightStep;
            lastConnectedSubNode = closeCondition;


            logger.Info("Fin Generar Ramas con Distintas Condiciones");
            return Tuple.Create(actualTopPosition, lastConnectedSubNode);
        }


        private Tuple<double, Node> generateConditionBranches(EventoAP evento, double leftPosition, double topPosition, Node lastConnectedSubNode)
        {
            logger.Info("Inicio Generar Condicion de Ramas");
            double actualTopPosition = topPosition;
            Node ifBranchLastNode = lastConnectedSubNode;

            Conditional conditionInIf = new Conditional(evento.Condiciones.First());
            setElementIntoCanvas(conditionInIf, leftPosition, topPosition);
            connectNodes(ifBranchLastNode, BOTTOM, conditionInIf, TOP);
            actualTopPosition += topHeightStep;
            ifBranchLastNode = conditionInIf;

            foreach (string efc in evento.EventosCondicionados.Where(efc => !String.IsNullOrEmpty(efc)))
            {
                EventoAP efcEvent = analisisPrevio.EventosEaE.First(item => item.Nombre == efc);
                if (isDato(efcEvent.Encadenador))
                {
                    SubdiagramCall subDiagramCall = new SubdiagramCall(efcEvent.Encadenador);

                    setElementIntoCanvas(subDiagramCall, leftPosition + leftWidthStep / 3.1, actualTopPosition);
                    connectNodes(ifBranchLastNode, RIGHT, subDiagramCall, TOP);
                    actualTopPosition += topHeightStep;
                    ifBranchLastNode = subDiagramCall;
                }

                string newTefVariable = getTEFVariableName(efcEvent.TEF);
                Sentence newTefValue = new Sentence(newTefVariable + " = T + " + efcEvent.Encadenador);

                setElementIntoCanvas(newTefValue, leftPosition + leftWidthStep / 3.1, actualTopPosition);
                connectNodes(ifBranchLastNode, BOTTOM, newTefValue, TOP);
                actualTopPosition += topHeightStep;
                ifBranchLastNode = newTefValue;
            }

            ConditionalClose closeCondition = new ConditionalClose();
            setElementIntoCanvas(closeCondition, leftPosition, actualTopPosition);
            connectNodes(conditionInIf, LEFT, closeCondition, TOP);
            connectNodes(ifBranchLastNode, BOTTOM, closeCondition, TOP);
            actualTopPosition += topHeightStep;
            ifBranchLastNode = closeCondition;

            logger.Info("Fin Generar Condición de Ramas");
            return Tuple.Create(actualTopPosition, ifBranchLastNode);
        }

        private bool eventHasDistinctConditions(System.Collections.ObjectModel.ObservableCollection<string> conditions)
        {
            return conditions.Any(condition => !String.Equals(condition, (conditions.First())));
        }

        private bool isDato(string encadenador)
        {
            return analisisPrevio.Datos.Contains(encadenador);
        }

        private string getTEFVariableName(string tefVariableName)
        {
            string variableIndex;
            if (vectorEventsIndexes.TryGetValue(tefVariableName, out variableIndex))
            {
                return tefVariableName + "(" + variableIndex + ")";
            }
            return tefVariableName;
        }

        private void connectNodes(Node nodeFrom, string nodeFromConnectionPosition, Node nodeTo, string nodeToConnectionPosition)
        {
            Connection connection = new Connection((canvas.GetConnector(nodeFrom.guid, nodeFromConnectionPosition)), (canvas.GetConnector(nodeTo.guid, nodeToConnectionPosition)));
            canvas.Children.Add(connection);
        }

        private List<Tuple<double, Node, Node>> generateBranchesWithIfAlgorithm()
        {   
            var createdConditionalNodes = new List<Tuple<Node, string, string>>();
            for (int i = 0; i < ifSteps; i++)
            {
                double thisLineIfCount = Math.Pow(2, i);
                if (thisLineIfCount == 1)
                {
                    string leftOperand = getTEFVariableName(analisisPrevio.EventosEaE.ElementAt(0).TEF);
                    string rightOperand = getTEFVariableName(analisisPrevio.EventosEaE.ElementAt(1).TEF);
                    
                    Conditional firstCond = new Conditional(leftOperand + " <= " + rightOperand);
                    setElementIntoCanvas(firstCond, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                    connectNodes(lastCenterNode, BOTTOM, firstCond, TOP);

                    createdConditionalNodes.Add(Tuple.Create<Node, string, string>(firstCond, leftOperand, rightOperand));
                    lastCenterNode = firstCond;

                    firstNode = (firstNode == null) ? firstCond : firstNode;
                }
                else
                {
                    int posInCondNodes = Convert.ToInt32((thisLineIfCount - 1) - (thisLineIfCount / 2));
                    double actualHeight = getHeight();
                    double multiplicator = Math.Pow(2, ifSteps - i) - 1;
                    double ifDistance = (branchCount * (leftWidthStep / 2)) / (Math.Pow(2, i - 1));

                    for (int j = 0; j < thisLineIfCount; j++)
                    {
                        double startPosition = (leftWidthStep / 2) * multiplicator;

                        string leftOperand = (j % 2 == 0) ? createdConditionalNodes.ElementAt(posInCondNodes).Item3 : createdConditionalNodes.ElementAt(posInCondNodes).Item2;
                        string rightOperand = getTEFVariableName(analisisPrevio.EventosEaE.ElementAt(i + 1).TEF);

                        Conditional cond = new Conditional(leftOperand + " <= " + rightOperand);
                        setElementIntoCanvas(cond, startLeft + startPosition + (j * ifDistance), actualHeight);
                        connectNodes(createdConditionalNodes.ElementAt(posInCondNodes).Item1, getLeftOrRight(j), cond, TOP);

                        createdConditionalNodes.Add(Tuple.Create<Node, string, string>(cond, leftOperand, rightOperand));
                        posInCondNodes = (j % 2 == 0) ? posInCondNodes : posInCondNodes + 1;                         
                    }

                    getHeightAndIncrement();
                }
            }

            //generating last if rows and event branches
            var createdBranchesInfo = new List<Tuple<double, Node, Node>>();
            int pos = Convert.ToInt32((branchCount - 1) - (branchCount / 2));
            for (int i = 0; i < branchCount / 2; i++)
            {
                SubdiagramCall leftEvent = new SubdiagramCall(getTEFEventName(createdConditionalNodes.ElementAt(pos).Item3));
                SubdiagramCall rightEvent = new SubdiagramCall(getTEFEventName(createdConditionalNodes.ElementAt(pos).Item2));
                setElementIntoCanvas(leftEvent, getLeftPositionForBranch(i), getHeight());
                setElementIntoCanvas(rightEvent, getRightPositionForBranch(i), getHeight());
                connectNodes(createdConditionalNodes.ElementAt(pos).Item1, LEFT, leftEvent, TOP);
                connectNodes(createdConditionalNodes.ElementAt(pos).Item1, RIGHT, rightEvent, TOP);

                ConditionalClose conditionalClose = new ConditionalClose();
                setElementIntoCanvas(conditionalClose, getCenterPositionForBranch(i), getHeight() + topHeightStep);
                connectNodes(leftEvent, BOTTOM, conditionalClose, TOP);
                connectNodes(rightEvent, BOTTOM, conditionalClose, TOP);

                createdBranchesInfo.Add(Tuple.Create<Double, Node, Node>(getHeight() + topHeightStep + topHeightStep , leftEvent, conditionalClose));

                pos++;
            }

            return createdBranchesInfo;
        }

        private string getTEFEventName(string tefVariable)
        {
            int indexOfVectorSymbol = tefVariable.IndexOf('(');
            string tefRealName = (indexOfVectorSymbol != -1)? tefVariable.Remove(indexOfVectorSymbol) : tefVariable;

            return analisisPrevio.EventosEaE.First(element => String.Equals(element.TEF, tefRealName)).Nombre;
        }

        private string getLeftOrRight(int i)
        {
            if(i % 2 == 0)
            {
                return LEFT;
            }

            return RIGHT;
        }

        private List<SubdiagramCall> generateVectorSubDiagramCalls()
        {
            logger.Info("Inicio Generar Vector llamadas Subdiagrama");
            var tefReferenceNodes = new List<SubdiagramCall>();
            var vectorEvents = analisisPrevio.EventosEaE.Where(item => item.Vector == true).ToArray();
            if(vectorEvents.Length > 0)
            {
                
                char indexCharacter = 'I';
                foreach (EventoAP evento in vectorEvents) 
                {
                    var referenceNode = new SubdiagramCall("Menor" + evento.TEF);
                    
                    setElementIntoCanvas(referenceNode, getCenterPositionForCentralBranch(), getHeightAndIncrement());
                    connectNodes(lastCenterNode, BOTTOM, referenceNode, TOP);
                  
                    lastCenterNode = referenceNode;
                    vectorEventsIndexes.Add(evento.TEF, indexCharacter.ToString());
                    indexCharacter++;

                    tefReferenceNodes.Add(referenceNode);
                    firstNode = (firstNode == null) ? referenceNode : firstNode; 
                }
            }
            logger.Info("Fin Generar Vector llamadas Subdiagrama");

            return tefReferenceNodes;
        }

        private InitPrincipal generateInitNode()
        {
            logger.Info("Inicio Generar Nodo Inicial");
            InitPrincipal init = new InitPrincipal("Diagrama Principal");
            setElementIntoCanvas(init, getCenterPositionForCentralBranch(), getHeightAndIncrement());

            this.lastCenterNode = init;
            logger.Info("Fin Generar Nodo Inicial");
            return init;
        }

        private void setElementIntoCanvas(Node node, double left, double top)
        {
            Canvas.SetLeft(node.designerItem, left + node.getLeftReference());
            Canvas.SetTop(node.designerItem, top);
            canvas.SetConnectorDecoratorTemplate(node.designerItem);
            canvas.Children.Add(node.designerItem);
        }


        private void setupAndShowDiagramWindow(Window1 VentanaDiagramador, AnalisisPrevio AnalisisPrevio)
        {
            logger.Info("Inicio Configurar y Mostrar Diagrama");
            this.canvas.InvalidateVisual();

            VentanaDiagramador.Height = 650;
            List<VariableAP> vars = AnalisisPrevio.ObtenerVariablesAP();
            addFinalVars(vars);
            VentanaDiagramador.dataGridVariables.ItemsSource = vars;
            VentanaDiagramador.dimensiones.ItemsSource = vars.Where(x => x.type == VariableType.Control);
            VentanaDiagramador.Show();
            logger.Info("Fin Configurar y Mostrar Diagrama");
        }

        private void addFinalVars(List<VariableAP> vars)
        {
            logger.Info("Inicio Agregar Variables Final");
            vars.Add(new VariableAP() { nombre = "TF", valor = 50000.0, vector = false, i = 0, type = VariableType.Other });
            vars.Add(new VariableAP() { nombre = "HV", valor = 99999999.0, vector = false, i = 0, type = VariableType.Other });
            if (analisisPrevio.EventosEaE.Any(evento => evento.Vector))
            {
                vars.Add(new VariableAP() { nombre = "H", valor = 1, vector = false, i = 0, type = VariableType.Other });
                vars.Add(new VariableAP() { nombre = "MEN", valor = 0, vector = false, i = 0, type = VariableType.Other });
                foreach (string index in vectorEventsIndexes.Values)
                {
                    vars.Add(new VariableAP() { nombre = index, valor = 1, vector = false, i = 0, type = VariableType.Other });
                }
            }
            if (analisisPrevio.TipoDeEjercicio.Equals(AnalisisPrevio.Tipo.DeltaT))
            {
                foreach (string tef in analisisPrevio.Tefs)
                {
                    vars.Add(new VariableAP() { nombre = tef, valor = 0, vector = false, i = 0, type = VariableType.Other });
                }

                vars.Add(new VariableAP() { nombre = "DeltaT", valor = 1, vector = false, i = 0, type = VariableType.Other });
                
                foreach (VariableAP estado in analisisPrevio.VariablesEstado)
                {
                    vars.Add(new VariableAP() { nombre = "Entra" + estado.nombre , valor = 0, vector = estado.vector, i = 0, type = VariableType.Other });
                    vars.Add(new VariableAP() { nombre = "Sale" + estado.nombre, valor = 0, vector = estado.vector, i = 0, type = VariableType.Other });
                    vars.Add(new VariableAP() { nombre = "Max" + estado.nombre, valor = 0, vector = estado.vector, i = 0, type = VariableType.Other });
                }
            }
            logger.Info("Fin Agregar Variables Final");
        }

        private double getHeightAndIncrement()
        {
            return startTop + topHeightStep * linesCount++;
        }
        private double getHeight(int rowNumber)
        {
            return startTop + topHeightStep * (rowNumber);
        }

        private double getHeight()
        {
            return startTop + topHeightStep * linesCount;
        }

        private double getNextHeight()
        {
            return getHeight() + topHeightStep;
        }

        private double getCenterPositionForCentralBranch()
        {
            return startLeft + leftWidthStep * (branchCount - 1) / 2;
        }

        private double getCenterPositionForBranch(int columnNumber)
        {
            return (startLeft + leftWidthStep / 2) + (leftWidthStep * 2 * columnNumber);
        }

        private double getLeftPositionForBranch(int columnNumber)
        {
            return startLeft + (leftWidthStep * 1 * columnNumber);
        }

        private double getRightPositionForBranch(int columnNumber)
        {
            return (startLeft + leftWidthStep) + (leftWidthStep * 2 * columnNumber);
        }

    }
}
