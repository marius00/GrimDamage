using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using EvilsoftCommons.DllInjector;
using EvilsoftCommons.Exceptions;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using GrimDamage.Statistics.Service;
using log4net;
// ReSharper disable NotAccessedField.Local

namespace GrimDamage.GD.Processors {
    class MessageProcessorCore : IDisposable {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageProcessorCore));
        private RegisterWindow _window;
        private InjectionHelper _injector;
        private ProgressChangedEventHandler _injectorCallbackDelegate;
        private bool _isFirstMessage = true;
        private readonly Action<RegisterWindow.DataAndType> _registerWindowDelegate;
        private readonly List<IMessageProcessor> _processors;
        private readonly AppSettings _appSettings;

        public delegate void HookActivationCallback(object sender, EventArgs e);

        public event HookActivationCallback OnHookActivation;

        public MessageProcessorCore(
            DamageParsingService damageParsingService, 
            PositionTrackerService positionTrackerService,
            GeneralStateService generalStateService,
            AppSettings appSettings
        ) {
            _processors = new List<IMessageProcessor> {
                new GdLogMessageProcessor(appSettings, damageParsingService),
                new PlayerPositionTrackerProcessor(positionTrackerService, appSettings),
                new GdGameEventProcessor(generalStateService),
                new PlayerDetectionProcessor(damageParsingService, appSettings)
            };

            _registerWindowDelegate = CustomWndProc;
            _injectorCallbackDelegate = InjectorCallback;
            _window = new RegisterWindow("GDDamageWindowClass", _registerWindowDelegate);
            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "Hook.dll");
            _appSettings = appSettings;
        }


        private void CustomWndProc(RegisterWindow.DataAndType bt) {
            if (Thread.CurrentThread.Name == null) {
                Thread.CurrentThread.Name = "Core";
                ExceptionReporter.EnableLogUnhandledOnThread();
            }

            if (_isFirstMessage) {
                Logger.Debug("Window message received");
                _isFirstMessage = false;
                OnHookActivation?.Invoke(null, null);
            }


            MessageType type = (MessageType) bt.Type;
            foreach (var processor in _processors) {
                if (processor.Process(type, bt.Data)) {
                    if (_appSettings.LogProcessedMessages) {
                        Logger.Debug($"Processor {processor.GetType().ToString()} handled message");
                    }
                    return;
                }
            }

            Logger.Warn($"Got a message of type {bt.Type}");
        }

        private void InjectorCallback(object sender, ProgressChangedEventArgs e) {
            //Logger.Debug("Injector callback");
        }

        public void Dispose() {
            _injector?.Dispose();
            _injector = null;
        }
    }
}
