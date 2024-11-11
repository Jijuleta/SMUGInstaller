using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SMUGInstaller
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            bool hasAdminRights = Checks.IsAdministrator();
            bool hasWinGet = Checks.IsWinGet();
            bool hasRightWinGet = Checks.IsRightWinGet();
            if (hasAdminRights)
             {
                 if (hasWinGet)
                 {
                     if (hasRightWinGet)
                     {
                        InitializeComponent();
                        LoadPrograms();
                     }
                     else
                     {
                         MessageBox.Show(
                         "Install a newer version of Winget by updating in the Microsoft Store.",
                         "Invalid Winget version",
                         MessageBoxButton.OK,
                         MessageBoxImage.Error);
                     }
                 }
                 else
                 {
                     MessageBox.Show(
                       "This application requires Windows Package Manager to function properly.\nWinget will be installed right now.",
                       "Winget check failed",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information);
                     var executor = new Winget();
                     executor.InstallWinGet();
                     MessageBox.Show(
                       "Restart application",
                       "Winget installed.",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information);
                     Application.Current.Shutdown();

                 }

             }
             else
             {
                 MessageBox.Show(
                     "This application requires administrator rights to function properly.\nPlease restart the application with administrator privileges.",
                     "UAC check failed",
                     MessageBoxButton.OK,
                     MessageBoxImage.Error);
                 Application.Current.Shutdown();

             }
         }

        public string winGetDefault = "winget install --accept-source-agreements --accept-package-agreements -e --id";
        private void LoadPrograms()
        {
            List<Program> tools = new List<Program>
            {
                new Program("DroidCam Client", $"{winGetDefault} dev47apps.DroidCam"),
                new Program("MSI Afterburner", $"{winGetDefault} Guru3D.Afterburner"),
                new Program("OBS Studio", $"{winGetDefault} OBSProject.OBSStudio"),
                new Program("Nyrna", $"{winGetDefault} KristenMcWilliam.Nyrna"),
                new Program("AnyDesk", $"{winGetDefault} AnyDeskSoftwareGmbH.AnyDesk"),
                new Program("Logitech G HUB", $"{winGetDefault} Logitech.GHUB"),
                new Program("PuTTY", $"{winGetDefault} PuTTY.PuTTY"),
                new Program("WinSCP", $"{winGetDefault} WinSCP.WinSCP")
            };
            List<Program> files = new List<Program>
            {
                new Program("7-Zip", $"{winGetDefault} 7zip.7zip"),
                new Program("qBittorrent", $"{winGetDefault} qBittorrent.qBittorrent"),
                new Program("balenaEtcher", $"{winGetDefault} Balena.Etcher"),
                new Program("VeraCrypt", $"{winGetDefault} IDRIX.VeraCrypt"),
                new Program("KeePassXC", $"{winGetDefault} KeePassXCTeam.KeePassXC -v 2.7.0"),
                new Program("CrystalDiskInfo", $"{winGetDefault} CrystalDewWorld.CrystalDiskInfo"),
                new Program("WinDirStat", $"{winGetDefault} WinDirStat.WinDirStat"),
                new Program("VLC media player", $"{winGetDefault} VideoLAN.VLC")
            };
            List<Program> vpn = new List<Program>
            {
                new Program("OpenVPN Connect", $"{winGetDefault} OpenVPNTechnologies.OpenVPNConnect"),
                new Program("WireGuard", $"{winGetDefault} WireGuard.WireGuard")
            };
            List<Program> browsers = new List<Program>
            {
                new Program("Google Chrome", $"{winGetDefault} Google.Chrome"),
                new Program("Chromium", $"{winGetDefault} Hibbiki.Chromium"),
                new Program("Mozilla Firefox", $"{winGetDefault} Mozilla.Firefox"),
                new Program("Brave", $"{winGetDefault} Brave.Brave")
            };
            List<Program> games = new List<Program>
            {
                new Program("ATLauncher", $"{winGetDefault} ATLauncher.ATLauncher"),
                new Program("EA App", $"{winGetDefault} ElectronicArts.EADesktop"),
                new Program("Epic Games Launcher", $"{winGetDefault} EpicGames.EpicGamesLauncher"),
                new Program("Ubisoft Connect", $"{winGetDefault} Ubisoft.Connect"),
                new Program("Steam", $"{winGetDefault} Valve.Steam")
            };
            List<Program> coding = new List<Program>
            {
                new Program("Python 3.11", $"{winGetDefault} Python.Python.3.11"),
                new Program("Rustup", $"{winGetDefault} Rustlang.Rustup"),
                new Program("Visual Studio Community 2022", $"{winGetDefault} Microsoft.VisualStudio.2022.Community"),
                new Program("Visual Studio Code", $"{winGetDefault} Microsoft.VisualStudioCode"),
                new Program("GitHub Desktop", $"{winGetDefault} GitHub.GitHubDesktop"),
                new Program("Git", $"{winGetDefault} Git.Git"),
                new Program("FFmpeg", $"{winGetDefault} Gyan.FFmpeg")
            };
            List<Program> messengers = new List<Program>
            {
                new Program("Telegram Desktop", $"{winGetDefault} Telegram.TelegramDesktop"),
                new Program("64Gram Desktop", $"{winGetDefault} 64Gram.64Gram"),
                new Program("Discord", $"{winGetDefault} Discord.Discord"),
                new Program("Discord Canary", $"{winGetDefault} Discord.Discord.Canary"),
                new Program("Mozilla Thunderbird", $"{winGetDefault} Mozilla.Thunderbird")
            };
            List<Program> redists = new List<Program>
            {
                new Program("Visual C++ 2015-2022 (x86)", $"{winGetDefault} Microsoft.VCRedist.2015+.x86"),
                new Program("Visual C++ 2015-2022 (x64)", $"{winGetDefault} Microsoft.VCRedist.2015+.x64"),
                new Program("OpenJDK 21", $"{winGetDefault} Microsoft.OpenJDK.21")
            };

            ToolsListBox.ItemsSource = tools;
            FilesListBox.ItemsSource = files;
            VPNListBox.ItemsSource = vpn;
            BrowsersListBox.ItemsSource = browsers;
            GamesListBox.ItemsSource = games;
            CodingListBox.ItemsSource = coding;
            MessengersListBox.ItemsSource = messengers;
            RedistsListBox.ItemsSource = redists;


        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentProgramLabel.Visibility = Visibility.Visible;
            //InstallProgress.Visibility = Visibility.Visible;

            List<Program> selectedPrograms = new List<Program>();

            selectedPrograms.AddRange(GetSelectedPrograms(ToolsListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(FilesListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(VPNListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(BrowsersListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(GamesListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(CodingListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(MessengersListBox));
            selectedPrograms.AddRange(GetSelectedPrograms(RedistsListBox));

            await InstallPrograms(selectedPrograms);

            CurrentProgramLabel.Visibility = Visibility.Hidden;
            //InstallProgress.Visibility = Visibility.Hidden;
        }

        private void Features_Click(object sender, RoutedEventArgs e)
        {
            FeaturesWindow featuresWindow = new FeaturesWindow();
            featuresWindow.Owner = this;
            featuresWindow.Show();
        }

        private List<Program> GetSelectedPrograms(ListBox listBox)
        {
            List<Program> selectedPrograms = new List<Program>();
            foreach (Program program in listBox.Items)
            {
                if (program.IsSelected)
                {
                    selectedPrograms.Add(program);
                }
            }
            return selectedPrograms;
        }

        private async Task InstallPrograms(List<Program> programs)
        {
            int totalPrograms = programs.Count;
            int installedPrograms = 0;

            foreach (var program in programs)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentProgramLabel.Content = $"Installing: {program.DisplayName}";
                });

                await InstallProgramAsync(program.ActualName);
                installedPrograms++;

            }

            MessageBox.Show(
            "Programs installed successfully.",
            "Programs installed successfully!",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        }


        private async Task InstallProgramAsync(string actualName)
        {
            var scriptArguments = $"-ExecutionPolicy Bypass -Command \"{actualName}; exit\"";
            var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
            int exitCode = process.ExitCode;

            if (exitCode != 0)
            {
                MessageBox.Show($"Failed to install: {actualName}\nError: {error}", "Installation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateAppsButton_Click(object sender, RoutedEventArgs e)
        {
            var scriptArguments = $"-ExecutionPolicy Bypass -Command \"winget upgrade; exit\"";
            var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            MessageBox.Show("Starting to update all apps...", "SMUGInstaller", MessageBoxButton.OK, MessageBoxImage.Information);

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
            int exitCode = process.ExitCode;

            MessageBox.Show("Apps have been updated successfully.", "SMUGInstaller", MessageBoxButton.OK, MessageBoxImage.Information);

            if (exitCode != 0)
            {
                MessageBox.Show($"Error: {error}", "Installation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class Program
        {
            public string DisplayName { get; set; }
            public string ActualName { get; set; }
            public bool IsSelected { get; set; }

            public Program(string displayName, string actualName)
            {
                DisplayName = displayName;
                ActualName = actualName;
            }
        }
    }
}
