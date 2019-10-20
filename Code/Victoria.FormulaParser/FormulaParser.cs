using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Victoria.FormulaParser
{
    public class FormulaParser
    {
        public enum Estado
        {
            EsperandoTerminoIzquierdo,
            EsperandoOperador,
            EsperandoTerminoDerecho,
        };

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        private readonly string formula;

        private readonly Expresion expresionRaiz;

        private int indice;
        private int indiceAnterior;

        public FormulaParser(string formula)
        {
            this.formula = Regex.Replace(formula, @"\s+", "").ToLower();

            this.indice = 0;
            this.indiceAnterior = 0;

            this.expresionRaiz = this.ConstruirExpresion();


            //logger.Info("Fin Formula Parser");
        }


        public override string ToString()
        {
            return this.expresionRaiz.ToString();
        }

        public double GetValor()
        {
            return this.expresionRaiz.GetValor();
        }

        public bool GetValorAsBool()
        {
            return this.expresionRaiz.GetValor() != 0;
        }

        public Elemento ProximoElemento()
        {

            //logger.Info("Inicio Proximo Elemento");
            Elemento elemento = null;

            string token = string.Empty;
            char caracter;

            string numeros = "0123456789.";

            string operadoresNumericos = "+-*/^%";
            string operadoresLogicosDeDosCaracteres = "|&=!";
            string operadoresLogicosDeUnoODosCaracteres = "<>";
            string operadoresLogicos = 
                operadoresLogicosDeDosCaracteres +
                operadoresLogicosDeUnoODosCaracteres;
            string operadores = 
                operadoresNumericos + 
                operadoresLogicos;

            string agrupadores = "()";
            string caracteres = "abcdefghijklmnopqrstuvwxyz";
            string separadores = ",";

            if (this.indice == this.formula.Length)
            {
                return elemento;
            }

            caracter = this.formula[this.indice];

            token += caracter;

            this.indiceAnterior = this.indice;

            this.indice++;

            if (numeros.Contains(caracter))
            {
                if (this.indice < this.formula.Length)
                {
                    caracter = this.formula[indice];
                    while (numeros.Contains(caracter) && this.indice < this.formula.Length)
                    {
                        token += caracter;
                        this.indice++;
                        if (this.indice < this.formula.Length)
                        {
                            caracter = this.formula[indice];
                        }
                    }
                }

                elemento = new ElementoNumerico(token);
            }
            else if (caracteres.Contains(caracter))
            {
                if (this.indice < this.formula.Length)
                {
                    caracter = this.formula[indice];
                    while (caracteres.Contains(caracter) && this.indice < this.formula.Length)
                    {
                        token += caracter;
                        this.indice++;
                        if (this.indice < this.formula.Length)
                        {
                            caracter = this.formula[indice];
                        }
                    }
                }

                elemento = ElementoFuncion.GetFuncion(token);
            }
            else if (operadoresNumericos.Contains(caracter))
            {
                elemento = ElementoOperador.GetOperador(token);
            }
            else if (operadoresLogicos.Contains(caracter))
            {
                if (this.indice < this.formula.Length)
                {
                    if (operadoresLogicosDeDosCaracteres.Contains(caracter))
                    {
                        caracter = this.formula[indice];
                        token += caracter;
                        this.indice++;
                    }
                    else if (operadoresLogicosDeUnoODosCaracteres.Contains(caracter))
                    {
                        caracter = this.formula[indice];
                        if (caracter == '=')
                        {
                            token += caracter;
                            this.indice++;
                        }
                    }
                }

                elemento = ElementoOperador.GetOperador(token);
            }
            else if (agrupadores.Contains(caracter))
            {
                elemento = new ElementoAgrupador(token);
            }
            else if (separadores.Contains(caracter))
            {
                elemento = new ElementoSeparador(token);
            }
            else
            {   
                //logger.Error("Caracter no esperado: '" + caracter + "'");
                throw new InvalidOperationException("Caracter no esperado: '" + caracter + "'");
            }



            //logger.Info("Fin Proximo Elemento");
            return elemento;
        }

        public Elemento VerProximoElemento()
        {

            //logger.Info("Inicio Ver Proximo Elemento");
            int indiceOriginal = this.indice;

            Elemento elemento = this.ProximoElemento();

            this.indice = indiceOriginal;
            
            //logger.Info("Fin Ver Proximo Elemento");
            return elemento;
        }

        public void RetrocedecerElemento()
        {

            //logger.Info("Inicio Retroceder Elemento");
            this.indice = this.indiceAnterior;
            //logger.Info("Fin Retroceder Elemento");
        }

        private Expresion ConstruirExpresion()
        {

            //logger.Info("Construir Expresion");
            return this.ConstruirExpresion(false);
            
        }

        private Expresion ConstruirExpresion(bool unaria)
        {

            //logger.Info("Construir Expresion");
            return this.ConstruirExpresion(unaria, false);
        }

        private Expresion ConstruirExpresion(bool unaria, bool argumento)
        {
            
            try
            {
                //logger.Info("Inicio Construir Expresion");
                Elemento elemento;

                Expresion expresionMadre = null;
                Expresion expresionActual = null;

                Estado estado = Estado.EsperandoTerminoIzquierdo;

                elemento = this.ProximoElemento();
                while (elemento != null)
                {
                    if (estado == Estado.EsperandoTerminoIzquierdo)
                    {
                        if (elemento.EsInicioDeAgrupacion())
                        {
                            expresionActual = this.ConstruirExpresion();
                            expresionActual.Agrupada = true;
                            expresionActual.ExpresionMadre = expresionMadre;
                        }
                        else if (elemento.DeterminaSigno())
                        {
                            expresionActual = new ExpresionUnaria(
                                expresionMadre,
                                this.ConstruirExpresion(true),
                                (ElementoOperador)elemento);
                        }
                        else if (elemento.EsNumerico())
                        {
                            expresionActual = new ExpresionNumerica(
                                expresionMadre,
                                elemento.Valor());
                        }
                        else if (elemento.EsFuncion())
                        {
                            ExpresionFuncion expresionFuncion = new ExpresionFuncion(
                                expresionMadre,
                                (ElementoFuncion)elemento);

                            elemento = this.ProximoElemento();
                            if (!elemento.EsInicioDeAgrupacion())
                            {
                                logger.Error("Estado EsperandoTerminoIzquierdo => Elemento Funcion => Se esperaba inicio de agrupación.");
                                throw new InvalidOperationException("Estado EsperandoTerminoIzquierdo => Elemento Funcion => Se esperaba inicio de agrupación.");
                            }

                            Expresion expresionArgumento = this.ConstruirExpresion(
                                unaria: false,
                                argumento: true);

                            while (expresionArgumento != null)
                            {
                                expresionArgumento.ExpresionMadre = expresionFuncion;
                                expresionFuncion.AgregarArgumento(expresionArgumento);

                                this.RetrocedecerElemento();

                                elemento = this.ProximoElemento();

                                if (elemento == null)
                                {
                                    expresionArgumento = null;
                                }
                                else
                                {
                                    if (elemento.EsFinDeAgrupacion())
                                    {
                                        expresionArgumento = null;
                                    }
                                    else if (elemento.EsSeparador())
                                    {
                                        expresionArgumento = this.ConstruirExpresion(
                                            unaria: false,
                                            argumento: true);
                                    }
                                    else
                                    {
                                        logger.Error("Estado EsperandoTerminoIzquierdo => Elemento Funcion => Se esperaba fin de agrupación o separador.");

                                        throw new InvalidOperationException("Estado EsperandoTerminoIzquierdo => Elemento Funcion => Se esperaba fin de agrupación o separador.");
                                    }
                                }
                            }

                            expresionActual = expresionFuncion;
                        }
                        else if (elemento.EsFinDeAgrupacion() && argumento)
                        {
                            break;
                        }
                        else
                        {
                            logger.Error("Estado EsperandoTerminoIzquierdo => Se esperaba inicio de agrupación, signo o valor numérico.");
                            throw new InvalidOperationException("Estado EsperandoTerminoIzquierdo => Se esperaba inicio de agrupación, signo o valor numérico.");
                        }

                        if (unaria)
                        {
                            break;
                        }
                        else
                        {
                            estado = Estado.EsperandoOperador;
                        }
                    }
                    else if (estado == Estado.EsperandoOperador)
                    {
                        if (elemento.EsFinDeAgrupacion())
                        {
                            break;
                        }
                        else if (elemento.EsOperador())
                        {
                            if (expresionActual.EsBinaria())
                            {
                                if (((ExpresionBinaria)expresionActual).Agrupada ||
                                    ((ExpresionBinaria)expresionActual).Operador.TienePrecedencia((ElementoOperador)elemento))
                                {
                                    while (expresionActual.ExpresionMadre != null &&
                                          !expresionActual.Agrupada &&
                                          expresionActual.ExpresionMadre.EsBinaria() &&
                                           ((ExpresionBinaria)expresionActual.ExpresionMadre).Operador.TienePrecedencia((ElementoOperador)elemento))
                                    {
                                        expresionActual = expresionActual.ExpresionMadre;
                                    }

                                    ExpresionBinaria expresionNueva = new ExpresionBinaria(
                                        expresionActual.ExpresionMadre,
                                        expresionActual,
                                        (ElementoOperador)elemento,
                                        null);

                                    if (expresionNueva.ExpresionMadre != null)
                                    {
                                        ((ExpresionBinaria)expresionNueva.ExpresionMadre).TerminoDerecho = expresionNueva;
                                    }

                                    expresionActual.ExpresionMadre = expresionNueva;
                                    expresionActual = expresionNueva;
                                }
                                else
                                {
                                    ExpresionBinaria nuevaExpresion = new ExpresionBinaria(
                                        expresionActual,
                                        ((ExpresionBinaria)expresionActual).TerminoDerecho,
                                        (ElementoOperador)elemento,
                                        null);

                                    ((ExpresionBinaria)expresionActual).TerminoDerecho.ExpresionMadre = nuevaExpresion;
                                    ((ExpresionBinaria)expresionActual).TerminoDerecho = nuevaExpresion;
                                    expresionActual = nuevaExpresion;
                                }
                            }
                            else
                            {
                                Expresion nuevaExpresion = new ExpresionBinaria(
                                    expresionActual.ExpresionMadre,
                                    expresionActual,
                                    (ElementoOperador)elemento,
                                    null);

                                expresionActual.ExpresionMadre = nuevaExpresion;
                                expresionActual = nuevaExpresion;
                            }

                            estado = Estado.EsperandoTerminoDerecho;
                        }
                        else if (elemento.EsSeparador() && argumento)
                        {
                            break;
                        }
                        else
                        {
                            logger.Error("Estado EsperandoOperador => Se esperaba un elemento fin de agrupacion u operador.");
                            throw new InvalidOperationException("Estado EsperandoOperador => Se esperaba un elemento fin de agrupacion u operador.");
                        }
                    }
                    else if (estado == Estado.EsperandoTerminoDerecho)
                    {

                        if (elemento.EsInicioDeAgrupacion())
                        {
                            ((ExpresionBinaria)expresionActual).TerminoDerecho = this.ConstruirExpresion();
                        }
                        else if (elemento.EsNumerico())
                        {
                            ((ExpresionBinaria)expresionActual).TerminoDerecho = new ExpresionNumerica(
                                expresionActual,
                                elemento.Valor());
                        }
                        else if (elemento.EsFuncion())
                        {
                            ExpresionFuncion expresionFuncion = new ExpresionFuncion(
                                expresionActual,
                                (ElementoFuncion)elemento);

                            elemento = this.ProximoElemento();
                            if (!elemento.EsInicioDeAgrupacion())
                            {
                                logger.Error("Estado EsperandoTerminoIzquierdo => Elemento Funcion => Se esperaba inicio de agrupación.");
                                throw new InvalidOperationException("Estado EsperandoTerminoIzquierdo => Elemento Funcion => Se esperaba inicio de agrupación.");
                            }

                            Expresion expresionArgumento = this.ConstruirExpresion(
                                unaria: false,
                                argumento: true);

                            while (expresionArgumento != null)
                            {
                                expresionArgumento.ExpresionMadre = expresionFuncion;
                                expresionFuncion.AgregarArgumento(expresionArgumento);

                                this.RetrocedecerElemento();

                                elemento = this.ProximoElemento();

                                if (elemento == null)
                                {
                                    expresionArgumento = null;
                                }
                                else
                                {
                                    if (elemento.EsFinDeAgrupacion())
                                    {
                                        expresionArgumento = null;
                                    }
                                    else if (elemento.EsSeparador())
                                    {
                                        expresionArgumento = this.ConstruirExpresion(
                                            unaria: false,
                                            argumento: true);
                                    }
                                    else
                                    {
                                        logger.Error("Estado EsperandoTerminoDerecho => Elemento Funcion => Se esperaba fin de agrupación o separador.");
                                        throw new InvalidOperationException("Estado EsperandoTerminoDerecho => Elemento Funcion => Se esperaba fin de agrupación o separador.");
                                    }
                                }
                            }

                            ((ExpresionBinaria)expresionActual).TerminoDerecho = expresionFuncion;
                        }
                        else
                        {
                            logger.Error("Estado EsperandoTerminoDerecho => Se esperaba inicio de agrupación o valor numérico.");
                            throw new InvalidOperationException("Estado EsperandoTerminoDerecho => Se esperaba inicio de agrupación o valor numérico.");
                        }
                        
                        estado = Estado.EsperandoOperador;
                    }

                    elemento = this.ProximoElemento();
                }

                if (expresionActual == null && argumento)
                {
                    return null;
                }

                //logger.Info("Fin Construir Expresion");
                return expresionActual.PrimeraExpresion();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
