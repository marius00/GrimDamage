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


        public static string BaseFolder {
            get {
                string appdata = Environment.GetEnvironmentVariable("LocalAppData");
                string dir = Path.Combine(appdata, "EvilSoft", "GDDamage");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                return dir;
            }
        }

    }
}
