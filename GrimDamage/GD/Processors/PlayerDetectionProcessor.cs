using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Service;

namespace GrimDamage.GD.Processors {
    class PlayerDetectionProcessor : IMessageProcessor {
        private readonly DamageParsingService _damageParsingService;

        public PlayerDetectionProcessor(DamageParsingService damageParsingService) {
            _damageParsingService = damageParsingService;
        }

        public bool Process(MessageType type, byte[] data) {
            if (type == MessageType.PlayerIdDetected) {
                bool isPrimary = data[0] != 0;
                int id = IOHelper.GetInt(data, 1);

                _damageParsingService.SetPlayerInfo(id, isPrimary);
                return true;
            }

            return false;
        }
    }
}
