using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons;
using EvilsoftCommons.DllInjector;
using GrimDamage.GD.Dto;
using GrimDamage.GD.Logger;
using GrimDamage.Parser.Service;
using GrimDamage.Statistics.Service;
using log4net;
using log4net.Repository.Hierarchy;

namespace GrimDamage.GD.Processors {
    class MessageProcessorCore : IDisposable {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageProcessorCore));
        private RegisterWindow _window;
        private InjectionHelper _injector;
        private ProgressChangedEventHandler _injectorCallbackDelegate;
        private bool _isFirstMessage = true;
        private readonly Action<RegisterWindow.DataAndType> _registerWindowDelegate;
        private readonly List<IMessageProcessor> _processors;

        public delegate void HookActivationCallback(object sender, EventArgs e);

        public event HookActivationCallback OnHookActivation;

        public MessageProcessorCore(
            DamageParsingService damageParsingService, 
            CombatFileLogger fileLogger, 
            PositionTrackerService positionTrackerService,
            GeneralStateService generalStateService
        ) {
            _processors = new List<IMessageProcessor> {
                new GdLogMessageProcessor(fileLogger, damageParsingService),
                new PlayerPositionTrackerProcessor(positionTrackerService),
                new GDPauseGameProcessor(generalStateService),
                new PlayerDetectionProcessor(damageParsingService)
            };

            _registerWindowDelegate = CustomWndProc;
            _injectorCallbackDelegate = InjectorCallback;
            _window = new RegisterWindow("GDDamageWindowClass", _registerWindowDelegate);
            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "Hook.dll");
        }


        private void CustomWndProc(RegisterWindow.DataAndType bt) {
            if (_isFirstMessage) {
                Logger.Debug("Window message received");
                _isFirstMessage = false;
                OnHookActivation?.Invoke(null, null);
            }


            MessageType type = (MessageType) bt.Type;
            foreach (var processor in _processors) {
                if (processor.Process(type, bt.Data))
                    return;
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
