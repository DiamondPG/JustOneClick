using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Just_One_Click
{
    public partial class FileBrowser : Window
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            public string lpszTitle;
            public uint ulFlags;
            public IntPtr lpfn;
            public int lParam;
            public IntPtr iImage;
        }

        private const uint BIF_RETURNONLYFSDIRS = 0x0001;
        private const uint MAX_PATH = 260;

        public FileBrowser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selectedFolder = ShowFolderBrowserDialog("Select a folder");
            if (!string.IsNullOrEmpty(selectedFolder))
            {
                MessageBox.Show($"Selected Folder: {selectedFolder}");
            }
        }

        private string ShowFolderBrowserDialog(string title)
        {
            IntPtr hwndOwner = new WindowInteropHelper(this).Handle;

            BROWSEINFO bi = new BROWSEINFO
            {
                hwndOwner = hwndOwner,
                ulFlags = BIF_RETURNONLYFSDIRS,
                lpszTitle = title
            };

            IntPtr pidl = SHBrowseForFolder(ref bi);

            if (pidl != IntPtr.Zero)
            {
                IntPtr buffer = Marshal.AllocHGlobal((int)MAX_PATH * 2);
                SHGetPathFromIDList(pidl, buffer);

                string result = Marshal.PtrToStringUni(buffer);
                Marshal.FreeHGlobal(buffer);
                return result;
            }

            return null;
        }
    }
}
