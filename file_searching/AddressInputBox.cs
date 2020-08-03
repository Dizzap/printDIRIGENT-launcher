using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_searching {
    public partial class AddressInputBox : Form {
        public AddressInputBox() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            DataContainer.remoteConfigFolder = textBox1.Text;
            DataContainer.remoteConfigFileAddress = textBox1.Text + @"\setting.meh";
            DataContainer.remoteDataFolder = textBox2.Text;
            this.Close();
            //return System.Windows.Forms.DialogResult.OK;
        }

        private void AddressInputBox_Load(object sender, EventArgs e) {
            textBox1.Text = DataContainer.remoteConfigFolder;
            textBox2.Text = DataContainer.remoteDataFolder;
        }
    }
}
