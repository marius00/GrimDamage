using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using AutoUpdaterDotNET;
using EvilsoftCommons.Exceptions;

namespace GrimDamage.Utilities {
    class AutoUpdateUtility : IDisposable {
        private System.Timers.Timer _timerReportUsage;
        private readonly Stopwatch _reportUsageStatistics;
        private DateTime _lastTimeNotMinimized = DateTime.Now;
        private DateTime _lastAutomaticUpdateCheck = default(DateTime);

        public AutoUpdateUtility() {
            _reportUsageStatistics = new Stopwatch();
            _reportUsageStatistics.Start();


#if !DEBUG
            ThreadPool.QueueUserWorkItem(m => ExceptionReporter.ReportUsage());
            CheckForUpdates();
#endif
        }

        [DllImport("kernel32")]
        private static extern UInt64 GetTickCount64();


        private void CheckForUpdates() {
            if (GetTickCount64() > 5 * 60 * 1000 && (DateTime.Now - _lastAutomaticUpdateCheck).TotalHours > 36) {
                AutoUpdater.LetUserSelectRemindLater = true;
                AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
                AutoUpdater.RemindLaterAt = 7;
                AutoUpdater.Start(UPDATE_XML);

                _lastAutomaticUpdateCheck = DateTime.Now;
            }
        }

        private string UPDATE_XML {
            get {
                var v = Assembly.GetExecutingAssembly().GetName().Version;
                string version = $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";

                return $"http://grimdawn.dreamcrash.org/damage/version.php?version={version}";
            }
        }

        private void ReportUsage() {
            if ((DateTime.Now - _lastTimeNotMinimized).TotalHours < 38) {
                if (_reportUsageStatistics.Elapsed.Hours > 12) {
                    _reportUsageStatistics.Restart();
                    ThreadPool.QueueUserWorkItem(m => ExceptionReporter.ReportUsage());
                    AutoUpdater.Start(UPDATE_XML);
                }
            }
        }

        public void StartReportUsageTimer() {

            _timerReportUsage = new System.Timers.Timer();
            _timerReportUsage.Start();
            _timerReportUsage.Elapsed += (a1, a2) => {
                if (Thread.CurrentThread.Name == null)
                    Thread.CurrentThread.Name = "ReportUsageThread";
                ReportUsage();
            };


            const int min = 1000 * 60;
            const int hour = 60 * min;
            _timerReportUsage.Interval = 12 * hour;
            _timerReportUsage.AutoReset = true;
            _timerReportUsage.Start();
        }

        public void Dispose() {
            _timerReportUsage?.Stop();
            _timerReportUsage?.Dispose();
            _timerReportUsage = null;
        }
    }
}
