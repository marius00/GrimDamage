using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.GD.Dto {
    enum MessageType {
        DamageToVictim = 45001,
        CharacterMovement1 = 1001,
        CharacterMovement2 = 1002,
        CharacterMovement3 = 1003,
        CharacterMovement4 = 1004,
        CharacterMovement5 = 1005,
        CharacterMovement6 = 1006,
        LifeLeech = 45002,
        TotalDamage = 45003,
        SetAttackerName = 45004,
        SetDefenderName = 45005,
        SetAttackerId = 45006,
        SetDefenderId = 45007,
        Deflect = 45008,
        Absorb = 45009,
        Reflect = 45010,
        Block = 45011,
        LogUnrecognized = 45000,

        Pause = 20000,
        Unpause = 20001,
        PlayerIdDetected = 20002, // TODO: Hopefully this isnt called for both thyself and the host. If it is, hopefully the first call is always thyself. Still an issue when starting a new game though.
        PlayerDied = 20003,
        BeginStun = 2001,
        EndStun = 2002,
        BeginTrap = 2003,
        EndTrap = 2004,
        DisableMovement = 2005,
        SetLifeState = 2006,

    }
}
