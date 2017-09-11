using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Model;
using GrimDamage.Parser.Service;
using EvilsoftCommons;

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
                    return true;
                case MessageType.BeginStun:
                    _generalStateService.PushState(GrimState.BeginStun);
                    return true;
                case MessageType.EndStun:
                    _generalStateService.PushState(GrimState.EndStun);
                    return true;
                case MessageType.BeginTrap:
                    _generalStateService.PushState(GrimState.BeginTrap);
                    return true;
                case MessageType.EndTrap:
                    _generalStateService.PushState(GrimState.EndTrap);
                    return true;
                case MessageType.DisableMovement:
                    _generalStateService.PushState(GrimState.DisableMovement);
                    return true;
                case MessageType.SetLifeState: 
                    {
                        int state = IOHelper.GetInt(data, 0);
                        switch (state) {
                            case 0:
                                _generalStateService.PushState(GrimState.Unknown);
                                break;

                            case 1:
                                _generalStateService.PushState(GrimState.Initializing);
                                break;

                            case 2:
                                _generalStateService.PushState(GrimState.Alive);
                                break;

                            case 3:
                                _generalStateService.PushState(GrimState.Dying);
                                break;

                            case 4:
                                _generalStateService.PushState(GrimState.Dead);
                                break;

                            case 5:
                                _generalStateService.PushState(GrimState.Respawning);
                                break;
                        }
                    }
                    return true;

            }

            return false;
        }
    }
}
