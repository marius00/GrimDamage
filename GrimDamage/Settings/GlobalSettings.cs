using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Settings {
    static class GlobalSettings {
        public static string LogPath => BaseFolder;

        public static string BineroHost => "http://ribbs.dreamcrash.org/gddamage";

        public static string SavedParsePath => CreateAndReturn(Path.Combine(BaseFolder, "SavedParses"));


        public static string BaseFolder {
            get {
                string appdata = Environment.GetEnvironmentVariable("LocalAppData");
                string dir = Path.Combine(appdata, "EvilSoft", "GDDamage");
                return CreateAndReturn(dir);
            }
        }

        public static string ItemAssistantFolder {
            get {
                string appdata = Environment.GetEnvironmentVariable("LocalAppData");
                string dir = Path.Combine(appdata, "EvilSoft", "iagd");
                return CreateAndReturn(dir);
            }
        }

        private static string CreateAndReturn(string path) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
