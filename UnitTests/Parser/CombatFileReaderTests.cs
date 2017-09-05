using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.GD.Logger;
using GrimDamage.Parser.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace UnitTests.Parser {
    [TestClass]
    public class CombatFileReaderTests {

        [TestMethod]
        public void TestAddDamage() {
            List<string> events = new List<string>() {
                "0 ^y    Damage 0.321876 to Defender 0x34108 (Physical)",
                "0 ^y    Damage 59.653908 to Defender 0x34108 (Lightning)",
                "0 ^y    Damage 2.341711 to Defender 0x34108 (Vitality)",
                "0 ^y    Damage 12.937283 to Defender 0x34108 (Lightning)",
                "0 ^y    Damage 0.507852 to Defender 0x34108 (Vitality)"
            };
            var reader = new CombatFileReader(new DamageParsingService(), events);
            reader.Next();
        }

        [TestMethod]
        public void TestDamageSum() {
            int id = 0x34108;
            List<string> events = new List<string>() {
                "0 ^y    Damage 10.1 to Defender 0x34108 (Physical)",
                "0     defenderName = records/creatures/pc/malepc01.dbr",
                $"0     defenderID = {id}",
                "0     attackerName = records/creatures/enemies/rifthound_swamp_a01.dbr",
                "0     attackerID = 1",
                "0 ^y    Damage 10.1 to Defender 0x34108 (Physical)",
                "0 ^y    Damage 20.1 to Defender 0x34108 (Lightning)",
                "0 ^y    Damage 391,223785400391 to Defender 0x34108 (Vitality)"
            };
            DamageParsingService dmg = new DamageParsingService();
            var reader = new CombatFileReader(dmg, events);
            reader.Next();
            var entries = dmg.GetEntity(id);
            entries.DamageTaken.Sum(m => m.Amount).Should().Be.GreaterThan(421).And.Be.LessThan(422);
        }

        [TestMethod]
        public void TestSetName() {
            List<string> events = new List<string>() {
                "0     attackerName = records/creatures/pc/malepc01.dbr",
                "0     attackerName = records/creatures/enemies/rifthound_swamp_a01.dbr",
                "0     defenderName = records/creatures/pc/malepc01.dbr",
                "0     defenderName = records/creatures/enemies/rifthound_swamp_a01.dbr"
            };
            var reader = new CombatFileReader(new DamageParsingService(), events);
            reader.Next();
        }

        [TestMethod]
        public void TestSetId() {
            List<string> events = new List<string>() {
                "0     defenderName = records/creatures/pc/malepc01.dbr",
                "0     defenderID = 159528",
                "0     defenderName = records/creatures/enemies/rifthound_swamp_a01.dbr",
                "0     defenderID = 1",
                "0     attackerName = records/creatures/pc/malepc01.dbr",
                "0     attackerID = 159528",
                "0     attackerName = records/creatures/enemies/rifthound_swamp_a01.dbr",
                "0     attackerID = 1"
            };
            var reader = new CombatFileReader(new DamageParsingService(), events);
            reader.Next();
        }
    }
}
