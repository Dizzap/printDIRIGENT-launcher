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

namespace file_searching {
    public static class DataContainer {
        public static string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string serverAddress = @"\\192.168.1.110";
        public static string remoteConfigPath = @"\NASfile\pDClient\pConf";
        public static string remoteConfigFolder = serverAddress + remoteConfigPath;
        public static string localConfigPath = @"\pDClient\pConf";
        public static string localConfigFolder = userPath + localConfigPath;

        public static string remoteConfigFileAddress = remoteConfigFolder + @"\setting.meh";                

        public static string oldRemoteFolder;
        //public static string oldLocalFolder;

        public static string remoteDataFolder;
        public static string localDataFolder = userPath + @"\pDClient\pData";
        public static string backupFolder = localDataFolder + @"\backup";
      
        public static string oldLocalFile;
        public static string newLocalFile;
        public static string remoteFile;
        public static string backupFile;

        public static double localVersion;
        public static double remoteVersion;

        public static bool[] RTL = new bool[3]; //[0] = localMissing; [1] = remoteMissing; [2] = getNewVersion

        public static bool RTDtimer = false;
        public static bool RTDcode = false;
        public static bool RTDmodal = true;

        public static string FileName = @"\PrintDIRIGENT2015aa";
        public static string[] exts = {".accdb", ".accde"};
        public static string test;
       
    }
}
