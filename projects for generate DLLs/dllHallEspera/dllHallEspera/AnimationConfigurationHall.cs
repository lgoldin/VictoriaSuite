using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Victoria.ViewModelWPF;

namespace Victoria.ModelWPF
{
    public class AnimationConfigurationHall : AnimationConfigurationBase
    {
        public int SelectedNsLimit { get; set; }
        public int SelectedNPosts { get; set; }
        public string NsVariable { get; set; }
        public string SelectedForma { get; set; }
        public int SelectedTamanio { get; set; }

        #region Constructor
        public AnimationConfigurationHall(List<Variable> variables):base(variables)
        {
            base.AnimationType = "Hall de espera";

            base.DllExtraConfigurations = new List<StackPanel>(); //esto va en el base

            var nsVariableLabel = new Label();
            nsVariableLabel.Content = "Variable de cantidad de personas:";

            var nsVariableCombo = new ComboBox();
            nsVariableCombo.Name = "nsVariableCombo";
            List<string> nsVariableOptions = new List<string>();
            foreach (var variable in variables)
            {
                nsVariableOptions.Add(variable.Name);
            }
            nsVariableCombo.ItemsSource = nsVariableOptions;
            nsVariableCombo.SelectedIndex = 0;

            var nsLimitLabel = new Label();
            nsLimitLabel.Content = "Limite de personas en hall a mostrar:";
            var nsLimitCombo = new ComboBox();
            nsLimitCombo.Name = "nsLimitCombo";
            List<int> nsLimitOptions = new List<int>();
            for (int i = 1; i <= 8; i++)
            {
                nsLimitOptions.Add(8 + (4*i));
            }
            nsLimitCombo.ItemsSource = nsLimitOptions;
            nsLimitCombo.SelectedIndex = 0;

            var nPostLabel = new Label();
            nPostLabel.Content = "Cantidad de personas a restar del hall:";
            var nPostCombo = new ComboBox();
            nPostCombo.Name = "nPostCombo";
            List<int> nsPostOptions = new List<int>();
            for (int i = 1; i <= 10; i++)
            {
                nsPostOptions.Add(i);
            }
            nPostCombo.ItemsSource = nsPostOptions;
            nPostCombo.SelectedIndex = 0;

            var formaLabel = new Label();
            formaLabel.Content = "Forma de las personas en el hall:";
            var formasCombo = new ComboBox();
            formasCombo.Name = "formasCombo";
            List<string> formasOptions = new List<string>();
            formasOptions.Add("Circulos");
            formasOptions.Add("Cuadrados");
            formasCombo.ItemsSource = formasOptions;
            formasCombo.SelectedIndex = 0;

            var tamanioLabel = new Label();
            tamanioLabel.Content = "Tamaño de la animación:";
            var tamaniosCombo = new ComboBox();
            tamaniosCombo.Name = "tamaniosCombo";
            List<int> tamanioOptions = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                tamanioOptions.Add(i);
            }
            tamaniosCombo.ItemsSource = tamanioOptions;
            tamaniosCombo.SelectedIndex = 0;

            var nsLimitStackPanel = new StackPanel();

            nsLimitStackPanel.Name = "extraConfigsQueue";
            nsLimitStackPanel.Children.Add(nsVariableLabel);
            nsLimitStackPanel.Children.Add(nsVariableCombo);
            nsLimitStackPanel.Children.Add(nsLimitLabel);
            nsLimitStackPanel.Children.Add(nsLimitCombo);
            nsLimitStackPanel.Children.Add(nPostLabel);
            nsLimitStackPanel.Children.Add(nPostCombo);
            nsLimitStackPanel.Children.Add(formaLabel);
            nsLimitStackPanel.Children.Add(formasCombo);
            nsLimitStackPanel.Children.Add(tamanioLabel);
            nsLimitStackPanel.Children.Add(tamaniosCombo);

            DllExtraConfigurations.Add(nsLimitStackPanel);
        }
        #endregion

