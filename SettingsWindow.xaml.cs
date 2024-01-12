using System;
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
        // Add more properties as needed
    }

    public partial class SettingsWindow : Window
    {
        private const string SettingsFilePath = "appsettings.json";

        private Settings _appSettings;

        public SettingsWindow()
        {
            InitializeComponent();

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

            // Return default settings if the file doesn't exist or there's an error
            return new Settings { DarkModeEnabled = false, TextEditor = "Notepad" };
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
            // Update your UI controls with the loaded settings
            DarkModeCheckbox.IsChecked = _appSettings.DarkModeEnabled;
            Path.Text = _appSettings.TextEditor;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Update settings with values from UI controls
            _appSettings.DarkModeEnabled = DarkModeCheckbox.IsChecked ?? false;
            _appSettings.TextEditor = Path.Text;

            // Save settings to the JSON file
            SaveSettings();
        }
        private void FileBrowserDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select JSON File",
                Filter = "JSON Files|*.json",
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

        }
    }
}
