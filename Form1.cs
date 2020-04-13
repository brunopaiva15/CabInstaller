using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CabInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxOpen.Text = openFileDialog1.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Ouvrir fichier CAB";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnStart.Text = "Installation in progress....";

            bool exists = System.IO.Directory.Exists(@"C:\Temp_CAB");

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(@"C:\Temp_CAB");
            }

            bool exists2 = System.IO.Directory.Exists(@"C:\Temp_CAB\INF_FILES");

            if (!exists2)
            {
                System.IO.Directory.CreateDirectory(@"C:\Temp_CAB\INF_FILES");
            }

            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/k echo F|xcopy /S /Q /Y /F \"" + tbxOpen.Text + "\" C:\\Temp_CAB\\DRIVERS_TO_INSTALL.cab* & exit";
            proc.StartInfo.ErrorDialog = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();

            Process proc2 = new Process();
            proc2.StartInfo.FileName = "cmd.exe";
            proc2.StartInfo.Arguments = "/k expand C:\\Temp_CAB\\DRIVERS_TO_INSTALL.cab -F:* C:\\Temp_CAB\\INF_FILES & exit";
            proc2.StartInfo.ErrorDialog = true;
            proc2.StartInfo.Verb = "runas";
            proc2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc2.Start();
            proc2.WaitForExit();

            Process proc3 = new Process();
            proc3.StartInfo.FileName = "cmd.exe";
            proc3.StartInfo.WorkingDirectory = "C:\\Temp_CAB\\INF_FILES";
            proc3.StartInfo.Arguments = "/k for /f %i in ('dir /b /s *.inf') do pnputil.exe -i -a %i & exit";
            proc3.StartInfo.ErrorDialog = true;
            proc3.StartInfo.Verb = "runas";
            proc3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc3.Start();
            proc3.WaitForExit();

            btnStart.Text = "Installation finished";

            Cursor.Current = Cursors.Default;
        }
    }
}
