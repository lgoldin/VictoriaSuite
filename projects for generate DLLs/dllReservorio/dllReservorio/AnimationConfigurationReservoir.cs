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
    public class AnimationConfigurationReservoir : AnimationConfigurationBase
    {
        public string NrVariable { get; set; }
        public string SelectedHVVariable { get; set; }
        public string SelectedForma { get; set; }
        public int SelectedTamanio { get; set; }

        #region Constructor
        public AnimationConfigurationReservoir(List<Variable> variables) : base(variables)
        {
            base.AnimationType = "Reservorio";

            base.DllExtraConfigurations = new List<StackPanel>(); //esto va en el base

            var nrVariableLabel = new Label();
            nrVariableLabel.Content = "Variable de nivel de reservorio:";

            var nrVariableCombo = new ComboBox();
            nrVariableCombo.Name = "nrVariableCombo";
            List<string> nrVariableOptions = new List<string>();
            foreach (var variable in variables)
            {
                nrVariableOptions.Add(variable.Name);
            }

            nrVariableCombo.ItemsSource = nrVariableOptions;
            nrVariableCombo.SelectedIndex = 0;

            var maxValueVariableLabel = new Label();
            maxValueVariableLabel.Content = "Variable de valor máximo reservorio:";

            var variablesForMaxValueCombo = new ComboBox();
            variablesForMaxValueCombo.Name = "maxValueCombo";
            variablesForMaxValueCombo.ItemsSource = base.Variables.Select(x => x.Name);
            variablesForMaxValueCombo.SelectedIndex = 0;

            var formaLabel = new Label();
            formaLabel.Content = "Forma del reservorio:";

            var formasCombo = new ComboBox();
            formasCombo.Name = "formasCombo";
            List<string> formasOptions = new List<string>();
            formasOptions.Add("Cuadrado");
            formasOptions.Add("Circulo");
            formasOptions.Add("Ovalo");

            formasCombo.ItemsSource = formasOptions;
            formasCombo.SelectedIndex = 0;

            var tamanioLabel = new Label();
            tamanioLabel.Content = "Tamaño de la animación:";

            var tamaniosCombo = new ComboBox();
            tamaniosCombo.Name = "tamaniosCombo";
            List<int> tamaniosOptions = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                tamaniosOptions.Add(i);
            }
            tamaniosCombo.ItemsSource = tamaniosOptions;
            tamaniosCombo.SelectedIndex = 0;

            var maxValueStackPanel = new StackPanel();

            maxValueStackPanel.Name = "maxValueStackPanel";
            maxValueStackPanel.Children.Add(nrVariableLabel);
            maxValueStackPanel.Children.Add(nrVariableCombo);
            maxValueStackPanel.Children.Add(maxValueVariableLabel);
            maxValueStackPanel.Children.Add(variablesForMaxValueCombo);
            maxValueStackPanel.Children.Add(formaLabel);
            maxValueStackPanel.Children.Add(formasCombo);
            maxValueStackPanel.Children.Add(tamanioLabel);
            maxValueStackPanel.Children.Add(tamaniosCombo);

            DllExtraConfigurations.Add(maxValueStackPanel);
        }
        #endregion

        public override void BindProperties()
        {
            var stackChildrens = DllExtraConfigurations.Where(x => x.Name == "maxValueStackPanel").First().Children;

            foreach (var s in stackChildrens)
            {
                if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "maxValueCombo")
                {
                    ComboBox maxValueCombo = (ComboBox)s;
                    SelectedHVVariable = (string)maxValueCombo.SelectedValue;
                }
                if (s.GetType() == typeof(ComboBox) && ((ComboBox)s).Name == "nrVariableCombo")
                {
                    ComboBox nrVariablesCombo = (ComboBox)s;
                    NrVariable = (string)nrVariablesCombo.SelectedValue;
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

    public class AnimationReservoirViewModel : AnimationViewModel
    {
        //ESTA PROPIEDAD LA USO PARA ENCONTRAR EN VICTORIA QUE ANIMATIONVIEWMODEL TENGO QUE CREAR.
        private string configurationType = "AnimationConfigurationReservoir";
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

        public double NrActualValue { get; set; }

        public List<double> NrValuesList { get; set; }

        public double MaxValue { get; set; }

        public AnimationReservoirViewModel()
        {

        }

        public override void BindSimulationVariableValues()
        {
            AnimationConfigurationReservoir animationConfigReservoir = (AnimationConfigurationReservoir)this.AnimationConfig;

            this.NrValuesList = AnimationConfig.Variables.Where(n => n.Name == animationConfigReservoir.NrVariable).First().ValuesEnumerable.ToList();
            this.MaxValue = animationConfigReservoir.Variables.Where(n => n.Name == animationConfigReservoir.SelectedHVVariable).First().ActualValue;
        }

        public override void InitializeAnimation(AnimationConfigurationBase animationConfig)
        {
            base.InitializeAnimation(animationConfig);

            AnimationConfigurationReservoir animationConfigQueue = (AnimationConfigurationReservoir)animationConfig;
            int tamanio = animationConfigQueue.SelectedTamanio;
            this.VariableToAnimateName = "Nivel reservorio: ";

            this.AnimationElementsList = new ObservableCollection<Shape>();
            Shape element = null;
            if (animationConfigQueue.SelectedForma.Equals("Circulo") || animationConfigQueue.SelectedForma.Equals("Ovalo"))
            {
                element = new Ellipse();
            }
            else if (animationConfigQueue.SelectedForma.Equals("Cuadrado"))
            {
                element = new Rectangle();
            }
            element.MaxHeight = 60 * tamanio;
            element.Height = 60 * tamanio;
            element.Width = 60 * tamanio;
            if (animationConfigQueue.SelectedForma.Equals("Ovalo"))
            {
                element.Width = element.Width * 2;
            }
            element.Fill = new SolidColorBrush(Colors.Blue);
            this.AnimationElementsList.Add(element);
        }

        public override void DoAnimation(int index)
        {
            this.VariableToAnimateValue = this.NrValuesList[index].ToString();
            this.NrActualValue = this.NrValuesList[index];

            if (this.NrActualValue < 0)
            {
                this.NrActualValue = 0;
            }

            if (this.AnimationElementsList != null && this.AnimationElementsList.Any())
            {
                Shape reservoir = (Shape)AnimationElementsList[0];
                var waterColor = new Color();

                var percentOfReservoir = NrActualValue * reservoir.MaxHeight / this.MaxValue;
                if (percentOfReservoir > 100)
                {
                    waterColor = Colors.Red;
                    percentOfReservoir = 100;
                }
                else
                {
                    waterColor = Colors.Blue;
                }

                double percentage = percentOfReservoir / 100;
                LinearGradientBrush waterEffect = new LinearGradientBrush();
                waterEffect.StartPoint = new Point(1, 1);
                waterEffect.EndPoint = new Point(1, 0);
                waterEffect.GradientStops.Add(new GradientStop(waterColor, 0));
                waterEffect.GradientStops.Add(new GradientStop(waterColor, percentage));
                waterEffect.GradientStops.Add(new GradientStop(Colors.Transparent, percentage));

                reservoir.Fill = waterEffect;
            }
        }

    }
}
