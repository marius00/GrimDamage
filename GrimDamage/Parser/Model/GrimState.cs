using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Parser.Model {
    public enum GrimState {
        Pause, Unpause, Dying,
        BeginStun, EndStun,
        BeginTrap, EndTrap,
        DisableMovement
    }
}
