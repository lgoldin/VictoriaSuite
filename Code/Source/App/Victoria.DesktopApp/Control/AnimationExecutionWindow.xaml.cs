﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Victoria.DesktopApp.View;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.Control
{
    /// <summary>
    /// Lógica de interacción para AnimationExecutionWindow.xaml
    /// </summary>
    public partial class AnimationExecutionWindow : Window
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));

        public AnimationExecutionWindow(ObservableCollection<AnimationViewModel> animations)
        {
       
            try
            {
                InitializeComponent();
                ((AnimationExecutionViewModel)this.DataContext).ExecuteButtonEnabled = true;
                Closing += ((AnimationExecutionViewModel)this.DataContext).OnWindowClosing;
                ((AnimationExecutionViewModel)this.DataContext).Animations = animations;
                var variables = CreateVariablesClone(animations[0].AnimationConfig.Variables);
                ((AnimationExecutionViewModel)this.DataContext).Variables = variables;
                //logger.Info("Fin Ejecución de Animación");
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private ObservableCollection<ModelWPF.Variable> CreateVariablesClone(IEnumerable<ModelWPF.Variable> variables)
        {

            logger.Info("Clonar Variables para Animación.");

            var variablesClones = new ObservableCollection<ModelWPF.Variable>();
            foreach(var variable in variables)
            {
                variablesClones.Add(CreateCopyOfVariable(variable));
            }

            //logger.Info("Fin Crear Vairables Clon");
            return variablesClones;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private ModelWPF.Variable CreateCopyOfVariable(ModelWPF.Variable variable)
        {
            //logger.Info("Inicio Crear copia de variable");
            var variableClone = new ModelWPF.Variable
            {
                VariableComponent = new Shared.Variable(),
                Name = variable.Name,
                InitialValue = variable.InitialValue,
                Values = new ObservableCollection<double>()
            };

            foreach(var value in variable.Values)
            {
                variableClone.Values.Add(value);
            }

            variableClone.ActualValue = 0;

            //logger.Info("Fin Crear copia de variable");
            return variableClone;
        }

        private void BtnExecute_Animations(object sender, RoutedEventArgs e)
        {
            try
            {

                logger.Info("Ejecutar Animaciones");
                ((AnimationExecutionViewModel)this.DataContext).ExecuteButtonEnabled = false;
                ((AnimationExecutionViewModel)this.DataContext).ExecuteAnimationsCommand.Execute(null);
                //logger.Info("Fin Crear copia de variable");
            }
            catch (Exception ex)
            {   
                var viewException = new AlertPopUp("Se produjo un error al intentar ejecutar las animaciones. Por favor revisa la configuración de las mismas.");
                //logger.Error("Se produjo un error al intentar ejecutar las animaciones:" + ex.Message);
                viewException.ShowDialog();
            }
        }

        private void BtnStop_Animations(object sender, RoutedEventArgs e)
        {
            ((AnimationExecutionViewModel)this.DataContext).ExecuteButtonEnabled = true;
            ((AnimationExecutionViewModel)this.DataContext).StopAnimationsCommand.Execute(null);
            //logger.Info("Fin Parar Animaciones");
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            DragMove();
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            int sliderValue = Convert.ToInt32(slider.Value);

            int animationWait = 400;

            switch (sliderValue)
            {
                case 1:
                    animationWait = 700;
                    break;
                case 2:
                    animationWait = 400;
                    break;
                case 3:
                    animationWait = 100;
                    break;
                default:
                    animationWait = 400;
                    break;
            }

            ((AnimationExecutionViewModel)this.DataContext).AnimationWait = animationWait;
        }
    }
}
