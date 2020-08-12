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

namespace file_searching
{
    partial class Form1 : Form {
        //StreamReader sr;
        StreamWriter sw;        
        //int RTL = 0;      
        //int index = 0;
        

        //string[] que = new string [10];
        //int arraynum=0;

        List<string> reportQueue = new List<string>();

        int a=0;

        public Form1() {
            InitializeComponent();
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
        double GetVersion(string manifestPath) {
                try { ///zkus najít soubor
                    StreamReader verFile = new StreamReader(manifestPath); ///připojení na soubor, pokud projde, zjisti verzi
                    string version = verFile.ReadLine(); ///čtení verze
                    verFile.Close();
                    return Convert.ToDouble(version); ///vratí verzi                                   
                }
                catch {
                    return -99; ///vrátí -99 pokud se nelze připojit k souboru
                }
                
        }
        void StartCheck() {
            int a;
            do {
                a = NetworkTest();
                if ((a == 2) || (a == -99)) {
                    MessageBox.Show("Bez zadání přihlašovacích údajů nelze pokračovat, program se vypne.");
                    Environment.Exit(1);
                }
                if (a == 1)
                    MessageBox.Show("Zadejte platné údaje!");
            } while (a == 1);
        }
        int NetworkTest() {
            try { ///zkouška sítě
                StreamReader conf = new StreamReader(DataContainer.remoteConfigFileAddress);
                string a = conf.ReadLine();
                conf.Close();
                return 0; /// síť je v pořádku
            }
            catch {
                try {
                    StreamReader conf = new StreamReader(DataContainer.localConfigFolder + @"\setting.meh");
                    DataContainer.remoteDataFolder = conf.ReadLine();
                    conf.Close();
                   // DataContainer.remoteConfigFileAddress = DataContainer.localConfigFolder + @"\setting.meh";
                    string [] parse = DataContainer.remoteDataFolder.Split('\\');
                    DataContainer.serverAddress = @"\\" + parse[2];
                    DataContainer.remoteConfigFolder = DataContainer.serverAddress + DataContainer.remoteConfigPath;
                    StreamReader conf2 = new StreamReader(DataContainer.remoteConfigFolder + @"\setting.meh");
                    string a = conf2.ReadLine();
                    conf2.Close();                    
                    return 0; /// síť je v pořádku
                }
                catch {
                   
                    //string message = "Nelze vytvořit připojení k serveru, jste připojeni k síti?";
                    string message = DataContainer.reportMessages["ServerConnectionError"];
                    string caption = "Server";
                    //MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        //zadání cesty
                        AddressInputBox ai = new AddressInputBox();
                        ai.ShowDialog();

                        try
                        {
                            StreamReader conf = new StreamReader(DataContainer.remoteConfigFileAddress);
                            DataContainer.remoteDataFolder = conf.ReadLine();
                            conf.Close();
                            return 0;
                        }
                        catch
                        {
                            ///credential test
                            Process.Start(DataContainer.serverAddress);
                            string message2 = "Jsou údaje úspěšně zadány?";
                            string caption2 = "Server";
                            DialogResult result2;

                            result2 = MessageBox.Show(message2, caption2, MessageBoxButtons.YesNo);

                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {
                                try
                                { ///test sítě
                                    StreamReader conf = new StreamReader(DataContainer.remoteConfigFileAddress);
                                    DataContainer.remoteDataFolder = conf.ReadLine();
                                    conf.Close();
                                    return 0;
                                }
                                catch
                                {
                                    MessageBox.Show("Kontaktujte správce", "Chyba"); ///kritická chyba
                                }
                            }
                            else
                            {
                                return -99; ///storno
                            }
                        }

                    }
                    return -99; ///storno
                }
                
            }
            
        }

