using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using GrimDamage.GD.Dto;
using GrimDamage.Statistics.model;
using GrimDamage.Statistics.Service;
using log4net;

namespace GrimDamage.GD.Processors {
    class PlayerPositionTrackerProcessor : IMessageProcessor {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PlayerPositionTrackerProcessor));
        private readonly PositionTrackerService _positionTrackerService;
        public PlayerPositionTrackerProcessor(PositionTrackerService positionTrackerService) {
            _positionTrackerService = positionTrackerService;
        }

        public bool Process(MessageType type, byte[] data) {
            switch (type) {
                case MessageType.CharacterMovement1:
                case MessageType.CharacterMovement2:
                case MessageType.CharacterMovement3:
                case MessageType.CharacterMovement4:
                case MessageType.CharacterMovement5:
                case MessageType.CharacterMovement6: 
                    {

                        int pos = 16;
                        int _trash = IOHelper.GetInt(data, pos);
                        pos += 4;
                        int b = IOHelper.GetInt(data, pos);
                        pos += 4;
                        int c = IOHelper.GetInt(data, pos);
                        pos += 4;
                        int d = IOHelper.GetInt(data, pos);

                        _positionTrackerService.SetPlayerPosition(new PlayerPosition {
                            X = IOHelper.GetFloat(data, 4),
                            Y = IOHelper.GetFloat(data, 8),
                            Z = IOHelper.GetFloat(data, 12),
                            Zone = b
                        });

                        Logger.Debug($"Received a {type}({b}, {c}, {d} => ({IOHelper.GetFloat(data, 4)}, {IOHelper.GetFloat(data, 8)}, {IOHelper.GetFloat(data, 12)}, {IOHelper.GetInt(data, 0)})");
                        //Logger.Debug($"Received a TYPE_Move({fb}, {fc}, {fd} =>");
                    }

                    
                   

                    return true;
            }

            return false;
        }
    }
}
