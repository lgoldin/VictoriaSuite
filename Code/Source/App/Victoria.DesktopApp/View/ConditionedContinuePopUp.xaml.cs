﻿using System;
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
using Victoria.UI.SharedWPF;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for ConditionedContinuePopUp.xaml
    /// </summary>
    public partial class ConditionedContinuePopUp : Window
    {

        public DialogResult Result { get; set; }

        public ConditionedContinuePopUp()
        {
            InitializeComponent(); 
        }

        private void btnAccept_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Accept;
            if (!this.textboxIsWrong())
            {
                this.Close();
            }
            
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private bool textboxIsWrong()
        {
            if (string.IsNullOrEmpty(this.conditionTextBox.Text) || 
                string.IsNullOrWhiteSpace(this.conditionTextBox.Text)
               )
            {
                this.conditionTextBox.BorderBrush = Brushes.Red;
                return true;
            }

            return false;
        }
    }
}