        void ThreadForForm() {
            AddressInputBox aib = new AddressInputBox();
            aib.ShowDialog();
        }
        /*DateTime GetLog(string path) {
            DateTime lastLaunch;
            StreamReader sr = new StreamReader(path);
     
            lastLaunch = Convert.ToDateTime(sr.ReadLine());
            return lastLaunch;
        }*/
        void MainCode() {                      
            
            if (!Directory.Exists(DataContainer.userPath + @"\pDClient")) {
                DirectoryInfo di = Directory.CreateDirectory(DataContainer.userPath + @"\pDClient");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            if (!Directory.Exists(DataContainer.localConfigFolder)) {
                DirectoryInfo di2 = Directory.CreateDirectory(DataContainer.userPath + @"\pDClient\pConf");
                di2.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (!Directory.Exists(DataContainer.userPath + @"\pDClient\pData")) {
                DirectoryInfo di3 = Directory.CreateDirectory(DataContainer.userPath + @"\pDClient\pData");
                di3.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            
            if (!File.Exists(DataContainer.localConfigFolder + @"\pVerManifest.meh")) {
                File.Create(DataContainer.localConfigFolder + @"\pVerManifest.meh").Close();
            }
            DataContainer.localDataFolder = DataContainer.userPath + @"\pDClient\pData";
            

            try {
                DataContainer.remoteConfigFileAddress = DataContainer.remoteConfigFolder + @"\setting.meh"; //test
                StreamReader conf = new StreamReader(DataContainer.remoteConfigFileAddress);
                DataContainer.remoteDataFolder = conf.ReadLine();
                conf.Close();
            }                                   
            catch (Exception ex) {
                //MessageBox.Show(Convert.ToString(ex));                
                try {
                    StreamReader conf = new StreamReader(DataContainer.localConfigFolder + @"\setting.meh");
                    DataContainer.remoteDataFolder = conf.ReadLine();
                    DataContainer.remoteConfigFolder = conf.ReadLine();
                    conf.Close();
                }
                catch {
                    MessageBox.Show(Convert.ToString(ex));
                    Thread th = new Thread(new ThreadStart(ThreadForForm));
                    th.Start();
                    while (th.IsAlive) {
                        Thread.Sleep(100);
                    }
                    StreamWriter conf = new StreamWriter(DataContainer.localConfigFolder + @"\setting.meh");
                    conf.WriteLine(DataContainer.remoteDataFolder);
                    conf.Close();
                }

            }
            
            try {
                StreamReader oldConf = new StreamReader(DataContainer.localConfigFolder + @"\setting.meh");
                DataContainer.oldRemoteFolder = oldConf.ReadLine();
                //DataContainer.oldLocalFolder = oldConf.ReadLine();
                oldConf.Close();
            }
            catch {
                DataContainer.oldRemoteFolder = null;            
            }

            if ((DataContainer.remoteDataFolder != DataContainer.oldRemoteFolder)) {
                         
               StreamWriter sw2 = new StreamWriter(DataContainer.localConfigFolder + @"\setting.meh");
               sw2.WriteLine(DataContainer.remoteDataFolder);
               //sw2.WriteLine(DataContainer.localFolder);
               sw2.Close();
            }
            /*if (!File.Exists(DataContainer.localConfigFolder + @"\launchLog.meh")) {
                File.Create(DataContainer.localConfigFolder + @"\launchLog.meh").Close();
            }
            else {
                try {
                    StreamReader srr = new StreamReader(DataContainer.localConfigFolder + @"\launchLog.meh");

                }
                catch { }
            }*/
            DataContainer.remoteFile = GetFile(DataContainer.remoteDataFolder);
            DataContainer.oldLocalFile = GetFile(DataContainer.localDataFolder);

            if (DataContainer.oldLocalFile == null) {
                DataContainer.RTL[0] = true;
            }
            else
                DataContainer.RTL[0] = false;

            if (DataContainer.remoteFile == null) {
                DataContainer.RTL[1] = true;
            }
            else {
                DataContainer.RTL[1] = false;
                string[] parsedString = DataContainer.remoteFile.Split('\\');
                DataContainer.newLocalFile = DataContainer.localDataFolder + "\\" + parsedString[parsedString.Length - 1];
            }  
            DataContainer.localVersion = GetVersion(DataContainer.localConfigFolder + @"\pVerManifest.meh"); ///zjištění lokální verze 
            DataContainer.remoteVersion = GetVersion(DataContainer.remoteConfigFolder + @"\pVerManifest.meh"); ///zjištění vzdálené verze
            //que[arraynum] = "Zjišťování verzí..."; ///přidání textu do výstupu
            //arraynum++; ///navýšení počtu výstupu
            reportQueue.Add(DataContainer.reportMessages["VersionCheck"]);
            if (DataContainer.remoteVersion == -99) { 
                    //AddressInputBox whee = new AddressInputBox(); ///vyvolá AddressInputBox
                    //whee.Show();
                    StreamWriter conf = new StreamWriter(DataContainer.localConfigFolder + @"\setting.meh");
                    Thread th = new Thread(new ThreadStart(ThreadForForm));
                    th.Start();
                    while (th.IsAlive) {
                        Thread.Sleep(100);
                    }
                    try {
                        DataContainer.remoteVersion = GetVersion(DataContainer.remoteConfigFolder + @"\pVerManifest.meh"); ///zjištění vzdálené verze
                        DataContainer.remoteFile = GetFile(DataContainer.remoteDataFolder);
                        DataContainer.RTL[1] = false;
                        string[] parsedString = DataContainer.remoteFile.Split('\\');
                        DataContainer.newLocalFile = DataContainer.localDataFolder + "\\" + parsedString[parsedString.Length - 1];                        
                        conf.WriteLine(DataContainer.remoteDataFolder);
                        conf.WriteLine(DataContainer.remoteConfigFolder);
                    }
                    finally {
                        conf.Close();
                    }
            }
            if (DataContainer.localVersion < DataContainer.remoteVersion) {                
                DataContainer.RTL[2] = true; ///update je potřeba
            }
            else {                
                DataContainer.RTL[2] = false; ///update není potřeba
            }
            sw = new StreamWriter(DataContainer.localConfigFolder + @"\log.txt", true);  ///logy

            Thread.Sleep(3000);
            if (DataContainer.RTDmodal == true) {
                Launch();
            }
            else {
                while (!DataContainer.RTDmodal == false) {
                    Thread.Sleep(100);
                }
            }
        }
        void Launch() {
            switch (DataContainer.RTL[2]) {
                    case (true):
                        if (DataContainer.RTL[1] == true) {
                            if (DataContainer.RTL[0] == true) {
                                MessageBox.Show(DataContainer.reportMessages["TotalError"], "Chyba");
                                //que[arraynum] = "Systémová chyba.";
                                //arraynum++;
                                reportQueue.Add(DataContainer.reportMessages["SystemError"]);
                                //error + vypnutí, asi i tlačítko
                            }
                            else {
                                //MessageBox.Show("While update: Only remote missing");
                                //modál s Y/N, zapni lokál
                                try {
                                    //que[arraynum] = "Serverová chyba.";
                                    //arraynum++;
                                    reportQueue.Add(DataContainer.reportMessages["ServerError"]);
                                    string message = DataContainer.reportMessages["ServerErrorRepeated"];
                                    string caption = DataContainer.reportMessages["ServerError"];
                                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                    DialogResult result;
                                    result = MessageBox.Show(message, caption, buttons);

                                    if (result == System.Windows.Forms.DialogResult.Yes) {
                                        //que[arraynum] = "Spouštím...";
                                        //arraynum++;
                                        reportQueue.Add(DataContainer.reportMessages["Start"]);
                                        Task.Delay(1000).ContinueWith(t => Process.Start(DataContainer.oldLocalFile));

                                        //Task.Delay(2800).ContinueWith(t => Application.Exit());
                                    }
                                    //Task.Delay(500).ContinueWith(t => Application.Exit());
                                    //button1.Visible = true;
                                }
                                catch (Exception ex) {
                                    MessageBox.Show(Convert.ToString(ex));
                                    sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": " + Convert.ToString(ex));
                                    sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": update_remoteNotPresent_oldLaunchFailed");
                                }
                                finally { }
                            }
                        }
                        else {
                            //MessageBox.Show("While update: Remote not missing");
                            //cool, stáhni to, spusť
                            try {
                                //que[arraynum] = "Nová verze k dispozici, stahuji...";
                                //arraynum++;
                                reportQueue.Add(DataContainer.reportMessages["NewVersion"]);
                                //File.Copy(DataContainer.oldLocalFile, DataContainer.localFolder + @"\backup\" + Path.GetFileName(DataContainer.oldLocalFile), true);
                                File.Copy(DataContainer.remoteFile, DataContainer.newLocalFile, true);
                                //que[arraynum] = "Spouštím...";
                                //arraynum++;
                                reportQueue.Add(DataContainer.reportMessages["Start"]);
                                Task.Delay(1000).ContinueWith(t => Process.Start(DataContainer.newLocalFile));
                                File.Copy(DataContainer.remoteConfigFolder + @"\pVerManifest.meh", DataContainer.localConfigFolder + @"\pVerManifest.meh", true);

                            }
                            catch (Exception ex) {
                                MessageBox.Show(Convert.ToString(ex));
                                sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": " + Convert.ToString(ex));
                                sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": update_remotePresent_download/launchFailed");
                            }
                            finally {
                                sw.Close();
                            }
                        }
                        break;
                    case (false):
                        if (DataContainer.RTL[0] == true) {
                            if (DataContainer.RTL[1] == true) {
                                MessageBox.Show(DataContainer.reportMessages["TotalError"], "Chyba");
                               // que[arraynum] = "Systémová chyba.";
                                //arraynum++;
                                reportQueue.Add(DataContainer.reportMessages["SystemError"]);
                            }
                            else {
                                try {
                                   // que[arraynum] = "Chyba souboru na straně klienta.";
                                   // arraynum++;
                                    reportQueue.Add(DataContainer.reportMessages["ClientFileError"]);
                                    //sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": case 2 (Before Down)");
                                   // que[arraynum] = "Probíhá oprava...";
                                    //arraynum++;
                                    reportQueue.Add(DataContainer.reportMessages["Repair"]);
                                    File.Copy(DataContainer.remoteFile, DataContainer.newLocalFile, true);
                                  //  que[arraynum] = "Spouštím...";
                                    //arraynum++;
                                    reportQueue.Add(DataContainer.reportMessages["Start"]);
                                    Task.Delay(1000).ContinueWith(t => Process.Start(DataContainer.newLocalFile));
                                }
                                catch (Exception ex) {
                                    MessageBox.Show(Convert.ToString(ex));
                                    //sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": " + Convert.ToString(ex));
                                    sw.WriteLine("{0} {1}: {2}", Environment.UserName, DateTime.Now, Convert.ToString(ex));                                
                                    //sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": noUpdate_localNotPresent_launch/downloadFailed");
                                    sw.WriteLine("{0} {1}: noUpdate_localNotPresent_launch/downloadFailed", Environment.UserName, DateTime.Now);                            
                                }
                                finally { sw.Close(); }
                            }
                        }
                        else {
                            try {
                                //que[arraynum] = "Vše OK.";
                                //arraynum++;
                                reportQueue.Add(DataContainer.reportMessages["OK"]);
                               // que[arraynum] = "Spouštím...";
                                //arraynum++;
                                reportQueue.Add(DataContainer.reportMessages["Start"]);
                                Task.Delay(1000).ContinueWith(t => Process.Start(DataContainer.oldLocalFile));
                            }
                            catch (Exception ex) {
                                MessageBox.Show(Convert.ToString(ex));
                                //sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": " + Convert.ToString(ex));
                                sw.WriteLine("{0} {1}: {2}",Environment.UserName, DateTime.Now, Convert.ToString(ex));
                                //sw.WriteLine(Environment.UserName + " " + DateTime.Now + ": noUpdate_localPresent_launchFailed");
                                sw.WriteLine("{0} {1}: noUpdate_localPresent_launchFailed", Environment.UserName, DateTime.Now);
                            }
                            finally {
                                sw.Close();
                            }
                        }
                        break;                
            }
                DataContainer.RTDcode = true;
                sw.Close();
            
        }       
        private void timer2_Tick(object sender, EventArgs e) {
            /*if (a <= arraynum && que[a] != null) {
                label1.Text = que[a];
                a++;
            }
            else
                DataContainer.RTDtimer = true;*/
            foreach (var c in reportQueue) {
                label1.Text = c;
            }
            DataContainer.RTDtimer = true;
        }
        private void button1_Click(object sender, EventArgs e) {
            Environment.Exit(1);
        }
        private void Form1_Shown(object sender, EventArgs e) {
            Thread networdCheckThread = new Thread(new ThreadStart(StartCheck));
            networdCheckThread.Start();
            networdCheckThread.Join();
           
            Thread mainCodeThread = new Thread(new ThreadStart(MainCode));
            mainCodeThread.Start();            
            
        }
        private void timer1_Tick(object sender, EventArgs e) {
            if((DataContainer.RTDtimer == true) && (DataContainer.RTDcode == true) && (DataContainer.RTDmodal == true))
                Task.Delay(2000).ContinueWith(t => Application.Exit());
        }

        private void label2_Click(object sender, EventArgs e) {

        }
        private void label3_Click(object sender, EventArgs e) {
           
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            
            if (e.KeyCode == Keys.R && e.Control) {
                DataContainer.RTDmodal = false;
                
                repair_modal rm = new repair_modal();
                rm.ShowDialog();
            }
        }       
    }
}
