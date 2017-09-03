using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Parser.Model {
    public enum EventType {
        DamageDealt,
        LifeLeech,
        SetAttackerName,
        SetAttackerId,
        SetDefenderName,
        SetDefenderId,
        Deflect,
        SetDOT,
        SetArmorAbsorb,
        SetFailedDeflect,
    }
}
