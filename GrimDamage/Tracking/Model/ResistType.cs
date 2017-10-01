using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    public enum ResistType {
        Pierce = 4,
        Aether = 11,
        Poison = 7,
        Stun = 42,
        Bleed = 15,
        Cold = 5,
        Fire = 6,
        Chaos = 10,
        Lightning = 8,
        Vitality = 9,
        Unknown1 = 38,
        Unknown2 = 39,
        Freeze = 45,
        Root = 44, //SkillBuff_DebufTrap
        Reflection = 62,
        DamageReductionByShield = 55,
        DesignerCalculateMeleeBlockChance = 54,
        Physical = 56, // Assuming physical resist, code says "blunt"
        UnknownSpeed1 = 17, // 13 => 17
        UnknownSpeed2 = 18, // 14 => 18
        UnknownSpeed3 = 19, // 12 => 19
    }
}
