using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace SMUGInstaller
{
    public partial class FeaturesWindow : Window
    {
        public FeaturesWindow()
        {
            InitializeComponent();
        }

        private void MouseSettings_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c main.cpl";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();
        }

        private void DisableFastStartup_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power", writable: true);
            if (key == null)
            {
                Console.WriteLine("Registry key not found.");
                return;
            }

            key.SetValue("HiberbootEnabled", 0, RegistryValueKind.DWord);

            key.Close();
            MessageBox.Show(
            "Fast startup was disabled!",
            "Fast startup was disabled.",
            MessageBoxButton.OK,
            MessageBoxImage.Exclamation);
        }
    }
}