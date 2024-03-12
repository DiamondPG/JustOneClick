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
using Windows.UI.Notifications;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Net.NetworkInformation;
using System.Security.Policy;
using ABI.System;
using Octokit;
using System.Net.Http;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase;
using auth = Just_One_Click.Auth;
using System.Buffers.Text;
using Google.Api;
using Windows.UI.Notifications;
using System.Windows.Threading;
using Microsoft.Toolkit.Uwp.Notifications;

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
        bool Authenticated = false;
        Settings dSettings = new Settings();
        int currentRelease = 2; // 3 For Alpha, 2 For Beta, 1 For Stable, 4 For Dev. Auth only works on 3.
        bool scheduled = false;
        public MainWindow()
        {
            InitializeComponent();
            //AuthenticateFirebase();
            
            

            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Checks Documents Folder for path
            savePath = System.IO.Path.Combine(savePath + "/Just One Click/");
            string saveFile = System.IO.Path.Combine(savePath + "savedata.json");
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical; //no clue what this does, but it fixed a bug so, let's keep it.
            Activated += MainWindow_Activated;

            string SerializedJSON = "";
            string ReserializedJSON = "";
            var DeserializedJSON = new List<Profile>();

            
            Initialize();
            
        }
        
        private async Task AuthenticatePaste(string key)
        {
            try
            {
                
                
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DiamondPG\\f.env";
                if(key.Length > 15)
                {
                    string apiKey = "";
                    if (File.Exists(path))
                    {
                        apiKey = File.ReadAllText(path);
                    }
                    else
                    {
                        Trace.WriteLine("Failed API");
                    }

                    if (!string.IsNullOrEmpty(apiKey))
                    {

                        string pasteCode = "vdZhbcKf";  // Replace with your actual paste content
                        string pasteName = "JOC keys";

                        using (HttpClient client = new HttpClient())
                        {
                            string apiEndpoint = "https://pastebin.com/api/api_post.php";

                            // Set your parameters
                            var parameters = new
                            {
                                api_dev_key = apiKey,
                                api_paste_code = pasteCode,
                                api_paste_private = "2"  // 2 means it's a private paste   
                            };

                            // Make the API request
                            HttpResponseMessage response = await client.PostAsJsonAsync(apiEndpoint, parameters);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                Trace.WriteLine(responseBody);
                                if (responseBody.Contains(key))
                                {
                                    Trace.WriteLine("Authed");
                                    Authenticated = true;
                                }
                                else
                                {
                                    Close();
                                }
                            }
                            else
                            {
                                Trace.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");

                            }
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Fail");
                    }
                }
                else
                {
                    Trace.WriteLine("20");
                }
                
                
            }
            catch
            {
                Trace.WriteLine("Catch");
            }
            
        }

        private void AuthorizePartTreaux()
        {
            string fullRegistryPath = @"HKEY_CURRENT_USER\Software\DiamondPG\JustOneClick";
            string registryValueName = "Key";

            string key = auth.GetRegister(fullRegistryPath, registryValueName);

            if (key != null && auth.IsLicensed(key, "Just_One_Click.keys.txt"))
            {
                auth._isLicensed = true;
                Authenticated = true;
            }
            else
            {
                auth._isLicensed = false;
                if (currentRelease == 3)
                {
                    InputBox input = new InputBox("Enter Product Key", "Activation");
                    input.ShowDialog();

                    if (auth.IsLicensed(input.ResponseText, "Just_One_Click.keys.txt"))
                    {
                        auth.SetRegister(@"HKEY_CURRENT_USER\Software\DiamondPG\JustOneClick", "Key", input.ResponseText);

                        auth._isLicensed = true;
                        MessageBox.Show("Successful registration.");
                        Authenticated = true;
                    }
                    else
                    {
                        MessageBox.Show("The key is not licensed.");
                    }

                }
                else
                {
                    auth._isLicensed = true;
                    Authenticated = true;
                }
            }   
        }


        private async void LaunchBTN_Click(object sender, RoutedEventArgs e)
        {
            if (scheduled == false)
            {
                Log("Launch Clicked");

                if (Profiles.SelectedItem is Profile selectedProfile)
                {
                    this.WindowState = WindowState.Minimized;
                    foreach (Apps selectedApp in selectedProfile.Applications)
                    {
                        try
                        {
                            if (selectedApp.isBrowserSource == true)
                            {
                                string path = "";
                                if (!selectedApp.Path.StartsWith("http://") && !selectedApp.Path.StartsWith("https://"))
                                {
                                    // If the URL doesn't start with "http://" or "https://", prepend "https://"
                                    path = "https://" + selectedApp.Path;
                                }
                                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                            }
                            else if (selectedApp.isTextSource == true)
                            {
                                try
                                {
                                    // Ensure the file exists before attempting to open it
                                    if (File.Exists(selectedApp.Path))
                                    {
                                        Process.Start(new ProcessStartInfo
                                        {
                                            FileName = selectedApp.Path,
                                            UseShellExecute = true
                                        });
                                    }
                                    else
                                    {
                                        MessageBox.Show($"File not found: {selectedApp.Path}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    this.WindowState = WindowState.Normal;
                                    MessageBox.Show($"Error opening text file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                // Start the selected application
                                System.Diagnostics.Process.Start(selectedApp.Path);
                                Log($"Launching: {selectedApp.Path}");
                                Profile profile = new Profile();
                                if (selectedApp.Delay >= 1)
                                {
                                    await Task.Delay(System.TimeSpan.FromSeconds(selectedApp.Delay));
                                }
                                else if (profile.GlobalDelay >= 1)
                                {
                                    await Task.Delay(System.TimeSpan.FromSeconds(profile.GlobalDelay));
                                }
                                else
                                {
                                    await Task.Delay(System.TimeSpan.FromSeconds(delay));
                                }

                            }
                        }
                        catch (System.Exception ex)
                        {
                            this.WindowState = WindowState.Normal;
                            // Handle any exceptions that may occur during the process start
                            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    MessageBox.Show("Please select a profile to launch applications.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        
        public async void Initialize()
        {

            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Checks Documents Folder for path
            savePath = System.IO.Path.Combine(savePath + "/Just One Click/");
            string saveFile = System.IO.Path.Combine(savePath + "savedata.json");
            string settingsFile = System.IO.Path.Combine(savePath + "appsettings.json");
            string readjson = File.ReadAllText(settingsFile);
            
            AuthorizePartTreaux();

            if(Authenticated == false && currentRelease == 3)
            {
                MessageBox.Show("Auth Failed. To Continue Please Enter a Valid License Key", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            Settings dSettings = new Settings(); // Initialize the object
            Write($"isFirstBoot: {dSettings.IsFirstBoot}"); // Log the initial value
                                                            // Read the settings file and deserialize it
            Write($"Read JSON from file: {readjson}"); // Log the JSON read from the file

            dSettings = JsonConvert.DeserializeObject<Settings>(readjson);
            Write($"isFirstBoot after deserialization: {dSettings?.IsFirstBoot}"); // Log the value after deserialization

            // Check if the settings file exists
            if (!File.Exists(settingsFile))
            {
                Write("Settings file does not exist");
                // Add any other relevant logs or debugging information
                // ...

                // Create a default Settings object with placeholder values
                Settings placeholderSettings = new Settings
                {
                    DarkModeEnabled = true,
                    DeleteConfirmation = true,
                    VersionInfo = true,
                    IsFirstBoot = false  // Set isFirstBoot to false
                };

                // Serialize the Settings object to JSON
                string json = JsonConvert.SerializeObject(placeholderSettings, Formatting.Indented);
                Write($"Serialized JSON: {json}"); // Log the serialized JSON

                try
                {
                    // Write the JSON to the specified file
                    File.WriteAllText(settingsFile, json);
                    Write("Settings file created successfully");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"An error occurred when creating your settings file for the first time: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
            if (!File.Exists(settingsFile) || string.IsNullOrEmpty(File.ReadAllText(settingsFile)))
            {
                MessageBox.Show("Settings Failed to Load, File does not exist or is empty","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            if(File.Exists(settingsFile))
            {
                string json = File.ReadAllText(settingsFile);
                dSettings = JsonConvert.DeserializeObject<Settings>(json);
            }
            if (!File.Exists(saveFile) || string.IsNullOrEmpty(File.ReadAllText(saveFile)))
            {
                // If the file doesn't exist or is empty, write a placeholder JSON file
                
                MessageBoxResult result = MessageBox.Show("Failed to Read Data. Write Placeholder JSON?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    WritePlaceholderJson(saveFile);
                }
                else {
                    Log("Failed to write Placeholder JSON");
                }
            }
            Apps apps = new Apps();
            if (File.Exists(saveFile))
            {
                string json = "";
                var djson = new List<Profile>();
                try
                {

                    json = File.ReadAllText(saveFile);

                    djson = JsonConvert.DeserializeObject<List<Profile>>(json);

                }
                catch
                {
                    Write("Error Code A0101. JSON deserialization failure");
                    MessageBox.Show("Error Code A0101: JSON deserialization failure", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Profile profile = new Profile();
                foreach (Profile profile1 in djson)
                {
                    profile1.Num = profile1.Applications.Count;
                    Profiles.Items.Refresh();
                }

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

            DispatcherTimer pulse = new DispatcherTimer();
            pulse.Interval = System.TimeSpan.FromMilliseconds(1000);
            
            pulse.Start();

        }
        
        private void ChangeGlobalDelay_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedprofile)
            {
                InputBox inputbox = new InputBox("Enter New Global Launch Delay", "Change Global Delay", selectedprofile.GlobalDelay.ToString());
                if (inputbox.ShowDialog() == true)
                {
                    try
                    {
                        int newdelay = Convert.ToInt32(inputbox.ResponseText);
                        if (newdelay <= 0)
                        {
                            MessageBox.Show("Please enter a positive number greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else if (newdelay > 0 && newdelay <= 120)
                        {
                            selectedprofile.GlobalDelay = newdelay;
                            SaveDataToJson();
                            Profiles.Items.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Please enter a value less than 120", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Please enter a valid integer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void RemoveDelay_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedprofile)
            {
                if (AppsLB.SelectedItem is Apps selectedapp)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to remove all launch delays from each app? Global Launch Delay will take effect.", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Hand);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (Apps selectedApp in selectedprofile.Applications)
                        {
                            selectedApp.Delay = 0;
                            SaveDataToJson();
                            AppsLB.Items.Refresh();
                        }
                    }
                }
            }
        }
        private void ChangeDelay_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles.SelectedItem is Profile selectedprofile)
            {
                if (AppsLB.SelectedItem is Apps selectedapp)
                {
                    InputBox inputbox = new InputBox("Enter a new launch delay in seconds", "Change Launch Delay", selectedapp.Delay.ToString());
                    if (inputbox.ShowDialog() == true)
                    {
                        
                        try
                        {
                            int newdelay = Convert.ToInt32(inputbox.ResponseText);
                            if (newdelay <= 0)
                            {
                                MessageBox.Show("Please enter a positive number greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (newdelay > 0 && newdelay <= 120)
                            {
                                selectedapp.Delay = newdelay;
                                SaveDataToJson();
                                AppsLB.Items.Refresh();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Please enter a valid integer","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                    }
                }
            }
            
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
                    delay = selectedProfile.GlobalDelay;
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

        private void ContextMenu_DeleteClick(object sender, RoutedEventArgs e) //Deletes Apps
        {
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                if (AppsLB.SelectedItem is Apps selectedApp)
                {
                    
                     if(dSettings.DeleteConfirmation == true) {                         // Confirm with the user before deleting the app
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
                    }
                    else if (dSettings.DeleteConfirmation == false)
                    {
                        selectedProfile.Applications.Remove(selectedApp);

                        // Update the ListBox's ItemsSource to reflect the changes
                        AppsLB.ItemsSource = selectedProfile.Applications;

                        // Save the updated data to the JSON file (you need to implement the save logic)
                        SaveDataToJson();
                        AppsLB.Items.Refresh();
                    }
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
                    Delay = 5,
                    Favicon = null,
                    isBrowserSource = false
                };

                // Add the new app to the profile's Applications list
                selectedProfile.Applications.Add(newApp);

                // Update the ListBox's ItemsSource to reflect the changes
                AppsLB.ItemsSource = selectedProfile.Applications;

                // Save the updated data to the JSON file
                SaveDataToJson();
                AppsLB.Items.Refresh();
                selectedProfile.Num = selectedProfile.Applications.Count;
                Profiles.Items.Refresh();
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
                GlobalDelay = 5, // You can set default values here
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
                Settings settings = new Settings();
                if (settings.DeleteConfirmation == true)
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
                            selectedProfile.Num = selectedProfile.Applications.Count;
                            Profiles.Items.Refresh();
                        }
                    }
                } 
                else
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
                        selectedProfile.Num = selectedProfile.Applications.Count;
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
        private string BrowseForTextFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
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
                if (selectedApp.isBrowserSource == true)
                {
                    
                    string newPath = PromptForNewUrl("Enter new URL");
                    if (newPath != null)
                    {
                        selectedApp.Path = newPath;
                        // Refresh the ListBox to reflect the changes
                        AppsLB.Items.Refresh();

                        //string newFaviconPath = ExtractFavicon(selectedApp.Path);
                        SaveDataToJson();
                    }
                }
                else if(selectedApp.isTextSource == true)
                {
                    string newPath = BrowseForTextFile();
                    if (newPath != null)
                    {
                        selectedApp.Path = newPath;
                        // Refresh the ListBox to reflect the changes
                        AppsLB.Items.Refresh();

                        //string newFaviconPath = ExtractFavicon(selectedApp.Path);
                        SaveDataToJson();
                    }
                }
                else
                {

                    string newPath = BrowseForFile();
                    if (newPath != null)
                    {
                        selectedApp.Path = newPath;
                        // Refresh the ListBox to reflect the changes
                        AppsLB.Items.Refresh();

                        //string newFaviconPath = ExtractFavicon(selectedApp.Path);
                        SaveDataToJson();
                    }

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
            GlobalDelay = 5,
            Num = 0,
            Applications = new List<Apps>
            {
                new Apps
                {
                    ID = "PlaceholderApp",
                    Name = "Placeholder",
                    Path = "C:\\Path\\To\\Application.exe", // Provide a default path
                    Delay = 5,
                    Favicon = null,
                    isBrowserSource = false,
                    isTextSource = false,
                    isVerified = false
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
            catch (System.Exception ex)
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
                savePath = System.IO.Path.Combine(savePath, "Just One Click\\");
                string saveFile = System.IO.Path.Combine(savePath, "savedata.json");

                string serializedData = JsonConvert.SerializeObject(Profiles.ItemsSource, Formatting.Indented);
                File.WriteAllText(saveFile, serializedData);

                Write("Data saved successfully.");
                Log($"Path: {saveFile}");
            }
            catch (System.Exception ex)
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
                

                return tempIconPath;
            }
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
                        Delay = selectedApp.Delay,
                        isBrowserSource = selectedApp.isBrowserSource,
                        Favicon = selectedApp.Favicon
                    };

                    // Add the new app to the profile's Applications list
                    selectedProfile.Applications.Add(newApp);

                    // Update the ListBox's ItemsSource to reflect the changes
                    AppsLB.ItemsSource = selectedProfile.Applications;

                    // Save the updated data to the JSON file
                    SaveDataToJson();
                    AppsLB.Items.Refresh();
                    selectedProfile.Num = selectedProfile.Applications.Count;
                    Profiles.Items.Refresh();
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
                Favicon = "",
                Delay = 5,
                isBrowserSource = true,
                isTextSource = false,
                isVerified = false
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
                selectedProfile.Num = selectedProfile.Applications.Count;
                Profiles.Items.Refresh();
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
        private void AddTextSource_Click(object sender, RoutedEventArgs e)
        {
            Apps newTextSource = new Apps
            {
                ID = Guid.NewGuid().ToString(),
                Name = "New Text Source",
                Path = "Path\\To\\Text\\Document",
                Favicon = "",
                Delay = 5,
                isBrowserSource = false,
                isTextSource = true
            };
            if (Profiles.SelectedItem is Profile selectedProfile)
            {
                selectedProfile.Applications.Add(newTextSource);

                // Update the ListBox's ItemsSource to reflect the changes
                AppsLB.ItemsSource = selectedProfile.Applications;

                // Save the updated data to the JSON file
                SaveDataToJson();
                AppsLB.Items.Refresh();
                selectedProfile.Num = selectedProfile.Applications.Count;
                Profiles.Items.Refresh();
            }
        }
        private void ScheduleLaunch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTimePicker picker = new DateTimePicker();
                picker.ShowDialog();
                scheduled = true;
                // Get user input from the DateTimePicker control
                DateTime selectedTime = picker.selectedDate;
                System.TimeSpan selectedTimeSpan = picker.selectedTime;
                
                // Combine the date and time parts to get the selected future time

                string time = selectedTime.ToString();
                string subtime = time.Substring(0, time.Length - 8);
                subtime += selectedTimeSpan.ToString();
                Trace.WriteLine(subtime);
                DateTime remaining = DateTime.Parse(subtime);
                System.TimeSpan remainingtime = System.TimeSpan.Parse(remaining.ToShortTimeString());
                Trace.WriteLine(remainingtime);
                DispatcherTimer timer = new DispatcherTimer();

                
                DateTime startDate = DateTime.Now;
                System.TimeSpan t = remaining - startDate;
                DateTime difference = new DateTime(t.Ticks);
                System.TimeSpan timediff = new System.TimeSpan(t.Ticks);
                LaunchBTN.Content = $"Launching at {subtime}";
                Trace.WriteLine(timediff);
                timer.Interval += timediff;
                
                timer.Tick += TimerTick;
                
                timer.Start();
            }
            catch
            {
                MessageBox.Show($"Error Scheduling Launch", "Error");
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                (sender as DispatcherTimer).Stop();
                Trace.WriteLine("Triggered");
                scheduled = false;
                LaunchBTN.Content = "Launch";
                new ToastContentBuilder()
                    .AddText("Launching Apps:")
                    .Show(); 
                LaunchBTN_Click(LaunchBTN, new RoutedEventArgs());
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            try
            {
                string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Checks Documents Folder for path
                savePath = System.IO.Path.Combine(savePath + "/Just One Click/");
                string settingsFile = System.IO.Path.Combine(savePath + "appsettings.json");
                string json = File.ReadAllText(settingsFile);
                dSettings = JsonConvert.DeserializeObject<Settings>(json);
                var black = System.Windows.Media.Brushes.Black;
                SolidColorBrush white = new SolidColorBrush(System.Windows.Media.Color.FromRgb(237, 237, 237));
                if(dSettings.deauth == true)
                {
                    dSettings.deauth = false;
                    string sjson = JsonConvert.SerializeObject(dSettings, Formatting.Indented);
                    File.WriteAllText(settingsFile, sjson);
                    Close();
                }

                if (dSettings.DarkModeEnabled == false)
                {
                    Console.Foreground = black;
                    grid.Background = white;
                    Title.Foreground = black;
                    LaunchBTN.Foreground = black;
                    LaunchBTN.Background = white;
                    SettingsBTN.Background = white;
                    RefreshButton.Background = white;
                    SettingsBTN.Foreground = black;
                    RefreshButton.Foreground = black;
                }
                else
                {
                    SolidColorBrush bg = new SolidColorBrush(System.Windows.Media.Color.FromRgb(40, 40, 43));
                    SolidColorBrush console = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 31, 33));
                    Console.Background = console;
                    Title.Foreground = System.Windows.Media.Brushes.White;
                    grid.Background = bg;
                    Console.Foreground = white;
                    grid.Background = bg;
                    Title.Foreground = white;
                    LaunchBTN.Foreground = white;
                    LaunchBTN.Background = console;
                    SettingsBTN.Background = console;
                    RefreshButton.Background = console;
                    SettingsBTN.Foreground = white;
                    RefreshButton.Foreground = white;
                }

                if (dSettings.VersionInfo == true)
                {
                    try
                    {

                        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DiamondPG\\version.txt";
                        Trace.WriteLine(path);
                        string text = File.ReadAllText(path);
                        VersionTXT.Text = "v" + text;
                    }
                    catch
                    {
                        MessageBox.Show("Version Info Setting Not Applied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    VersionTXT.Text = "";
                }

            }
            catch
            {
                MessageBox.Show("Settings have not been applied. Please Restart the App", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }
        public class Profile
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int GlobalDelay { get; set; }
            public int Num { get; set; }
            public List<Apps> Applications { get; set; }
        }

        public class Apps
        {
            public string ID { get; set; }
            public int Delay { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Favicon { get; set; }
            public bool isBrowserSource { get; set; }
            public bool isTextSource { get; set; }
            public bool isVerified { get; set; }
        }

        public class Settings
        {
        public bool DarkModeEnabled { get; set; }
        public bool DeleteConfirmation { get; set; }
        public bool VersionInfo { get; set; }
        public bool IsFirstBoot { get; set; }
        public bool deauth { get; set; }

        }

        
    }
}