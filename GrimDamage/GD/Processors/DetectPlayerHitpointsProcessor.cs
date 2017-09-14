using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using EvilsoftCommons.Exceptions;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using log4net;

namespace GrimDamage.GD.Processors {
    class DetectPlayerHitpointsProcessor : IMessageProcessor {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DetectPlayerHitpointsProcessor));
        private readonly DamageParsingService _damageParsingService;
        private readonly AppSettings _appSettings;

        public DetectPlayerHitpointsProcessor(DamageParsingService damageParsingService, AppSettings appSettings) {
            _damageParsingService = damageParsingService;
            _appSettings = appSettings;
        }


        public bool Process(MessageType type, byte[] data) {
            switch (type) {
                case MessageType.TYPE_PlayerHealthOffsetDetected:
                    Logger.Info("Player health offset has been successfully detected");
                    return true;

                case MessageType.TYPE_ErrorDetectingPlayerHealthOffset:
                    Logger.Warn("Player health offset could not be detect, health graphs will be unavailable");
                    ExceptionReporter.ReportIssue("No health offset");

                    StringBuilder hex = new StringBuilder(data.Length * 2);
                    foreach (byte b in data)
                        hex.AppendFormat("{0:x2} ", b);
                    Logger.Debug(hex.ToString());

                    return true;

                case MessageType.TYPE_HitpointMonitor: {
                        int entity = IOHelper.GetInt(data, 0);
                        float hp = IOHelper.GetFloat(data, 4);

                        if (_appSettings.LogEntityHitpointEvent) {
                            Logger.Info($"Entity {entity} has {hp} hitpoints.");
                        }

                        _damageParsingService.SetHealth(entity, hp);

                }
                    return true;
            }

            return false;
        }
    }
}
