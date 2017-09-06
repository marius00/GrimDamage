using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.GD.Dto;

namespace GrimDamage.GD.Processors {
    interface IMessageProcessor {
        bool Process(MessageType type, byte[] data);
    }
}
