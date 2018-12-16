﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Victoria.ViewModelWPF;

namespace Victoria.ModelWPF
{
    public class AnimationConfigurationPost : AnimationConfigurationBase
    {        
        public string SelectedNSVariable { get; set; }
        public int SelectedNPostsVariable { get; set; }
        public string SelectedTPSVariable { get; set; }
        public string SelectedHVVariable { get; set; }
        public int SelectedTamanio { get; set; }

        #region Constructor
        public AnimationConfigurationPost(List<Variable> variables) : base(variables)
        {
            base.AnimationType = "Puesto";

            base.DllExtraConfigurations = new List<StackPanel>(); //esto va en el base

            addExtraConf("Variable de Tiempo de próxima salida:", "TPS", base.Variables.Select(x => x.Name));            
            addExtraConf("Variable de High value:", "HV", base.Variables.Select(x => x.Name));

            var tamanioLabel = new Label();
            tamanioLabel.Content = "Tamaño de la animación:";

            var tamaniosCombo = new ComboBox();
            tamaniosCombo.Name = "tamaniosCombo";
            List<int> tamaniosOptions = new List<int>();
            for (int i = 1; i <= 6; i++)
            {
                tamaniosOptions.Add(i);
            }
            tamaniosCombo.ItemsSource = tamaniosOptions;
            tamaniosCombo.SelectedIndex = 0;
            var StackPanel = new StackPanel();
            StackPanel.Name = "tamaniosCombo";
            StackPanel.Children.Add(tamanioLabel);
            StackPanel.Children.Add(tamaniosCombo);
            DllExtraConfigurations.Add(StackPanel);
        }

        public void addExtraConf(string label, string variableName, IEnumerable<string> items)
        {
            var Label = new Label();
            Label.Content = label;

            var variablesForCombo = new ComboBox();
            variablesForCombo.ItemsSource = items;
            variablesForCombo.SelectedIndex = 0;

            var StackPanel = new StackPanel();

            StackPanel.Name = variableName;
            StackPanel.Children.Add(Label);
            StackPanel.Children.Add(variablesForCombo);

            DllExtraConfigurations.Add(StackPanel);
        }
        #endregion

        public override void BindProperties()
        {
            var stackChildrensTPS = DllExtraConfigurations.Where(x => x.Name == "TPS").First().Children;
            foreach (var s in stackChildrensTPS)
            {
                if (s.GetType() == typeof(ComboBox))
                {
                    ComboBox variablesCombo = (ComboBox)s;
                    SelectedTPSVariable = (string)variablesCombo.SelectedValue;
                }
            }

            var stackChildrensHV = DllExtraConfigurations.Where(x => x.Name == "HV").First().Children;
            foreach (var s in stackChildrensHV)
            {
                if (s.GetType() == typeof(ComboBox))
                {
                    ComboBox variablesCombo = (ComboBox)s;
                    SelectedHVVariable = (string)variablesCombo.SelectedValue;
                }
            }

            var stackChildrensTamanios = DllExtraConfigurations.Where(x => x.Name == "tamaniosCombo").First().Children;
            foreach (var s in stackChildrensTamanios)
            {
                if (s.GetType() == typeof(ComboBox))
                {
                    ComboBox tamaniosCombo = (ComboBox)s;
                    SelectedTamanio = (int)tamaniosCombo.SelectedValue;
                }
            }
        }
    }

    public class AnimationPostViewModel : AnimationViewModel
    {
        //ESTA PROPIEDAD LA USO PARA ENCONTRAR EN VICTORIA QUE ANIMATIONVIEWMODEL TENGO QUE CREAR.
        private string configurationType = "AnimationConfigurationPost";
        public override string ConfigurationType
        {
            get
            {
                return configurationType;
            }
            set
            {
                configurationType = value;
            }
        }

        public double TPSActualValue { get; set; }        
        public double HighValue { get; set; }
        public double TimeActualValue { get; set; }

        public List<double> TimeValuesList { get; set; }
        public List<double> TPSValuesList { get; set; }
       
        public AnimationPostViewModel()
        {

        }

        public override void BindSimulationVariableValues()
        {
            AnimationConfigurationPost animationConfigPost = (AnimationConfigurationPost)this.AnimationConfig;
            this.HighValue = animationConfigPost.Variables.Where(n => n.Name == animationConfigPost.SelectedHVVariable).First().InitialValue;
            this.TPSValuesList = animationConfigPost.Variables.Where(n => n.Name == animationConfigPost.SelectedTPSVariable).First().ValuesEnumerable.ToList();
            this.TimeValuesList = animationConfigPost.Variables.Where(n => n.Name == "T").First().ValuesEnumerable.ToList();
        }

        //posicion 0 rectangulo (puesto) posicion 1 circulo (persona)
        public void FreePost()
        {
            this.AnimationElementsList[0].Fill = new SolidColorBrush(Colors.Green);
            this.AnimationElementsList[1].Fill = new SolidColorBrush(Colors.White);
        }

        public void BusyPost()
        {
            this.AnimationElementsList[0].Fill = new SolidColorBrush(Colors.Red);
            this.AnimationElementsList[1].Fill = new SolidColorBrush(Colors.Black);
        }

        public override void InitializeAnimation(AnimationConfigurationBase animationConfig)
        {
            base.InitializeAnimation(animationConfig);
            AnimationConfigurationPost animationConfigPost = (AnimationConfigurationPost)this.AnimationConfig;
            int tamanio = animationConfigPost.SelectedTamanio;

            this.AnimationElementsList = new ObservableCollection<Shape>();
            var r = new Rectangle();
            r.Height = 30 * tamanio;
            r.Width = 30 * tamanio;
            r.Fill = new SolidColorBrush(Colors.Green);

            var e = new Ellipse();
            e.Height = 10 * tamanio;
            e.Width = 10 * tamanio;
            e.Fill = new SolidColorBrush(Colors.White);
            e.Margin = new Thickness(10.0 * tamanio, 5.0 * tamanio, 0, 0);

            this.AnimationElementsList.Add(r);
            this.AnimationElementsList.Add(e);
        }

        public override void DoAnimation(int index)
        {
            this.TPSActualValue = this.TPSValuesList[index];
            this.TimeActualValue = this.TimeValuesList[index];

            if (TPSActualValue == HighValue || TimeActualValue == 0)
            {
                FreePost();
            }
            else
            {
                BusyPost();
            }
        }

    }

}
