using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Just_One_Click
{
    public class Settings
    {
        public bool DarkModeEnabled { get; set; }
        public string TextEditor { get; set; }
        public bool DeleteConfirmation { get; set; }
    }

    public partial class SettingsWindow : Window
    {
        string SettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        

        private Settings _appSettings;

        public SettingsWindow()
        {
            InitializeComponent();
            SettingsFilePath = SettingsFilePath + "appsettings.json";
            Trace.WriteLine(SettingsFilePath);
            // Load settings when the window is initialized
            _appSettings = LoadSettings();
            UpdateUI();
        }

        private Settings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            return new Settings { DarkModeEnabled = false, TextEditor = "Notepad" };
            // Return default settings if the file doesn't exist or there's an error
        }

        private void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_appSettings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUI()
        {
<<<<<<< Updated upstream
            // Update your UI controls with the loaded settings
            DarkModeCheckbox.IsChecked = _appSettings.DarkModeEnabled;
            Path.Text = _appSettings.TextEditor;
            ConfirmationCheckbox.IsChecked = _appSettings.DeleteConfirmation;
=======
            if (_appSettings != null)
            {
                // Update your UI controls with the loaded settings
                DarkModeCheckbox.IsChecked = _appSettings.DarkModeEnabled;
               
                ConfirmationCheckbox.IsChecked = _appSettings.DeleteConfirmation;
                
            }
            else
            {
                // Handle the case where _appSettings is null
                // You can provide default values or take appropriate action
                DarkModeCheckbox.IsChecked = false;
                ConfirmationCheckbox.IsChecked = false;

                MessageBox.Show("Failed to save settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
>>>>>>> Stashed changes
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< Updated upstream
            // Update settings with values from UI controls
            _appSettings.DarkModeEnabled = DarkModeCheckbox.IsChecked ?? false;
            _appSettings.TextEditor = Path.Text;
=======
            if (_appSettings != null)
            {
                // Update settings with values from UI controls
                _appSettings.DarkModeEnabled = DarkModeCheckbox.IsChecked ?? false;
                
                
                
>>>>>>> Stashed changes

            // Save settings to the JSON file
            SaveSettings();
        }
<<<<<<< Updated upstream
=======
        // Add this method to your SettingsWindow class
        public void WritePlaceholderJson(string filePath)
        {
            try
            {
                // Create a default Settings object with placeholder values
                Settings placeholderSettings = new Settings
                {
                    DarkModeEnabled = true,
                    DeleteConfirmation = true
                };

                // Serialize the Settings object to JSON
                string json = JsonConvert.SerializeObject(placeholderSettings, Formatting.Indented);

                // Write the JSON to the specified file
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing placeholder JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


>>>>>>> Stashed changes
        private void FileBrowserDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Executable File",
                Filter = "Executable Files|*.exe",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Load settings from the selected JSON file
                string json = File.ReadAllText(openFileDialog.FileName);
                _appSettings = JsonConvert.DeserializeObject<Settings>(json);

                // Update UI with the loaded settings
                UpdateUI();
            }
        }
        private void ResetClick(object sender, RoutedEventArgs e)
        {

            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Checks Documents Folder for path
            savePath = System.IO.Path.Combine(savePath + "/Just One Click/");
            string saveFile = System.IO.Path.Combine(savePath + "savedata.json");
            MainWindow main = new MainWindow();
            main.WritePlaceholderJson(saveFile);
        }
    }
}
