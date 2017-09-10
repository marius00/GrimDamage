using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Model;
using GrimDamage.Parser.Service;

namespace GrimDamage.GD.Processors {
    class GdGameEventProcessor : IMessageProcessor {
        private GeneralStateService _generalStateService;

        public GdGameEventProcessor(GeneralStateService generalStateService) {
            _generalStateService = generalStateService;
        }
        public bool Process(MessageType type, byte[] data) {
            switch (type) {
                case MessageType.Pause:
                    _generalStateService.PushState(GrimState.Pause);
                    return true;
                case MessageType.Unpause:
                    _generalStateService.PushState(GrimState.Unpause);
                    return true;
                case MessageType.PlayerDied:
                    _generalStateService.PushState(GrimState.Dying);
                    break;
                case MessageType.BeginStun:
                    _generalStateService.PushState(GrimState.BeginStun);
                    break;
                case MessageType.EndStun:
                    _generalStateService.PushState(GrimState.EndStun);
                    break;
                case MessageType.BeginTrap:
                    _generalStateService.PushState(GrimState.BeginTrap);
                    break;
                case MessageType.EndTrap:
                    _generalStateService.PushState(GrimState.EndTrap);
                    break;
                case MessageType.DisableMovement:
                    _generalStateService.PushState(GrimState.DisableMovement);
                    break;
            }

            return false;
        }
    }
}
