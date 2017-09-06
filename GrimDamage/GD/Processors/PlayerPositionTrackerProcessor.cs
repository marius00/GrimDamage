using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using GrimDamage.GD.Dto;
using GrimDamage.Statistics.model;
using GrimDamage.Statistics.Service;

namespace GrimDamage.GD.Processors {
    class PlayerPositionTrackerProcessor : IMessageProcessor {
        private readonly PositionTrackerService _positionTrackerService;
        public PlayerPositionTrackerProcessor(PositionTrackerService positionTrackerService) {
            _positionTrackerService = positionTrackerService;
        }

        public bool Process(MessageType type, byte[] data) {
            if (type == MessageType.CharacterMovement) {
                _positionTrackerService.SetPlayerPosition(new PlayerPosition {
                    X = IOHelper.GetFloat(data, 4),
                    Y = IOHelper.GetFloat(data, 8),
                    Z = IOHelper.GetFloat(data, 12),
                    Zone = IOHelper.GetInt(data, 0)
                });

                return true;
            }

            return false;
        }
    }
}
