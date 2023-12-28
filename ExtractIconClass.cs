using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Just_One_Click
{
    public partial class ExtractIconClass : Window
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Use OpenFileDialog to allow the user to select multiple executable files
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select executable files",
                Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Iterate through selected files
                foreach (string exeFilePath in openFileDialog.FileNames)
                {
                    // Get the icon from the executable file
                    Icon exeIcon = ExtractIcon(exeFilePath);

                    if (exeIcon != null)
                    {
                        // Convert the icon to a BitmapSource
                        BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            exeIcon.Handle,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

                        // Save the icon as a PNG file
                        string tempFolderPath = Path.GetTempPath();
                        string tempIconPath = Path.Combine(tempFolderPath, $"ExtractedIcon_{Path.GetFileNameWithoutExtension(exeFilePath)}.png");

                        using (FileStream stream = new FileStream(tempIconPath, FileMode.Create))
                        {
                            PngBitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                            encoder.Save(stream);
                        }

                        MessageBox.Show($"Icon extracted from {Path.GetFileName(exeFilePath)} and saved to {tempIconPath}", "Icon Extractor", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"No icon found in {Path.GetFileName(exeFilePath)}", "Icon Extractor", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private Icon ExtractIcon(string exeFilePath)
        {
            try
            {
                return System.Drawing.Icon.ExtractAssociatedIcon(exeFilePath);
            }
            catch (COMException ex)
            {
                // Handle the COMException here
                MessageBox.Show($"Error extracting icon from {Path.GetFileName(exeFilePath)}: {ex.Message}", "Icon Extractor", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
