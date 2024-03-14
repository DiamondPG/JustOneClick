using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.IO;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Just_One_Click
{
    public class PathColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string savepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Just One Click\\savedata.json";
            string json = File.ReadAllText(savepath);
            List<MainWindow.Profile> profiles = JsonConvert.DeserializeObject<List<MainWindow.Profile>>(json);

            bool isPathValid = false;
            foreach (MainWindow.Profile profile in profiles)
            {
                List<MainWindow.Apps> apps = profile.Applications;
                foreach (MainWindow.Apps app in apps)
                {
                    if (value is string selectedpath)
                    {
                        if (selectedpath == app.Path)
                        {
                            if (app.isVerified == true)
                            {
                                return Brushes.Gray;
                            }
                            else if (app.isVerified == null)
                            {
                                if (value is string path && !string.IsNullOrEmpty(path) && (File.Exists(path) || DomainExists(path)))
                                {
                                    isPathValid = true;
                                    app.isVerified = true;
                                    WriteSave(profiles);
                                    break;

                                }
                                else
                                {
                                    isPathValid = false;
                                    app.isVerified = false;
                                    WriteSave(profiles);
                                    break;
                                }

                            }
                            else
                            {
                                isPathValid = false;
                                app.isVerified = false;
                                WriteSave(profiles);
                            }
                        }
                    }
                }
            }

            if (isPathValid)
                return Brushes.Gray;
            else
                return Brushes.Red;
        }
        private void WriteSave(object obj)
        {
            try
            {
                string savepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Just One Click\\savedata.json";
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(savepath, json);
            }
            catch
            {
                MessageBox.Show("Error writing save file");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        public static bool DomainExists(string domainName)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(domainName);
                return true; // If the DNS lookup succeeds, the domain exists
            }
            catch (SocketException)
            {
                return false; // If a SocketException is thrown, the domain doesn't exist
            }
        }
    }
}
