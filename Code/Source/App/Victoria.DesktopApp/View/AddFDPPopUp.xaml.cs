using System;
using System.Collections.Generic;
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
using Victoria.Shared.AnalisisPrevio;
using System.Xml.Linq;
using Microsoft.Win32;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class AddFDPPopUp : Window
    {

        private AnalisisPrevio analisisPrevio;

        public void FDPGenerator(AnalisisPrevio analisisPrevio)
        {
            this.analisisPrevio = analisisPrevio;
        }

        public AddFDPPopUp()
        {
            InitializeComponent();          

        }



        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)comboBox.SelectedItem;
            string value = typeItem.Content.ToString();
            if (value == "Archivo Excel")
            {
                Archivo.Visibility = Visibility.Visible;
                rutaFile.IsReadOnly = true;
                rutaFile.Text ="Ingrese la ruta del archivo";
            }
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private String getFileName()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xls)|*.xlsx|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                return openFile.FileName;
            }

            return null;
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCalcularFDP_OnClick(object sender, RoutedEventArgs e)
        {
            Archivo.Visibility = Visibility.Visible; 
        }

        private void Btnserch_Click(object sender, RoutedEventArgs e)
        {
            rutaFile.Text = getFileName();
        }
    }
}
