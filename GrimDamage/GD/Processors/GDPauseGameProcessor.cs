using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Model;
using GrimDamage.Parser.Service;

namespace GrimDamage.GD.Processors {
    class GDPauseGameProcessor : IMessageProcessor {
        private GeneralStateService _generalStateService;

        public GDPauseGameProcessor(GeneralStateService generalStateService) {
            _generalStateService = generalStateService;
        }
        public bool Process(MessageType type, byte[] data) {
            if (type == MessageType.Pause) {
                _generalStateService.PushState(GrimState.Pause);
                return true;
            }
            else if (type == MessageType.Unpause) {
                _generalStateService.PushState(GrimState.Unpause);
                return true;
            }

            return false;
        }
    }
}
