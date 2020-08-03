using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Security;
using System.Windows;

namespace file_searching {
    public partial class repair_modal : Form {
        public repair_modal() {
            InitializeComponent();
        }

        private void repairButton_Click(object sender, EventArgs e) {
            try {
                File.Delete(DataContainer.oldLocalFile);
                Application.Restart();
            }
            catch (Exception installEx) {
                MessageBox.Show(Convert.ToString(installEx));
            }
        }
        string GetFile(string path) {
            try {
                if (File.Exists(path + DataContainer.FileName + DataContainer.exts[1])) {
                    return path + DataContainer.FileName + DataContainer.exts[1];
                }
                else if (File.Exists(path + DataContainer.FileName + DataContainer.exts[0])) {
                    return path + DataContainer.FileName + DataContainer.exts[0];
                }
                return null;
            }
            catch {
                return null;
            }
        }

        private void lastButton_Click(object sender, EventArgs e) {
            DataContainer.backupFile = GetFile(DataContainer.backupFolder);
            try {
                Process.Start(DataContainer.backupFile);
                Application.Exit();
            }
            catch (Exception ex) {
                MessageBox.Show(Convert.ToString(ex));
            }            
        }
        private void cancelButton_Click(object sender, EventArgs e) {
            DataContainer.RTDmodal = true;
            this.Close();
        }
    }
}
