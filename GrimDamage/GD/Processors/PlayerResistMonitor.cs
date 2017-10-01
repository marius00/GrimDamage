using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using GrimDamage.Tracking.Model;
using log4net;

namespace GrimDamage.GD.Processors {
    class PlayerResistMonitor : IMessageProcessor {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PlayerResistMonitor));
        private readonly DamageParsingService _damageParsingService;
        private readonly AppSettings _appSettings;

        public PlayerResistMonitor(DamageParsingService damageParsingService, AppSettings appSettings) {
            _damageParsingService = damageParsingService;
            _appSettings = appSettings;
        }

        public bool Process(MessageType type, byte[] data) {
            if (type == MessageType.TypeResistMonitor) {
                int entityId = IOHelper.GetInt(data, 0);
                ResistType resistType = (ResistType) IOHelper.GetInt(data, 4);
                float amount = IOHelper.GetFloat(data, 8);

                _damageParsingService.SetResist(entityId, resistType, amount);

                if (_appSettings.LogResistEntries) {
                    Logger.Debug($"Got a resist entry for {entityId}, {resistType}, {amount}");
                }
                return true;
            }

            return false;
        }
    }
}
