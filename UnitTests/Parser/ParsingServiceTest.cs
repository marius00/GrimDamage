using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrimDamage.Parser.Config;
using GrimDamage.Parser.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using GrimDamage.Parser.Service;

namespace UnitTests.Parser {
    [TestClass]
    public class ParsingServiceTest {
        private ParsingService parser = new ParsingService();

        [TestMethod]
        public void CanParseDamage() {
            foreach (string testData in new[] {
                "^y    Damage 0.321876 to Defender 0x34108 (Physical)",
                "^y    Damage 59.653908 to Defender 0x34108 (Lightning)",
                "^y    Damage 2.341711 to Defender 0x34108 (Vitality)",
                "^y    Damage 12.937283 to Defender 0x34108 (Lightning)",
                "^y    Damage 0.507852 to Defender 0x34108 (Vitality)",
                "^y    Damage 0,507852 to Defender 0x34108 (Vitality)"
            }) {
                var regex = EventMapping.RegexMap[EventType.DamageDealt];
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(4);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseLifeLeech() {
            foreach (string testData in new[] {
                "    ^b7.000000% Life Leech return 906.158630 Life",
                "    ^b0.882000% Life Leech return 56.199940 Life",
                "    ^b0.882000% Life Leech return 47.907486 Life"
            }) {
                string pattern = EventMapping.PatternMap[EventType.LifeLeech];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseSetAttackerName() {
            foreach (string testData in new[] {
                "    attackerName = records/creatures/pc/malepc01.dbr",
                "    attackerName = records/creatures/enemies/rifthound_swamp_a01.dbr"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetAttackerName];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseSetAttackerId() {
            foreach (string testData in new[] {
                "    attackerID = 159528",
                "    attackerID = 1"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetAttackerId];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseSetDefenderName() {
            foreach (string testData in new[] {
                "    defenderName = records/creatures/pc/malepc01.dbr",
                "    defenderName = records/creatures/enemies/rifthound_swamp_a01.dbr"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetDefenderName];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseSetDefenderId() {
            foreach (string testData in new[] {
                "    defenderID = 159528",
                "    defenderID = 1"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetDefenderId];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseDeflect() {
            foreach (string testData in new[] {
                "    ^yDeflect Projectile Chance (18.000000) caused a deflection",
                "    ^yDeflect Projectile Chance (18.000000) caused a deflection",
                "    ^yDeflect Projectile Chance (14.000000) caused a deflection"
            }) {
                string pattern = EventMapping.PatternMap[EventType.Deflect];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }

        [TestMethod]
        public void CanParseSetDOT() {
            foreach (string testData in new[] {
                "    Total Damage:  Absolute (3955.457764), Over Time (223.250229)",
                "    Total Damage:  Absolute (4683.350098), Over Time (325.475250)",
                "    Total Damage:  Absolute (0.000000), Over Time (0.000000)"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetDOT];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(3);
                parser.Parse(testData);
            }
        }
        
            [TestMethod]
        public void CanParseSetArmorAbsorb() {
            foreach (string testData in new[] {
                "    protectionAbsorption = 324.741364",
                "    protectionAbsorption = 198.321365",
                "    protectionAbsorption = 920.583984"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetArmorAbsorb];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(2);
                parser.Parse(testData);
            }
        }
        [TestMethod]
        public void CanParseSetFailedDeflect() {
            foreach (string testData in new[] {
                "    ^yDeflect Projectile Chance (5.000000) not met (85.392929)",
                "    ^yDeflect Projectile Chance (5.000000) not met (46.613770)",
                "    ^yDeflect Projectile Chance (14.000000) not met (24.556601)"
            }) {
                string pattern = EventMapping.PatternMap[EventType.SetFailedDeflect];

                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(testData);

                match.Success.Should().Be.True();
                match.Groups.Count.Should().Be.EqualTo(3);
                parser.Parse(testData);
            }
        }


        [TestMethod]
        public void CanParseTestdataSet() {
            var lines = File.ReadAllLines(Path.Combine("testdata", "131482213376447842.txt"));
            foreach (string testData in lines) {
                parser.Parse(testData);
            }
        }
    }
}
