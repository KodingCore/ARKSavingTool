using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata.Ecma335;

namespace ARKSavingTool
{

    public partial class Form1 : Form
    {
        static string cheminArk = "";
        static string nomDerniereSauvegarde = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public FileInfo info;
        private void Actualiser()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string[] allfiles;

            try
            {
                allfiles = Directory.GetDirectories(cheminArk + "/ShooterGame/Saved", "*.*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
                reselectPath();
                throw;
            }
            
            foreach (var file in allfiles)
            {
                info = new FileInfo(file);
                if (info.Name != "SavedArksLocal" && info.Name != "Config" && info.Name != "LocalProfiles" && info.Name != "Logs")
                {
                    listBox1.Items.Add(info.Name);
                    listBox2.Items.Add("Le " + info.CreationTime.Day + "/" + info.CreationTime.Month + " à " + info.CreationTime.Hour + "h" + info.CreationTime.Minute);
                }
                // Do something with the Folder or just add them to a list via nameoflist.add();
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(listBox1.SelectedItem != null)
                {
                    textBox3.Text = "" + listBox1.SelectedItem.ToString();
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Aucun index sélectionnés.");
                throw;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {

                var result = MessageBox.Show("La joueur précédent à t'il bien sauvegarder sa partie?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    label3.ForeColor = Color.Red;
                    label3.Text = "En cours de Chargement...";
                    var result2 = MessageBox.Show("Voulez-vous vraiment charger " + textBox3.Text + "?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {

                        var dir = new DirectoryInfo(cheminArk + "/ShooterGame/Saved/SavedArksLocal");
                        if (dir.Exists)
                        {
                            Directory.Delete(cheminArk + "/ShooterGame/Saved/SavedArksLocal", true);
                        }
                        string sourceDir = cheminArk + "/ShooterGame/Saved/" + textBox3.Text;
                        CopyDirectory(@sourceDir, cheminArk + @"/ShooterGame/Saved/SavedArksLocal", true);
                        label3.ForeColor = Color.Green;
                        label3.Text = "Partie " + textBox3.Text + " Chargée!";
                        if(checkBox1.Checked == true)
                        {
                            LancementDuJeu();
                        }
                    }
                    else
                    {
                        label3.ForeColor = Color.Green;
                        label3.Text = "Chargement abandonnée";
                    }
                }
                else
                {
                    label3.ForeColor = Color.Green;
                    label3.Text = "Chargement abandonnée";
                }

            }
            Actualiser();
        }

        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
            label3.Text = "En cours de Sauvegarde...";
            string newDoss = cheminArk + "/ShooterGame/Saved/" + textBox3.Text;
            var result = MessageBox.Show("Voulez-vous vraiment écraser " + textBox3.Text + "?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                var dir = new DirectoryInfo(newDoss);
                if (dir.Exists)
                {
                    Directory.Delete(newDoss, true);
                }
                string sourceDir = cheminArk + "/ShooterGame/Saved/SavedArksLocal";
                CopyDirectory(@sourceDir, @newDoss, true);
                nomDerniereSauvegarde = textBox3.Text;
                textBox4.Text = nomDerniereSauvegarde;
                label3.ForeColor = Color.Green;
                label3.Text = "Sauvegarde Terminée";
                WriterParams();
            }
            else
            {
                label3.ForeColor = Color.Green;
                label3.Text = "Sauvegarde abandonnée";
            }
            Actualiser();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
            label3.Text = "En cours de Sauvegarde...";
            string newDoss = cheminArk + "/ShooterGame/Saved/Saved" + textBox1.Text;
            var result = MessageBox.Show("Voulez-vous vraiment créer une sauvegarde au nom de Saved" + textBox1.Text + "?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var dir = new DirectoryInfo(newDoss);
                if (dir.Exists)
                {
                    Directory.Delete(newDoss, true);
                }
                string sourceDir = cheminArk + "/ShooterGame/Saved/SavedArksLocal";
                CopyDirectory(@sourceDir, @newDoss, true);
                nomDerniereSauvegarde = textBox1.Text;
                textBox4.Text = nomDerniereSauvegarde;
                WriterParams();
                label3.ForeColor = Color.Green;
                label3.Text = "Sauvegarde Terminée";
            }
            else
            {
                label3.ForeColor = Color.Green;
                label3.Text = "Sauvegarde abandonnée";
            }

            Actualiser();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReaderParams();
            Actualiser();
            textBox4.Text = nomDerniereSauvegarde;
            if (SteamProcessOpen())
            {
                label3.ForeColor = Color.Blue;
                label3.Text = "Charger une partie ou lancez le jeu";
            }
            else
            {
                MessageBox.Show("Attention, Steam n'est pas lancé");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
            label3.Text = "En cours de Suppression...";
            string newDoss = cheminArk + "/ShooterGame/Saved/" + textBox3.Text;
            var result = MessageBox.Show("Voulez-vous vraiment supprimer " + textBox3.Text + "?", "Form Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                var dir = new DirectoryInfo(newDoss);
                if (dir.Exists)
                {
                    Directory.Delete(newDoss, true);
                }
                label3.ForeColor = Color.Green;
                label3.Text = "Suppression Terminée";
            }
            else
            {
                MessageBox.Show("Suppression abandonnée");
                label3.ForeColor = Color.Green;
                label3.Text = "Suppression abandonnée";
            }
            Actualiser();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LancementDuJeu();
        }

        public bool SteamProcessOpen() //Control d'ouverture de processus
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains("steam"))
                {
                    return true;
                }
            }
            return false;
        }

        public void LancementDuJeu()
        {
            if (SteamProcessOpen())
            {
                Process process = Process.Start(cheminArk + @"/ShooterGame/Binaries/Win64/ShooterGame.exe");
                int id = process.Id;
                Process tempProc = Process.GetProcessById(id);
                this.Visible = false;
                tempProc.WaitForExit();
                this.Visible = true;
                label3.ForeColor = Color.Orange;
                label3.Text = "Partie non-sauvegardée";
            }
            else
            {
                MessageBox.Show("Veuillez lancez steam avant de lancer ARK");
            }
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            reselectPath();
            
        }

        public void reselectPath()
        {
            MessageBox.Show("Veuillez selectionner le répertoire du dossier ARK.");
            var browserDiag = new FolderBrowserDialog();
            if (browserDiag.ShowDialog() == DialogResult.OK)
            {
                cheminArk = browserDiag.SelectedPath;
                WriterParams();
            }
        }

        public void WriterParams()
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\ArkSaveTool.par");
                sw.WriteLine(cheminArk + "!" + nomDerniereSauvegarde);
                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public void ReaderParams()
        {
            string lineParams = "";
            int nombreDeParametres = 2;
            string[] paramArray = new string[nombreDeParametres - 1];
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamReader sr = new StreamReader("C:/ArkSaveTool.par");
                if (File.Exists("C:\\ArkSaveTool.par"))
                {
                    float poids = new FileInfo("C:/ArkSaveTool.par").Length;
                    if (poids > 0)
                    {
                        lineParams = sr.ReadLine()!;
                    }
                }
                paramArray = lineParams.Split('!', StringSplitOptions.None);
                sr.Close();
                cheminArk = paramArray[0];
                nomDerniereSauvegarde = paramArray[1];
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void LancementDeSteam()
        {
            Process process = Process.Start("C:/Program Files (x86)/Steam/Steam.exe");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LancementDeSteam();
        }
    }
}
