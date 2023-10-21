using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game
{
    public static class Directories
    {
        public static string BaseDataDirectory;
        public static string SettingsFile;
        public static string LogsDirectory;

        static Directories()
        {
            BaseDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HorrorShorts fde4868a");
            LogsDirectory = Path.Combine(BaseDataDirectory, "Logs");
            SettingsFile = Path.Combine(BaseDataDirectory, "Settings.xml");
        }
    }
}
