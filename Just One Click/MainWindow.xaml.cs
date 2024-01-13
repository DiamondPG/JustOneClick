using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Printing;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using static Just_One_Click.MainWindow;
using System.Net.NetworkInformation;

namespace Just_One_Click
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    [SuppressUnmanagedCodeSecurity]


    public partial class MainWindow : Window
    {
        string log = "";
        void Write(string str)
        {

            log += str + "\n";
            Console.Text = log;
        }
        void Log(string str)
        {
            Trace.WriteLine(str);

        }
        int delay = 5;
        ImageSource FaviconSource;
        
        public MainWindow()
        {
            InitializeComponent();
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Checks Documents Folder for path
            savePath = System.IO.Path.Combine(savePath + "/Just One Click/");
            string saveFile = System.IO.Path.Combine(savePath + "savedata.json");
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            string path = Directory.GetCurrentDirectory() + "/data.json";
            string spath = Directory.GetCurrentDirectory() + "/sdata.json";

            var test = new                              //Test JSON string in indented form, 1 command until line 93
            {
                Name = "Test",
                ID = "1",
                Applications = new[] {

                        new {
                            name = "X-Plane 12",
                            path = "/hi",
                            favicon = "/d/dd/",
                            ID = "1"
                        },

                        new
                        {
                            name = "Other X-Plane",
                            path = "/bye",
                            favicon = "not used",
                            ID = "2"
                        }


                    }


            };
            string SerializedJSON = "";
            string ReserializedJSON = "";
            var DeserializedJSON = new List<Profile>();

            void TestJSONWrite()
            {
                Profile profile = new Profile();            //unnecesssary?
                profile.Name = "Test";                      //.
                profile.ID = "1";                           //..

                var ReadJSON = "";
                int testExitCode = 0;

                try
                {
                    SerializedJSON = JsonConvert.SerializeObject(test, Formatting.Indented);
                }
                catch
                {
                    Log("Error: C0101. Serialization Failure");
                    testExitCode = 1;
                }
                try
                {

                }
                catch
                {
                    Write("Error C0102. Data Write Failed. Make sure this app has access to the Documents folder");
                    testExitCode = 2;
                }

                try
                {
                    ReadJSON = File.ReadAllText(saveFile);
                    //Reading File - Error here
                }
                catch
                {
                    Log("Error C0103. File Read Error.");
                    testExitCode = 3;
                }
                try
                {
                    DeserializedJSON = JsonConvert.DeserializeObject<List<Profile>>(ReadJSON);
                }
                catch
                {
                    Write("Error C0104. Deserialization Failure");

                    testExitCode = 4;
                }
                try
                {
                    ReserializedJSON = JsonConvert.SerializeObject(DeserializedJSON, Formatting.Indented);
                }
                catch
                {
                    Write("Error C0105. Reserialization Failure");
                    testExitCode = 5;
                }
                if (ReserializedJSON == SerializedJSON)
                {
                    Write($"Tests Succeeded (Exit Code {testExitCode})");
                }
                else
                {
                    Write("");
                    Log(SerializedJSON);
                    Log("Reserialized:");
                    Log(ReserializedJSON);
                }
            }





            TestJSONWrite();
            Initialize();
        }




        private async void LaunchBTN_Click(object sender, RoutedEventArgs e)
        {
            Log("Launch Clicked");

            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                this.WindowState = WindowState.Minimized;
                foreach (Apps selectedApp in selectedProfile.Applications)
                {
                    try
                    {
                        // Start the selected application
                        System.Diagnostics.Process.Start(selectedApp.Path);
                        Log($"Launching: {selectedApp.Path}");

                        // Introduce a delay between launches (adjust the delay time as needed)
                        await Task.Delay(TimeSpan.FromSeconds(delay)); // 5 seconds delay
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during the process start
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a profile to launch applications.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        public void Initialize()
        {

            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Checks Documents Folder for path
            savePath = System.IO.Path.Combine(savePath + "/Just One Click/");
            string saveFile = System.IO.Path.Combine(savePath + "savedata.json");
            
            if (!File.Exists(saveFile) || string.IsNullOrEmpty(File.ReadAllText(saveFile)))
            {
                // If the file doesn't exist or is empty, write a placeholder JSON file
                
                MessageBoxResult result = MessageBox.Show("Failed to Read Data. Write Placeholder JSON?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    WritePlaceholderJson(saveFile);
                }
            }
            Apps apps = new Apps();
            if (File.Exists(saveFile))
            {
                string json = "";
                var djson = new List<Profile>();
                try
                {

                    Clipboard.SetText(saveFile);
                    json = File.ReadAllText(saveFile);

                    djson = JsonConvert.DeserializeObject<List<Profile>>(json);

                }
                catch
                {
                    Write("Error Code A0101. JSON deserialization failure");
                    MessageBox.Show("Error Code A0101: JSON deserialization failure", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Profile profile = new Profile();


                string ajson = JsonConvert.SerializeObject(djson, Formatting.Indented);
                try
                {
                    Profiles.DataContext = djson;
                    AppsLB.DataContext = djson;
                    AppsLB.ItemsSource = djson;
                    Log($"AJSON: {ajson}");
                    Log("Data Found");
                    AppsLB.Items.Refresh();
                    Profiles.Items.Refresh();
                    AppsLB.Items.Refresh();

                }
                catch
                {
                    Write("Error A0401. Error Setting Data Source");
                    MessageBox.Show("Error Code A0401: Error Setting Data Source", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Write("Data Not Found");
                MessageBox.Show("Error Code A0301: Save File Not Found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            //Clipboard.SetText(log);

            void CreateFile(string serOBJ, string path)
            {
                string saveFile = System.IO.Path.Combine(path + "savedata.json");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Write("Directory not found, New directory created at:" + path);
                }
                StreamWriter writer = new StreamWriter(saveFile);
                writer.Write(serOBJ);
            }
            AppsLB.ItemsSource = null;
            AppsLB.Items.Refresh();
        }


        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();



        }
        private void Profiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                Log($"Selected Profile: {selectedProfile.Name}");

                if (selectedProfile.Applications != null)
                {
                    Log($"Number of Applications: {selectedProfile.Applications.Count}");

                    // Set the ItemsSource directly to the AppsLB ListBox
                    AppsLB.ItemsSource = selectedProfile.Applications;
                    delay = selectedProfile.Delay;
                }
                else
                {
                    Log("Applications list is null.");
                    // Clear the ListBox
                    AppsLB.ItemsSource = null;
                }
                foreach (var item in AppsLB.Items)
                {
                    // Print debug information for each item in AppsLB
                    Log($"AppsLB Item: {(item as dynamic).Name}");
                }
            }
            else
            {
                Log("Selected Profile is null.");
                // Clear the ListBox
                AppsLB.ItemsSource = null;
            }
        }

        private void ContextMenu_DeleteClick(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                if (AppsLB.SelectedItem is Apps selectedApp)
                {
                    //if () {
                        // Confirm with the user before deleting the app
                        MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the application '{selectedApp.Name}' from the profile '{selectedProfile.Name}'?",
                                                                  "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Remove the selected app from the profile's Applications list
                            selectedProfile.Applications.Remove(selectedApp);

                            // Update the ListBox's ItemsSource to reflect the changes
                            AppsLB.ItemsSource = selectedProfile.Applications;

                            // Save the updated data to the JSON file (you need to implement the save logic)
                            SaveDataToJson();
                            AppsLB.Items.Refresh();
                        }
                    //}
                }
            }
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                // Create a new App instance (customize this based on your data structure)
                Apps newApp = new Apps
                {
                    ID = "new",
                    Name = "New App",
                    Path = "C:\\Program Files\\NewApp.exe",
                    Favicon = null
                };

                // Add the new app to the profile's Applications list
                selectedProfile.Applications.Add(newApp);

                // Update the ListBox's ItemsSource to reflect the changes
                AppsLB.ItemsSource = selectedProfile.Applications;

                // Save the updated data to the JSON file
                SaveDataToJson();
                AppsLB.Items.Refresh();
            }
        }
        private void AddProfile_Click(object sender, RoutedEventArgs e)
        {
            string newProfileName = "New Profile";

            // Create a new profile
            Profile newProfile = new Profile
            {
                ID = Guid.NewGuid().ToString(),
                Name = newProfileName,
                Delay = 5, // You can set default values here
                Applications = new List<Apps>() // Initialize the Applications list
            };

            // Add the new profile to the existing data
            if (Profiles.ItemsSource is List<Profile> profileList)
            {
                profileList.Add(newProfile);

                // Refresh the ListBox's ItemsSource to reflect the changes
                Profiles.ItemsSource = profileList;

                // Save the updated data to the JSON file
                SaveDataToJson();
                Profiles.Items.Refresh();
            }
        }

        private void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                // Confirm with the user before deleting the profile
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the profile '{selectedProfile.Name}'?",
                                                          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Remove the selected profile from the existing data
                    if (Profiles.ItemsSource is List<Profile> profileList)
                    {
                        profileList.Remove(selectedProfile);

                        // Refresh the ListBox's ItemsSource to reflect the changes
                        Profiles.ItemsSource = profileList;

                        // Save the updated data to the JSON file
                        SaveDataToJson();
                        Profiles.Items.Refresh();
                    }
                }
            }
        }
    
        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                if (AppsLB.SelectedItem is Apps selectedApp)
                {
                    // Prompt the user for a new name (you can use an input dialog or another method)
                    string newName = PromptForNewName(selectedApp.Name);

                    // Update the selected app's name
                    selectedApp.Name = newName;

                    // Update the ListBox's ItemsSource to reflect the changes
                    AppsLB.ItemsSource = selectedProfile.Applications;

                    // Save the updated data to the JSON file
                    SaveDataToJson();
                    AppsLB.Items.Refresh();
                }
            }
        }

        private string PromptForNewName(string currentName)
        {
            // You can implement a dialog or use an input box to prompt the user for a new name
            // For simplicity, this example uses MessageBox, but you may want to implement a custom dialog.
            string newName = currentName; // Default to the current name if the user cancels

            InputBox inputBox = new InputBox("Enter a new name:", "Rename", currentName);
            if (inputBox.ShowDialog() == true)
            {
                // User clicked OK, update the newName
                newName = inputBox.ResponseText;
            }

            return newName;
        }
        private string PromptForFaviconPath()
        {
            InputBox inputBox = new InputBox("Enter Path to PNG image", "Change Favicon Path");
            return inputBox.ResponseText;
        }

        private string BrowseForFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
        private void ChangePathMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Apps app = new Apps();
            

                if (AppsLB.SelectedItem is Apps selectedApp)
                {
                    string newPath = BrowseForFile();
                    if (newPath != null)
                    {
                        selectedApp.Path = newPath;
                        // Refresh the ListBox to reflect the changes
                        AppsLB.Items.Refresh();
                        
                        string newFaviconPath = ExtractFavicon(selectedApp.Path);
                        SaveDataToJson();
                    }
                }
            
           
            

        }

        private void RenameProfile_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                
                    // Prompt the user for a new name (you can use an input dialog or another method)
                string newName = PromptForNewName(selectedProfile.Name);

                // Update the selected app's name
                selectedProfile.Name = newName;

                Profiles.Items.Refresh();

                    // Save the updated data to the JSON file
                    SaveDataToJson();
            }
            
        }


        public void WritePlaceholderJson(string filePath)
        {
            var placeholderData = new List<Profile>
    {
        new Profile
        {
            ID = "PlaceholderProfile",
            Name = "Placeholder Profile",
            Delay = 5,
            Applications = new List<Apps>
            {
                new Apps
                {
                    ID = "PlaceholderApp",
                    Name = "Placeholder",
                    Path = "C:\\Path\\To\\Application.exe", // Provide a default path
                    Favicon = null,
                    isBrowserSource = false,
                }
            }
        }
    };

            try
            {
                // Serialize and write the placeholder data to the file
                string placeholderJson = JsonConvert.SerializeObject(placeholderData, Formatting.Indented);
                File.WriteAllText(filePath, placeholderJson);
                Log("Placeholder JSON file written.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file write
                Log($"Error writing placeholder JSON file: {ex.Message}");
            }
        }
        private void SaveDataToJson()
        {
            try
            {
                string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                savePath = System.IO.Path.Combine(savePath, "Just One Click/");
                string saveFile = System.IO.Path.Combine(savePath, "savedata.json");

                string serializedData = JsonConvert.SerializeObject(Profiles.ItemsSource, Formatting.Indented);
                File.WriteAllText(saveFile, serializedData);

                Write("Data saved successfully.");
                Log($"Path: {saveFile}");
            }
            catch (Exception ex)
            {
                Write($"Error saving data: {ex.Message}");
            }
        }
        private void ChangeFavicon_Click(object sender, RoutedEventArgs e)
        {
            if (AppsLB.SelectedItem is Apps selectedApp)
            {
                // Show a file picker dialog for selecting the PNG file
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // Set the Favicon property to the selected file path
                    selectedApp.Favicon = openFileDialog.FileName;

                    // Refresh the ListBox to reflect the changes
                    AppsLB.Items.Refresh();

                    // Save the updated data to the JSON file
                    SaveDataToJson();
                }
            }
        }


        BitmapSource bitmapSource;
        private void ResetFavicon_Click(object sender, RoutedEventArgs e)
        {
            if (AppsLB.SelectedItem is Apps selectedApp)
            {
                string newFaviconPath = ExtractFavicon(selectedApp.Path); // UNUSED FUNCTION

                if (newFaviconPath != null)
                {
                    
                    // Refresh the ListBox to reflect the changes
                    AppsLB.Items.Refresh();
                }
            }
        }

        private string ExtractFavicon(string exeFilePath)
        {
            try
            {
                Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(exeFilePath);
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHIcon(
                    appIcon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                // Save the extracted icon as an image file (e.g., PNG) in the temp folder
                string tempFolderPath = System.IO.Path.GetTempPath();
                string tempIconPath = System.IO.Path.Combine(tempFolderPath, "ExtractedFavicon.png");
                Apps app = new Apps();
                
                AppsLB.Items.Refresh();
                using (FileStream stream = new FileStream(tempIconPath, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                }

                return tempIconPath;
            }
            catch (Exception ex)
            {
                Write($"Error extracting favicon: {ex.Message}");
                return null;
            }
        }


        
        private void LaunchBrowser(string url)
        {
            try
            {
                // Use the Process.Start method to launch the default web browser
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process start
                MessageBox.Show($"Error launching browser: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Duplicate_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                if (AppsLB.SelectedItem is Apps selectedApp)
                {
                    // Create a new app by cloning the selected app
                    Apps newApp = new Apps
                    {
                        ID = Guid.NewGuid().ToString(), // Generate a new ID for the duplicated app
                        Name = selectedApp.Name,
                        Path = selectedApp.Path,
                        Favicon = selectedApp.Favicon
                    };

                    // Add the new app to the profile's Applications list
                    selectedProfile.Applications.Add(newApp);

                    // Update the ListBox's ItemsSource to reflect the changes
                    AppsLB.ItemsSource = selectedProfile.Applications;

                    // Save the updated data to the JSON file
                    SaveDataToJson();
                    AppsLB.Items.Refresh();
                }
            }
        }

        private void AddBrowserSource(object sender, RoutedEventArgs e)
        {
            // Prompt the user for a new name and URL (you can use an input dialog or another method)
            string newName = PromptForNewBSName("Enter a name for the browser source:");
            string newUrl = PromptForNewUrl("Enter the URL for the browser source:");

            // Create a new Apps object for the browser source
            Apps newBrowserSource = new Apps
            {
                ID = Guid.NewGuid().ToString(),
                Name = newName,
                Path = newUrl,
                Favicon = null
            };

            // Add the new browser source to the selected profile's Applications list
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                selectedProfile.Applications.Add(newBrowserSource);

                // Update the ListBox's ItemsSource to reflect the changes
                AppsLB.ItemsSource = selectedProfile.Applications;

                // Save the updated data to the JSON file
                SaveDataToJson();
                AppsLB.Items.Refresh();
            }
        }

        private string PromptForNewBSName(string prompt)
        {
            // You can implement a dialog or use an input box to prompt the user for a new name
            // For simplicity, this example uses MessageBox, but you may want to implement a custom dialog.
            string newName = "New Browser Source"; // Default to a generic name if the user cancels

            InputBox inputBox = new InputBox(prompt, "Add Browser Source", "");
            if (inputBox.ShowDialog() == true)
            {
                // User clicked OK, update the newName
                newName = inputBox.ResponseText;
            }

            return newName;
        }

        private string PromptForNewUrl(string prompt)
        {
            // Similar to PromptForNewName, prompt the user for a new URL
            // Implement your own logic to handle URL input, possibly using a custom dialog or input box.
            // For simplicity, this example uses MessageBox, but it's not suitable for entering URLs.
            string newUrl = "https://www.example.com"; // Default to a placeholder URL

            InputBox inputBox = new InputBox(prompt, "Add URL", "");
            if (inputBox.ShowDialog() == true)
            {
                // User clicked OK, update the newUrl
                newUrl = inputBox.ResponseText;
            }
            

            return newUrl;
        }
        
        

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            AppsLB.Items.Refresh();
            Profiles.Items.Refresh();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Initialize();
        }
        public class Profile
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int Delay { get; set; }
            public List<Apps> Applications { get; set; }
        }

        public class Apps
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Favicon { get; set; }
            public bool isBrowserSource { get; set; }



        }
    }
}