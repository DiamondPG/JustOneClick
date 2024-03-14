using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Octokit;
using System.Net.Http;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Timers;
using System.IO.Compression;


namespace Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string version;
        bool versionMatches = false;
        string currentVersion;
        string appPath = Environment.ProcessPath;
        string releaseNotes = "";

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Exception during initialization: {ex}");
                // Handle the exception as needed.
            }



        }
        void Init()
        {
            string owner = "DiamondPG";
            string repo = "JustOneClick";
            DownloadProgress.Value = 0;
            appPath.Replace("updater.exe", "");
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
                    version = versionNumber;
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
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string saveFile = System.IO.Path.Combine(path + "\\DiamondPG\\version.txt");
                currentVersion = File.ReadAllText(saveFile);
            }
            catch
            {
                Trace.WriteLine("Read Failed");
            }
            if (version == currentVersion){
                string path = Directory.GetCurrentDirectory();
                
                string exe = System.IO.Path.Combine(path + "\\Just One Click.exe");
                Trace.WriteLine(exe);
                try
                {
                    Process.Start(exe);
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Error launching application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    try
                    {
                        //Process.Start("C:\\Users\\Harma\\source\\repos\\Just One Click\\Just One Click\\bin\\Release\\net6.0-windows10.0.22621.0\\Just One Click.exe");
                    }
                    catch
                    {
                        MessageBox.Show($"Error launching application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                Close();
            } 
            else
            {
                txtbox.Text = $"Update Available. Current Version: {currentVersion}, New Version: {version}";

            }
             

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string GetDownloadFolderPath()
                {
                    return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
                }
                var client = new GitHubClient(new Octokit.ProductHeaderValue("JustOneClick"));
                string accessToken = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DiamondPG/.env");
                client.Credentials = new Credentials(accessToken);
                var releases = client.Repository.Release.GetAll("DiamondPG", "JustOneClick").Result;
                string fileNameToDownload = "setup.zip";
                string filePathToDownload = GetDownloadFolderPath() + "/setup.zip";
                if (releases.Count > 0)
                {
                    var latestRelease = releases[0];
                    var setupAsset = latestRelease.Assets.FirstOrDefault(asset => asset.Name == fileNameToDownload);

                    if (setupAsset != null)
                    {
                        // Download the asset with progress
                        DownloadAssetWithProgress(setupAsset.BrowserDownloadUrl, filePathToDownload, "setup.exe");
                        Trace.WriteLine($"Downloaded {fileNameToDownload} from the latest release.");
                    }
                    else
                    {
                        Trace.WriteLine($"No asset with the name {fileNameToDownload} found in the latest release.");
                    }
                }
                else
                {
                    Trace.WriteLine("No releases found for the repository.");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error launching application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ReleaseNotesBTN_Click(object sender, RoutedEventArgs e)
        {
            ShowCustomMessageBox(releaseNotes);
        }
        private void ShowCustomMessageBox(string markdown)
        {
            MarkdownDialog dialog = new MarkdownDialog();
            dialog.SetMarkdownContent(markdown);
            dialog.ShowDialog();
        }
        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory();

            string exe = System.IO.Path.Combine(path + "\\Just One Click.exe");
            try
            {
                Process.Start(exe);
            }
            catch (Exception ex)
            {
                try
                {
                    //Process.Start("C:\\Users\\Harma\\source\\repos\\Just One Click\\Just One Click\\bin\\Release\\net6.0-windows10.0.22621.0\\Just One Click.exe");
                }
                catch
                {
                    MessageBox.Show($"Error launching application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            Close();
        }

        async void DownloadAssetWithProgress(string downloadUrl, string zipFileName, string setupFileName)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    using (var fileStream = File.Create(zipFileName))
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            var totalSize = response.Content.Headers.ContentLength ?? -1L;
                            var buffer = new byte[8192];
                            var bytesRead = 0L;

                            while (true)
                            {
                                var read = await stream.ReadAsync(buffer, 0, buffer.Length);

                                if (read <= 0)
                                    break;

                                fileStream.Write(buffer, 0, read);
                                bytesRead += read;

                                // Report progress
                                var progressPercentage = (int)((double)bytesRead / totalSize * 100);

                                // Update the ProgressBar
                                DownloadProgress.Dispatcher.Invoke(() =>
                                {
                                    DownloadProgress.Value = progressPercentage;
                                });
                            }
                        }
                    }
                }
            }

            // Extract the contents of the ZIP file
            try
            {
                string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                downloadsPath = System.IO.Path.Combine(downloadsPath, "Downloads");
                string extractPath = downloadsPath;
                ZipFile.ExtractToDirectory(zipFileName, extractPath);

                // Run the setup.exe file
                string setupExePath = System.IO.Path.Combine(extractPath, setupFileName);

                if (File.Exists(setupExePath))
                {
                    // Run the setup.exe file
                    Process.Start(setupExePath);
                    Close();
                }
                else
                {
                    MessageBox.Show($"Error: {setupFileName} not found in the extracted folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during extraction or execution: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                File.Delete(zipFileName);
                Trace.WriteLine($"Deleted {zipFileName} after extraction.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error deleting {zipFileName}: {ex.Message}");
            }
        }
    }
    
}
