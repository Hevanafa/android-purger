using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Reflection;

namespace AndroidPurger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txbMessage.Text += "Welcome to Android Purger!\nUse this tool only after using `yarn bundle-android` to make cleaning easier.\n";
        }

        string executingLocation => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // android\app\src\main\res
                txbMessage.Text += "Scanning...\n";

                var targetDir = Path.Combine(executingLocation, "android", "app", "src", "main", "res");
                txbMessage.Text += $"{targetDir}\n";

                int drawableCount = 0;
                bool rawExists = false;

                foreach (var s in Directory.GetDirectories(targetDir))
                {
                    if (s.Contains("drawable"))
                        drawableCount++;
                    if (s.Contains("raw"))
                        rawExists = true;
                }

                if (drawableCount > 0)
                    txbMessage.Text += $"Found {drawableCount} drawable folders.\n";
                if (rawExists)
                    txbMessage.Text += "Found raw folder.\n";
            }
            catch (Exception ex)
            {
                txbMessage.Text += $"An error occurred.\n{ex.Message}\n";
            }
            finally {
                txbMessage.Text += "Scanning finished.\n";
            }
        }

        private void BtnPurge_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Begin purging?  This is not undoable!", "Purge", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;

            // android\app\src\main\res
            try
            {
                var targetDir = Path.Combine(executingLocation, "android", "app", "src", "main", "res");
                txbMessage.Text += $"{targetDir}\n";

                var candidatePaths = Directory
                    .GetDirectories(targetDir)
                    .Where(x =>
                        x.Contains("drawable") ||
                        x.Contains("raw")
                    );

                foreach (var s in candidatePaths)
                {
                    txbMessage.Text += $"Clearing {s}...\n";
                    deleteRecursively(s);
                }
            }
            catch (Exception ex)
            {
                txbMessage.Text += $"An error occurred.\n{ex.Message}\n";
            }
            finally {
                txbMessage.Text += "Done.\n";
            }
        }

        void deleteRecursively(string path) {
            foreach (var s in Directory.GetDirectories(path))
                deleteRecursively(s);

            foreach (var s in Directory.GetFiles(path))
                File.Delete(s);

            Directory.Delete(path);
        }

        private void BtnResetBundle_Click(object sender, RoutedEventArgs e)
        {
            //Debug.Print(Assembly.GetExecutingAssembly().Location);
            try
            {
                txbMessage.Text += "Resetting asset bundle file...\n";

                // android\app\src\main\assets\index.assets.bundle
                var targetFilename = Path.Combine(executingLocation, "android", "app", "src", "main", "assets", "index.android.bundle");

                txbMessage.Text += $"{targetFilename}\n";

                Debug.Print(targetFilename);

                if (File.Exists(targetFilename))
                {
                    txbMessage.Text += "File exists.\nResetting file...\n";
                    File.Delete(targetFilename);
                } 
                else txbMessage.Text += "File doesn't exist.\nCreating file...\n";

                File.Create(targetFilename);
            }
            catch (Exception ex)
            {
                txbMessage.Text += $"An error occurred.\n{ex.Message}\n";
            }
            finally {
                txbMessage.Text += "Done.\n";
            }
        }

        private void TxbMessage_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            txbMessage.ScrollToEnd();
        }

        private void BtnExploreRelease_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // android\app\build\outputs\apk\release
                txbMessage.Text += "Attempting to start Explorer...\n";

                var targetDir = Path.Combine(executingLocation, "android", "app", "build", "outputs", "apk", "release");
                txbMessage.Text += $"{targetDir}\n";

                if (Directory.Exists(targetDir))
                {
                    Process.Start("explorer.exe", targetDir);
                }
                else
                {
                    txbMessage.Text += "Error: The folder doesn't exist.";
                }
            }
            catch (Exception ex)
            {
                txbMessage.Text += $"An error occurred.\n{ex.Message}\n";
            }
            finally {
                txbMessage.Text += "Done\n";
            }
        }

    }
}
