﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace VirtualBoxUUIDChanger
{
    public partial class MainWindow : Window
    {
        private OpenFileDialog _openFileDialog = new OpenFileDialog();
        public ObservableCollection<string> vdiFiles = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            vdiList.ItemsSource = vdiFiles;
        }

        private void FileExplorer_Click(object sender, RoutedEventArgs e)
        {
            _openFileDialog.Filter = "Virtual Disk Image (.vdi)|*.vdi|All files (*.*)|*.*";

            if ((bool)_openFileDialog.ShowDialog()!)
            {
                foreach (string vdiFile in _openFileDialog.FileNames)
                {
                    vdiFiles.Add(vdiFile);
                }
            }
        }

        private void UUIDChanger_Click(object sender, RoutedEventArgs e)
        {
            if (vdiFiles.Count == 0)
            {
                MessageBox.Show("Nincs kiválasztva VDI fájl!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProcessStartInfo process = new ProcessStartInfo();
            process.WindowStyle = ProcessWindowStyle.Hidden;
            process.FileName = @"C:\windows\system32\cmd.exe";
            process.WorkingDirectory = @"C:\Program Files\Oracle\VirtualBox\";

            foreach (string vdiFile in vdiFiles)
            {
                try
                {
                    process.Arguments = $"/c VBoxManage internalcommands sethduuid \"{vdiFile}\"";
                    Process.Start(process)!.WaitForExit();
                }
                catch (Exception)
                {
                    MessageBox.Show($"Hiba {vdiFile} fájl UUID lecserélése közben!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MessageBox.Show("Sikeres UUID módosítás", "Siker!", MessageBoxButton.OK);
        }

        private void vdiList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vdiFiles.RemoveAt(vdiList.SelectedIndex);
        }
    }
}