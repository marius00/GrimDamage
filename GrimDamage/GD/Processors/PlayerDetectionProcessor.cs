using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using log4net;

namespace GrimDamage.GD.Processors {
    class PlayerDetectionProcessor : IMessageProcessor {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PlayerDetectionProcessor));
        private readonly DamageParsingService _damageParsingService;
        private readonly AppSettings _appSettings;

        public PlayerDetectionProcessor(DamageParsingService damageParsingService, AppSettings appSettings) {
            _damageParsingService = damageParsingService;
            _appSettings = appSettings;
        }

        public bool Process(MessageType type, byte[] data) {
            if (type == MessageType.PlayerIdDetected) {
                bool isPrimary = data[0] != 0;
                int id = IOHelper.GetInt(data, 1);

                _damageParsingService.SetPlayerInfo(id, isPrimary);

                if (_appSettings.LogPlayerDetection) {
                    Logger.Debug($"Player {id} has been detected as primary={isPrimary}");
                }
                return true;
            }

            return false;
        }
    }
}
