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

using System.Windows;

namespace Just_One_Click
{
    public partial class InputBox : Window
    {
        public string ResponseText { get; set; }

        public InputBox(string prompt, string title, string defaultValue = "")
        {
            InitializeComponent();
            DataContext = this;
            Prompt = prompt;
            Title = title;
            InputTextBox.Text = defaultValue;
            InputTextBox.Focus();
        }

        public string Prompt { get; set; }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton_Click(sender, e);
            }
            if (e.Key == Key.Escape)
            {
                CancelButton_Click(sender, e);
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = InputTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
