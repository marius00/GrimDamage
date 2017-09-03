using log4net;
//using EvilsoftCommons.Exceptions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace EvilsoftCommons.DllInjector {
    public class InjectionHelper : IDisposable {
        private static ILog logger = LogManager.GetLogger(typeof(InjectionHelper));
        private BackgroundWorker bw;
        private HashSet<uint> previouslyInjected = new HashSet<uint>();
        private HashSet<uint> dontLog = new HashSet<uint>();
        private Dictionary<uint, IntPtr> pidModuleHandleMap = new Dictionary<uint, IntPtr>();
        private bool unloadOnExit;
        private RunArguments ExitArguments;

        public const int INJECTION_ERROR = 0;
        public const int NO_PROCESS_FOUND_ON_STARTUP = 1;
        public const int NO_PROCESS_FOUND = 2;

        private ProgressChangedEventHandler registeredProgressCallback;

        class RunArguments {
            public string WindowName;
            public string ClassName;
            public string DllName;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressChanged">Callback for errors and notifications</param>
        /// <param name="unloadOnExit">Whetever this DLL should get unloaded on exit</param>
        /// <param name="windowName">Name of the window you wish to inject to</param>
        /// <param name="className">Name of the class you wish to inject to (IFF window name is empty/null)</param>
        /// <param name="dll">Name of the DLL you wish to inject</param>
        public InjectionHelper(BackgroundWorker bw, ProgressChangedEventHandler progressChanged, bool unloadOnExit, string windowName, string className, string dll) {
            if (string.IsNullOrEmpty(windowName) && string.IsNullOrEmpty(className)) {
                throw new ArgumentException("Either window or class name must be specified");
            }
            else if (string.IsNullOrEmpty(dll)) {
                throw new ArgumentException("DLL name must be specified");
            }

            this.registeredProgressCallback = progressChanged;
            this.unloadOnExit = unloadOnExit;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += progressChanged;
            //bw.ProgressChanged += bw_ProgressChanged;

            ExitArguments = new RunArguments {
                WindowName = windowName,
                ClassName = className,
                DllName = dll
            };
            bw.RunWorkerAsync(ExitArguments);
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            
        }

        public void Cancel() {
            if (bw != null) {
                bw.CancelAsync();
                bw = null;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            
            try {
                BackgroundWorker worker = sender as BackgroundWorker;

                if (Thread.CurrentThread.Name == null)
                    Thread.CurrentThread.Name = "InjectionHelper";
                
                while (!worker.CancellationPending) {
                    process(worker, e.Argument as RunArguments);
                }
            }
            catch (Exception ex) {
                logger.Fatal(ex.Message);
                logger.Fatal(ex.StackTrace);
                //ExceptionReporter.ReportException(ex);
                throw;
            }
        }

        public void Dispose() {
            if (bw != null) {
                bw.ProgressChanged -= registeredProgressCallback;
                bw.CancelAsync();
                bw = null;
            }

            if (unloadOnExit) {
                // Unload the DLL from any still running instance
                HashSet<uint> pids = FindProcesses(ExitArguments);
                foreach (uint pid in pidModuleHandleMap.Keys) {
                    if (pids.Contains(pid)) {
                        if (DllInjector.UnloadDll(pid, pidModuleHandleMap[pid])) {
                            logger.InfoFormat("Unloaded module from pid {0}", pid);
                        }
                        else {
                            logger.InfoFormat("Failed to unload module from pid {0}", pid);
                        }
                    }
                }
            }
            else {
                logger.Info("Exiting without unloading DLL (as per configuration)");
            }
        }


        private HashSet<uint> FindProcesses(RunArguments args) {

            if (!string.IsNullOrEmpty(args.WindowName)) {
                return DllInjector.FindProcessForWindow(args.WindowName);
            }
            else {
                throw new NotSupportedException("Class name provided instead of window name, not yet implemented.");
            }
        }
        private void process(BackgroundWorker worker, RunArguments arguments) {
            System.Threading.Thread.Sleep(1200);

            HashSet<uint> pids = FindProcesses(arguments);
            
            
            if (pids.Count == 0 && previouslyInjected.Count == 0)
                worker.ReportProgress(NO_PROCESS_FOUND_ON_STARTUP, null);
            else if (pids.Count == 0)
                worker.ReportProgress(NO_PROCESS_FOUND, null);

            string dll = Path.Combine(Directory.GetCurrentDirectory(), arguments.DllName);
            if (!File.Exists(dll)) {
                logger.FatalFormat("Could not find {1} at \"{0}\"", dll, arguments.DllName);
            }
            else {
                foreach (uint pid in pids) {
                    if (!previouslyInjected.Contains(pid)) {


                        //DllInjector.adjustDebugPriv(pid);
                        IntPtr remoteModuleHandle = DllInjector.NewInject(pid, dll);
                        if (remoteModuleHandle == IntPtr.Zero) {
                            if (!dontLog.Contains(pid)) {
                                logger.WarnFormat("Could not inject dll into process {0}, if this is a recurring issue, try running as administrator.", pid);
                                worker.ReportProgress(INJECTION_ERROR, "Could not inject dll into process " + pid);
                            }
                        }
                        else {
                            if (!dontLog.Contains(pid))
                                logger.Info("Injected dll into process " + pid);

                            if (!InjectionVerifier.VerifyInjection(pid, arguments.DllName)) {
                                if (!dontLog.Contains(pid)) {
                                    logger.Warn("InjectionVerifier reports injection failed.");
                                    //ExceptionReporter.ReportIssue("InjectionVerifier reports injection failed into PID " + pid);
                                    worker.ReportProgress(INJECTION_ERROR, string.Format("InjectionVerifier reports injection failed into PID {0}, try running as administrator.", pid));
                                }


                                dontLog.Add(pid);
                            }
                            else {
                                logger.Info("InjectionVerifier reports injection succeeded.");
                                previouslyInjected.Add(pid);
                                pidModuleHandleMap[pid] = remoteModuleHandle;
                            }

                        }
                    }
                }
            }
        }

    }
}
