using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Just_One_Click
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    
    public partial class DateTimePicker : Window
    {
        public DateTime selectedDate;
        public TimeSpan selectedTime;
        public DateTime futureTime;
        public DateTimePicker()
        {
            InitializeComponent();
        }
        private void SelectFutureTimeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                // Get user input from the DatePicker and TextBox controls
                selectedDate = DatePicker.SelectedDate.Value;
                selectedTime = TimeSpan.Parse(TimeTextBox.Text);
                
                // Combine the date and time parts to get the selected future time
                futureTime = selectedDate + selectedTime;
                Trace.WriteLine(futureTime.ToString());
                // Create and show the second window, passing the selected future time
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void TimeTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numeric input for time (HH:mm)
            e.Handled = !IsNumericInput(e.Text);
        }

        private bool IsNumericInput(string text)
        {
            // Use regex to allow only numeric input
            return Regex.IsMatch(text, "^[0-9]+$");
        }
    }
}
