using STEMUtil.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STEMUtil
{
    public partial class Form1 : Form
    {

        BackgroundWorker backgroundWorker;

        public Form1()
        {
            InitializeComponent();
        }

        private void ExtractBtn_Click(object sender, EventArgs e)
        {
            
            FileInfo PTRFile = new FileInfo(ExtractInputTextBox.Text);
            DirectoryInfo exportDir = new DirectoryInfo(ExtractOutputTextBox.Text);

            String PTRDir = Path.GetDirectoryName(PTRFile.FullName);
            FileInfo PKRFile = new FileInfo(PTRDir + "\\" + Path.GetFileNameWithoutExtension(PTRFile.Name) + ".pkr");

            if (!PKRFile.Exists)
            {
                PKRFile = new FileInfo(Directory.GetParent(PTRDir) + "\\" + Path.GetFileNameWithoutExtension(PTRFile.Name) + ".pkr");
            }

            if (PTRFile.Exists && PKRFile.Exists)
            {
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

                progressBar1.Enabled = true;

                //Export block
                ExtractInputChooser.Enabled = false;
                ExtractOutputChooser.Enabled = false;
                ExtractInputTextBox.Enabled = false;
                ExtractOutputTextBox.Enabled = false;
                ExtractBtn.Enabled = false;

                //Import Block
                ImportInputChooser.Enabled = false;
                ImportOutputChooser.Enabled = false;
                ImportInputTextBox.Enabled = false;
                ImportOutputTextBox.Enabled = false;
                ImportBtn.Enabled = false;

                Extractor PKTRExtractor;

                try
                {
                    PKTRExtractor = new Extractor(PTRFile.FullName, PKRFile.FullName, exportDir.FullName);
                    PKTRExtractor.OnProgressUpdate += t1_OnProgressUpdate;

                    backgroundWorker.RunWorkerAsync(PKTRExtractor);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ooops! And error occured: " + exc.Message, "An error occured =-(", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                PKTRExtractor = null;
                backgroundWorker = null;
            }
            else
            {
                MessageBox.Show("PTR or PKR file is missing. Please, make sure that PKR file placed in the same level with ptr file or 1 level", "Files missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            FileInfo importFile = new FileInfo(ImportOutputTextBox.Text);
            DirectoryInfo importDir = new DirectoryInfo(ImportInputTextBox.Text);

            String ptrFile = importFile.FullName;
            String pkrFile = Path.GetDirectoryName(ptrFile) + "\\" + Path.GetFileNameWithoutExtension(ptrFile) + ".pkr";

            if (importDir.Exists)
            {
                if (new FileInfo(importDir.FullName + "\\HEADER.data").Exists)
                {
                    backgroundWorker = new BackgroundWorker();
                    backgroundWorker.WorkerReportsProgress = true;
                    backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
                    backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

                    progressBar1.Enabled = true;

                    //Export block
                    ExtractInputChooser.Enabled = false;
                    ExtractOutputChooser.Enabled = false;
                    ExtractInputTextBox.Enabled = false;
                    ExtractOutputTextBox.Enabled = false;
                    ExtractBtn.Enabled = false;

                    //Import Block
                    ImportInputChooser.Enabled = false;
                    ImportOutputChooser.Enabled = false;
                    ImportInputTextBox.Enabled = false;
                    ImportOutputTextBox.Enabled = false;
                    ImportBtn.Enabled = false;

                    Importer PKTRImporter;

                    try
                    {
                        PKTRImporter = new Importer(importDir.FullName, pkrFile, ptrFile);
                        PKTRImporter.OnProgressUpdate += t1_OnProgressUpdate;

                        backgroundWorker.RunWorkerAsync(PKTRImporter);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Ooops! And error occured: " + exc.Message, "An error occured =-(", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    PKTRImporter = null;
                    backgroundWorker = null;
                }
                else
                {
                    MessageBox.Show("\"HEADER.data\" is absent in current directory. Please, make shure that files extracted by this program!", "HEADER.data is absent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExtractInputChooser_Click(object sender, EventArgs e)
        {
            OpenFileDialog ExtractFileDialog = new OpenFileDialog();
            ExtractFileDialog.Filter = "PTR files (*.ptr)|*.ptr";

            if (ExtractFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExtractInputTextBox.Text = ExtractFileDialog.FileName;

                FileInfo fi = new FileInfo(ExtractInputTextBox.Text);
                string exportDir = fi.Directory.FullName + "\\" + Path.GetFileNameWithoutExtension(fi.FullName);

                ExtractOutputTextBox.Text = exportDir;
            }
        }

        private void ExtractOutputChooser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ExtractFolderDialog = new FolderBrowserDialog();

            if (ExtractFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExtractOutputTextBox.Text = ExtractFolderDialog.SelectedPath;
            }
        }

        private void ImportInputChooser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog importFolderDialog = new FolderBrowserDialog();

            if (importFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImportInputTextBox.Text = importFolderDialog.SelectedPath;

                DirectoryInfo di = new DirectoryInfo(ImportInputTextBox.Text);
                string saveDir = di.FullName + ".ptr";

                ImportOutputTextBox.Text = saveDir;
            }
        }

        private void ImportOutputChooser_Click(object sender, EventArgs e)
        {
            SaveFileDialog importFileDialog = new SaveFileDialog();
            importFileDialog.FileName = "untitled";
            importFileDialog.Filter = "PTR (*.ptr)|*.ptr";

            if (importFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImportOutputTextBox.Text = importFileDialog.FileName;
            }
        }

        private void t1_OnProgressUpdate(int currentData, int maxData, string textStatus)
        {
            base.Invoke((Action)delegate
            {
                progressBar1.Refresh();

                progressBar1.Maximum = maxData;
                progressBar1.Value = currentData;
                String pbText = currentData.ToString() + " / " + maxData.ToString();
                using (Graphics gr = progressBar1.CreateGraphics())
                {
                    float halfTextWidth = gr.MeasureString(pbText, SystemFonts.DefaultFont).Width / 2.0F;
                    float halfTextHeight = gr.MeasureString(pbText, SystemFonts.DefaultFont).Height / 2.0F;

                    PointF textPoint = new PointF(progressBar1.Width / 2 - halfTextWidth, progressBar1.Height / 2 - halfTextHeight);

                    gr.DrawString(pbText, SystemFonts.DefaultFont, Brushes.Black, textPoint);
                }

                label1.Text = textStatus;
            });
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PKTRGeneral pktr = e.Argument as PKTRGeneral;

            pktr.Run();

            pktr = null;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Йой! Щось недобре сталося. Якщо ти це бачиш, то напиши мені що за гра та пакунок.", "Біда =-(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Export block
            ExtractInputChooser.Enabled = true;
            ExtractOutputChooser.Enabled = true;
            ExtractInputTextBox.Enabled = true;
            ExtractOutputTextBox.Enabled = true;
            ExtractBtn.Enabled = true;

            //Import Block
            ImportInputChooser.Enabled = true;
            ImportOutputChooser.Enabled = true;
            ImportInputTextBox.Enabled = true;
            ImportOutputTextBox.Enabled = true;
            ImportBtn.Enabled = true;

            progressBar1.Enabled = false;
            progressBar1.Value = 0;
            label1.Text = "";

            MessageBox.Show("Done!");
        }
    }
}
