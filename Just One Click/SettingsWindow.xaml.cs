using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using static Google.Rpc.Context.AttributeContext.Types;
using main = Just_One_Click.MainWindow;
using auth = Just_One_Click.Auth;
using Google.Apis.Download;
using Google.Protobuf.Compiler;
using Octokit;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Just_One_Click
{
    public class Settings
    {
        public bool DarkModeEnabled { get; set; }
        public bool DeleteConfirmation { get; set; }
        public bool VersionInfo { get; set; }
        public bool isFirstBoot { get; set; }
        public bool LaunchAtStartup { get; set; }
        public bool deauth { get; set; }
        public bool isOffline { get; set; }
    }

    public partial class SettingsWindow : Window
    {
        string SettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string releaseNotes = "";

        private Settings _appSettings;

        public SettingsWindow()
        {
            InitializeComponent();
            SettingsFilePath = SettingsFilePath + "\\Just One Click\\appsettings.json";
            Trace.WriteLine(SettingsFilePath);
            
            // Load settings when the window is initialized
            LoadSettings();
            DataContext = _appSettings;
            UpdateUI();
            HideSpinner();
        }
        private void ShowSpinner()
        {
            Spinner.Visibility = System.Windows.Visibility.Visible;
            Spinner.StartSpin();
        }

        private void HideSpinner()
        {
            Spinner.StopSpin();
            Spinner.Visibility = System.Windows.Visibility.Hidden;
        }
        private void CheckForUpdates()
        {
            
            string owner = "DiamondPG";
            string repo = "JustOneClick";
            
            
            string accessToken = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DiamondPG/.env");
            Trace.WriteLine(accessToken);
            var client = new GitHubClient(new Octokit.ProductHeaderValue("JustOneClick"));
            client.Credentials = new Credentials(accessToken);

            try
            {
                var releases = client.Repository.Release.GetAll(owner, repo).Result;

                if (releases.Count > 0)
                {
                    var latestRelease = releases[0]; // Assuming releases are sorted by date, so the first release is the latest.
                    Trace.WriteLine($"Latest release name: {latestRelease.Name}");
                    releaseNotes = latestRelease.Body;
                    string versionNumber = latestRelease.Name.Replace("v", "");
                    
                }
                else
                {
                    Trace.WriteLine("No releases found for the repository.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error retrieving releases: {ex.Message}");
            }
            
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    _appSettings = JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                WritePlaceholderJson(SettingsFilePath);
            }

            // If settings couldn't be loaded, initialize with default values
            
        }

        


        private void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_appSettings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
                Trace.WriteLine(json);
                if(_appSettings.LaunchAtStartup == true)
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "JustOneClick", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\DiamondPG\\JustOneClick\\Updater.exe");
                    Trace.WriteLine("Key creation attempted");
                }
                else
                {
                    try
                    {
                        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                            key.DeleteValue("JustOneClick");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ReleaseNotesBTN_Click(object sender, RoutedEventArgs e)
        {
            ShowSpinner();

            // Start the asynchronous task
            await Task.Run(async () =>
            {
                CheckForUpdates();
                ShowCustomMessageBox(releaseNotes);
            });

            // Task is completed, stop the spinner
            Dispatcher.Invoke(() =>
            {
                HideSpinner();
            });
        }
        private void ShowCustomMessageBox(string markdown)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                MarkdownDialog dialog = new MarkdownDialog();
                dialog.SetMarkdownContent(markdown);
                dialog.ShowDialog();
            });
        }


        private void UpdateUI()
        {
            if (_appSettings != null)
            {
                // Update your UI controls with the loaded settings
                DarkModeCheckbox.IsChecked = _appSettings.DarkModeEnabled;
                VersionCheckbox.IsChecked = _appSettings.VersionInfo;
                ConfirmationCheckbox.IsChecked = _appSettings.DeleteConfirmation;
                LaunchAtStartupCheck.IsChecked = _appSettings.LaunchAtStartup;
            }
            else
            {
                // Handle the case where _appSettings is null
                // You can provide default values or take appropriate action
                DarkModeCheckbox.IsChecked = false;
                ConfirmationCheckbox.IsChecked = false;
                _appSettings.LaunchAtStartup = false;

                MessageBox.Show("Failed to save settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_appSettings != null)
            {
                // Update settings with values from UI controls
                _appSettings.DarkModeEnabled = DarkModeCheckbox.IsChecked ?? false;
                
                
                

                // Save settings to the JSON file
                SaveSettings();
                string json = JsonConvert.SerializeObject( _appSettings, Formatting.Indented );
                Trace.WriteLine(json);
                
            }
            else
            {
                // Handle the case where _appSettings is null
                // You might want to initialize it or take appropriate action
                WritePlaceholderJson(SettingsFilePath);
                MessageBox.Show("Failed to save settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //_appSettings = new Settings { DarkModeEnabled = false, TextEditor = "Notepad" };
            }

            // Close the window or perform other actions as needed
            Close();
        }
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

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            Just_One_Click.Credits credits = new Just_One_Click.Credits();
            credits.Show();
        }

        private void DeAuth_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("Are you sure you want to de-auth your account? This will CLOSE the program and REMOVE your activation key.","Confirmation",MessageBoxButton.YesNoCancel,MessageBoxImage.Stop);
            if(message == MessageBoxResult.Yes)
            {
                auth.SetRegister(@"HKEY_CURRENT_USER\Software\DiamondPG\JustOneClick", "Key", "");
                _appSettings.deauth = true;
                SaveSettings();
                Close();
            }
        }
    }
}
