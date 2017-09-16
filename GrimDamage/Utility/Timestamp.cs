using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Utility {
    static class Timestamp {
        public static long UTCSeconds => ((DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000000);
        public static long UTCMilliseconds => ((DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000);

        public static long ToUtcMilliseconds(DateTime dt) => ((dt.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000);

        public static DateTime ToDateTimeFromMilliseconds(long utcMilliseconds) {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(utcMilliseconds);
        }
    }
}