        public override void BindProperties()
        {
            //Esto lo usamos para bindear las propiedades propias de la config. En este caso seria el valor seleccionado de nslimit a partir del combo
            var stackChildrens = DllExtraConfigurations.Where(x => x.Name == "extraConfigsQueue").First().Children;

            foreach (var s in stackChildrens)
            {
                if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "nsLimitCombo")
                {
                    ComboBox nsLimitCombo = (ComboBox)s;
                    SelectedNsLimit = (int)nsLimitCombo.SelectedValue;
                }
                else if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "nsVariableCombo")
                {
                    ComboBox nsVariablesCombos = (ComboBox)s;
                    NsVariable = (string)nsVariablesCombos.SelectedValue;
                }
                else if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "nPostCombo")
                {
                    ComboBox nPostCombos = (ComboBox)s;
                    SelectedNPosts = (int)nPostCombos.SelectedValue;
                }
                else if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "formasCombo")
                {
                    ComboBox formasCombo = (ComboBox)s;
                    SelectedForma = (string)formasCombo.SelectedValue;
                }
                else if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "tamaniosCombo")
                {
                    ComboBox tamaniosCombo = (ComboBox)s;
                    SelectedTamanio = (int)tamaniosCombo.SelectedValue;
                }
            }

        }
    }

    public class AnimationHallViewModel : AnimationViewModel
    {
        public int NsLimit { get; set; }

        public int tamanio { get; set; }

        //ESTA PROPIEDAD LA USO PARA ENCONTRAR EN VICTORIA QUE ANIMATIONVIEWMODEL TENGO QUE CREAR.
        private string configurationType = "AnimationConfigurationHall";
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
        public double NsActualValue { get; set; }

        public List<double> NsValuesList { get; set; }

        public int NPostsValue { get; set; }

        public AnimationHallViewModel()
        {

        }

        public override void BindSimulationVariableValues()
        {
            this.NsValuesList = AnimationConfig.Variables.Where(n => n.Name == ((AnimationConfigurationHall)AnimationConfig).NsVariable).First().ValuesEnumerable.ToList();
        }

        public override void InitializeAnimation(AnimationConfigurationBase animationConfig)
        {
            base.InitializeAnimation(animationConfig);

            AnimationConfigurationHall animationConfigQueue = (AnimationConfigurationHall)animationConfig;
            this.NsLimit = animationConfigQueue.SelectedNsLimit;
            this.NPostsValue = animationConfigQueue.SelectedNPosts;
            this.VariableToAnimateName = "Personas: ";
            this.tamanio = animationConfigQueue.SelectedTamanio;

            this.AnimationElementsList = new ObservableCollection<Shape>();
            for (int i = 1; i <= NsLimit; i++)
            {
                Shape a = null;
                if (animationConfigQueue.SelectedForma.Equals("Circulos"))
                {
                    a = new Ellipse();
                }
                else if (animationConfigQueue.SelectedForma.Equals("Cuadrados"))
                {
                    a = new Rectangle();
                }
                a.Height = 10 * tamanio;
                a.Width = 10 * tamanio;
                a.Fill = new SolidColorBrush(Colors.White);
                a.Margin = createMargin(i);
                this.AnimationElementsList.Add(a);
            }
            this.PropertyChanged += propertyOrientationChanged;
        }

        private Thickness createMargin(int i)
        {
            double left = 0;
            double top = 0;
            switch (i)
            {
                    case 1:
                        break;
                    case 2:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 3:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 4:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 5:
                        //top = -(10.0 * tamanio);
                        break;
                    case 6:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 7:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 8:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 9:
                        //top = -(10.0 * tamanio);
                        break;
                    case 10:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 11:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 12:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 13:
                        //top = -(10.0 * tamanio);
                        break;
                    case 14:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 15:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 16:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 17:
                        //top = -(10.0 * tamanio);
                        break;
                    case 18:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 19:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 20:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 21:
                        //top = -(10.0 * tamanio);
                        break;
                    case 22:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 23:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 24:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 25:
                        //top = -(10.0 * tamanio);
                        break;
                    case 26:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 27:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 28:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 29:
                        //top = -(10.0 * tamanio);
                        break;
                    case 30:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 31:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 32:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 33:
                        //top = -(10.0 * tamanio);
                        break;
                    case 34:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 35:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 36:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                    case 37:
                        //top = -(10.0 * tamanio);
                        break;
                    case 38:
                        left = (10.0 * tamanio) + (5 * tamanio);
                        top = -(10.0 * tamanio) - 4;
                        break;
                    case 39:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 2;
                        top = -(10.0 * tamanio * 2) - 2;
                        break;
                    case 40:
                        left = ((10.0 * tamanio) + (5 * tamanio)) * 3;
                        top = -(10.0 * tamanio * 3) + 2;
                        break;
                }
            if (this.animationOrientation.Equals("Vertical"))
            {
                return new Thickness(left, top, 0, 0);
            } 
            else 
            {
                return new Thickness(-left, -top, 0, 0);
            }
        }

        private void propertyOrientationChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AnimationOrientation"))
            {
                UpdateMargins();
            }
        }

        private void UpdateMargins()
        {
            for (int i = 0; i < this.AnimationElementsList.Count; i++)
            {
                int j = i + 1;
                var element = this.AnimationElementsList.ElementAt(i);
                element.Margin = createMargin(j);
            }
        }

        public override void DoAnimation(int index)
        {
            this.VariableToAnimateValue = this.NsValuesList[index].ToString();
            this.NsActualValue = this.NsValuesList[index];

            if (this.AnimationElementsList != null && this.AnimationElementsList.Any())
            {
                foreach (var elipse in this.AnimationElementsList)
                {
                    elipse.Fill = new SolidColorBrush(Colors.White);
                }

                var QueueRealValue = this.NsActualValue - this.NPostsValue;
                if (QueueRealValue < 0)
                {
                    QueueRealValue = 0;
                }

                if (QueueRealValue <= this.NsLimit)
                {
                    for (int i = 0; i < QueueRealValue; i++)
                    {
                        if (this.AnimationElementsList[i] != null)
                        {
                            this.AnimationElementsList[i].Fill = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.NsLimit; i++)
                    {
                        if (this.AnimationElementsList[i] != null)
                        {
                            this.AnimationElementsList[i].Fill = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

    }

}
