using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrimDamage.Parser.Model;
using GrimDamage.Tracking.Model;

namespace GrimDamage.Parser.Config {
    public static class EventMapping {

        public static Dictionary<EventType, Regex> RegexMap = new Dictionary<EventType, Regex>()
        {
            //{ EventType.DamageDealt, new Regex(@".*Damage (\d+\.\d+) to Defender 0x([A-Fa-f0-9]+) \(([A-Za-z]+)\)", RegexOptions.Compiled) },
            { EventType.DamageDealt, new Regex(@".*Damage (\d+[\.\,]\d+) to Defender 0x([A-Fa-f0-9]+) \(([A-Za-z]+)\)", RegexOptions.Compiled) },
            { EventType.LifeLeech, new Regex(@".*Life Leech return (\d+\.\d+) Life", RegexOptions.Compiled) },
            { EventType.SetAttackerName, new Regex(@"\s*attackerName = (.*)", RegexOptions.Compiled) },
            { EventType.SetAttackerId, new Regex(@"\s*attackerID = (\d+)", RegexOptions.Compiled) },
            { EventType.SetDefenderName, new Regex(@"\s*defenderName = (.*)", RegexOptions.Compiled) },
            { EventType.SetDefenderId, new Regex(@"\s*defenderID = (\d+)", RegexOptions.Compiled) },
            { EventType.Deflect, new Regex(@".*Deflect Projectile Chance \((\d+\.\d+)\) caused a deflection", RegexOptions.Compiled) },
            { EventType.SetDOT, new Regex(@"\s*Total Damage:  Absolute \((\d+\.\d+)\), Over Time \((\d+\.\d+)\)", RegexOptions.Compiled) }, // 1274+1128?
            { EventType.SetArmorAbsorb, new Regex(@"\s*protectionAbsorption = (\d+\.\d+)", RegexOptions.Compiled) },
            { EventType.SetFailedDeflect, new Regex(@".*Deflect Projectile Chance \((\d+\.\d+)\) not met \((\d+\.\d+\))", RegexOptions.Compiled) },
        };

        public static Dictionary<EventType, string> PatternMap = new Dictionary<EventType, string>()
        {
            //{ EventType.DamageDealt, @".*Damage (\d+\\.\d+) to Defender 0x([A-Za-z0-9]+) \(([A-Za-z]+)\)" },
            { EventType.LifeLeech, @".*Life Leech return (\d+\.\d+) Life" },
            { EventType.SetAttackerName, @"\s*attackerName = (.*)" },
            { EventType.SetAttackerId, @"\s*attackerID = (\d+)" },
            { EventType.SetDefenderName, @"\s*defenderName = (.*)" },
            { EventType.SetDefenderId, @"\s*defenderID = (\d+)" },
            { EventType.Deflect, @".*Deflect Projectile Chance \((\d+\.\d+)\) caused a deflection" },
            { EventType.SetDOT, @"\s*Total Damage:  Absolute \((\d+\.\d+)\), Over Time \((\d+\.\d+)\)" }, // 1274+1128?
            { EventType.SetArmorAbsorb, @"\s*protectionAbsorption = (\d+\.\d+)" },
            { EventType.SetFailedDeflect, @".*Deflect Projectile Chance \((\d+\.\d+)\) not met \((\d+\.\d+\))" },
        };

        // ^bShield: Reduced (%f) Damage by (%f%) percent, remaining damage (%f)
        //    ^bRacial Bonus Damage:  (%f)
        //     ^b%f%% Damage Reflected
        public static Dictionary<string, DamageType> DamageTypes = new Dictionary<string, DamageType>()
        {
            { "Lightning", DamageType.Lightning },
            { "Physical", DamageType.Physical },
            { "Vitality", DamageType.Vitality },
            { "Chaos", DamageType.Chaos },
            { "Aether", DamageType.Aether },
            { "Bleeding", DamageType.Bleeding },
            { "Acid", DamageType.Acid },
            { "Cold", DamageType.Cold },
            { "Fire", DamageType.Fire },
        };
    }
}
