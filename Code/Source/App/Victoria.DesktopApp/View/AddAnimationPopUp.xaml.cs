﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Victoria.ModelWPF;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for AddAnimationPopUp.xaml
    /// </summary>
    public partial class AddAnimationPopUp : Window
    {
        public UI.SharedWPF.DialogResult Result { get; set; }
        public IEnumerable<Variable> Variables { get; set; }


        public string AnimationType //esto deberia ser un enum o algo asi
        {
            get { return this.animationTypeCombo.SelectedValue.ToString(); }
        }

        public string AnimationName { get; set; }

        public bool CanExecute { get; set; }

        public List<AnimationConfigurationBase> DllConfigurations { get; set; }

        public AnimationConfigurationBase ResultConfig { get; set; }

        public AddAnimationPopUp(IEnumerable<Variable> variables, List<AnimationConfigurationBase> dllConfigurations)
        {
            this.Variables = variables;
            this.AnimationName = "Animación";
            this.CanExecute = false;
            this.InitializeComponent();
            this.DllConfigurations = dllConfigurations;
            this.FillCombos();
        }

        //CONSTRUCTOR PARA LA EDICION
        public AddAnimationPopUp(AnimationViewModel animationVm, List<AnimationConfigurationBase> dllConfigurations) //Mas problemas DLL...aca le puedellegar cualquier tipo de animationviewmodel, con mas propiedades a tocar...ver
        {
            this.Variables = animationVm.AnimationConfig.Variables;
            this.AnimationName = animationVm.AnimationName;
            this.CanExecute = animationVm.AnimationConfig.CanExecute;
            this.InitializeComponent();
            this.DllConfigurations = dllConfigurations;
            this.FillCombos();
            this.animationTypeCombo.SelectedIndex = ((List<string>)animationTypeCombo.ItemsSource).FindIndex(i=>i.ToString() == animationVm.AnimationConfig.AnimationType); 
        }

        private void FillCombos()
        {
            //////////////////ANIMATION TYPES FROM DLLS
            List<string> types = new List<string>();

            foreach(var config in DllConfigurations)
            {
                types.Add(config.AnimationType);             
            }

            animationTypeCombo.ItemsSource = types;

            animationTypeCombo.SelectedIndex = 0;


            //////////////////VARIABLES
            var variablesOptions = this.Variables.Select(x => x.Name);

        }

        private void AnimationTypes_SelectionChanged(object sender, RoutedEventArgs e)
        {
            extraConfigsContainer.Children.Clear();

            var animationTypeSelected = ((System.Windows.Controls.ComboBox)sender).SelectedValue.ToString();

            var configurationType = DllConfigurations.First(x => x.AnimationType == animationTypeSelected);

            if (configurationType.DllExtraConfigurations != null && configurationType.DllExtraConfigurations.Any())
            {
                foreach (var extraField in configurationType.DllExtraConfigurations)
                {
                    extraConfigsContainer.Children.Add(extraField);
                }

            }
        }


        private void btnAccept_OnClick(object sender, RoutedEventArgs e)
        {
            ResultConfig = DllConfigurations.First(x => x.AnimationType == AnimationType);
            ResultConfig.BindProperties(); 
            ResultConfig.AnimationName = AnimationName;
            ResultConfig.CanExecute = this.CanExecute;
            ResultConfig.Variables = Variables;

            this.Result = UI.SharedWPF.DialogResult.Accept;
            this.Close();
        }

        private void CanExecuteChecked(object sender, RoutedEventArgs e)
        {
            this.CanExecute = true;
        }

        private void CanExecuteUnchecked(object sender, RoutedEventArgs e)
        {
            this.CanExecute = false;
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            this.Close();
        }

        private void mainBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {

            }

        }
    }
}
