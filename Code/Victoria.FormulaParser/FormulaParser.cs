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

        private readonly string formula;

        private readonly Expresion expresionRaiz;

        private int indice;

        public FormulaParser(string formula)
        {
            this.formula = Regex.Replace(formula, @"\s+", "");

            this.indice = 0;

            this.expresionRaiz = this.ConstruirExpresion();
        }

        public string ToJavaScriptString()
        {
            return this.expresionRaiz.ToJavaScriptString();
        }

        public Elemento ProximoElemento()
        {
            Elemento elemento = null;

            string token = string.Empty;
            char caracter;

            string numeros = "0123456789.";
            string operadores = "+-*/^";
            string agrupadores = "()";

            if (this.indice == this.formula.Length)
            {
                return elemento;
            }

            caracter = this.formula[this.indice];

            token += caracter;
            this.indice++;

            if (numeros.Contains(caracter))
            {
                if(this.indice < this.formula.Length)
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
            else if (operadores.Contains(caracter))
            {
                elemento = new ElementoOperador(token);
            }
            else if (agrupadores.Contains(caracter))
            {
                elemento = new ElementoAgrupador(token);
            }
            else
            {
                throw new InvalidOperationException("Caracter no esperado.");
            }

            return elemento;
        }

        public Elemento VerProximoElemento()
        {
            int indiceOriginal = this.indice;

            Elemento elemento = this.ProximoElemento();

            this.indice = indiceOriginal;

            return elemento;
        }

        private Expresion ConstruirExpresion()
        {
            return this.ConstruirExpresion(false);
        }

        private Expresion ConstruirExpresion(bool unaria)
        {
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
                    else
                    {
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
                    else
                    {
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
                    else
                    {
                        throw new InvalidOperationException("Estado EsperandoTerminoDerecho => Se esperaba inicio de agrupación o valor numérico.");
                    }

                    estado = Estado.EsperandoOperador;
                }

                elemento = this.ProximoElemento();
            }

            return expresionActual.PrimeraExpresion();
        }
    }
}
