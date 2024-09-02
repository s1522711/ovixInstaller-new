using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ovixInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const bool isRdr = false;
        string appdataPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Ovix";

        private async void InstallBtn_Click(object sender, EventArgs e)
        {
            InstallBtn.Enabled = false;
            //string ovixDownloadUrl = $"https://dash.ovix.one/{game}/OvixBundle.zip";
            string ovixDownloadUrl = $"https://ovix.retardhub.xyz/" + (isRdr ? "Rdr" : "Gta") + "Bundle.zip";
            string zipFilePath = $"{appdataPath}\\" + (isRdr ? "Rdr" : "Gta") + "Bundle.zip";
            string extractPath = $"{appdataPath}\\OvixBundle";

            toolStripStatusLabel1.Text = "Creating directories...";
            // Ensure the directory exists
            Directory.CreateDirectory(appdataPath);
            toolStripStatusLabel1.Text = "Downloading Ovix " + (isRdr ? "Rdr2" : "Gta") + "...";

            // Download the zip file
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }
            using (HttpClient client = new HttpClient())
            {
                byte[] fileBytes = await client.GetByteArrayAsync(ovixDownloadUrl);
                await File.WriteAllBytesAsync(zipFilePath, fileBytes);
            }

            if (!File.Exists(zipFilePath))
            {
                MessageBox.Show("Failed to download Ovix " + (isRdr ? "Rdr2" : "Gta") + "!\n\nMake sure your antivirus is off!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                InstallBtn.Enabled = true;
                toolStripStatusLabel1.Text = "Failed to download Ovix " + (isRdr ? "Rdr2" : "Gta") + "!";
                return;
            }

            toolStripStatusLabel1.Text = "Extracting Ovix " + (isRdr ? "Rdr2" : "Gta") + "...";
            // Unzip the file
            if (Directory.Exists(extractPath))
            {
                Directory.Delete(extractPath, true);
            }
            ZipFile.ExtractToDirectory(zipFilePath, extractPath);

            if (!File.Exists($"{extractPath}\\OvixBundle\\Ovix" + (isRdr ? "Rdr" : "Gta").ToUpper() + "Launcher.exe") || !File.Exists($"{extractPath}\\OvixBundle\\Ovix\\" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "\\Ovix.dll"))
            {
                MessageBox.Show("Failed to extract Ovix " + (isRdr ? "Rdr2" : "Gta") + "!\n\nMake sure your antivirus is off!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                InstallBtn.Enabled = true;
                toolStripStatusLabel1.Text = "Failed to extract Ovix " + (isRdr ? "Rdr2" : "Gta") + "!";
                return;
            }

            // Delete the zip file
            File.Delete(zipFilePath);

            toolStripStatusLabel1.Text = "Installing Ovix " + (isRdr ? "Rdr2" : "Gta") + "...";
            // Copy the ovix folder to the appdata folder
            string ovixPath = $"{extractPath}\\OvixBundle\\Ovix\\" + (isRdr ? "Rdr2" : "Gta").ToUpper();
            if (Directory.Exists($"{appdataPath}\\" + (isRdr ? "Rdr2" : "Gta").ToUpper()))
            {
                makeBackup();
                toolStripStatusLabel1.Text = "Deleting the current Ovix " + (isRdr ? "Rdr2" : "Gta") + " installation...";
                if (Directory.Exists($"{appdataPath}\\" + (isRdr ? "Rdr2" : "Gta").ToUpper()))
                {
                    Directory.Delete($"{appdataPath}\\" + (isRdr ? "Rdr2" : "Gta").ToUpper(), true);
                }
            }
            toolStripStatusLabel1.Text = "Moving the Ovix " + (isRdr ? "Rdr2" : "Gta") + " folder to the appdata folder...";
            Directory.Move(ovixPath, $"{appdataPath}\\" + (isRdr ? "Rdr2" : "Gta").ToUpper());
            restoreBackup();


            toolStripStatusLabel1.Text = "Moving the Ovix" + (isRdr ? "Rdr2" : "Gta") + "Launcher.exe to the root of the appdata folder...";
            // Copy OvixGTALauncher.exe to the root of the appdata folder
            try
            {
                File.Move($"{extractPath}\\OvixBundle\\Ovix" + (isRdr ? "Rdr" : "Gta").ToUpper() + "Launcher.exe", $"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe");
            }
            catch (IOException)
            {
                File.Delete($"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe");
                File.Move($"{extractPath}\\OvixBundle\\Ovix" + (isRdr ? "Rdr" : "Gta").ToUpper() + "Launcher.exe", $"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe");
            }
            // Delete the extracted folder
            Directory.Delete(extractPath, true);

            if (!File.Exists($"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe") || !Directory.Exists($"{appdataPath}\\" + (isRdr ? "Rdr2" : "Gta").ToUpper()))
            {
                MessageBox.Show("Failed to install Ovix " + (isRdr ? "Rdr2" : "Gta") + "!\n\nMake sure your antivirus is off!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                InstallBtn.Enabled = true;
                toolStripStatusLabel1.Text = "Failed to install Ovix " + (isRdr ? "Rdr2" : "Gta") + "!";
                return;
            }

            toolStripStatusLabel1.Text = "Creating shortcuts...";
            // Create a shortcut on the desktop
            string desktopPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Ovix-" + (isRdr ? "Rdr2" : "Gta").ToUpper() + ".lnk";
            try
            {
                CreateShortcut($"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe", desktopPath, isRdr);
            }
            catch (IOException)
            {
                File.Delete(desktopPath);
                CreateShortcut($"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe", desktopPath, isRdr);
            }

            // If the c++ redist check box is checked, install the c++ redist
            if (InstallRedist.Checked)
            {
                InstallCppRedist();
            }


            toolStripStatusLabel1.Text = "Waiting for user input";
            MessageBox.Show($"Ovix " + (isRdr ? "Rdr2" : "Gta").ToUpper() + " has been installed successfully!", "Good Job! you can press buttons!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            InstallBtn.Enabled = true;
            toolStripStatusLabel1.Text = "Ready";
        }

        private void CreateShortcut(string targetFile, string shortcutPath, bool isRdr)
        {
            // Use Windows Script Host to create a shortcut
            Type t = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(t);
            var shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetFile;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetFile);
            shortcut.Description = $"Shortcut for Ovix " + (isRdr ? "Rdr2" : "Gta").ToUpper() + " Launcher";
            shortcut.Save();
        }

        private void makeBackup()
        {
            toolStripStatusLabel1.Text = "Checking if Ovix " + (isRdr ? "Rdr2" : "Gta") + " is already installed...";
            string ovixDataFolder = appdataPath + "\\" + (isRdr ? "Rdr2" : "Gta").ToUpper();
            string backupPath = $"{appdataPath}\\backup";
            if (Directory.Exists(backupPath))
            {
                Directory.Delete(backupPath, true);
            }

            if (File.Exists($"{ovixDataFolder}\\config.json") || Directory.Exists($"{ovixDataFolder}\\handling_profiles") || Directory.Exists($"{ovixDataFolder}\\headers") || Directory.Exists($"{ovixDataFolder}\\saved_outfits") || Directory.Exists($"{ovixDataFolder}\\translations") || File.Exists($"{ovixDataFolder}\\protection_settings.json"))
            {
                toolStripStatusLabel1.Text = "Ovix " + (isRdr ? "Rdr2" : "Gta") + " is already installed";
                bool isFresh = MessageBox.Show("Ovix " + (isRdr ? "Rdr2" : "Gta").ToUpper() + " is already installed, would you like to keep your current configuration (yes) or do a fresh install (no)?\n\nwe do not guarantee that the ovix installation will work if you do this, if it doesnt please do a fresh install instead!", "Ovix is already installed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                if (!isFresh)
                {
                    toolStripStatusLabel1.Text = "Deleting the current configuration";
                    Directory.Delete(ovixDataFolder, true);
                    return;
                }

                toolStripStatusLabel1.Text = "informing the user about the backup";
                MessageBox.Show("The installer will try to keep your current configuration.\n\nwe do not guarantee that the ovix installation will work if you do this, if it doesnt please do a fresh install instead!", "Ovix is already installed", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // create a backup of the current config files
                toolStripStatusLabel1.Text = "Creating a backup of the current configuration";
                Directory.CreateDirectory(backupPath);
                if (File.Exists($"{ovixDataFolder}\\config.json"))
                {
                    toolStripStatusLabel1.Text = "Creating a backup of the config.json file";
                    File.Move($"{ovixDataFolder}\\config.json", $"{backupPath}\\config.json");
                }

                if (Directory.Exists($"{ovixDataFolder}\\handling_profiles"))
                {
                    toolStripStatusLabel1.Text = "Creating a backup of the handling_profiles folder";
                    Directory.Move($"{ovixDataFolder}\\handling_profiles", $"{backupPath}\\handling_profiles");
                }

                if (Directory.Exists($"{ovixDataFolder}\\headers"))
                {
                    toolStripStatusLabel1.Text = "Creating a backup of the headers folder";
                    Directory.Move($"{ovixDataFolder}\\headers", $"{backupPath}\\headers");
                }

                if (Directory.Exists($"{ovixDataFolder}\\saved_outfits"))
                {
                    toolStripStatusLabel1.Text = "Creating a backup of the saved_outfits folder";
                    Directory.Move($"{ovixDataFolder}\\saved_outfits", $"{backupPath}\\saved_outfits");
                }

                if (Directory.Exists($"{ovixDataFolder}\\translations"))
                {
                    toolStripStatusLabel1.Text = "Creating a backup of the translations folder";
                    Directory.Move($"{ovixDataFolder}\\translations", $"{backupPath}\\translations");
                }

                if (File.Exists($"{ovixDataFolder}\\protection_settings.json"))
                {
                    toolStripStatusLabel1.Text = "Creating a backup of the protection_settings.json file";
                    File.Move($"{ovixDataFolder}\\protection_settings.json", $"{backupPath}\\protection_settings.json");
                }

                toolStripStatusLabel1.Text = "Backup created successfully";
            }
        }

        private void restoreBackup()
        {
            string ovixDataFolder = appdataPath + "\\" + (isRdr ? "Rdr2" : "Gta").ToUpper();
            string backupPath = $"{appdataPath}\\backup";
            toolStripStatusLabel1.Text = "Checking if a backup exists";
            if (!Directory.Exists(backupPath))
            {
                return;
            }

            toolStripStatusLabel1.Text = "Backup found! Restoring the backup";

            if (File.Exists($"{backupPath}\\config.json"))
            {
                toolStripStatusLabel1.Text = "Restoring the config.json file";
                // override the config.json file if it exists
                if (File.Exists($"{ovixDataFolder}\\config.json"))
                {
                    File.Delete($"{ovixDataFolder}\\config.json");
                }
                File.Move($"{backupPath}\\config.json", $"{ovixDataFolder}\\config.json");
            }

            if (Directory.Exists($"{backupPath}\\handling_profiles"))
            {
                toolStripStatusLabel1.Text = "Restoring the handling_profiles folder";
                // override the handling_profiles folder if it exists
                if (Directory.Exists($"{ovixDataFolder}\\handling_profiles"))
                {
                    Directory.Delete($"{ovixDataFolder}\\handling_profiles", true);
                }
                Directory.Move($"{backupPath}\\handling_profiles", $"{ovixDataFolder}\\handling_profiles");
            }

            if (Directory.Exists($"{backupPath}\\headers"))
            {
                toolStripStatusLabel1.Text = "Restoring the headers folder";
                // override the headers folder if it exists
                if (Directory.Exists($"{ovixDataFolder}\\headers"))
                {
                    Directory.Delete($"{ovixDataFolder}\\headers", true);
                }
                Directory.Move($"{backupPath}\\headers", $"{ovixDataFolder}\\headers");
            }

            if (Directory.Exists($"{backupPath}\\saved_outfits"))
            {
                toolStripStatusLabel1.Text = "Restoring the saved_outfits folder";
                // override the saved_outfits folder if it exists
                if (Directory.Exists($"{ovixDataFolder}\\saved_outfits"))
                {
                    Directory.Delete($"{ovixDataFolder}\\saved_outfits", true);
                }
                Directory.Move($"{backupPath}\\saved_outfits", $"{ovixDataFolder}\\saved_outfits");
            }

            if (Directory.Exists($"{backupPath}\\translations"))
            {
                toolStripStatusLabel1.Text = "Restoring the translations folder";
                // override the translations folder if it exists
                if (Directory.Exists($"{ovixDataFolder}\\translations"))
                {
                    Directory.Delete($"{ovixDataFolder}\\translations", true);
                }
                Directory.Move($"{backupPath}\\translations", $"{ovixDataFolder}\\translations");
            }

            if (File.Exists($"{backupPath}\\protection_settings.json"))
            {
                toolStripStatusLabel1.Text = "Restoring the protection_settings.json file";
                // override the protection_settings.json file if it exists
                if (File.Exists($"{ovixDataFolder}\\protection_settings.json"))
                {
                    File.Delete($"{ovixDataFolder}\\protection_settings.json");
                }
                File.Move($"{backupPath}\\protection_settings.json", $"{ovixDataFolder}\\protection_settings.json");
            }

            // Delete the backup folder
            toolStripStatusLabel1.Text = "Deleting the backup folder";
            Directory.Delete(backupPath, true);

            toolStripStatusLabel1.Text = "Backup restored successfully (we think)";
        }

        private void InstallCppRedist()
        {
            toolStripStatusLabel1.Text = "Installing C++ Redistributable...";
            // Check if the user is running a 64-bit system
            bool is64Bit = RuntimeInformation.OSArchitecture == Architecture.X64;

            // Download the c++ redist installer
            string redistUrl = is64Bit ? "https://aka.ms/vs/17/release/vc_redist.x64.exe" : "https://aka.ms/vs/17/release/vc_redist.x86.exe";
            string redistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Ovix\\vc_redist.exe";

            if (File.Exists(redistPath))
            {
                File.Delete(redistPath);
            }

            toolStripStatusLabel1.Text = "Downloading C++ Redistributable...";
            using (HttpClient client = new HttpClient())
            {
                byte[] fileBytes = client.GetByteArrayAsync(redistUrl).Result;
                File.WriteAllBytes(redistPath, fileBytes);
            }

            toolStripStatusLabel1.Text = "Installing C++ Redistributable...";
            // Set the installer to run silently
            System.Diagnostics.Process.Start(redistPath, "/q /passive /norestart").WaitForExit();

            // Delete the installer
            File.Delete(redistPath);
        }

        private void UninstallBtn_Click(object sender, EventArgs e)
        {
            UninstallBtn.Enabled = false;

            toolStripStatusLabel1.Text = "Uninstalling Ovix " + (isRdr ? "Rdr2" : "Gta") + "...";

            toolStripStatusLabel1.Text = "Checking if Ovix " + (isRdr ? "Rdr2" : "Gta") + " is installed...";
            // Check if the Ovix folder exists
            if (!Directory.Exists(appdataPath))
            {
                MessageBox.Show($"Ovix " + (isRdr ? "Rdr2" : "Gta").ToUpper() + " is not installed!", "ERROR! dumbass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                UninstallBtn.Enabled = true;
                toolStripStatusLabel1.Text = "Ovix wasnt installed dumbass!";
                return;
            }

            toolStripStatusLabel1.Text = "Waiting for user input";
            // Check if the user is sure about uninstalling
            bool isCancelled = MessageBox.Show($"Are you sure you want to uninstall Ovix " + (isRdr ? "Rdr2" : "Gta").ToUpper() + "?", "Uninstall Ovix", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No;
            if (isCancelled)
            {
                UninstallBtn.Enabled = true;
                toolStripStatusLabel1.Text = "Ready";
                return;
            }

            toolStripStatusLabel1.Text = "Deleting Ovix Folder";
            // Delete the Ovix folder
            string ovixDataFolder = appdataPath + "\\" + (isRdr ? "Rdr2" : "Gta").ToUpper();
            if (Directory.Exists(ovixDataFolder))
            {
                Directory.Delete(ovixDataFolder, true);
            }

            toolStripStatusLabel1.Text = "Deleting Ovix Launcher";
            // Delete the Ovix launcher
            if (File.Exists($"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe"))
            {
                File.Delete($"{appdataPath}\\Ovix" + (isRdr ? "Rdr2" : "Gta").ToUpper() + "Launcher.exe");
            }

            toolStripStatusLabel1.Text = "Deleting Ovix Shortcut";
            // Delete the desktop shortcut
            string desktopPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Ovix-" + (isRdr ? "Rdr2" : "Gta").ToUpper() + ".lnk";
            if (File.Exists(desktopPath))
            {
                File.Delete(desktopPath);
            }

            toolStripStatusLabel1.Text = "Checking if the appdata folder is empty";
            // Check if the appdata folder is empty
            if (Directory.GetDirectories(appdataPath).Length == 0)
            {
                toolStripStatusLabel1.Text = "Deleting the appdata folder";
                Directory.Delete(appdataPath);
            }

            toolStripStatusLabel1.Text = "Waiting for user input";
            MessageBox.Show($"Ovix " + (isRdr ? "Rdr2" : "Gta").ToUpper() + " has been uninstalled successfully!", "You actually did it!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UninstallBtn.Enabled = true;
            toolStripStatusLabel1.Text = "Ready";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Changing the title...";
            if (isRdr)
            {
                MainLabel.Text = "Ovix RDR";
                this.Text = "Ovix RDR2 Installer";
            }
            else
            {
                MainLabel.Text = "Ovix GTA";
                this.Text = "Ovix GTA Installer";
            }
            toolStripStatusLabel1.Text = "Ready";
        }

        private void InstallRedist_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Checking if the user is stupid...";

            if (InstallRedist.Checked)
            {
                toolStripStatusLabel1.Text = "User is not stupid";
                return;
            }

            if (!InstallRedist.Checked)
            {
                toolStripStatusLabel1.Text = "Dumbass detected";
                bool isCancelled = MessageBox.Show("Are you sure about that?", "Stupidity Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel;
                if (isCancelled)
                {
                    InstallRedist.Checked = true;
                    toolStripStatusLabel1.Text = "nvm hes a bit smart";
                    return;
                }
                toolStripStatusLabel1.Text = "Double dumbass detected";
                isCancelled = MessageBox.Show("Are you really sure about that?", "Stupidity Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel;
                if (isCancelled)
                {
                    InstallRedist.Checked = true;
                    toolStripStatusLabel1.Text = "nvm hes a bit smart";
                    return;
                }
                toolStripStatusLabel1.Text = "why the fuck did you disable it?";
                MessageBox.Show("Please note that Ovix may not work without the C++ Redistributable installed.", "Stupidity Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
